using Android.App;

namespace SharedMauiLib.Platforms.Android.CurrentActivity
{
    public class ActivityEventArgs : EventArgs
    {
        internal ActivityEventArgs(Activity activity, ActivityEvent ev)
        {
            Event = ev;
            Activity = activity;
        }

        public ActivityEvent Event { get; }
        public Activity Activity { get; }
    }
}