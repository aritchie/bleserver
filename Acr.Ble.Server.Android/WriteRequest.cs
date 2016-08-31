using System;


namespace Acr.Ble.Server
{
    public class WriteRequest : IWriteRequest
    {
        public int Offset { get; }
        public bool IsReplyNeeded { get; }
        public byte[] Value { get; }
    }
}