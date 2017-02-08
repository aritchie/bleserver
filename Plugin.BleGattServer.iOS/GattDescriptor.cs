using System;
using CoreBluetooth;
using Foundation;


namespace Plugin.BleGattServer
{
    public class GattDescriptor : AbstractGattDescriptor, IIosGattDescriptor
    {
        public CBMutableDescriptor Native { get; }


        public GattDescriptor(IGattCharacteristic characteristic,
                              Guid descriptorUuid,
                              byte[] value) : base(characteristic, descriptorUuid, value)
        {
            this.Native = new CBMutableDescriptor(descriptorUuid.ToCBUuid(), NSData.FromArray(value));
        }
    }
}