using System;
using Acr.Ble.Server.Internals;
using Android.Bluetooth;


namespace Acr.Ble.Server
{
    public class GattDescriptor : AbstractGattDescriptor
    {
        readonly GattContext context;


        public GattDescriptor(GattContext context,
                              IGattCharacteristic characteristic,
                              Guid descriptorUuid)
            : base(characteristic, descriptorUuid)
        {
            this.context = context;

            this.Native = new BluetoothGattDescriptor(
                descriptorUuid.ToUuid(),
                GattDescriptorPermission.Read // TODO
            );
        }


        public BluetoothGattDescriptor Native { get; }


        public override IObservable<ReadRequest> WhenReadReceived()
        {
            throw new NotImplementedException();
        }


        public override IObservable<WriteRequest> WhenWriteReceived()
        {
            throw new NotImplementedException();
        }
    }
}