using System;


namespace Acr.Ble.Server.Beacons
{
    public class Beacon
    {
        public Beacon(Guid uuid, ushort major, ushort minor)
        {
            this.Uuid = uuid;
            this.Major = major;
            this.Minor = minor;
        }


        public Guid Uuid { get; }
        public ushort Major { get; }
        public ushort Minor { get; }
    }
}
