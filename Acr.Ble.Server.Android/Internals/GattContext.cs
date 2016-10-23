using System;
using Android.Bluetooth;


namespace Acr.Ble.Server.Internals
{
    public class GattContext
    {
        public GattContext()
        {
            this.Callbacks = new GattServerCallbacks();
        }


        public BluetoothGattServer Server { get; internal set; }
        public GattServerCallbacks Callbacks { get; }
    }
}