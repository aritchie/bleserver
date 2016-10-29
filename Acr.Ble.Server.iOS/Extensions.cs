using System;
using CoreBluetooth;


namespace Acr.Ble.Server
{
    public static class Extensions
    {
        public static CBUUID ToCBUuid(this Guid guid)
        {
            return CBUUID.FromBytes(guid.ToByteArray());
        }


        public static Guid FromCBUuid(this CBUUID uuid)
        {
            var bytes = uuid.Data.ToArray();
            return new Guid(bytes);
        }
    }
}
