using System;
using System.Threading.Tasks;


namespace Acr.Ble.Server
{
    public interface IUwpGattService : IGattService
    {
        Task Init();
        void Stop();
    }
}
