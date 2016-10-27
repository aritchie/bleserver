using System;
using System.Reactive.Linq;
using CoreBluetooth;
using Foundation;


namespace Acr.Ble.Server
{
    public class GattDescriptor : AbstractGattDescriptor
    {
        readonly CBPeripheralManager manager;
        public CBMutableDescriptor Native { get; }


        public GattDescriptor(IGattCharacteristic characteristic, Guid descriptorUuid, CBPeripheralManager manager) : base(characteristic, descriptorUuid)
        {
            this.Native = new CBMutableDescriptor(descriptorUuid.ToCBUuid(), new NSData());
            this.manager = manager;
        }


        IObservable<ReadRequest> readOb;
        public override IObservable<ReadRequest> WhenReadReceived()
        {
            this.readOb = this.readOb ?? Observable.Create<ReadRequest>(ob =>
            {
                return () => { };
            })
            .Publish()
            .RefCount();

            return this.readOb;
        }


        IObservable<WriteRequest> writeOb;
        public override IObservable<WriteRequest> WhenWriteReceived()
        {
            this.writeOb = this.writeOb ?? Observable.Create<WriteRequest>(ob =>
            {

                return () => { };
            })
            .Publish()
            .RefCount();

            return this.writeOb;
        }
    }
}