using System;


namespace Acr.Ble.Server
{
    public interface IDevice
    {
        //Guid Address
        //void Disconnect();
        DateTimeOffset DateConnected { get; }
    }
}
