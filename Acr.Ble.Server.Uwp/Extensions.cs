using System;
using Windows.Devices.Bluetooth.GenericAttributeProfile;


namespace Acr.Ble.Server
{
    public static class Extensions
    {
        public static GattUuid ToNative(this Guid guid)
        {
            return GattUuid.FromUuid(guid);
        }
    }
}
