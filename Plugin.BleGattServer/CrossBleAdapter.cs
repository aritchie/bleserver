using System;


namespace Plugin.BleGattServer
{
    public static class CrossBleAdapter
    {
        static IBleAdapter current;
        public static IBleAdapter Current
        {
            get
            {
#if BAIT
                if (current == null)
                    throw new ArgumentException("[Plugin.BleGattServer] No platform plugin found.  Did you install the nuget package in your app project as well?");
#else
                current = current ?? new BleAdapterImpl();
#endif
                return current;
            }
            set { current = value; }
        }
    }
}
