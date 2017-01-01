using System;


namespace Acr.Ble.Server
{
    public interface IBleAdapter
    {
        AdapterStatus Status { get; }
        IObservable<AdapterStatus> WhenStatusChanged();
        IGattServer CreateGattServer();
    }
}
