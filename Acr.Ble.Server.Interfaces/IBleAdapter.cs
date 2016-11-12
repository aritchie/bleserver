using System;


namespace Acr.Ble.Server
{
    public interface IBleAdapter
    {
        AdapterStatus AdapterStatus { get; }
        IObservable<AdapterStatus> WhenAdapterStatusChanged();
        IGattServer CreateGattServer();
    }
}
