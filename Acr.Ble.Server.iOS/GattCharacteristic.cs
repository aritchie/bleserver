using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Linq;
using CoreBluetooth;
using Foundation;


namespace Acr.Ble.Server
{
    public class GattCharacteristic : AbstractGattCharacteristic
    {
        readonly ConcurrentDictionary<CBUUID, IDevice> devices;
        readonly CBPeripheralManager manager;

        public CBMutableCharacteristic Native { get; }


        public GattCharacteristic(CBPeripheralManager manager,
                                  IGattService service,
                                  Guid characteristicUuid,
                                  CharacteristicProperties properties,
                                  CharacteristicPermissions permissions) : base(service, characteristicUuid, properties, permissions)
        {
            this.manager = manager;
            this.devices = new ConcurrentDictionary<CBUUID, IDevice>();

            this.Native = new CBMutableCharacteristic(
                characteristicUuid.ToCBUuid(),
                (CBCharacteristicProperties)(int)properties,
                new NSData(),
                (CBAttributePermissions)(int)permissions
            );
        }


        public override void Broadcast(byte[] value, params IDevice[] devices)
        {
            var data = NSData.FromArray(value);
            this.manager.UpdateValue(data, this.Native, this.Native.SubscribedCentrals);
        }


        public override void BroadcastToAll(byte[] value)
        {
            throw new NotImplementedException();
        }


        IObservable<DeviceSubscriptionEvent> subOb;
        public override IObservable<DeviceSubscriptionEvent> WhenDeviceSubscriptionChanged()
        {
            this.subOb = this.subOb ?? Observable.Create<DeviceSubscriptionEvent>(ob =>
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


        IObservable<WriteRequest> writeOb;
        public override IObservable<WriteRequest> WhenWriteReceived()
        {
            this.writeOb = this.writeOb ?? Observable.Create<WriteRequest>(ob =>
            {
                var handler = new EventHandler<CBATTRequestsEventArgs>((sender, args) =>
                {
                    foreach (var native in args.Requests)
                    {
                        if (native.Characteristic.Equals(this.Native))
                        {
                            var request = new WriteRequest(this.manager, native);
                            ob.OnNext(request);
                        }
                    }
                });
                this.manager.WriteRequestsReceived += handler;
                return () => this.manager.WriteRequestsReceived -= handler;;
            })
            .Publish()
            .RefCount();

            return this.writeOb;
        }


        IObservable<ReadRequest> readOb;
        public override IObservable<ReadRequest> WhenReadReceived()
        {
            this.readOb = this.readOb ?? Observable.Create<ReadRequest>(ob =>
            {
                var handler = new EventHandler<CBATTRequestEventArgs>((sender, args) =>
                {
                    if (args.Request.Characteristic.Equals(this.Native))
                    {
                        var request = new ReadRequest(null);
                        ob.OnNext(request);
                        if (request.Value == null)
                            return;

                        args.Request.Value = NSData.FromArray(request.Value);
                        this.manager.RespondToRequest(args.Request, CBATTError.Success);
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
            var descriptor = new GattDescriptor(this, uuid, this.manager);
            var list = new List<CBDescriptor>();
            if (this.Native.Descriptors != null)
                list.AddRange(this.Native.Descriptors);

            list.Add(descriptor.Native);
            this.Native.Descriptors = list.ToArray();
            return descriptor;
        }


        protected virtual EventHandler<CBPeripheralManagerSubscriptionEventArgs> CreateSubHandler(IObserver<DeviceSubscriptionEvent> ob, bool subscribing)
        {
            return (sender, args) =>
            {
                // on has a subcription or has none
                //ob.OnNext(subscribing);
                ob.OnNext(new DeviceSubscriptionEvent(null, subscribing));
            };
        }
    }
}
