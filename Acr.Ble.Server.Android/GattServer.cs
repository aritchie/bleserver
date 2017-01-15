﻿using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Acr.Ble.Server.Internals;
using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.OS;


namespace Acr.Ble.Server
{
    public class GattServer : AbstractGattServer
    {
        readonly BluetoothManager manager;
        readonly AdvertisementCallbacks adCallbacks;
        readonly GattContext context;
        readonly Subject<bool> runningSubj;
        BluetoothGattServer server;


        public GattServer()
        {
            this.manager = (BluetoothManager)Application.Context.GetSystemService(Context.BluetoothService);
            this.adCallbacks = new AdvertisementCallbacks();
            this.context = new GattContext();
            this.runningSubj = new Subject<bool>();
        }


        bool isRunning = false;
        public override bool IsRunning => this.isRunning;


        IObservable<bool> runningOb;
        public override IObservable<bool> WhenRunningChanged()
        {
            this.runningOb = this.runningOb ?? Observable.Create<bool>(ob =>
            {
                this.adCallbacks.Failed = ob.OnError;
                this.adCallbacks.Started = () => ob.OnNext(true);
                var sub = this.runningSubj
                    .AsObservable()
                    .Subscribe(x => ob.OnNext(false));

                return () =>
                {
                    sub.Dispose();
                    this.adCallbacks.Failed = null;
                    this.adCallbacks.Started = null;
                };
            })
            .Publish()
            .RefCount();

            return this.runningOb;
        }


        public override Task Start(AdvertisementData adData)
        {
            if (this.isRunning)
                return Task.CompletedTask;

            this.StartAdvertising(adData);
            this.StartGatt();
            this.isRunning = true;
            return Task.CompletedTask;
        }


        public override void Stop()
        {
            if (!this.isRunning)
                return;

            this.isRunning = false;
            this.manager.Adapter.BluetoothLeAdvertiser.StopAdvertising(this.adCallbacks);
            this.context.Server = null;
            this.server?.Close();
            this.server = null;
            this.runningSubj.OnNext(false);
        }


        protected override IGattService CreateNative(Guid uuid, bool primary)
        {
            var service  = new GattService(this.context, this, uuid, primary);
            this.server?.AddService(service.Native);
            return service;
        }


        protected override void RemoveNative(IGattService service)
        {
            var nuuid = Java.Util.UUID.FromString(service.Uuid.ToString());
            var native = this.server.Services.FirstOrDefault(x => x.Uuid.Equals(nuuid));
            if (native != null)
                this.server?.RemoveService(native);
        }


        protected override void ClearNative()
        {
            this.server?.ClearServices();
        }


        protected virtual void StartAdvertising(AdvertisementData adData)
        {
            var settings = new AdvertiseSettings.Builder()
                .SetAdvertiseMode(AdvertiseMode.Balanced)
                .SetConnectable(true);

            var data = new AdvertiseData.Builder()
                .SetIncludeDeviceName(true)
                .SetIncludeTxPowerLevel(true);

            //if (adData.ManufacturerData != null)
            //    data.AddManufacturerData(adData.ManufacturerData.CompanyId, adData.ManufacturerData.Data);

            foreach (var serviceUuid in adData.ServiceUuids)
            {
                var uuid = ParcelUuid.FromString(serviceUuid.ToString());
                data.AddServiceUuid(uuid);
            }

            this.manager
                .Adapter
                .BluetoothLeAdvertiser
                .StartAdvertising(
                    settings.Build(),
                    data.Build(),
                    this.adCallbacks
                );
        }


        protected virtual void StartGatt()
        {
            this.server = this.manager.OpenGattServer(Application.Context, this.context.Callbacks);
            this.context.Server = this.server;

            foreach (var service in this.Services.OfType<IDroidGattService>())
            {
                foreach (var characteristic in service.Characteristics.OfType<IDroidGattCharacteristic>())
                {
                    foreach (var descriptor in characteristic.Descriptors.OfType<IDroidGattDescriptor>())
                    {
                        characteristic.Native.AddDescriptor(descriptor.Native);
                    }
                    service.Native.AddCharacteristic(characteristic.Native);
                }
                this.server.AddService(service.Native);
            }
        }
    }
}