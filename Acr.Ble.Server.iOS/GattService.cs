using System;
using System.Collections.Generic;
using System.Linq;
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


        protected override IGattCharacteristic CreateNative(Guid uuid, CharacteristicProperties properties, CharacteristicPermissions permissions)
        {
            var characteristic = new GattCharacteristic(this.manager, this, uuid, properties, permissions);
            var list = new List<CBCharacteristic>(this.Native.Characteristics.ToList());
            list.Add(characteristic.Native);
            this.Native.Characteristics = list.ToArray();

            return characteristic;
        }
    }
}
