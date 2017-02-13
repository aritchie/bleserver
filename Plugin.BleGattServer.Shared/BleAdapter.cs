using System;


namespace Plugin.BleGattServer
{
    public static class BleAdapter
    {
        static readonly Lazy<IBleAdapter> currentAdapter = new Lazy<IBleAdapter>(() =>
        {
            #if PCL
            throw new ArgumentException("[Acr.Ble.Server] No platform plugin found.  Did you install the nuget package in your app project as well?");
            #else
            return new BleAdapterImpl();
            #endif
        });


        static IBleAdapter instance;
        public static IBleAdapter Current
        {
            get { return instance ?? currentAdapter.Value; }
            set { instance = value; }
        }
    }
}
