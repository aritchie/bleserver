using System;


namespace Acr.Ble.Server.Beacons
{
    public interface IBeaconAdvertiser : IDisposable
    {
        bool IsAdvertising { get; }
        void Start(Beacon beacon);
        void Stop();
    }
}
