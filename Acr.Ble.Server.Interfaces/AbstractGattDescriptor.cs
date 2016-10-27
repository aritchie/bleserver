using System;


namespace Acr.Ble.Server
{
    public abstract class  AbstractGattDescriptor : IGattDescriptor
    {
        protected AbstractGattDescriptor(IGattCharacteristic characteristic, Guid descriptorUuid)
        {
            this.Characteristic = characteristic;
            this.Uuid = descriptorUuid;
        }


        public IGattCharacteristic Characteristic { get; }
        public Guid Uuid { get; }

        public abstract IObservable<ReadRequest> WhenReadReceived();
        public abstract IObservable<WriteRequest> WhenWriteReceived();
    }
}
