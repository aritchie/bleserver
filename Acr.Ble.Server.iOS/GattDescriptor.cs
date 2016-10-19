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


        IObservable<IReadRequest> readOb;
        public override IObservable<IReadRequest> WhenReadReceived()
        {
            this.readOb = this.readOb ?? Observable.Create<IReadRequest>(ob =>
            {
                return () => { };
            })
            .Publish()
            .RefCount();

            return this.readOb;
        }


        IObservable<IWriteRequest> writeOb;
        public override IObservable<IWriteRequest> WhenWriteReceived()
        {
            this.writeOb = this.writeOb ?? Observable.Create<IWriteRequest>(ob =>
            {

                return () => { };
            })
            .Publish()
            .RefCount();

            return this.writeOb;
        }
    }
}