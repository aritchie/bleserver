using System;
using CoreBluetooth;


namespace Plugin.BleGattServer
{
    public interface IIosGattService : IGattService
    {
        CBMutableService Native { get; }
    }
}
