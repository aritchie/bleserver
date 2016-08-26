using System;


namespace Acr.Ble.Server
{
    public class GattServer : AbstractGattServer
    {
        public override bool IsRunning { get; }
        public override void Start(AdvertisementData adData)
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        protected override IGattService CreateNative(Guid uuid, bool primary)
        {
            throw new NotImplementedException();
        }

        protected override void ClearNative()
        {
            throw new NotImplementedException();
        }

        protected override void RemoveNative(IGattService service)
        {
            throw new NotImplementedException();
        }
    }
}
