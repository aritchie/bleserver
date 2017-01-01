using System;
using System.Collections.Generic;
using Acr;
using Autofac;
using Samples.Pages.Server;
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

            this.MainPage = new NavigationPage(new EasyServerPage
            {
                Title = "Quick Server",
                BindingContext = this.container.Resolve<EasyServerViewModel>()
            });
        }
    }
}