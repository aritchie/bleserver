using System;
using System.Collections.Generic;


namespace Acr.Ble.Server
{
    public interface IGattServer : IDisposable
    {
        bool IsRunning { get; }
        void Start(AdvertisementData adData);
        void Stop();

        IGattService AddService(Guid uuid, bool primary);
        void RemoveService(IGattService service);
        void ClearServices();
        IReadOnlyList<IGattService> Services { get; }

        // IObservable<bool> WhenRunningStateChanged();
        // IEnumerable<IDevice> GetConnectedDevices();
        // Observable<IDevice, bool> WhenDeviceConnectedDisconnected()
        // IObservable<IGattCharacteristic> WhenReadRequestReceived();
        // IObservable<IGattCharacteristic, byte[]> WhenWriteRequestReceived();
        // IObservable<IGattCharacteristic, bool> WhenCharacteristicSubscriptionStateChanged();
    }
}