using System;
using System.Collections.Generic;


namespace Acr.Ble.Server
{
    public class GattCharactertistic : IGattCharacteristic
    {
        public GattCharacteristic()
        {
            throw new ArgumentException("This is the PCL library");
        }
        public IGattService Service { get; }
        public Guid Uuid { get; }
        public CharacteristicProperties Properties { get; }
        public IGattDescriptor AddDescriptor(Guid uuid)
        {
            throw new NotImplementedException();
        }

        public void RemoveDescriptor(IGattDescriptor descriptor)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IGattDescriptor> Descriptors { get; }
        public void Broadcast(byte[] value)
        {
            throw new NotImplementedException();
        }

        public IObservable<bool> WhenSubscriptionStateChanged()
        {
            throw new NotImplementedException();
        }

        public IObservable<IWriteRequest> WhenWriteReceived()
        {
            throw new NotImplementedException();
        }

        public IObservable<IReadRequest> WhenReadReceived()
        {
            throw new NotImplementedException();
        }
    }
}
