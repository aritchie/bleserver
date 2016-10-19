using System;
using System.Collections.Generic;
using Acr;
using Autofac;
using Samples.Pages.Server;
using Samples.Services;
using Samples.ViewModels;
using Xamarin.Forms;


namespace Samples
{
    public class App : Application
    {
        readonly IContainer container;


        public App(IContainer container)
        {
            this.container = container;

            this.MainPage = new NavigationPage(new TabbedPage
            {
                Title = "BLE Server",
                Children =
                {
                    new EasyServerPage
                    {
                        Title = "Quick Server",
                        BindingContext = this.container.Resolve<EasyServerViewModel>()
                    },
                    new BeaconAdvertisementPage
                    {
                        Title = "Beacon Ads",
                        BindingContext = this.container.Resolve<BeaconAdvertisementViewModel>()
                    }
                }
            });
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