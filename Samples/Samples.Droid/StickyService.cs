using System;
using Android.App;
using Android.Content;
using Android.OS;


namespace Samples.Droid
{
    [Service]
    public class StickyService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }
    }
}
