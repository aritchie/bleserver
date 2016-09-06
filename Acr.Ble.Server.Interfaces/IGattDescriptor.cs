using System;


namespace Acr.Ble.Server
{
    public interface IGattDescriptor
    {
        IGattCharacteristic Characteristic { get; }
        Guid Uuid { get; }
        IObservable<object> WhenReadReceived();
        IObservable<byte[]> WhenWriteReceived();
    }
}
