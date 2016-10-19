using System;
using System.Linq;
using CoreBluetooth;
using CoreFoundation;


namespace Acr.Ble.Server
{
    public class GattServer : AbstractGattServer
    {
        readonly CBPeripheralManager manager = new CBPeripheralManager();


        public override bool IsRunning => this.manager.Advertising;

        public override void Start(AdvertisementData adData)
        {
            //            //this.manager.AdvertisingStarted
            //            //this.manager.ReadRequestReceived += (sender, args) => { };
            //            //this.manager.WriteRequestsReceived += (sender, args) => { };
            //            //this.manager.CharacteristicUnsubscribed
            //            //this.manager.CharacteristicSubscribed
            //this.manager.SetDesiredConnectionLatency(CBPeripheralManagerConnectionLatency.High, );
            //this.manager.RespondToRequest(CBATTRequest, CBATTError.AttributeNotFound);

            var native = new StartAdvertisingOptions
            {
                LocalName = adData.LocalName,
                ServicesUUID = adData
                    .ServiceUuids
                    .Select(x => CBUUID.FromString(x.ToString()))
                    .ToArray()
            };
            this.manager.StartAdvertising(native);
        }


        public override void Stop()
        {
            this.manager.StopAdvertising();
        }


        protected override IGattService CreateNative(Guid uuid, bool primary)
        {
            var service = new GattService(this.manager, this, uuid, primary);
            this.manager.AddService(service.Native);
            return service;
        }


        protected override void RemoveNative(IGattService service)
        {
            var native = ((GattService)service).Native;
            this.manager.RemoveService(native);
        }


        protected override void ClearNative()
        {
            this.manager.RemoveAllServices();
        }
    }
}