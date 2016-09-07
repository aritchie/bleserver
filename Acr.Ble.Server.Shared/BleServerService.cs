using System;


namespace Acr.Ble.Server
{
    public static class BleServerService
    {
        public static IGattServerFactory Factory { get; set; } = new GattServerFactory();
    }
}
