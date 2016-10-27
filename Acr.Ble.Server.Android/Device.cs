using System;
using System.Linq;
using Android.Bluetooth;


namespace Acr.Ble.Server
{
    public class Device : IDevice
    {
        readonly Lazy<Guid> deviceUuidLazy;


        public Device(BluetoothDevice native)
        {
            this.deviceUuidLazy = new Lazy<Guid>(() =>
            {
                var deviceGuid = new byte[16];
                var mac = native.Address.Replace(":", "");
                var macBytes = Enumerable
                    .Range(0, mac.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(mac.Substring(x, 2), 16))
                    .ToArray();

                macBytes.CopyTo(deviceGuid, 10);
                return new Guid(deviceGuid);
            });
        }


        public Guid Uuid => this.deviceUuidLazy.Value;
    }
}
