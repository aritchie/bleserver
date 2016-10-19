using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace Acr.Ble.Server
{
    public abstract class AbstractGattCharacteristic : IGattCharacteristic
    {
        readonly IList<IGattDescriptor> internalList;


        protected AbstractGattCharacteristic(IGattService service, Guid characteristicUuid, CharacteristicProperties properties)
        {
            this.Service = service;
            this.Uuid = characteristicUuid;
            this.Properties = properties;

            this.internalList = new List<IGattDescriptor>();
            this.Descriptors = new ReadOnlyCollection<IGattDescriptor>(this.internalList);
        }


        public IGattService Service { get; }

        public Guid Uuid { get; }
        public CharacteristicProperties Properties { get; }
        public IReadOnlyList<IGattDescriptor> Descriptors { get; }


        public IGattDescriptor AddDescriptor(Guid uuid)
        {
            var native = this.CreateNative(uuid);
            this.internalList.Add(native);
            return native;
        }


        public abstract void Broadcast(byte[] value);
        public abstract IObservable<bool> WhenSubscriptionStateChanged();
        public abstract IObservable<IWriteRequest> WhenWriteReceived();
        public abstract IObservable<IReadRequest> WhenReadReceived();

        protected abstract IGattDescriptor CreateNative(Guid uuid);
    }
}
