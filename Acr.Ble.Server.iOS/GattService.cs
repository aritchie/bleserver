using System;
using CoreBluetooth;


namespace Acr.Ble.Server
{
    public class GattService : AbstractGattService
    {
        readonly CBPeripheralManager manager;
        public CBMutableService Native { get; }


        public GattService(CBPeripheralManager manager, IGattServer server, Guid serviceUuid, bool primary) : base(server, serviceUuid, primary)
        {
            this.manager = manager;
            this.Native = new CBMutableService(serviceUuid.ToCBUuid(), primary);
        }


        protected override IGattCharacteristic CreateNative(Guid uuid, CharacteristicProperties properties, GattPermissions permissions)
        {
            return new GattCharacteristic(this.manager, this, uuid, properties, permissions);
        }
    }
}
