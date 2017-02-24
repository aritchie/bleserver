using System;
using Sample.Pages;
using Xamarin.Forms;


namespace Sample
{
    public class App : Application
    {
        public App()
        {
            this.MainPage = new NavigationPage(new MainPage());
        }
    }
}