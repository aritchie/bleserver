﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using CoreBluetooth;
using Foundation;


namespace Acr.Ble.Server
{
    public class GattCharacteristic : AbstractGattCharacteristic
    {
        readonly CBPeripheralManager manager;
        readonly IDictionary<CBUUID, IDevice> subscribers;

        public CBMutableCharacteristic Native { get; }


        public GattCharacteristic(CBPeripheralManager manager,
                                  IGattService service,
                                  Guid characteristicUuid,
                                  CharacteristicProperties properties,
                                  GattPermissions permissions)
            : base(service, characteristicUuid, properties, permissions)
        {
            this.manager = manager;
            this.subscribers = new ConcurrentDictionary<CBUUID, IDevice>();

            this.Native = new CBMutableCharacteristic(
                characteristicUuid.ToCBUuid(),
                (CBCharacteristicProperties) (int) properties, // TODO
                new NSData(),
                (CBAttributePermissions) (int) permissions // TODO
            );
        }


        public override IReadOnlyList<IDevice> SubscribedDevices
        {
            get
            {
                lock (this.subscribers)
                {
                    return new ReadOnlyCollection<IDevice>(this.subscribers.Values.ToArray());
                }
            }
        }


        public override void Broadcast(byte[] value, params IDevice[] devices)
        {
            var data = NSData.FromArray(value);
            var centrals = devices
                .Cast<Device>()
                .Select(x => x.Central)
                .ToArray();

            this.manager.UpdateValue(data, this.Native, centrals);
        }


        public override void BroadcastToAll(byte[] value)
        {
            var data = NSData.FromArray(value);
            this.manager.UpdateValue(data, this.Native, this.Native.SubscribedCentrals);
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
                            // TODO: is reply needed?
                            var device = new Device(native.Central);
                            var request = new WriteRequest(device, native.Value.ToArray(), (int)native.Offset, false);
                            ob.OnNext(request);

                            var status = (CBATTError)Enum.Parse(typeof(CBATTError), request.Status.ToString());
                            this.manager.RespondToRequest(native, status);
                        }
                    }
                });
                this.manager.WriteRequestsReceived += handler;
                return () => this.manager.WriteRequestsReceived -= handler;
                ;
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
                        var device = new Device(args.Request.Central);
                        var request = new ReadRequest(device, (int)args.Request.Offset);
                        ob.OnNext(request);

                        var nativeStatus = (CBATTError) Enum.Parse(typeof(CBATTError), request.Status.ToString());
                        args.Request.Value = NSData.FromArray(request.Value);
                        this.manager.RespondToRequest(args.Request, nativeStatus);
                    }
                });
                this.manager.ReadRequestReceived += handler;
                return () => this.manager.ReadRequestReceived -= handler;
            })
            .Publish()
            .RefCount();

            return this.readOb;
        }


        protected override IGattDescriptor CreateNative(Guid uuid, byte[] value)
        {
            return new GattDescriptor(this, uuid, value);
        }


        protected virtual EventHandler<CBPeripheralManagerSubscriptionEventArgs> CreateSubHandler(IObserver<DeviceSubscriptionEvent> ob, bool subscribing)
        {
            return (sender, args) =>
            {
                // on has a subcription or has none
                if (subscribing)
                {
                    var device = this.GetOrAdd(args.Central);
                    ob.OnNext(new DeviceSubscriptionEvent(device, true));
                }
                else
                {
                    var device = this.Remove(args.Central);
                    if (device != null)
                        ob.OnNext(new DeviceSubscriptionEvent(device, false));
                }
            };
        }


        IDevice GetOrAdd(CBCentral central)
        {
            lock (this.subscribers)
            {
                if (this.subscribers.ContainsKey(central.UUID))
                    return this.subscribers[central.UUID];

                var device = new Device(central);
                this.subscribers.Add(central.UUID, device);
                return device;
            }
        }


        IDevice Remove(CBCentral central)
        {
            lock (this.subscribers)
            {
                if (this.subscribers.ContainsKey(central.UUID))
                {
                    var device = this.subscribers[central.UUID];
                    this.subscribers.Remove(central.UUID);
                    return device;
                }
                return null;
            }
        }
    }
}
