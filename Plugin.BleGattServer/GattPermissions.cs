using System;


namespace Plugin.BleGattServer
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