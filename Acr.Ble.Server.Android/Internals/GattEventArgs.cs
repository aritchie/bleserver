using System;
using Android.Bluetooth;


namespace Acr.Ble.Server.Internals
{
    public class GattEventArgs : EventArgs
    {
        public GattEventArgs(BluetoothDevice device)
        {
            this.Device = device;
        }


        public BluetoothDevice Device { get; }
    }
}