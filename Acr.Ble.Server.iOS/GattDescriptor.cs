using System;


namespace Acr.Ble.Server
{
    public class GattDescriptor : AbstractGattDescriptor
    {
        public GattDescriptor(IGattCharacteristic characteristic, Guid descriptorUuid) : base(characteristic, descriptorUuid)
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