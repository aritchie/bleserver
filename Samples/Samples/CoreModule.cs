using System;
using Acr.Ble.Server;
using Acr.UserDialogs;
using Autofac;


namespace Samples
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .Register(_ => UserDialogs.Instance)
                .As<IUserDialogs>()
                .SingleInstance();

            builder
                .Register(_ => BleAdapter.Current)
                .As<IBleAdapter>()
                .SingleInstance();

            builder
                .RegisterAssemblyTypes(this.ThisAssembly)
                .Where(x => x.Namespace.StartsWith("Samples.ViewModels"))
                .AsSelf()
                .InstancePerDependency();
        }
    }
}
