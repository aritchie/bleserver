using System;


namespace Acr.Ble.Server
{
    public interface IGattDescriptor
    {
        IGattCharacteristic Characteristic { get; }
        Guid Uuid { get; }
        byte[] Value { get; }
    }
}
