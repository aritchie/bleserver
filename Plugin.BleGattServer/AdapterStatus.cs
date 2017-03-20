using System;


namespace Plugin.BleGattServer
{
    public enum AdapterStatus
    {
        Unknown,
        Resetting,
        Unsupported,
        Unauthorized,
        PoweredOff,
        PoweredOn
    }
}