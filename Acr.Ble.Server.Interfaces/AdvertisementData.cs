using System;
using System.Collections.Generic;


namespace Acr.Ble.Server
{
    public class AdvertisementData
    {
        //Guid deviceUuid, string deviceName
        // Mode - High, Balanced, Low
        //int TxPower { get; set; }
        public bool IncludeDeviceName { get; set; }
        public bool IncludeTxPower { get; set; }
        public bool IsConnectable { get; set; }
        public string LocalName { get; set; }
        public int TxPower { get; set; }

        public IDictionary<int, byte[]> ManufacturerData { get; set; } = new Dictionary<int, byte[]>();
        public IDictionary<Guid, byte[]> ServiceData { get; set; } = new Dictionary<Guid, byte[]>();
        public List<Guid> ServiceUuids { get; set; } = new List<Guid>();
    }
}