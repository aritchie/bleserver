using System;


namespace Acr.Ble.Server
{
    public class BleAdapterImpl : IBleAdapter
    {
        public AdapterStatus AdapterStatus { get; }
        public IObservable<AdapterStatus> WhenAdapterStatusChanged()
        {
            throw new NotImplementedException();
        }

        public IGattServer CreateGattServer()
        {
            throw new NotImplementedException();
        }
    }
}
