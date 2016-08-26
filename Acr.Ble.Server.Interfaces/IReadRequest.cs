using System;


namespace Acr.Ble.Server
{
    public interface IReadRequest
    {
        // TODO: device?
        void Reply(byte[] value);
    }
}
