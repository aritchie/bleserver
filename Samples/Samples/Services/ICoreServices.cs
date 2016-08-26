using System;
//using Acr.Beacons;
//using Acr.Bluetooth;
using Acr.UserDialogs;


namespace Samples.Services {

    public interface ICoreServices
    {
        IAppSettings AppSettings { get; }
        IUserDialogs Dialogs { get; }
        IViewModelManager VmManager { get; }
        //IAdapter Bluetooth { get; }
        //IBeaconManager Beacons { get; }
        //SampleDbConnection SqlConnection { get; }
    }
}
