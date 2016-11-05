using System;
using System.Collections.Generic;
using System.Linq;
using CoreBluetooth;
using CoreFoundation;


namespace Acr.Ble.Server
{
    public class GattServer : AbstractGattServer
    {
        readonly CBPeripheralManager manager = new CBPeripheralManager();
        readonly IList<IGattService> services = new List<IGattService>();


        public override bool IsRunning => this.manager.Advertising;


        public override void Start(AdvertisementData adData)
        {
            this.services
                .Cast<GattService>()
                .Select(x =>
                {
                    x.Native.Characteristics = x
                        .Characteristics
                        .Cast<GattCharacteristic>()
                        .Select(y =>
                        {
                            //y.Native.Descriptors = y
                            //    .Descriptors
                            //    .Cast<GattDescriptor>()
                            //    .Select(z => z.Native)
                            //    .ToArray();
                            return y.Native;
                        })
                        .ToArray();

                    return x.Native;
                })
                .ToList()
                .ForEach(x => {
                this.manager.AddService(x);
            }                    );

            this.manager.StartAdvertising(new StartAdvertisingOptions
            {
                LocalName = adData.LocalName,
                ServicesUUID = adData
                    .ServiceUuids
                    .Select(x => CBUUID.FromString(x.ToString()))
                    .ToArray()
            });
        }


        public override void Stop()
        {
            this.manager.RemoveAllServices();
            this.manager.StopAdvertising();
        }


        protected override IGattService CreateNative(Guid uuid, bool primary)
        {
            var service = new GattService(this.manager, this, uuid, primary);
            this.services.Add(service);
            //this.context?.Manager.AddService(service.Native); // TODO: build the service out?
            return service;
        }


        protected override void RemoveNative(IGattService service)
        {
            if (this.services.Remove(service))
            {
                var native = ((GattService)service).Native;
                //this.context.Manager?.RemoveService(native);
            }
        }


        protected override void ClearNative()
        {
            this.services.Clear();
            //this.context.Manager?.RemoveAllServices();
        }
    }
}