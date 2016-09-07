using System;


namespace Acr.Ble.Server
{
    public interface IGattServerFactory
    {
        IGattServer CreateInstance();
    }
}
