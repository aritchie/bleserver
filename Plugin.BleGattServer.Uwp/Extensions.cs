using System;
using Windows.Devices.Bluetooth.GenericAttributeProfile;


namespace Plugin.BleGattServer
{
    public static class Extensions
    {
        public static GattUuid ToNative(this Guid guid)
        {
            return GattUuid.FromUuid(guid);
        }
    }
}
