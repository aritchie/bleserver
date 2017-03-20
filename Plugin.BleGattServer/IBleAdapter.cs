using System;


namespace Plugin.BleGattServer
{
    public interface IBleAdapter
    {
        AdapterStatus Status { get; }
        IObservable<AdapterStatus> WhenStatusChanged();
        IGattServer CreateGattServer();
        bool IsAdvertisementManufacturerDataSupported { get; }
    }
}
