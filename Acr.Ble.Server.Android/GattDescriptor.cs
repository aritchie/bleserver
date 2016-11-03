using System;
using Android.Bluetooth;


namespace Acr.Ble.Server
{
    public class GattDescriptor : AbstractGattDescriptor
    {
        public GattDescriptor(IGattCharacteristic characteristic,
                              Guid descriptorUuid,
                              byte[] value) : base(characteristic, descriptorUuid, value)
        {
            this.Native = new BluetoothGattDescriptor(
                descriptorUuid.ToUuid(),
                GattDescriptorPermission.Read // TODO
            );
            this.Native.SetValue(value);
        }


        public BluetoothGattDescriptor Native { get; }
    }
}