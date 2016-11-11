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


        public static GattProperty ToNative(this CharacteristicProperties properties)
        {
            if (properties.HasFlag(CharacteristicProperties.NotifyEncryptionRequired))
                throw new ArgumentException("NotifyEncryptionRequired not supported on Android");

            if (properties.HasFlag(CharacteristicProperties.IndicateEncryptionRequired))
                throw new ArgumentException("IndicateEncryptionRequired not supported on Android");

            var value = properties
                .ToString()
                .Replace(
                    CharacteristicProperties.WriteWithoutResponse.ToString(),
                    GattProperty.WriteNoResponse.ToString()
                )
                .Replace(
                    CharacteristicProperties.AuthenticatedSignedWrites.ToString(),
                    GattProperty.SignedWrite.ToString()
                )
                .Replace(
                    CharacteristicProperties.ExtendedProperties.ToString(),
                    GattProperty.ExtendedProps.ToString()
                );

            return (GattProperty)Enum.Parse(typeof(GattProperty), value);
        }


        public static GattPermission ToNative(this GattPermissions permissions)
        {
            return (GattPermission) Enum.Parse(typeof(GattPermission), permissions.ToString());
        }


        public static DroidGattStatus ToNative(this GattStatus status)
        {
            return (DroidGattStatus) Enum.Parse(typeof(DroidGattStatus), status.ToString());
        }
    }
}