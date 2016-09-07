using System;
using Acr.Ble.Server.Internals;
using Android.Bluetooth;
using Java.Util;


namespace Acr.Ble.Server
{
    public class GattService : AbstractGattService
    {
        public BluetoothGattService Native { get; }
        readonly GattServerCallbacks callbacks;


        public GattService(GattServerCallbacks callbacks, IGattServer server, Guid uuid, bool primary) : base(server, uuid, primary)
        {
            this.callbacks = callbacks;
            this.Native = new BluetoothGattService(
                UUID.FromString(uuid.ToString()),
                primary ? GattServiceType.Primary : GattServiceType.Secondary
            );
        }


        protected override IGattCharacteristic CreateCharacteristic(Guid uuid, CharacteristicProperties properties, CharacteristicPermissions permissions)
        {
            return new GattCharacteristic(this.callbacks, this, uuid, properties, permissions);
        }
    }
}
