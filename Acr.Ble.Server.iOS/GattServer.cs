using System;
using System.Collections.Generic;
using System.Linq;
using CoreBluetooth;
using CoreFoundation;


namespace Acr.Ble.Server
{
    public class GattServer : AbstractGattServer
    {
        readonly IList<IGattService> services = new List<IGattService>();
        CBPeripheralManager manager;


        bool isRunning;
        public override bool IsRunning => this.isRunning;

        public override void Start(AdvertisementData adData)
        {
            this.manager = new CBPeripheralManager(null, DispatchQueue.DefaultGlobalQueue);
            foreach (var service in this.services)
            {
                var ns = ((GattService) service).Native;
                this.manager.AddService(ns);
            }
            var native = new StartAdvertisingOptions
            {
                LocalName = adData.LocalName,
                ServicesUUID = adData
                    .ServiceUuids
                    .Select(x => CBUUID.FromString(x.ToString()))
                    .ToArray()
            };
            this.manager.StartAdvertising(native);
            this.isRunning = true;
        }


        public override void Stop()
        {
            if (this.manager == null)
                return;

            this.manager.RemoveAllServices();
            this.manager.StopAdvertising();
            this.manager.Dispose();
            this.manager = null;
        }


        protected override IGattService CreateNative(Guid uuid, bool primary)
        {
            this.isRunning = false;
            var service = new GattService(this.manager, this, uuid, primary);
            this.services.Add(service);
            return service;
        }


        protected override void RemoveNative(IGattService service)
        {
            if (this.services.Remove(service))
            {
                var native = ((GattService)service).Native;
                this.manager.RemoveService(native);
            }
        }


        protected override void ClearNative()
        {
            this.services.Clear();
            this.manager?.RemoveAllServices();
        }
    }
}