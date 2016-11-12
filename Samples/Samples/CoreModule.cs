using System;
using Acr.Ble.Server;
using Acr.Notifications;
using Acr.Settings;
using Acr.UserDialogs;
using Autofac;
using Samples.Services;
using Samples.Services.Impl;


namespace Samples
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterType<ViewModelManagerImpl>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .Register(_ => Settings.Local.Bind<AppSettingsImpl>())
                .As<IAppSettings>()
                .SingleInstance();

            builder
                .Register(_ => Notifications.Instance)
                .As<INotifications>()
                .SingleInstance();

            builder
                .Register(_ => UserDialogs.Instance)
                .As<IUserDialogs>()
                .SingleInstance();

            builder
                .Register(_ => BleAdapter.Current)
                .As<IBleAdapter>()
                .SingleInstance();

            builder
                .RegisterType<CoreServicesImpl>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterAssemblyTypes(this.ThisAssembly)
                .Where(x => x.Namespace.StartsWith("Samples.Tasks"))
                .AsImplementedInterfaces()
                .AutoActivate()
                .SingleInstance();

            builder
                .RegisterAssemblyTypes(this.ThisAssembly)
                .Where(x => x.Namespace.StartsWith("Samples.ViewModels"))
                .AsSelf()
                .InstancePerDependency();
        }
    }
}
