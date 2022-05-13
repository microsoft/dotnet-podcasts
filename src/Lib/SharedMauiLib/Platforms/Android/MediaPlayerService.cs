using Android.App;
using Android.Content;
using Android.Media;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;
using Android.Media.Session;
using AndroidNet = Android.Net;
using Android.Graphics;

namespace SharedMauiLib.Platforms.Android;


[Service(Exported = true)]
[IntentFilter(new[] { ActionPlay, ActionPause, ActionStop, ActionTogglePlayback, ActionNext, ActionPrevious })]
public class MediaPlayerService : Service,
   AudioManager.IOnAudioFocusChangeListener,
   MediaPlayer.IOnBufferingUpdateListener,
   MediaPlayer.IOnCompletionListener,
   MediaPlayer.IOnErrorListener,
   MediaPlayer.IOnPreparedListener
{
    //Actions
    public const string ActionPlay = "com.xamarin.action.PLAY";
    public const string ActionPause = "com.xamarin.action.PAUSE";
    public const string ActionStop = "com.xamarin.action.STOP";
    public const string ActionTogglePlayback = "com.xamarin.action.TOGGLEPLAYBACK";
    public const string ActionNext = "com.xamarin.action.NEXT";
    public const string ActionPrevious = "com.xamarin.action.PREVIOUS";

    public MediaPlayer mediaPlayer;
    private AudioManager audioManager;

    private MediaSession mediaSession;
    public MediaController mediaController;

    private WifiManager wifiManager;
    private WifiManager.WifiLock wifiLock;

    public event StatusChangedEventHandler StatusChanged;

    public event CoverReloadedEventHandler CoverReloaded;

    public event PlayingEventHandler Playing;

    public event BufferingEventHandler Buffering;

    public event PlayingChangedEventHandler PlayingChanged;

    public string AudioUrl;

    public bool isCurrentEpisode = true;

    private readonly Handler PlayingHandler;
    private readonly Java.Lang.Runnable PlayingHandlerRunnable;

    private ComponentName remoteComponentName;

    public PlaybackStateCode MediaPlayerState
    {
        get
        {
            return mediaController.PlaybackState != null
                ? mediaController.PlaybackState.State
                : PlaybackStateCode.None;
        }
    }

    public MediaPlayerService()
    {
        PlayingHandler = new Handler(Looper.MainLooper);

        // Create a runnable, restarting itself if the status still is "playing"
        PlayingHandlerRunnable = new Java.Lang.Runnable(() =>
        {
            OnPlaying(EventArgs.Empty);

            if (MediaPlayerState == PlaybackStateCode.Playing)
            {
                PlayingHandler.PostDelayed(PlayingHandlerRunnable, 250);
            }
        });

        // On Status changed to PLAYING, start raising the Playing event
        StatusChanged += (sender, e) =>
        {
            if (MediaPlayerState == PlaybackStateCode.Playing)
            {
                PlayingHandler.PostDelayed(PlayingHandlerRunnable, 0);
            }
        };
    }

    protected virtual void OnStatusChanged(EventArgs e)
    {
        StatusChanged?.Invoke(this, e);
    }

    protected virtual void OnPlayingChanged(bool e)
    {
        PlayingChanged?.Invoke(this, e);
    }

    protected virtual void OnCoverReloaded(EventArgs e)
    {
        if (CoverReloaded != null)
        {
            CoverReloaded(this, e);
            StartNotification();
            UpdateMediaMetadataCompat();
        }
    }

    protected virtual void OnPlaying(EventArgs e)
    {
        Playing?.Invoke(this, e);
    }

    protected virtual void OnBuffering(EventArgs e)
    {
        Buffering?.Invoke(this, e);
    }

    /// <summary>
    /// On create simply detect some of our managers
    /// </summary>
    public override void OnCreate()
    {
        base.OnCreate();
        //Find our audio and notificaton managers
        audioManager = (AudioManager)GetSystemService(AudioService);
        wifiManager = (WifiManager)GetSystemService(WifiService);

        remoteComponentName = new ComponentName(PackageName, new RemoteControlBroadcastReceiver().ComponentName);
    }

    /// <summary>
    /// Will register for the remote control client commands in audio manager
    /// </summary>
    private void InitMediaSession()
    {
        try
        {
            if (mediaSession == null)
            {
                Intent nIntent = new Intent(ApplicationContext, typeof(Activity));

                remoteComponentName = new ComponentName(PackageName, new RemoteControlBroadcastReceiver().ComponentName);

                mediaSession = new MediaSession(ApplicationContext, "MauiStreamingAudio"/*, remoteComponentName*/); //TODO
                mediaSession.SetSessionActivity(PendingIntent.GetActivity(ApplicationContext, 0, nIntent, PendingIntentFlags.Mutable));
                mediaController = new MediaController(ApplicationContext, mediaSession.SessionToken);
            }

            mediaSession.Active = true;
            mediaSession.SetCallback(new MediaSessionCallback((MediaPlayerServiceBinder)binder));

            mediaSession.SetFlags(MediaSessionFlags.HandlesMediaButtons | MediaSessionFlags.HandlesTransportControls);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    /// <summary>
    /// Intializes the player.
    /// </summary>
    private void InitializePlayer()
    {
        mediaPlayer = new MediaPlayer();

        mediaPlayer.SetAudioAttributes(
            new AudioAttributes.Builder()
            .SetContentType(AudioContentType.Music)
            .SetUsage(AudioUsageKind.Media)
                .Build());

        mediaPlayer.SetWakeMode(ApplicationContext, WakeLockFlags.Partial);

        mediaPlayer.SetOnBufferingUpdateListener(this);
        mediaPlayer.SetOnCompletionListener(this);
        mediaPlayer.SetOnErrorListener(this);
        mediaPlayer.SetOnPreparedListener(this);
    }


    public void OnBufferingUpdate(MediaPlayer mp, int percent)
    {
        int duration = 0;
        if (MediaPlayerState == PlaybackStateCode.Playing || MediaPlayerState == PlaybackStateCode.Paused)
            duration = mp.Duration;

        int newBufferedTime = duration * percent / 100;
        if (newBufferedTime != Buffered)
        {
            Buffered = newBufferedTime;
        }
    }

    public async void OnCompletion(MediaPlayer mp)
    {
        await PlayNext();
    }

    public bool OnError(MediaPlayer mp, MediaError what, int extra)
    {
        UpdatePlaybackState(PlaybackStateCode.Error);
        return true;
    }

    public void OnPrepared(MediaPlayer mp)
    {
        mp.Start();
        UpdatePlaybackState(PlaybackStateCode.Playing);
    }

    public int Position
    {
        get
        {
            if (mediaPlayer == null
                || MediaPlayerState != PlaybackStateCode.Playing
                    && MediaPlayerState != PlaybackStateCode.Paused)
                return -1;
            else
                return mediaPlayer.CurrentPosition;
        }
    }

    public int Duration
    {
        get
        {
            if (mediaPlayer == null
                || MediaPlayerState != PlaybackStateCode.Playing
                    && MediaPlayerState != PlaybackStateCode.Paused)
                return 0;
            else
                return mediaPlayer.Duration;
        }
    }

    private int buffered = 0;

    public int Buffered
    {
        get
        {
            if (mediaPlayer == null)
                return 0;
            else
                return buffered;
        }
        private set
        {
            buffered = value;
            OnBuffering(EventArgs.Empty);
        }
    }

    private Bitmap cover;

    public object Cover
    {
        get
        {
            if (cover == null)
                cover = BitmapFactory.DecodeResource(Resources, Resource.Drawable.abc_ab_share_pack_mtrl_alpha); //TODO player_play
            return cover;
        }
        private set
        {
            cover = value as Bitmap;
            OnCoverReloaded(EventArgs.Empty);
        }
    }

    /// <summary>
    /// Intializes the player.
    /// </summary>
    public async Task Play()
    {
        if (mediaPlayer != null && MediaPlayerState == PlaybackStateCode.Paused)
        {
            //We are simply paused so just start again
            mediaPlayer.Start();
            UpdatePlaybackState(PlaybackStateCode.Playing);
            StartNotification();

            //Update the metadata now that we are playing
            UpdateMediaMetadataCompat();
            return;
        }

        if (mediaPlayer == null)
            InitializePlayer();

        if (mediaSession == null)
            InitMediaSession();

        if (mediaPlayer.IsPlaying && isCurrentEpisode)
        {
            UpdatePlaybackState(PlaybackStateCode.Playing);
            return;
        }

        isCurrentEpisode = true;

        await PrepareAndPlayMediaPlayerAsync();
    }

    private async Task PrepareAndPlayMediaPlayerAsync()
    {
        try
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(21))
            {
                MediaMetadataRetriever metaRetriever = new MediaMetadataRetriever();

                await mediaPlayer.SetDataSourceAsync(ApplicationContext, AndroidNet.Uri.Parse(AudioUrl));

                await metaRetriever.SetDataSourceAsync(AudioUrl, new Dictionary<string, string>());

                var focusResult = audioManager.RequestAudioFocus(new AudioFocusRequestClass
                    .Builder(AudioFocus.Gain)
                    .SetOnAudioFocusChangeListener(this)
                    .Build());

                if (focusResult != AudioFocusRequest.Granted)
                {
                    // Could not get audio focus
                    Console.WriteLine("Could not get audio focus");
                }

                UpdatePlaybackState(PlaybackStateCode.Buffering);
                mediaPlayer.PrepareAsync();

                AquireWifiLock();
                UpdateMediaMetadataCompat(metaRetriever);
                StartNotification();

                byte[] imageByteArray = metaRetriever.GetEmbeddedPicture();
                if (imageByteArray == null)
                    Cover = await BitmapFactory.DecodeResourceAsync(Resources, Resource.Drawable.abc_ab_share_pack_mtrl_alpha); //TODO player_play
                else
                    Cover = await BitmapFactory.DecodeByteArrayAsync(imageByteArray, 0, imageByteArray.Length);
            }
        }
        catch (Exception ex)
        {
            UpdatePlaybackStateStopped();

            // Unable to start playback log error
            Console.WriteLine(ex);
        }
    }

    public async Task Seek(int position)
    {
        await Task.Run(() =>
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.SeekTo(position);
            }
        });
    }

    public async Task PlayNext()
    {
        if (mediaPlayer != null)
        {
            mediaPlayer.Reset();
            mediaPlayer.Release();
            mediaPlayer = null;
        }

        UpdatePlaybackState(PlaybackStateCode.SkippingToNext);

        await Play();
    }

    public async Task PlayPrevious()
    {
        // Start current track from beginning if it's the first track or the track has played more than 3sec and you hit "playPrevious".
        if (Position > 3000)
        {
            await Seek(0);
        }
        else
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Reset();
                mediaPlayer.Release();
                mediaPlayer = null;
            }

            UpdatePlaybackState(PlaybackStateCode.SkippingToPrevious);

            await Play();
        }
    }

    public async Task PlayPause()
    {
        if (mediaPlayer == null || mediaPlayer != null && MediaPlayerState == PlaybackStateCode.Paused)
        {
            await Play();
        }
        else
        {
            await Pause();
        }
    }

    public async Task Pause()
    {
        await Task.Run(() =>
        {
            if (mediaPlayer == null)
                return;

            if (mediaPlayer.IsPlaying)
                mediaPlayer.Pause();

            UpdatePlaybackState(PlaybackStateCode.Paused);
        });
    }

    public async Task Stop()
    {
        await Task.Run(() =>
        {
            if (mediaPlayer == null)
                return;

            if (mediaPlayer.IsPlaying)
            {
                mediaPlayer.Stop();
            }

            UpdatePlaybackState(PlaybackStateCode.Stopped);
            mediaPlayer.Reset();
            NotificationHelper.StopNotification(ApplicationContext);
            StopForeground(true);
            ReleaseWifiLock();
            UnregisterMediaSessionCompat();
        });
    }

    public void UpdatePlaybackStateStopped()
    {
        UpdatePlaybackState(PlaybackStateCode.Stopped);

        if (mediaPlayer != null)
        {
            mediaPlayer.Reset();
            mediaPlayer.Release();
            mediaPlayer = null;
        }
    }

    private void UpdatePlaybackState(PlaybackStateCode state)
    {
        if (mediaSession == null || mediaPlayer == null)
            return;

        try
        {
            PlaybackState.Builder stateBuilder = new PlaybackState.Builder()
                .SetActions(
                    PlaybackState.ActionPause |
                    PlaybackState.ActionPlay |
                    PlaybackState.ActionPlayPause |
                    PlaybackState.ActionSkipToNext |
                    PlaybackState.ActionSkipToPrevious |
                    PlaybackState.ActionStop
                )
                .SetState(state, Position, 1.0f, SystemClock.ElapsedRealtime());

            mediaSession.SetPlaybackState(stateBuilder.Build());

            OnStatusChanged(EventArgs.Empty);

            if (state == PlaybackStateCode.Playing || state == PlaybackStateCode.Paused)
            {
                StartNotification();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private void StartNotification()
    {
        if (mediaSession == null)
            return;

        NotificationHelper.StartNotification(
            ApplicationContext,
            mediaController.Metadata,
            mediaSession,
            Cover,
            MediaPlayerState == PlaybackStateCode.Playing);
    }

    internal void SetMuted(bool value)
    {
        mediaPlayer.SetVolume(0, 0);
    }

    internal void SetVolume(int value)
    {
        mediaPlayer.SetVolume(value, value);
    }

    /// <summary>
    /// Updates the metadata on the lock screen
    /// </summary>
    private void UpdateMediaMetadataCompat(MediaMetadataRetriever metaRetriever = null)
    {
        if (mediaSession == null)
            return;

        MediaMetadata.Builder builder = new MediaMetadata.Builder();

        if (metaRetriever != null)
        {
            builder
            .PutString(MediaMetadata.MetadataKeyAlbum, metaRetriever.ExtractMetadata(MetadataKey.Album))
            .PutString(MediaMetadata.MetadataKeyArtist, metaRetriever.ExtractMetadata(MetadataKey.Artist))
            .PutString(MediaMetadata.MetadataKeyTitle, metaRetriever.ExtractMetadata(MetadataKey.Title));
        }
        else
        {
            builder
                .PutString(MediaMetadata.MetadataKeyAlbum, mediaSession.Controller.Metadata.GetString(MediaMetadata.MetadataKeyAlbum))
                .PutString(MediaMetadata.MetadataKeyArtist, mediaSession.Controller.Metadata.GetString(MediaMetadata.MetadataKeyArtist))
                .PutString(MediaMetadata.MetadataKeyTitle, mediaSession.Controller.Metadata.GetString(MediaMetadata.MetadataKeyTitle));
        }
        builder.PutBitmap(MediaMetadata.MetadataKeyAlbumArt, Cover as Bitmap);

        mediaSession.SetMetadata(builder.Build());
    }

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        HandleIntent(intent);
        return base.OnStartCommand(intent, flags, startId);
    }

    private void HandleIntent(Intent intent)
    {
        if (intent == null || intent.Action == null)
            return;

        string action = intent.Action;

        if (action.Equals(ActionPlay))
        {
            mediaController.GetTransportControls().Play();
        }
        else if (action.Equals(ActionPause))
        {
            mediaController.GetTransportControls().Pause();
        }
        else if (action.Equals(ActionPrevious))
        {
            mediaController.GetTransportControls().SkipToPrevious();
        }
        else if (action.Equals(ActionNext))
        {
            mediaController.GetTransportControls().SkipToNext();
        }
        else if (action.Equals(ActionStop))
        {
            mediaController.GetTransportControls().Stop();
        }
    }

    /// <summary>
    /// Lock the wifi so we can still stream under lock screen
    /// </summary>
    private void AquireWifiLock()
    {
        if (wifiLock == null)
        {
            wifiLock = wifiManager.CreateWifiLock(WifiMode.Full, "xamarin_wifi_lock");
        }
        wifiLock.Acquire();
    }

    /// <summary>
    /// This will release the wifi lock if it is no longer needed
    /// </summary>
    private void ReleaseWifiLock()
    {
        if (wifiLock == null)
            return;

        wifiLock.Release();
        wifiLock = null;
    }

    private void UnregisterMediaSessionCompat()
    {
        try
        {
            if (mediaSession != null)
            {
                mediaSession.Dispose();
                mediaSession = null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    IBinder binder;

    public override IBinder OnBind(Intent intent)
    {
        binder = new MediaPlayerServiceBinder(this);
        return binder;
    }

    public override bool OnUnbind(Intent intent)
    {
        NotificationHelper.StopNotification(ApplicationContext);
        return base.OnUnbind(intent);
    }

    /// <summary>
    /// Properly cleanup of your player by releasing resources
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
        if (mediaPlayer != null)
        {
            mediaPlayer.Release();
            mediaPlayer = null;

            NotificationHelper.StopNotification(ApplicationContext);
            StopForeground(true);
            ReleaseWifiLock();
            UnregisterMediaSessionCompat();
        }
    }

    public async void OnAudioFocusChange(AudioFocus focusChange)
    {
        switch (focusChange)
        {
            case AudioFocus.Gain:
                if (mediaPlayer == null)
                    InitializePlayer();

                if (!mediaPlayer.IsPlaying)
                {
                    mediaPlayer.Start();
                }

                mediaPlayer.SetVolume(1.0f, 1.0f);
                break;
            case AudioFocus.Loss:
                //We have lost focus stop!
                await Stop();
                break;
            case AudioFocus.LossTransient:
                //We have lost focus for a short time, but likely to resume so pause
                await Pause();
                break;
            case AudioFocus.LossTransientCanDuck:
                //We have lost focus but should till play at a muted 10% volume
                if (mediaPlayer.IsPlaying)
                    mediaPlayer.SetVolume(.1f, .1f);
                break;
        }
    }

    public class MediaSessionCallback : MediaSession.Callback
    {
        private readonly MediaPlayerServiceBinder mediaPlayerService;
        public MediaSessionCallback(MediaPlayerServiceBinder service)
        {
            mediaPlayerService = service;
        }

        public override async void OnPause()
        {
            mediaPlayerService.GetMediaPlayerService().OnPlayingChanged(false);
            base.OnPause();
        }

        public override async void OnPlay()
        {
            mediaPlayerService.GetMediaPlayerService().OnPlayingChanged(true);
            base.OnPlay();
        }

        public override async void OnSkipToNext()
        {
            await mediaPlayerService.GetMediaPlayerService().PlayNext();
            base.OnSkipToNext();
        }

        public override async void OnSkipToPrevious()
        {
            await mediaPlayerService.GetMediaPlayerService().PlayPrevious();
            base.OnSkipToPrevious();
        }

        public override async void OnStop()
        {
            await mediaPlayerService.GetMediaPlayerService().Stop();
            base.OnStop();
        }
    }
}

public class MediaPlayerServiceBinder : Binder
{
    private readonly MediaPlayerService service;

    public MediaPlayerServiceBinder(MediaPlayerService service)
    {
        this.service = service;
    }

    public MediaPlayerService GetMediaPlayerService()
    {
        return service;
    }
}
