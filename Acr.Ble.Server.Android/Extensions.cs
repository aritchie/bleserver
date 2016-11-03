using System;
using Android.Bluetooth;
using Java.Util;
using DroidGattStatus = Android.Bluetooth.GattStatus;


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


        public static GattPermission ToNative(this GattPermissions permissions)
        {
            return (GattPermission)Enum.Parse(typeof(GattPermission), permissions.ToString());
        }


        public static DroidGattStatus ToNative(this GattStatus status)
        {
            return (DroidGattStatus) Enum.Parse(typeof(DroidGattStatus), status.ToString());
        }
    }
}