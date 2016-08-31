using System;


namespace Acr.Ble.Server
{
    public interface IReadRequest
    {
        int Offset { get; }
        byte[] Value { get; set; }
    }
}
