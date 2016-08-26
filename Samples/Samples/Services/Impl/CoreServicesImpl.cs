using System;
using Acr.UserDialogs;


namespace Samples.Services.Impl
{

    public class CoreServicesImpl : ICoreServices
    {

        public CoreServicesImpl(IUserDialogs dialogs,
                                IViewModelManager vmManager,
                                IAppSettings appSettings)
        {
            this.Dialogs = dialogs;
            this.VmManager = vmManager;
            this.AppSettings = appSettings;
        }


        public IAppSettings AppSettings { get; }
        public IUserDialogs Dialogs { get; }
        public IViewModelManager VmManager { get; }
    }
}
