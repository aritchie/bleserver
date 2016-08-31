using System;


namespace Acr.Ble.Server
{
    public class ReadRequest : IReadRequest
    {
        public int Offset { get; }
        public byte[] Value { get; set; }
    }
}