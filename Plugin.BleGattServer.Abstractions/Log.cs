using System;
using System.Diagnostics;


namespace Plugin.BleGattServer
{
    public static class Log
    {
        public static Action<string> Out { get; set; } = action => Debug.WriteLine(action);


        public static void Write(string msg)
        {
            Out?.Invoke(msg);
        }
    }
}
