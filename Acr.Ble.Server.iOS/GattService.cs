using System;
using CoreBluetooth;


namespace Acr.Ble.Server
{
    public class GattService : AbstractGattService
    {
        public CBMutableService Native { get; }
        //        //public override IGattCharacteristic CreateCharacteristic(string uuid, CharacteristicProperties properties)
        //        //{
        //        //    var characteristic = new GattCharacteristic();
        //        //    return characteristic;
        //        //}


        //        //public override void AddCharacteristic(IGattCharacteristic characteristic)
        //        //{
        //        //    this.InternalCharacteristics.Add(characteristic);
        //        //    throw new NotImplementedException();
        //        //}


        //        //public CBMutableService ToNative()
        //        //{
        //        //    var uuid = CBUUID.FromString(this.Identifier);
        //        //    var native = new CBMutableService(uuid, this.IsPrimary);
        //        //    // TODO: add any charactertistics
        //        //    return native;
        //        //}


        public GattService(CBPeripheralManager manager, IGattServer server, Guid serviceUuid, bool primary) : base(server, serviceUuid, primary)
        {
            this.Native = new CBMutableService(CBUUID.FromString(serviceUuid.ToString()), primary);
        }


        protected override IGattCharacteristic CreateCharacteristic(Guid uuid, CharacteristicProperties properties, CharacteristicPermissions permissions, byte[] initialValue)
        {
            return null;
        }
    }
}
