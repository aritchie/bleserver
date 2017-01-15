using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;


namespace Acr.Ble.Server
{
    public interface IUwpGattDescriptor : IGattDescriptor
    {
        Task Init(GattLocalCharacteristic native);
    }
}
