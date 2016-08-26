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
                                  CharacteristicPermissions permissions,
                                  byte[] value) : base(service, characteristicUuid, properties, value)
        {
            this.manager = manager;

            this.Native = new CBMutableCharacteristic(
                CBUUID.FromBytes(characteristicUuid.ToByteArray()),
                (CBCharacteristicProperties)(int)properties,
                NSData.FromArray(value),
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


        IObservable<byte[]> writeOb;
        public override IObservable<byte[]> WhenWriteReceived()
        {
            this.writeOb = this.writeOb ?? Observable.Create<byte[]>(ob =>
            {
                var handler = new EventHandler<CBATTRequestsEventArgs>((sender, args) =>
                {
                    // TODO: comes in as multiple requests... yay
                });
                this.manager.WriteRequestsReceived += handler;
                return () => this.manager.WriteRequestsReceived -= handler;;
            })
            .Publish()
            .RefCount();
            return this.writeOb;
        }


        IObservable<byte[]> readOb;
        public override IObservable<byte[]> WhenReadReceived()
        {
            this.readOb = this.readOb ?? Observable.Create<byte[]>(ob =>
            {
                var handler = new EventHandler<CBATTRequestEventArgs>((sender, args) =>
                {
                    if (args.Request.Characteristic.Equals(this.Native))
                    {
                        //args.Request.Central.MaximumUpdateValueLength
                        //args.Request.Offset = 0;
                        //args.Request.Value = null;
                    }
                });
                this.manager.ReadRequestReceived += handler;
                return () => this.manager.ReadRequestReceived -= handler;;
            })
            .Publish()
            .RefCount();
            return this.writeOb;
        }


        protected override IGattDescriptor CreateNative(Guid uuid, byte[] initialValue)
        {
            throw new NotImplementedException();
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
