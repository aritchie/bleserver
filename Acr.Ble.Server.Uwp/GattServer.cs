﻿using System;
using System.Reactive.Linq;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Foundation;
using Windows.Storage.Streams;


namespace Acr.Ble.Server
{
    public class GattServer : AbstractGattServer
    {
        readonly BluetoothLEAdvertisementPublisher publisher;


        public GattServer()
        {
            this.publisher = new BluetoothLEAdvertisementPublisher();
        }


        IObservable<bool> runOb;
        public override IObservable<bool> WhenRunningChanged()
        {
            this.runOb = this.runOb ?? Observable.Create<bool>(ob =>
            {
                ob.OnNext(this.IsRunning);
                var handler = new TypedEventHandler<BluetoothLEAdvertisementPublisher, BluetoothLEAdvertisementPublisherStatusChangedEventArgs>(
                    (sender, args) => ob.OnNext(this.IsRunning)
                );
                this.publisher.StatusChanged += handler;
                return () => this.publisher.StatusChanged -= handler;
            })
            .Repeat(1);

            return this.runOb;
        }


        public override bool IsRunning => this.publisher.Status == BluetoothLEAdvertisementPublisherStatus.Started;


        public override void Start(AdvertisementData adData)
        {
            this.publisher.Advertisement.Flags = BluetoothLEAdvertisementFlags.ClassicNotSupported;
            this.publisher.Advertisement.ManufacturerData.Clear();
            this.publisher.Advertisement.ServiceUuids.Clear();

            if (adData.ManufacturerData != null)
            {
                using (var writer = new DataWriter())
                {
                    writer.WriteBytes(adData.ManufacturerData.Data);
                    var md = new BluetoothLEManufacturerData(adData.ManufacturerData.CompanyId, writer.DetachBuffer());
                    this.publisher.Advertisement.ManufacturerData.Add(md);
                }
            }
            foreach (var serviceUuid in adData.ServiceUuids)
            {
                this.publisher.Advertisement.ServiceUuids.Add(serviceUuid);
            }
            this.publisher.Start();
            //GattCharacteristicUuids.
            //GattServiceUuids.AlertNotification
            //new GattReliableWriteTransaction().Write/Commit
        }


        public override void Stop()
        {
            this.publisher.Stop();
        }


        protected override IGattService CreateNative(Guid uuid, bool primary)
        {
            throw new NotImplementedException();
        }


        protected override void ClearNative()
        {
            throw new NotImplementedException();
        }


        protected override void RemoveNative(IGattService service)
        {
            throw new NotImplementedException();
        }
    }
}
/*
using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Radios;
using Windows.Storage.Streams;


namespace Plugin.BeaconAds
{
    public class BeaconAdvertiser : IBeaconAdvertiser
    {
        readonly BluetoothLEAdvertisementPublisher publiser;
        readonly Lazy<Radio> radio;


        public BeaconAdvertiser()
        {
            this.publiser = new BluetoothLEAdvertisementPublisher();
            this.radio = new Lazy<Radio>(() =>
                Radio
                    .GetRadiosAsync()
                    .AsTask()
                    .Result
                    .FirstOrDefault(x => x.Kind == RadioKind.Bluetooth)
            );
        }


        public Status Status
        {
            get
            {
                if (this.radio.Value == null)
                    return Status.Unsupported;

                switch (this.radio.Value.State)
                {
                    case RadioState.Disabled:
                    case RadioState.Off:
                        return Status.PoweredOff;

                    case RadioState.Unknown:
                        return Status.Unknown;

                    default:
                        return Status.PoweredOn;
                }
            }
        }


        public Beacon AdvertisedBeacon { get; private set; }


        public void Start(Beacon beacon)
        {


            var writer = new DataWriter();
            writer.WriteBytes(beacon.ToIBeaconPacket(10));
            var md = new BluetoothLEManufacturerData(76, writer.DetachBuffer());
            this.publiser.Advertisement.ManufacturerData.Add(md);
            this.publiser.Start();

            //var trigger = new BluetoothLEAdvertisementPublisherTrigger();
            //trigger.Advertisement.ManufacturerData.Add(md);
            this.AdvertisedBeacon = beacon;
        }


        public void Stop()
        {
            this.publiser.Stop();
        }
    }
}

     */
