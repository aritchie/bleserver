using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Acr.Ble.Server.Internals;
using Android.Bluetooth;


namespace Acr.Ble.Server
{
    public class GattCharacteristic : AbstractGattCharacteristic
    {
        public static readonly Guid NotifyDescriptorId = new Guid("00002902-0000-1000-8000-00805f9b34fb");
        public static readonly byte[] NotifyEnabledBytes = BluetoothGattDescriptor.EnableNotificationValue.ToArray();

        public BluetoothGattCharacteristic Native { get; }
        readonly GattContext context;
        readonly IList<BluetoothDevice> subscribers = new List<BluetoothDevice>();
        readonly BluetoothGattDescriptor subscribeDescriptor;


        public GattCharacteristic(GattContext context,
                                  IGattService service,
                                  Guid uuid,
                                  CharacteristicProperties properties,
                                  CharacteristicPermissions permissions) : base(service, uuid, properties, permissions)
        {
            this.context = context;
            // TODO: create the client config descriptor
            this.Native = new BluetoothGattCharacteristic(
                uuid.ToUuid(),
                (GattProperty) (int) properties,
                GattPermission.Read | GattPermission.Write
            );
            if (properties.HasFlag(CharacteristicProperties.Notify))
            {
                this.subscribeDescriptor = new BluetoothGattDescriptor(null, GattDescriptorPermission.Write);
            }
        }


        public override void Broadcast(byte[] value, params IDevice[] devices)
        {
            this.Native.SetValue(value);

            // TODO: request response true/false
            //client that requests notifications/indications by writing to the "Client Configuration" descriptor for the given characteristic.
            foreach (var dev in this.context.Server.ConnectedDevices)
                this.context.Server.NotifyCharacteristicChanged(dev, this.Native, true);
        }


        public override void BroadcastToAll(byte[] value)
        {
            throw new NotImplementedException();
        }


        public override IObservable<DeviceSubscriptionEvent> WhenDeviceSubscriptionChanged()
        {
            return Observable.Create<DeviceSubscriptionEvent>(ob =>
            {
                var handler = new EventHandler<DescriptorWriteEventArgs>((sender, args) =>
                {
                    //this.subscribers.Add(null);
                    //if (this.subscribers.Count == 1)
                    //    ob.OnNext(true); // has subscribers now

                    //this.subscribers.Remove(null);
                    //if (this.subscribers.Count == 0)
                    //    ob.OnNext(false); // no subscribes
                });

                this.context.Callbacks.DescriptorWrite += handler;

                return () => 
                { 
                    this.context.Callbacks.DescriptorWrite -= handler;
                };
            });
        }


        public override IObservable<IWriteRequest> WhenWriteReceived()
        {
            return Observable.Create<IWriteRequest>(ob =>
            {
                var handler = new EventHandler<CharacteristicWriteEventArgs>((sender, args) => 
                {
                    var request = new WriteRequest();
                    request.Device = null;

                    ob.OnNext(request);

                    // TODO: exception if not reply set?
                    if (request.IsReplyNeeded)
                    {
                        this.context.Server.SendResponse
                        (
                            null, 
                            args.RequestId, 
                            GattStatus.Success,
                            request.Offset, 
                            request.Value
                        );
                    }
                });
                this.context.Callbacks.CharacteristicWrite += handler;

                return () => 
                { 
                    this.context.Callbacks.CharacteristicWrite -= handler;
                };
            });
        }


        public override IObservable<IReadRequest> WhenReadReceived()
        {
            return Observable.Create<IReadRequest>(ob =>
            {
                return () => { };
            });
        }


        protected override IGattDescriptor CreateNative(Guid uuid)
        {
            return new GattDescriptor(this.context, this, uuid);
        }
    }
}
