using System;
using Android.Bluetooth;


namespace Plugin.BleGattServer
{
    public interface IDroidGattCharacteristic : IGattCharacteristic
    {
        BluetoothGattCharacteristic Native { get; }
    }
}