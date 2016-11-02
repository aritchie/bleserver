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

            this.services
                .Cast<GattService>()
                .Select(x =>
                {
                    x.Native.Characteristics = x
                        .Characteristics
                        .Cast<GattCharacteristic>()
                        .Select(y =>
                        {
                            y.Native.Descriptors = y
                                .Descriptors
                                .Cast<GattDescriptor>()
                                .Select(z => z.Native)
                                .ToArray();
                            return y.Native;
                        })
                        .ToArray();

                    return x.Native;
                })
                .ToList()
                .ForEach(this.manager.AddService);

            this.manager.StartAdvertising(new StartAdvertisingOptions
            {
                LocalName = adData.LocalName,
                ServicesUUID = adData
                    .ServiceUuids
                    .Select(x => CBUUID.FromString(x.ToString()))
                    .ToArray()
            });

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
            if (this.IsRunning)
                throw new ArgumentException("You can't add a service to a server that is running");

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