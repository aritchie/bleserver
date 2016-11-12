using System;
using System.Reactive.Linq;
using Android.App;
using Android.Bluetooth;
using Android.Content;


namespace Acr.Ble.Server.Internals
{
    public static class BluetoothObservables
    {
        public static IObservable<object> WhenAdapterStatusChanged()
        {
            return BluetoothObservables.WhenIntentReceived(BluetoothAdapter.ActionStateChanged);
        }

 
        public static IObservable<Intent> WhenIntentReceived(string intentAction)
        {
            return Observable.Create<Intent>(ob =>
            {
                var receiver = new ObservableBroadcastReceiver { OnEvent = ob.OnNext };
                Application.Context.RegisterReceiver(receiver, new IntentFilter(intentAction));
                return () => Application.Context.UnregisterReceiver(receiver);
            });
        }
    }


    public class ObservableBroadcastReceiver : BroadcastReceiver
    {
        public Action<Intent> OnEvent { get; set; }

        public override void OnReceive(Context context, Intent intent)
        {
            System.Diagnostics.Debug.WriteLine($"{intent.Action} firing");
            this.OnEvent?.Invoke(intent);
        }
    }
}
