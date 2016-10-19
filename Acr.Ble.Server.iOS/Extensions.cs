using System;
using CoreBluetooth;


namespace Acr.Ble.Server
{
    public static class Extensions
    {
        //public CBMutableCharacteristic ToNative()
        //{
        //    var uuid = CBUUID.FromString(this.Identifier);
        //    var cbc = new CBMutableCharacteristic(uuid, CBCharacteristicProperties.AuthenticatedSignedWrites, null, CBAttributePermissions.ReadEncryptionRequired);
        //    return cbc;
        //}


        public static CBUUID ToCBUuid(this Guid guid)
        {
            return CBUUID.FromBytes(guid.ToByteArray());
        }
    }
}
