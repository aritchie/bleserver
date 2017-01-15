using System;
using Android.Bluetooth;


namespace Acr.Ble.Server
{
    public interface IDroidGattDescriptor : IGattDescriptor
    {
        BluetoothGattDescriptor Native { get; }
    }
}