using System;


namespace Acr.Ble.Server
{
    public class WriteRequest : IWriteRequest
    {
        public int Offset { get; internal set; }
        public bool IsReplyNeeded { get; internal set; }
        public byte[] Value { get; internal set; }
        public IDevice Device { get; internal set; }
    }
}