using System;
using System.Reactive.Linq;
using CoreBluetooth;
using Foundation;


namespace Acr.Ble.Server
{
    public class GattCharacteristic : AbstractGattCharacteristic
    {
        readonly CBPeripheralManager manager;
        public CBMutableCharacteristic Native { get; }


        public GattCharacteristic(CBPeripheralManager manager,
                                  IGattService service,
                                  Guid characteristicUuid,
                                  CharacteristicProperties properties,
                                  CharacteristicPermissions permissions) : base(service, characteristicUuid, properties)
        {
            this.manager = manager;

            this.Native = new CBMutableCharacteristic(
                CBUUID.FromBytes(characteristicUuid.ToByteArray()),
                (CBCharacteristicProperties)(int)properties,
                new NSData(),
                (CBAttributePermissions)(int)permissions
            );
        }


        public override void Broadcast(byte[] value)
        {
            var data = NSData.FromArray(value);
            this.manager.UpdateValue(data, this.Native, this.Native.SubscribedCentrals);
        }


        IObservable<bool> subOb;
        public override IObservable<bool> WhenSubscriptionStateChanged()
        {
            this.subOb = this.subOb ?? Observable.Create<bool>(ob =>
            {
                var sub = this.CreateSubHandler(ob, true);
                var unsub = this.CreateSubHandler(ob, false);

                this.manager.CharacteristicSubscribed += sub;
                this.manager.CharacteristicUnsubscribed += unsub;

                return () =>
                {
                    this.manager.CharacteristicSubscribed -= sub;
                    this.manager.CharacteristicUnsubscribed -= unsub;
                };
            })
            .Publish()
            .RefCount();

            return this.subOb;
        }


        IObservable<IWriteRequest> writeOb;
        public override IObservable<IWriteRequest> WhenWriteReceived()
        {
            this.writeOb = this.writeOb ?? Observable.Create<IWriteRequest>(ob =>
            {
                var handler = new EventHandler<CBATTRequestsEventArgs>((sender, args) =>
                {
                    foreach (var native in args.Requests)
                    {
                        var request = new WriteRequest(native);
                        ob.OnNext(request);
                    }
                });
                this.manager.WriteRequestsReceived += handler;
                return () => this.manager.WriteRequestsReceived -= handler;;
            })
            .Publish()
            .RefCount();

            return this.writeOb;
        }


        IObservable<IReadRequest> readOb;
        public override IObservable<IReadRequest> WhenReadReceived()
        {
            this.readOb = this.readOb ?? Observable.Create<IReadRequest>(ob =>
            {
                var handler = new EventHandler<CBATTRequestEventArgs>((sender, args) =>
                {
                    if (args.Request.Characteristic.Equals(this.Native))
                    {
                        var request = new ReadRequest(this.manager, args.Request);
                        ob.OnNext(request);
                    }
                });
                this.manager.ReadRequestReceived += handler;
                return () => this.manager.ReadRequestReceived -= handler;
            })
            .Publish()
            .RefCount();

            return this.readOb;
        }


        protected override IGattDescriptor CreateNative(Guid uuid)
        {
        }


        protected override void RemoveNative(IGattDescriptor descriptor)
        {
            throw new NotImplementedException();
        }


        protected virtual EventHandler<CBPeripheralManagerSubscriptionEventArgs> CreateSubHandler(IObserver<bool> ob, bool subscribing)
        {
            return (sender, args) =>
            {
                ob.OnNext(subscribing);
            };
        }
    }
}
