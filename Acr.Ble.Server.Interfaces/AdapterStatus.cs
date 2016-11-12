using System;


namespace Acr.Ble.Server
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