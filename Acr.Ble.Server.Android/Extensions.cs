using System;
using Java.Util;


namespace Acr.Ble.Server
{
    public static class Extensions
    {
        public static UUID ToUuid(this Guid guid)
        {
            return UUID.FromString(guid.ToString());
        }


        public static Guid ToGuid(this UUID uuid)
        {
            return new Guid(uuid.ToString());
        }
    }
}