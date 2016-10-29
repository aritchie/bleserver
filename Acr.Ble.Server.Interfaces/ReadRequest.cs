using System;


namespace Acr.Ble.Server
{
    public class ReadRequest
    {
        public ReadRequest(IDevice device)
        {
            this.Device = device;
        }


        public int Offset { get; }
        public byte[] Value { get; set; }
        public GattStatus Status { get; set; } = GattStatus.Success;
        public IDevice Device { get; }
    }
}
