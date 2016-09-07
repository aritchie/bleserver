using System;


namespace Acr.Ble.Server
{
    public class GattDescriptor : AbstractGattDescriptor
    {
        public GattDescriptor(IGattCharacteristic characteristic, Guid descriptorUuid) : base(characteristic, descriptorUuid)
        {
        }

        public override IObservable<IReadRequest> WhenReadReceived()
        {
            throw new NotImplementedException();
        }


        public override IObservable<IWriteRequest> WhenWriteReceived()
        {
            throw new NotImplementedException();
        }
    }
}