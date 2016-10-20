using System;
using Android.Bluetooth;


namespace Acr.Ble.Server.Internals
{
    public class GattContext
    {
        public GattContext(BluetoothGattServer server)
        {
            this.Server = server;
            this.Callbacks = new GattServerCallbacks();
        }


        public BluetoothGattServer Server { get; }
        public GattServerCallbacks Callbacks { get; }
    }
}