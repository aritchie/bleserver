using System;
using System.Linq;
using System.Reactive.Linq;
using Windows.Devices.Radios;
using Windows.Foundation;


namespace Acr.Ble.Server
{
    public class BleAdapterImpl : IBleAdapter
    {
        readonly Lazy<Radio> radio;


        public BleAdapterImpl()
        {
            this.radio = new Lazy<Radio>(() =>
                Radio
                    .GetRadiosAsync()
                    .AsTask()
                    .Result
                    .FirstOrDefault(x => x.Kind == RadioKind.Bluetooth)
            );
        }


        public AdapterStatus Status
        {
            get
            {
                if (this.radio.Value == null)
                    return AdapterStatus.Unsupported;

                switch (this.radio.Value.State)
                {
                    case RadioState.Disabled:
                    case RadioState.Off:
                        return AdapterStatus.PoweredOff;

                    case RadioState.Unknown:
                        return AdapterStatus.Unknown;

                    default:
                        return AdapterStatus.PoweredOn;
                }
            }
        }


        IObservable<AdapterStatus> statusOb;
        public IObservable<AdapterStatus> WhenStatusChanged()
        {
            this.statusOb = this.statusOb ?? Observable.Create<AdapterStatus>(ob =>
            {
                ob.OnNext(this.Status);
                var handler = new TypedEventHandler<Radio, object>((sender, args) =>
                    ob.OnNext(this.Status)
                );
                this.radio.Value.StateChanged += handler;
                return () => this.radio.Value.StateChanged -= handler;
            })
            .Replay(1)
            .RefCount();

            return this.statusOb;
        }


        public IGattServer CreateGattServer()
        {
            return new GattServer();
        }


        public bool IsAdvertisementManufacturerDataSupported { get; } = true;
    }
}
