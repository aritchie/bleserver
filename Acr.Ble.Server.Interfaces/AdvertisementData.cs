using System;
using System.Collections.Generic;


namespace Acr.Ble.Server
{
    public class AdvertisementData
    {
        // android only
        //public bool IncludeDeviceName { get; set; }
        //public bool IncludeTxPower { get; set; }

        // all
        //public bool IsConnectable { get; set; }
        public string LocalName { get; set; }
        public List<Guid> ServiceUuids { get; set; } = new List<Guid>();

        //public IDictionary<Guid, byte[]> ServiceData { get; set; } = new Dictionary<Guid, byte[]>();

        public int? ManufacturerId { get; set; }
        public byte[] ManufacturerData { get; set; }


        public void SetManufacturerData(int manufacturerId, byte[] data)
        {
            this.ManufacturerId = manufacturerId;
            this.ManufacturerData = data;
        }
    }
}