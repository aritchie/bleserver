using System;


namespace Acr.Ble.Server
{
    public interface IWriteRequest
    {
        bool IsReplyNeeded { get; }
        byte[] IncomingValue { get; }
        // TODO: reply?  device?
    }
}
