using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Acr.Ble.Server
{
    public abstract class AbstractGattCharacteristic : IGattCharacteristic
    {
        protected AbstractGattCharacteristic(IGattService service, Guid characteristicUuid, CharacteristicProperties properties, CharacteristicPermissions permissions)
        {
            this.Service = service;
            this.Uuid = characteristicUuid;
            this.Properties = properties;
            this.Permissions = permissions;

            this.InternalSubscribedDevices = new List<IDevice>();
            this.SubscribedDevices = new ReadOnlyCollection<IDevice>(this.InternalSubscribedDevices);

            this.InternalDescriptors = new List<IGattDescriptor>();
            this.Descriptors = new ReadOnlyCollection<IGattDescriptor>(this.InternalDescriptors);
        }


        protected IList<IDevice> InternalSubscribedDevices { get; }
        protected IList<IGattDescriptor> InternalDescriptors { get; }


        public IGattService Service { get; }
        public Guid Uuid { get; }
        public CharacteristicProperties Properties { get; }
        public CharacteristicPermissions Permissions { get; }
        public IReadOnlyList<IGattDescriptor> Descriptors { get; }
        public IReadOnlyList<IDevice> SubscribedDevices { get; }


        public IGattDescriptor AddDescriptor(Guid uuid)
        {
            var native = this.CreateNative(uuid);
            this.InternalDescriptors.Add(native);
            return native;
        }


        public abstract void Broadcast(byte[] value, params IDevice[] devices);
        public abstract void BroadcastToAll(byte[] value);
        public abstract IObservable<IWriteRequest> WhenWriteReceived();
        public abstract IObservable<IReadRequest> WhenReadReceived();
        public abstract IObservable<DeviceSubscriptionEvent> WhenDeviceSubscriptionChanged();

        protected abstract IGattDescriptor CreateNative(Guid uuid);
    }
}
