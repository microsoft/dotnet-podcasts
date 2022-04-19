using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.NetConf2021.Maui.Platforms.Android.Services;

namespace Microsoft.NetConf2021.Maui;

[Activity(
        Theme = "@style/Maui.SplashTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
public class MainActivity : MauiAppCompatActivity
{
    internal static MainActivity instance;
    public MediaPlayerServiceBinder binder;
    MediaPlayerServiceConnection mediaPlayerServiceConnection;

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
            InitializeMedia();
    }

    private void InitializeMedia()
    {
        mediaPlayerServiceConnection = new MediaPlayerServiceConnection(this);
        var mediaPlayerServiceIntent = new Intent(ApplicationContext, typeof(MediaPlayerService));
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
            if (service is MediaPlayerServiceBinder binder)
            {
                instance.binder = binder;

                var mediaPlayerService = binder.GetMediaPlayerService();
                mediaPlayerService.CoverReloaded += (object sender, EventArgs e) => { instance.CoverReloaded?.Invoke(sender, e); };
                mediaPlayerService.StatusChanged += (object sender, EventArgs e) => { instance.StatusChanged?.Invoke(sender, e); };
                mediaPlayerService.Playing += (object sender, EventArgs e) => { instance.Playing?.Invoke(sender, e); };
                mediaPlayerService.Buffering += (object sender, EventArgs e) => { instance.Buffering?.Invoke(sender, e); };
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
        }
    }
}
