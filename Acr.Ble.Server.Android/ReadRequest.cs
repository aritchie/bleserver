using System;


namespace Acr.Ble.Server
{
    public class ReadRequest : IReadRequest
    {
        public IDevice Device { get; internal set; }
        public int Offset { get; internal set; }
        public byte[] Value { get; set; }
    }
}