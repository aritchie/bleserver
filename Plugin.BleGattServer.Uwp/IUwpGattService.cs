using System;
using System.Threading.Tasks;


namespace Plugin.BleGattServer
{
    public interface IUwpGattService : IGattService
    {
        Task Init();
        void Stop();
    }
}
