using System;
using CoreBluetooth;


namespace Acr.Ble.Server
{
    public interface IIosGattService : IGattService
    {
        CBMutableService Native { get; }
    }
}
