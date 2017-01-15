using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;


namespace Acr.Ble.Server
{
    public interface IUwpGattCharacteristic : IGattCharacteristic
    {
        Task Init(GattLocalService gatt);
    }
}
