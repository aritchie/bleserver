using System;
using CoreBluetooth;


namespace Acr.Ble.Server
{
    public class WriteRequest : IWriteRequest
    {
        readonly CBATTRequest request;


        public WriteRequest(CBATTRequest request)
        {
            this.request = request;
        }


        public IDevice Device { get; }
        public int Offset => (int) this.request.Offset;
        public bool IsReplyNeeded { get; } = true;
        public byte[] Value => this.request.Value.ToArray();
    }
}
