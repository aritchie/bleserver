using System;


namespace Plugin.BleGattServer
{
    public class ReadRequest
    {
        public ReadRequest(IDevice device, int offset)
        {
            this.Device = device;
            this.Offset = offset;
        }


        public int Offset { get; }
        public byte[] Value { get; set; }
        public GattStatus Status { get; set; } = GattStatus.Success;
        public IDevice Device { get; }
    }
}
