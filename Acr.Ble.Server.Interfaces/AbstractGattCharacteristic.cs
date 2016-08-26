using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Acr.Ble.Server
{
    public abstract class AbstractGattCharacteristic : IGattCharacteristic
    {
        readonly IList<IGattDescriptor> internalList;


        protected AbstractGattCharacteristic(IGattService service, Guid characteristicUuid, CharacteristicProperties properties, byte[] value)
        {
            this.Service = service;
            this.Uuid = characteristicUuid;
            this.Properties = properties;
            this.Value = value;

            this.internalList = new List<IGattDescriptor>();
            this.Descriptors = new ReadOnlyCollection<IGattDescriptor>(this.internalList);
        }


        public IGattService Service { get; }

        public Guid Uuid { get; }
        public CharacteristicProperties Properties { get; }
        public byte[] Value { get; set; }
        public IReadOnlyList<IGattDescriptor> Descriptors { get; }


        public IGattDescriptor AddDescriptor(Guid uuid, byte[] initialValue)
        {
            var native = this.CreateNative(uuid, initialValue);
            this.internalList.Add(native);
            return native;
        }


        public void RemoveDescriptor(IGattDescriptor descriptor)
        {
            this.RemoveNative(descriptor);
            this.internalList.Remove(descriptor);
        }


        public abstract void Broadcast(byte[] value);
        public abstract IObservable<bool> WhenSubscriptionStateChanged();
        public abstract IObservable<IWriteRequest> WhenWriteReceived();
        public abstract IObservable<IReadRequest> WhenReadReceived();


        protected abstract IGattDescriptor CreateNative(Guid uuid, byte[] initialValue);
        protected abstract void RemoveNative(IGattDescriptor descriptor);
    }
}
