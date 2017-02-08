using System;
using Android.Bluetooth;


namespace Plugin.BleGattServer.Internals
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