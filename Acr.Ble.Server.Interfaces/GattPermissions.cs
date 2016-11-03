using System;


namespace Acr.Ble.Server
{
    [Flags]
    public enum GattPermissions
    {
        Read = 1,
        ReadEncrypted = 4,
        Write = 2,
        WriteEncrypted = 8
    }
}