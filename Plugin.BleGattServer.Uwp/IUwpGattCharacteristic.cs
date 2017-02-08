using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;


namespace Plugin.BleGattServer
{
    public interface IUwpGattCharacteristic : IGattCharacteristic
    {
        Task Init(GattLocalService gatt);
    }
}
