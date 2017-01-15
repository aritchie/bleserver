using System;
using Windows.Devices.Bluetooth.GenericAttributeProfile;


namespace Acr.Ble.Server
{
    public class UwpDevice : IDevice
    {
        public UwpDevice(GattSession session)
        {
            this.Uuid = new Guid(session.DeviceId.Id);
        }


        public Guid Uuid { get; }
        public object Context { get; set; }
    }
}
