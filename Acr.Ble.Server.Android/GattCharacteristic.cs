using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Linq;
using Acr.Ble.Server.Internals;
using Android.Bluetooth;
using Java.Util;
using Observable = System.Reactive.Linq.Observable;


namespace Acr.Ble.Server
{
    public class GattCharacteristic : AbstractGattCharacteristic
    {
        public static readonly Guid NotifyDescriptorId = new Guid("00002902-0000-1000-8000-00805f9b34fb");
        public static readonly UUID NotifyDescriptorUuid = Java.Util.UUID.FromString("00002902-0000-1000-8000-00805f9b34fb");
        public static readonly byte[] NotifyEnabledBytes = BluetoothGattDescriptor.EnableNotificationValue.ToArray();

        public BluetoothGattCharacteristic Native { get; }
        readonly GattContext context;
        readonly ConcurrentDictionary<string, IDevice> subscribers;
        readonly BluetoothGattDescriptor subscribeDescriptor;


        public GattCharacteristic(GattContext context,
                                  IGattService service,
                                  Guid uuid,
                                  CharacteristicProperties properties,
                                  GattPermissions permissions) : base(service, uuid, properties, permissions)
        {
            this.context = context;
            this.subscribers = new ConcurrentDictionary<string, IDevice>();

            this.Native = new BluetoothGattCharacteristic(
                uuid.ToUuid(),
                (GattProperty) (int) properties,
                permissions.ToNative()
            );
            this.subscribeDescriptor = new BluetoothGattDescriptor(
                NotifyDescriptorId.ToUuid(),
                GattDescriptorPermission.Write
            );
        }


        public override void Broadcast(byte[] value, params IDevice[] devices)
        {
            this.Native.SetValue(value);
            devices
                .Cast<Device>()
                .Select(x => x.Native)
                .ToList()
                .ForEach(x => this.context.Server.NotifyCharacteristicChanged(x, this.Native, false));
        }


        public override void BroadcastToAll(byte[] value)
        {
            this.Native.SetValue(value);
            foreach (var dev in this.context.Server.ConnectedDevices)
                this.context.Server.NotifyCharacteristicChanged(dev, this.Native, false);
        }


        IObservable<DeviceSubscriptionEvent> subscriptionOb;
        public override IObservable<DeviceSubscriptionEvent> WhenDeviceSubscriptionChanged()
        {
            this.subscriptionOb = this.subscriptionOb ?? Observable.Create<DeviceSubscriptionEvent>(ob =>
            {
                var handler = new EventHandler<DescriptorWriteEventArgs>((sender, args) =>
                {
                    if (args.Descriptor.Uuid.Equals(NotifyDescriptorUuid))
                    {
                        if (args.Value.Equals(NotifyEnabledBytes))
                        {
                            var device = this.subscribers.GetOrAdd(args.Device.Address, address => new Device(args.Device));
                            ob.OnNext(new DeviceSubscriptionEvent(device, true));
                        }
                        else
                        {
                            IDevice device;
                            if (this.subscribers.TryRemove(args.Device.Address, out device))
                                ob.OnNext(new DeviceSubscriptionEvent(device, false));
                        }
                    }
                });

                this.context.Callbacks.DescriptorWrite += handler;

                return () => this.context.Callbacks.DescriptorWrite -= handler;
            })
            .Publish()
            .RefCount();

            return this.subscriptionOb;
        }


        public override IObservable<WriteRequest> WhenWriteReceived()
        {
            return Observable.Create<WriteRequest>(ob =>
            {
                var handler = new EventHandler<CharacteristicWriteEventArgs>((sender, args) =>
                {
                    if (!args.Characteristic.Equals(this.Native))
                        return;

                    var device = new Device(args.Device);
                    var request = new WriteRequest(device, args.Value, args.Offset, args.ResponseNeeded);
                    ob.OnNext(request);

                    if (request.IsReplyNeeded)
                    {
                        this.context.Server.SendResponse
                        (
                            args.Device,
                            args.RequestId,
                            request.Status.ToNative(),
                            request.Offset,
                            request.Value
                        );
                    }
                });
                this.context.Callbacks.CharacteristicWrite += handler;

                return () => this.context.Callbacks.CharacteristicWrite -= handler;
            });
        }


        public override IObservable<ReadRequest> WhenReadReceived()
        {
            return Observable.Create<ReadRequest>(ob =>
            {
                var handler = new EventHandler<CharacteristicReadEventArgs>((sender, args) =>
                {
                    if (!args.Characteristic.Equals(this.Native))
                        return;

                    var device = new Device(args.Device);
                    var request = new ReadRequest(device, args.Offset);
                    ob.OnNext(request);

                    this.context.Server.SendResponse(
                        args.Device,
                        args.RequestId,
                        request.Status.ToNative(),
                        args.Offset,
                        request.Value
                    );
                });
                this.context.Callbacks.CharacteristicRead += handler;
                return () => this.context.Callbacks.CharacteristicRead -= handler;
            });
        }


        protected override IGattDescriptor CreateNative(Guid uuid, byte[] value)
        {
            return new GattDescriptor(this, uuid, value);
        }
    }
}
