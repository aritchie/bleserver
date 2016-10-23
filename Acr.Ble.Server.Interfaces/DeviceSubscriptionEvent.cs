using System;
namespace Acr.Ble.Server
{
    public class DeviceSubscriptionEvent
    {
        public DeviceSubscriptionEvent(IDevice device, bool subscribed)
        {
            this.Device = device;
            this.IsSubscribed = subscribed;
        }


        public bool IsSubscribed { get; }
        public IDevice Device { get; }
    }
}
