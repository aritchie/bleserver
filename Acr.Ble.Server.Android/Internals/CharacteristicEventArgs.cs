using System;
using Android.Bluetooth;


namespace Acr.Ble.Server.Internals
{
    public class CharacteristicEventArgs : GattRequestEventArgs
    {
        public CharacteristicEventArgs(BluetoothDevice device,
                                       BluetoothGattCharacteristic characteristic,
                                       int requestId,
                                       int offset)
                : base(device, requestId, offset)
        {
            this.Characteristic = characteristic;
        }


        public BluetoothGattCharacteristic Characteristic { get; }
    }
}