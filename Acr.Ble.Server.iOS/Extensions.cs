using System;
using System.Collections.Generic;
using CoreBluetooth;


namespace Acr.Ble.Server
{
    public static class Extensions
    {
        public static CBUUID ToCBUuid(this Guid guid)
        {
            //return CBUUID.FromBytes(guid.ToByteArray());
            return CBUUID.FromString(guid.ToString());
        }


        //public static Guid FromCBUuid(this CBUUID uuid)
        //{
            //var bytes = uuid.Data.ToArray();
            //return new Guid(bytes);
        //}


        public static CBCharacteristicProperties ToNative(this CharacteristicProperties properties)
        {
            var native = ConvertFlags<CBCharacteristicProperties>(properties);
            return native;
        }


        static T ConvertFlags<T>(Enum flags1)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(typeof(T) + " is not an enum!");

            var values = new List<string>();
            var allValues = Enum.GetValues(flags1.GetType());
            foreach (var all in allValues)
            {
                if (flags1.HasFlag((Enum)all))
                    values.Add(all.ToString());
            }
            var raw = String.Join(",", values.ToArray());
            var result = (T)Enum.Parse(typeof(T), raw);

            return result;
        }
    }
}
