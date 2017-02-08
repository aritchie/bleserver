using System;
using CoreBluetooth;


namespace Plugin.BleGattServer
{
    public interface IIosGattDescriptor : IGattDescriptor
    {
        CBMutableDescriptor Native { get; }
    }
}
