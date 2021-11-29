using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui;
using Microsoft.NetConf2021.Maui.Platforms.Android.Services;
using System;

namespace Microsoft.NetConf2021.Maui
{
	[Activity(
		Theme = "@style/Maui.SplashTheme", 
		MainLauncher = true, 
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : MauiAppCompatActivity
	{
        internal static MainActivity instance;
		public MediaPlayerServiceBinder binder;
		MediaPlayerServiceConnection mediaPlayerServiceConnection;
		private Intent mediaPlayerServiceIntent;

		public event StatusChangedEventHandler StatusChanged;

		public event CoverReloadedEventHandler CoverReloaded;

		public event PlayingEventHandler Playing;

		public event BufferingEventHandler Buffering;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            instance = this;
            NotificationHelper.CreateNotificationChannel(ApplicationContext);
            if (mediaPlayerServiceConnection == null)
                InitilizeMedia();
        }

        private void InitilizeMedia()
        {
            mediaPlayerServiceIntent = new Intent(ApplicationContext, typeof(MediaPlayerService));
            mediaPlayerServiceConnection = new MediaPlayerServiceConnection(this);
            BindService(mediaPlayerServiceIntent, mediaPlayerServiceConnection, Bind.AutoCreate);
        }

        class MediaPlayerServiceConnection : Java.Lang.Object, IServiceConnection
        {
            readonly MainActivity instance;

            public MediaPlayerServiceConnection(MainActivity mediaPlayer)
            {
                this.instance = mediaPlayer;
            }

            public void OnServiceConnected(ComponentName name, IBinder service)
            {
                if (service is MediaPlayerServiceBinder mediaPlayerServiceBinder)
                {
                    var binder = (MediaPlayerServiceBinder)service;
                    instance.binder = binder;

                    binder.GetMediaPlayerService().CoverReloaded += (object sender, EventArgs e) => { instance.CoverReloaded?.Invoke(sender, e); };
                    binder.GetMediaPlayerService().StatusChanged += (object sender, EventArgs e) => { instance.StatusChanged?.Invoke(sender, e); };
                    binder.GetMediaPlayerService().Playing += (object sender, EventArgs e) => { instance.Playing?.Invoke(sender, e); };
                    binder.GetMediaPlayerService().Buffering += (object sender, EventArgs e) => { instance.Buffering?.Invoke(sender, e); };
                }
            }

            public void OnServiceDisconnected(ComponentName name)
            {
            }
        }
    }
}