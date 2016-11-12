using System;
using CoreBluetooth;


namespace Acr.Ble.Server
{
    public class Device : IDevice
    {
        public Device(CBCentral central)
        {
            this.Central = central;
            this.Uuid = new Guid(central.Identifier.ToString());
        }


        public Guid Uuid { get; }
        public CBCentral Central { get; }
    }
}
