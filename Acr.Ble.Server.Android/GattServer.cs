using System;
using System.Linq;
using Acr.Ble.Server.Internals;
using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.OS;
using Java.Util;


namespace Acr.Ble.Server
{
    public class GattServer : AbstractGattServer
    {
        readonly BluetoothManager manager;
        readonly AdvertisementCallbacks adCallbacks;
        readonly GattContext context;
        BluetoothGattServer server;


        public GattServer()
        {
            this.manager = (BluetoothManager)Application.Context.GetSystemService(Context.BluetoothService);
            this.adCallbacks = new AdvertisementCallbacks();
            this.context = new GattContext();
        }


        bool isRunning = false;
        public override bool IsRunning => this.isRunning;

        public override void Start(AdvertisementData adData)
        {
            this.StartAdvertising(adData);
            this.StartGatt();
            this.isRunning = true;
        }


        public override void Stop()
        {
            this.manager.Adapter.BluetoothLeAdvertiser.StopAdvertising(this.adCallbacks);
            this.context.Server = null;
            this.server?.Close();
            this.server = null;
        }


        protected override IGattService CreateNative(Guid uuid, bool primary)
        {
            var service  = new GattService(this.context, this, uuid, primary);
            this.server?.AddService(service.Native);
            return service;
        }


        protected override void RemoveNative(IGattService service)
        {
            var nuuid = UUID.FromString(service.Uuid.ToString());
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
                .SetConnectable(adData.IsConnectable);
                //.SetTxPowerLevel(AdvertiseTx.PowerHigh);

            var data = new AdvertiseData.Builder()
                .SetIncludeDeviceName(adData.IncludeDeviceName)
                .SetIncludeTxPowerLevel(adData.IncludeTxPower);

            foreach (var keyValue in adData.ManufacturerData)
                data.AddManufacturerData(keyValue.Key, keyValue.Value);

            foreach (var serviceUuid in adData.ServiceUuids)
            {
                var uuid = ParcelUuid.FromString(serviceUuid.ToString());
                data.AddServiceUuid(uuid);
            }

            foreach (var keyValue in adData.ServiceData)
            {
                var uuid = ParcelUuid.FromString(keyValue.Key.ToString());
                data.AddServiceData(uuid, keyValue.Value);
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

            foreach (var service in this.Services)
            {
                var nservice = ((GattService) service).Native;

                foreach (var characteristic in service.Characteristics)
                {
                    var ncharacter = ((GattCharacteristic) characteristic).Native;
                    nservice.AddCharacteristic(ncharacter);

                    foreach (var descriptor in characteristic.Descriptors)
                    {
                        var ndescriptor = ((GattDescriptor) descriptor).Native;
                        ncharacter.AddDescriptor(ndescriptor);
                    }
                }
                this.server.AddService(nservice);
            }
        }
    }
}