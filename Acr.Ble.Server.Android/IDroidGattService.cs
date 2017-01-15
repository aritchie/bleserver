using System;
using Android.Bluetooth;


namespace Acr.Ble.Server
{
    public interface IDroidGattService : IGattService
    {
        BluetoothGattService Native { get; }
    }
}