﻿using System;
using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Autofac;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


namespace Samples.Droid
{
    [Activity(
        Label = "ACR BLE Server",
        Icon = "@drawable/icon",
        MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
    )]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

            UserDialogs.Init(() => (Activity)Forms.Context);

            var builder = new ContainerBuilder();
            builder.RegisterModule(new PlatformModule());
            var container = builder.Build();

            this.LoadApplication(new App(container));

            //ActivityCompat.RequestPermissions(this, new [] { Manifest.Permission.AccessCoarseLocation }, -1);
        }

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        //{
        //}
    }
}

