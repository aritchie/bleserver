using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CoreBluetooth;
using Foundation;


namespace Acr.Ble.Server
{
    public class GattServer : AbstractGattServer
    {
        readonly IList<IGattService> services = new List<IGattService>();
        readonly CBPeripheralManager manager;
        readonly Subject<bool> runningSubj;


        public GattServer(CBPeripheralManager manager)
        {
            this.manager = manager;
            this.runningSubj = new Subject<bool>();
        }


        public override bool IsRunning => this.manager.Advertising;


        IObservable<bool> runningOb;
        public override IObservable<bool> WhenRunningChanged()
        {
            this.runningOb = this.runningOb ?? Observable.Create<bool>(ob =>
            {
                var handler = new EventHandler<NSErrorEventArgs>((sender, args) => 
                {
                    if (args.Error == null)
                    {
                        ob.OnNext(true);
                    }
                    else 
                    {
                        ob.OnError(new ArgumentException(args.Error.LocalizedDescription));
                    }
                });
                var sub = this.runningSubj
                    .AsObservable()
                    .Subscribe(x => ob.OnNext(false));
                
                this.manager.AdvertisingStarted += handler;

                return () =>
                {
                    this.manager.AdvertisingStarted -= handler;
                    sub.Dispose();
                };
            })
            .Publish()
            .RefCount();
            
            return this.runningOb;
        }


        public override void Start(AdvertisementData adData)
        {
            if (this.manager.Advertising)
                return;
            
            if (CBPeripheralManager.AuthorizationStatus != CBPeripheralManagerAuthorizationStatus.Authorized)
                throw new ArgumentException("Permission Denied - " + CBPeripheralManager.AuthorizationStatus);

            if (this.manager.State != CBPeripheralManagerState.PoweredOn)
                throw new ArgumentException("Invalid State - " + this.manager.State);


            this.manager.AdvertisingStarted += (sender, e) =>
            {
                if (e.Error != null)
                    System.Diagnostics.Debug.WriteLine("[ERROR] starting - " + e.Error.LocalizedDescription);
                else
                    System.Diagnostics.Debug.WriteLine("Advertising State - " + this.manager.Advertising);

            };

            this.manager.StartAdvertising(new StartAdvertisingOptions
            {
                LocalName = adData.LocalName,
                ServicesUUID = adData
                    .ServiceUuids
                    .Select(x => CBUUID.FromString(x.ToString()))
                    .ToArray()
            });

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
                .ForEach(this.manager.AddService);
        }


        public override void Stop()
        {
            if (!this.manager.Advertising)
                return;
            
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