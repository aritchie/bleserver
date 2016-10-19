using System;


namespace Acr.Ble.Server
{
    [Flags]
    public enum CharacteristicProperties
    {
        Broadcast = 1, //0x1
        Read = 2, //0x2
        AppleWriteWithoutResponse = 4, //0x4
        Write = 8, //0x8
        Notify = 16, //0x10
        Indicate = 32, //0x20
        AuthenticatedSignedWrites = 64, //0x40
        ExtendedProperties = 128, //0x80
        NotifyEncryptionRequired = 256, //0x100
        IndicateEncryptionRequired = 512
    }
}
