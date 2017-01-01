using System;
using System.Reactive.Linq;
using Acr.Ble.Server.Internals;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;


namespace Acr.Ble.Server
{
    public class BleAdapterImpl : IBleAdapter
    {
        readonly BluetoothManager manager;


        public BleAdapterImpl()
        {
            this.manager = (BluetoothManager)Application.Context.GetSystemService(Context.BluetoothService);
        }


        public AdapterStatus Status
        {
            get
            {
                if (Build.VERSION.SdkInt < BuildVersionCodes.JellyBeanMr2)
                    return AdapterStatus.Unsupported;

                //if (!Application.Context.PackageManager.HasSystemFeature(PackageManager.FeatureBluetoothLe))
                //    return Status.Unsupported;

                if (this.manager?.Adapter == null)
                    return AdapterStatus.Unsupported;

                if (!this.manager.Adapter.IsEnabled)
                    return AdapterStatus.PoweredOff;

                switch (this.manager.Adapter.State)
                {
                    case State.Off:
                    case State.TurningOff:
                    case State.Disconnecting:
                    case State.Disconnected:
                        return AdapterStatus.PoweredOff;

                        //case State.Connecting
                    case State.On:
                    case State.Connected:
                        return AdapterStatus.PoweredOn;

                    default:
                        return AdapterStatus.Unknown;
                }
            }
        }


        IObservable<AdapterStatus> statusOb;
        public IObservable<AdapterStatus> WhenStatusChanged()
        {
            this.statusOb = this.statusOb ?? Observable.Create<AdapterStatus>(ob =>
            {
                ob.OnNext(this.Status);
                var aob = BluetoothObservables
                    .WhenAdapterStatusChanged()
                    .Subscribe(_ => ob.OnNext(this.Status));

                return aob.Dispose;
            })
            .Replay(1)
            .RefCount();

            return this.statusOb;
        }


        public IGattServer CreateGattServer()
        {
            return new GattServer();
        }
    }
}
