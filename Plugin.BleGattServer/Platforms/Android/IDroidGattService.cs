using System;
using Android.Bluetooth;


namespace Plugin.BleGattServer
{
    public interface IDroidGattService : IGattService
    {
        BluetoothGattService Native { get; }
    }
}