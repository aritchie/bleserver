using System;


namespace Acr.Ble.Server
{
    public class GattServer : AbstractGattServer
    {
        public override IObservable<bool> WhenRunningChanged()
        {
            throw new NotImplementedException();
        }


        public override bool IsRunning { get; }
        public override void Start(AdvertisementData adData)
        {
            throw new NotImplementedException();
        }


        public override void Stop()
        {
            throw new NotImplementedException();
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
