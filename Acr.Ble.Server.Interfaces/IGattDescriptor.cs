using System;


namespace Acr.Ble.Server
{
    public interface IGattDescriptor
    {
        IGattCharacteristic Characteristic { get; }

        Guid Uuid { get; }
        byte[] Value { get; set; }
        IObservable<object> WhenReadReceived();
        IObservable<byte[]> WhenWriteReceived();
    }
}
