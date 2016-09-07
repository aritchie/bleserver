using System;


namespace Acr.Ble.Server
{
    public interface IGattDescriptor
    {
        IGattCharacteristic Characteristic { get; }
        Guid Uuid { get; }
        IObservable<IReadRequest> WhenReadReceived();
        IObservable<IWriteRequest> WhenWriteReceived();
    }
}
