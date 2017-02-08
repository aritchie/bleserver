using System;
using CoreBluetooth;


namespace Plugin.BleGattServer
{
    public interface IIosGattCharacteristic : IGattCharacteristic
    {
        CBMutableCharacteristic Native { get; }
    }
}
