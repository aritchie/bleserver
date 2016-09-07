using System;
using System.Collections.Generic;
using System.Linq;


namespace Acr.Ble.Server.Beacons
{
    public class BeaconAdvertiserImpl : IBeaconAdvertiser
    {
        readonly IGattServer server;


        public BeaconAdvertiserImpl(IGattServer server = null)
        {
            this.server = server;
        }


        public void Dispose()
        {
            this.server.Dispose();
        }


        public bool IsAdvertising => this.server.IsRunning;
        public void Start(Beacon beacon)
        {
            var packet = new AdvertisementData();
            var bytes = new List<byte>();
            bytes.AddRange(beacon.Uuid.ToByteArray());
            bytes.AddRange(BitConverter.GetBytes(beacon.Major).Reverse());
            bytes.AddRange(BitConverter.GetBytes(beacon.Minor).Reverse());

            packet.ManufacturerData.Add(27, bytes.ToArray());
            this.server.Start(packet);
        }

        public void Stop()
        {
            this.server.Stop();
        }
    }
}
