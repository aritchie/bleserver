using System;
using CoreBluetooth;


namespace Acr.Ble.Server
{
    public interface IIosGattDescriptor : IGattDescriptor
    {
        CBMutableDescriptor Native { get; }
    }
}
