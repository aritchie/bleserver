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
        public IDevice Device { get; }
    }
}
// TODO: GATT reply status
