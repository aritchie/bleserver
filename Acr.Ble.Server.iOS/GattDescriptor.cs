using System;
using System.Reactive.Linq;
using CoreBluetooth;
using Foundation;


namespace Acr.Ble.Server
{
    public class GattDescriptor : AbstractGattDescriptor
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