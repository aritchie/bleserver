using System;
using System.Collections.Generic;


namespace Acr.Ble.Server
{
    public interface IGattCharacteristic
    {
        IGattService Service { get; }

        // permissions
        Guid Uuid { get; }
        CharacteristicProperties Properties { get; }
        //byte[] Value { get; set; }
        //bool IsSubscribed { get; }
        //int Subscribers { get; } <- better

        IGattDescriptor AddDescriptor(Guid uuid);
        void RemoveDescriptor(IGattDescriptor descriptor);
        IReadOnlyList<IGattDescriptor> Descriptors { get; }

        void Broadcast(byte[] value);

        IObservable<bool> WhenSubscriptionStateChanged();
        IObservable<IWriteRequest> WhenWriteReceived();
        IObservable<IReadRequest> WhenReadReceived();
    }
}
