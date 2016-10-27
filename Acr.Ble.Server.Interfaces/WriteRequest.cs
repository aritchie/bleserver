using System;


namespace Acr.Ble.Server
{
    public class WriteRequest
    {
        public WriteRequest(IDevice device, int offset, bool isReplyNeeded)
        {
            this.Device = device;
            this.Offset = offset;
            this.IsReplyNeeded = isReplyNeeded;
        }


        public IDevice Device { get; }
        public int Offset { get; }
        public bool IsReplyNeeded { get; }
        public byte[] Value { get; set; }
    }
}
// TODO: GATT reply status