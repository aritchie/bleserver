using System;


namespace Acr.Ble.Server
{
    public class GattServerFactory : IGattServerFactory
    {
        public IGattServer CreateInstance()
        {
#if PCL
            throw new ArgumentException("[Acr.Ble.Server] No platform plugin found.  Did you install the nuget package in your app project as well?");
#else
            return new GattServer();
#endif
        }
    }
}
