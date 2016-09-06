using System;


namespace Acr.Ble.Server
{
    public class GattCharacteristic
    {
        public GattCharacteristic(Guid uuid)
        {
            // bait & switch at a class level
        }
        public Guid Uuid { get; }
        public CharacteristicPermissions Permissions { get; set; }
        public CharacteristicProperties Properties { get; set; }

     //IGattDescriptor AddDescriptor(Guid uuid);
     //   void RemoveDescriptor(IGattDescriptor descriptor);
     //   IReadOnlyList<IGattDescriptor> Descriptors { get; }

     //   void Broadcast(byte[] value);

     //   IObservable<bool> WhenSubscriptionStateChanged();
     //   IObservable<IWriteRequest> WhenWriteReceived();
     //   IObservable<IReadRequest> WhenReadReceived();
    }
}
