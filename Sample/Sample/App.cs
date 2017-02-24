using System;
using Samples.Pages.Server;
using Xamarin.Forms;


namespace Samples
{
    public class App : Application
    {
        public App()
        {
            this.MainPage = new NavigationPage(new MainPage());
        }
    }
}