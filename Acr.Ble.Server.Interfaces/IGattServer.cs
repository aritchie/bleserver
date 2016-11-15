using System;
using System.Collections.Generic;


namespace Acr.Ble.Server
{
    public interface IGattServer : IDisposable
    {
        IObservable<bool> WhenRunningChanged();
        bool IsRunning { get; }
        void Start(AdvertisementData adData);
        void Stop();

        IGattService AddService(Guid uuid, bool primary);
        void RemoveService(Guid serviceUuid);
        void ClearServices();
        IReadOnlyList<IGattService> Services { get; }

        IObservable<CharacteristicSubscription> WhenAnyCharacteristicSubscriptionChanged();
        IList<IDevice> GetAllSubscribedDevices();
    }
}