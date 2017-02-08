using System;
using Android.Bluetooth;


namespace Plugin.BleGattServer.Internals
{
    public class MtuChangedEventArgs : GattEventArgs
    {
        public MtuChangedEventArgs(BluetoothDevice device, int mtu) : base(device)
        {
            this.Mtu = mtu;
        }


        public int Mtu { get; }
    }
}