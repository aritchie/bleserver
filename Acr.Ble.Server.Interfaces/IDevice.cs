using System;


namespace Acr.Ble.Server
{
    public interface IDevice
    {
        //string Identifier { get; }
        // I can get this on iOS and Droid
        Guid Uuid { get; }
    }
}
