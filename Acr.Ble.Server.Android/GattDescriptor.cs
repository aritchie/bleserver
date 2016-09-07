using System;
using Acr.Ble.Server.Internals;
using Android.Bluetooth;


namespace Acr.Ble.Server
{
    public class GattDescriptor : AbstractGattDescriptor
    {
        readonly GattServerCallbacks callbacks;


        public GattDescriptor(GattServerCallbacks callbacks,
                             IGattCharacteristic characteristic,
                             Guid descriptorUuid)
            : base(characteristic, descriptorUuid)
        {
            this.callbacks = callbacks;

            this.Native = new BluetoothGattDescriptor(
                descriptorUuid.ToUuid(),
                GattDescriptorPermission.Read // TODO
            );
        }


        public BluetoothGattDescriptor Native { get; }


        public override IObservable<object> WhenReadReceived()
        {
            throw new NotImplementedException();
        }


        public override IObservable<byte[]> WhenWriteReceived()
        {
            throw new NotImplementedException();
        }
    }
}