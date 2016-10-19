using System;
using Acr.Ble.Server.Internals;
using Android.Bluetooth;


namespace Acr.Ble.Server
{
    public class GattCharacteristic : AbstractGattCharacteristic
    {
        public BluetoothGattCharacteristic Native { get; }
        readonly GattServerCallbacks callbacks;

        public GattCharacteristic(GattServerCallbacks callbacks,
                                  IGattService service,
                                  Guid uuid,
                                  CharacteristicProperties properties,
                                  CharacteristicPermissions permissions) : base(service, uuid, properties)
        {
            this.callbacks = callbacks;

            this.Native = new BluetoothGattCharacteristic(
                uuid.ToUuid(),
                (GattProperty) (int) properties,
                GattPermission.Read | GattPermission.Write
            );
        }


        public override void Broadcast(byte[] value)
        {
            throw new NotImplementedException();
        }


        public override IObservable<bool> WhenSubscriptionStateChanged()
        {
            throw new NotImplementedException();
        }


        public override IObservable<IWriteRequest> WhenWriteReceived()
        {
            throw new NotImplementedException();
        }


        public override IObservable<IReadRequest> WhenReadReceived()
        {
            throw new NotImplementedException();
        }


        protected override IGattDescriptor CreateNative(Guid uuid)
        {
            throw new NotImplementedException();
        }
    }
}
