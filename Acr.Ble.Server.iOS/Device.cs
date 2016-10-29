using System;
using CoreBluetooth;


namespace Acr.Ble.Server
{
    public class Device : IDevice
    {
        public Device(CBCentral central)
        {
            this.Central = central;
            this.Uuid = central.UUID.FromCBUuid();
        }


        public Guid Uuid { get; }
        public CBCentral Central { get; }
    }
}
