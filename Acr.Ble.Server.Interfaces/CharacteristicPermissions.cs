using System;


namespace Acr.Ble.Server
{
    [Flags]
    public enum CharacteristicPermissions
    {
        Readable = 1,
        ReadEncryptionRequired = 4,
        Writeable = 2,
        WriteEncryptionRequired = 8
    }
}