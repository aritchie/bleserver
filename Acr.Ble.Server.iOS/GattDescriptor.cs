using System;


namespace Acr.Ble.Server
{
    public class GattDescriptor : AbstractGattDescriptor
    {
        public GattDescriptor(IGattCharacteristic characteristic, Guid descriptorUuid, byte[] value) : base(characteristic, descriptorUuid, value)
        {
        }

        public override IObservable<object> WhenReadReceived()
        {
            throw new NotImplementedException();
        }

        public override IObservable<byte[]> WhenWriteReceived()
        {
            throw new NotImplementedException();
        }
    }
}