using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;


namespace Sample.Droid
{
	[Activity(
        Label = "Sample",
        Icon = "@drawable/icon",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
    )]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Forms.Init (this, bundle);
			this.LoadApplication(new Sample.App());
		}
	}
}

