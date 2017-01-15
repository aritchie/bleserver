using System;
using CoreBluetooth;


namespace Acr.Ble.Server
{
    public interface IIosGattCharacteristic : IGattCharacteristic
    {
        CBMutableCharacteristic Native { get; }
    }
}
