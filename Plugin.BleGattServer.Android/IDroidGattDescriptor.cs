using System;
using Android.Bluetooth;


namespace Plugin.BleGattServer
{
    public interface IDroidGattDescriptor : IGattDescriptor
    {
        BluetoothGattDescriptor Native { get; }
    }
}