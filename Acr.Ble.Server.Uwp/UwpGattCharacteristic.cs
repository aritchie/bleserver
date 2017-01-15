using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;


namespace Acr.Ble.Server
{
    public class UwpGattCharacteristic : AbstractGattCharacteristic, IUwpGattCharacteristic
    {
        public UwpGattCharacteristic(IGattService service, Guid characteristicUuid, CharacteristicProperties properties, GattPermissions permissions) : base(service, characteristicUuid, properties, permissions)
        {
        }


        public override IObservable<DeviceSubscriptionEvent> WhenDeviceSubscriptionChanged()
        {
            throw new NotImplementedException();
        }


        protected override IGattDescriptor CreateNative(Guid uuid, byte[] value)
        {
            throw new NotImplementedException();
        }


        public override IReadOnlyList<IDevice> SubscribedDevices { get; }
        public override void Broadcast(byte[] value, params IDevice[] devices)
        {
            throw new NotImplementedException();
        }


        public override IObservable<WriteRequest> WhenWriteReceived()
        {
            throw new NotImplementedException();
        }


        public override IObservable<ReadRequest> WhenReadReceived()
        {
            throw new NotImplementedException();
        }


        public override IObservable<CharacteristicBroadcast> BroadcastObserve(byte[] value, params IDevice[] devices)
        {
            throw new NotImplementedException();
        }


        public async Task Init(GattLocalService gatt)
        {
            var ch = await gatt.CreateCharacteristicAsync(
                GattUuid.FromUuid(this.Uuid),
                new GattLocalCharacteristicParameters
                {

                }
            );
            foreach (var descriptor in this.Descriptors.OfType<IUwpGattDescriptor>())
            {
                await descriptor.Init(ch.Characteristic);
            }
        }
    }
}
