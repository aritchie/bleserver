using System;
using Android.Bluetooth;


namespace Acr.Ble.Server
{
    public interface IDroidGattCharacteristic : IGattCharacteristic
    {
        BluetoothGattCharacteristic Native { get; }
    }
}