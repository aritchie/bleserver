using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;


namespace Acr.Ble.Server
{
    public class UwpGattDescriptor : AbstractGattDescriptor, IUwpGattCharacteristic
    {
        public UwpGattDescriptor(IGattCharacteristic characteristic, Guid descriptorUuid, byte[] value) : base(characteristic, descriptorUuid, value)
        {
        }

        public IGattService Service { get; }
        public CharacteristicProperties Properties { get; }
        public GattPermissions Permissions { get; }
        public IGattDescriptor AddDescriptor(Guid uuid, byte[] value)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IGattDescriptor> Descriptors { get; }
        public IObservable<CharacteristicBroadcast> BroadcastObserve(byte[] value, params IDevice[] devices)
        {
            throw new NotImplementedException();
        }


        public void Broadcast(byte[] value, params IDevice[] device)
        {
            throw new NotImplementedException();
        }


        public IObservable<WriteRequest> WhenWriteReceived()
        {
            throw new NotImplementedException();
        }


        public IObservable<ReadRequest> WhenReadReceived()
        {
            throw new NotImplementedException();
        }


        public IObservable<DeviceSubscriptionEvent> WhenDeviceSubscriptionChanged()
        {
            throw new NotImplementedException();
        }


        public IReadOnlyList<IDevice> SubscribedDevices { get; }
        public Task Init(GattLocalService gatt)
        {
            throw new NotImplementedException();
        }
    }
}
