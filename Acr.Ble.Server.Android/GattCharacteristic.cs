using System;
using System.Reactive.Linq;
using Acr.Ble.Server.Internals;
using Android.Bluetooth;


namespace Acr.Ble.Server
{
    public class GattCharacteristic : AbstractGattCharacteristic
    {
        public BluetoothGattCharacteristic Native { get; }
        readonly GattContext context;


        public GattCharacteristic(GattContext context,
                                  IGattService service,
                                  Guid uuid,
                                  CharacteristicProperties properties,
                                  CharacteristicPermissions permissions) : base(service, uuid, properties)
        {
            this.context = context;
            this.Native = new BluetoothGattCharacteristic(
                uuid.ToUuid(),
                (GattProperty) (int) properties,
                GattPermission.Read | GattPermission.Write
            );
        }


        public override void Broadcast(byte[] value)
        {
            this.Native.SetValue(value);

            // TODO: request response true/false
            this.context.Server.NotifyCharacteristicChanged(null, this.Native, true);
        }


        public override IObservable<bool> WhenSubscriptionStateChanged()
        {
            return Observable.Create<bool>(ob =>
            {
                return () => { };
            });
        }


        public override IObservable<IWriteRequest> WhenWriteReceived()
        {
            return Observable.Create<IWriteRequest>(ob =>
            {
                return () => { };
            });
        }


        public override IObservable<IReadRequest> WhenReadReceived()
        {
            return Observable.Create<IReadRequest>(ob =>
            {
                return () => { };
            });
        }


        protected override IGattDescriptor CreateNative(Guid uuid)
        {
            return new GattDescriptor(this.context, this, uuid);
        }
    }
}
