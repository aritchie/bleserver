using System;


namespace Acr.Ble.Server
{
    public interface IWriteRequest
    {
        int Offset { get; }
        bool IsReplyNeeded { get; }
        byte[] Value { get; }
        IDevice Device { get; }
    }
}
