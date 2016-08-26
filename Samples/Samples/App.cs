using System;
using System.Collections.Generic;
using Acr;
using Autofac;
using Samples.Services;
using Samples.ViewModels.Server;
using Xamarin.Forms;


namespace Samples
{
    public class App : Application
    {
        readonly IContainer container;


        public App(IContainer container)
        {
            this.container = container;

            this.MainPage = container
                .Resolve<IViewModelManager>()
                .CreatePage<SetupViewModel>();
        }


        protected override void OnResume()
        {
            base.OnResume();
            this.container.Resolve<IEnumerable<IAppLifecycle>>().Each(x => x.OnForeground());
        }


        protected override void OnSleep()
        {
            base.OnSleep();
            this.container.Resolve<IEnumerable<IAppLifecycle>>().Each(x => x.OnBackground());
        }
    }
}