using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Acr.Ble.Server
{
    public abstract class AbstractGattService : IGattService
    {
        protected AbstractGattService(IGattServer server, Guid serviceUuid, bool primary)
        {
            this.Server = server;

            this.Uuid = serviceUuid;
            this.IsPrimary = primary;

            this.internalList = new List<IGattCharacteristic>();
            this.Characteristics = new ReadOnlyCollection<IGattCharacteristic>(this.internalList);
        }


        public IGattServer Server { get; }
        public Guid Uuid { get; }
        public bool IsPrimary { get; }


        public virtual IGattCharacteristic AddCharacteristic(Guid uuid, CharacteristicProperties properties, CharacteristicPermissions permissions, byte[] initialValue = null)
        {
            var characteristic = this.CreateCharacteristic(uuid, properties, permissions, initialValue);
            this.internalList.Add(characteristic);
            return characteristic;
        }


        public virtual void RemoveCharacteristic(IGattCharacteristic characteristic)
        {
            this.internalList.Remove(characteristic);
        }


        readonly IList<IGattCharacteristic> internalList;
        public IReadOnlyList<IGattCharacteristic> Characteristics { get; }


        protected abstract IGattCharacteristic CreateCharacteristic(Guid uuid, CharacteristicProperties properties, CharacteristicPermissions permissions, byte[] initialValue);
    }
}
