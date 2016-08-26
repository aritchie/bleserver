﻿using System;
using Acr.Ble.Server.Internals;
using Android.Bluetooth;


namespace Acr.Ble.Server
{
    public class GattCharacteristic : AbstractGattCharacteristic
    {
        public BluetoothGattCharacteristic Native { get; }
        readonly GattServerCallbacks callbacks;

        public GattCharacteristic(GattServerCallbacks callbacks,
                                  IGattService service,
                                  Guid uuid,
                                  CharacteristicProperties properties,
                                  CharacteristicPermissions permissions,
                                  byte[] value) : base(service, uuid, properties, value)
        {
            this.callbacks = callbacks;

            this.Native = new BluetoothGattCharacteristic(
                uuid.ToUuid(),
                (GattProperty) (int) properties,
                GattPermission.Read | GattPermission.Write
            );
        }


        public override void Broadcast(byte[] value)
        {
            throw new NotImplementedException();
        }


        public override IObservable<bool> WhenSubscriptionStateChanged()
        {
            throw new NotImplementedException();
        }


        public override IObservable<byte[]> WhenWriteReceived()
        {
            throw new NotImplementedException();
        }


        public override IObservable<byte[]> WhenReadReceived()
        {
            throw new NotImplementedException();
        }


        protected override IGattDescriptor CreateNative(Guid uuid, byte[] initialValue)
        {
            throw new NotImplementedException();
        }


        protected override void RemoveNative(IGattDescriptor descriptor)
        {
            throw new NotImplementedException();
        }
    }
}
