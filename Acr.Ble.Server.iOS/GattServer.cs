using System;
using System.Collections.Generic;
using System.Linq;
using CoreBluetooth;
using Foundation;


namespace Acr.Ble.Server
{
    public class GattServer : AbstractGattServer
    {
        readonly CBPeripheralManager manager = new CBPeripheralManager();
        readonly IList<IGattService> services = new List<IGattService>();


        public override bool IsRunning => this.manager.Advertising;


        public override void Start(AdvertisementData adData)
        {
            if (CBPeripheralManager.AuthorizationStatus != CBPeripheralManagerAuthorizationStatus.Authorized)
                throw new ArgumentException("Permission Denied - " + CBPeripheralManager.AuthorizationStatus);

            if (this.manager.State != CBPeripheralManagerState.PoweredOn)
                throw new ArgumentException("Invalid State - " + this.manager.State);

            this.manager.StateUpdated += (sender, e) => {
                
            this.services
                .Cast<GattService>()
                .Select(x =>
                {
                    x.Native.Characteristics = x
                        .Characteristics
                        .OfType<GattCharacteristic>()
                        .Select(y =>
                        {
                            y.Native.Descriptors = y
                                .Descriptors
                                .OfType<GattDescriptor>()
                                .Select(z => z.Native)
                                .ToArray();
                            return y.Native;
                        })
                        .ToArray();

                    return x.Native;
                })
                .ToList()
                .ForEach(x => 
                {
                    this.manager.AddService(x);
                });
            };

            var opts = new StartAdvertisingOptions
            {
                LocalName = adData.LocalName,
                ServicesUUID = adData
                    .ServiceUuids
                    .Select(x => CBUUID.FromString(x.ToString()))
                    .ToArray()
            };
            opts.Dictionary.SetValueForKey(NSObject.FromObject(adData.IsConnectable), CBAdvertisement.IsConnectable);

            if (adData.ManufacturerData != null)
            {
                var md = new List<byte>();
                md.AddRange(BitConverter.GetBytes(adData.ManufacturerId.Value));
                md.AddRange(adData.ManufacturerData);
                var data = md.ToArray();
                opts.Dictionary.SetValueForKey(NSData.FromArray(data), CBAdvertisement.DataManufacturerDataKey);    
            }
             
            this.manager.AdvertisingStarted += (sender, e) => 
            {
                if (e.Error != null)
                    System.Diagnostics.Debug.WriteLine("[ERROR] starting - " + e.Error.LocalizedDescription);
                else
                    System.Diagnostics.Debug.WriteLine("Advertising State - " + this.manager.Advertising);
                    
            };

            this.manager.StartAdvertising(opts);
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