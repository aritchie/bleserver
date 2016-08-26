using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Acr.Ble.Server
{
    public abstract class AbstractGattServer : IGattServer
    {
        readonly IList<IGattService> internalList;


        protected AbstractGattServer()
        {
            this.internalList = new List<IGattService>();
            this.Services = new ReadOnlyCollection<IGattService>(this.internalList);

        }


        ~AbstractGattServer()
        {
            this.Dispose(false);
        }


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            this.Stop();
        }


        public IReadOnlyList<IGattService> Services { get; }


        public abstract bool IsRunning { get; }
        public abstract void Start(AdvertisementData adData);
        public abstract void Stop();


        public IGattService AddService(Guid uuid, bool primary)
        {
            var native = this.CreateNative(uuid, primary);
            this.internalList.Add(native);
            return native;
        }


        public void RemoveService(IGattService service)
        {
            this.RemoveNative(service);
            this.internalList.Remove(service);
        }


        public void ClearServices()
        {
            this.ClearNative();
            this.internalList.Clear();
        }

        protected abstract IGattService CreateNative(Guid uuid, bool primary);
        protected abstract void ClearNative();
        protected abstract void RemoveNative(IGattService service);
    }
}
