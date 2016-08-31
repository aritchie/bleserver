using System;
using CoreBluetooth;
using Foundation;


namespace Acr.Ble.Server
{
    public class ReadRequest : IReadRequest
    {
        readonly CBATTRequest request;


        public ReadRequest(CBATTRequest request)
        {
            this.request = request;
        }


        public int Offset => (int)this.request.Offset;

        public byte[] Value
        {
            get { return this.request.Value.ToArray(); }
            set { this.request.Value = NSData.FromArray(value); }
        }
    }
}
