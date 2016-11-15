using System;
using System.Collections.Generic;


namespace Acr.Ble.Server
{
    public class AdvertisementData
    {
        public string LocalName { get; set; }
        public List<Guid> ServiceUuids { get; set; } = new List<Guid>();

        // ANDROID ONLY
        //public bool IncludeDeviceName { get; set; }
        //public bool IncludeTxPower { get; set; }
        //public IDictionary<Guid, byte[]> ServiceData { get; set; } = new Dictionary<Guid, byte[]>();
        //public int? ManufacturerId { get; set; }
        //public byte[] ManufacturerData { get; set; }
        //public void SetManufacturerData(int manufacturerId, byte[] data)
        //{
        //    this.ManufacturerId = manufacturerId;
        //    this.ManufacturerData = data;
        //}
    }
}