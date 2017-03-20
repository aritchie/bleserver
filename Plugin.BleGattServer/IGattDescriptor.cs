using System;


namespace Plugin.BleGattServer
{
    public interface IGattDescriptor
    {
        IGattCharacteristic Characteristic { get; }
        Guid Uuid { get; }
        byte[] Value { get; }
    }
}
