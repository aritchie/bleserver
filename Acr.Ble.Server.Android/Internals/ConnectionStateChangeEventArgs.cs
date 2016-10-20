using System;
using Android.Bluetooth;


namespace Acr.Ble.Server.Internals
{
    public class ConnectionStateChangeEventArgs : GattEventArgs
    {
        public ConnectionStateChangeEventArgs(BluetoothDevice device, ProfileState oldState, ProfileState newState) : base(device)
        {
            this.OldState = oldState;
            this.NewState = newState;
        }


        public ProfileState OldState { get; }
        public ProfileState NewState { get; }
    }
}