using Android.Content;
using Android.OS;

namespace SharedMauiLib.Platforms.Android
{
    public class MediaPlayerServiceConnection : Java.Lang.Object, IServiceConnection
    {
        readonly IAudioActivity instance;

        public MediaPlayerServiceConnection(IAudioActivity mediaPlayer)
        {
            this.instance = mediaPlayer;
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            if (service is MediaPlayerServiceBinder binder)
            {
                instance.Binder = binder;

                var mediaPlayerService = binder.GetMediaPlayerService();
                //mediaPlayerService.CoverReloaded += (object sender, EventArgs e) => { instance.CoverReloaded?.Invoke(sender, e); };
                //mediaPlayerService.StatusChanged += (object sender, EventArgs e) => { instance.StatusChanged?.Invoke(sender, e); };
                //mediaPlayerService.Playing += (object sender, EventArgs e) => { instance.Playing?.Invoke(sender, e); };
                //mediaPlayerService.Buffering += (object sender, EventArgs e) => { instance.Buffering?.Invoke(sender, e); };
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
        }
    }
}
