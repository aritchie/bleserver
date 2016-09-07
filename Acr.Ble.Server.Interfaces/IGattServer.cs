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
        void ClearServices();
        IReadOnlyList<IGattService> Services { get; }


        //IGattService CreateService();
        //IGattCharacteristic CreateCharacteristic();
        //IGattDescriptor CreateDescriptor();

        // IObservable<bool> WhenRunningStateChanged();
        // IEnumerable<IDevice> GetConnectedDevices();
        // Observable<IDevice, bool> WhenDeviceConnectedDisconnected()
        // IObservable<IGattCharacteristic, IReadRequest> WhenReadRequestReceived();
        // IObservable<IGattCharacteristic, IWriteRequest> WhenWriteRequestReceived();
        // IObservable<IGattCharacteristic, bool> WhenCharacteristicSubscriptionStateChanged();
    }
}