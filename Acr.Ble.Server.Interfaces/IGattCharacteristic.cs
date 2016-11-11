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

        /// <summary>
        /// Send null to broadcast to all
        /// </summary>
        /// <param name="value"></param>
        /// <param name="devices"></param>
        void Broadcast(byte[] value, params IDevice[] devices);

        IObservable<WriteRequest> WhenWriteReceived();
        IObservable<ReadRequest> WhenReadReceived();
        IObservable<DeviceSubscriptionEvent> WhenDeviceSubscriptionChanged();
        IReadOnlyList<IDevice> SubscribedDevices { get; }
    }
}
