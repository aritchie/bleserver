using System;
using System.Reactive.Linq;
using CoreBluetooth;


namespace Acr.Ble.Server
{
    public class BleAdapterImpl : IBleAdapter
    {
        readonly CBPeripheralManager manager = new CBPeripheralManager();


        public AdapterStatus Status
        {
            get
            {
                switch (this.manager.State)
                {
                    case CBPeripheralManagerState.PoweredOff:
                        return AdapterStatus.PoweredOff;

                    case CBPeripheralManagerState.PoweredOn:
                        return AdapterStatus.PoweredOn;

                    case CBPeripheralManagerState.Resetting:
                        return AdapterStatus.Resetting;

                    case CBPeripheralManagerState.Unauthorized:
                        return AdapterStatus.Unauthorized;

                    case CBPeripheralManagerState.Unsupported:
                        return AdapterStatus.Unsupported;

                    case CBPeripheralManagerState.Unknown:
                    default:
                        return AdapterStatus.Unknown;
                }
            }
        }


        IObservable<AdapterStatus> statusOb;
        public IObservable<AdapterStatus> WhenStatusChanged()
        {
            this.statusOb = this.statusOb ?? Observable.Create<AdapterStatus>(ob =>
            {
                ob.OnNext(this.Status);
                var handler = new EventHandler((sender, args) => ob.OnNext(this.Status));
                this.manager.StateUpdated += handler;

                return () => this.manager.StateUpdated -= handler;
            })
            .Replay(1)
            .RefCount();

            return this.statusOb;
        }


        public IGattServer CreateGattServer()
        {
            return new GattServer(this.manager);
        }


        public bool IsAdvertisementManufacturerDataSupported { get; } = false;
    }
}
