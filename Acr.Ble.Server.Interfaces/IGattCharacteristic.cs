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
        GattPermissions Permissions { get; }

        IGattDescriptor AddDescriptor(Guid uuid, byte[] value);
        IReadOnlyList<IGattDescriptor> Descriptors { get; }

        void Broadcast(byte[] value, params IDevice[] devices);
        void BroadcastToAll(byte[] value);

        IObservable<WriteRequest> WhenWriteReceived();
        IObservable<ReadRequest> WhenReadReceived();
        IObservable<DeviceSubscriptionEvent> WhenDeviceSubscriptionChanged();
        IReadOnlyList<IDevice> SubscribedDevices { get; }
    }
}
