using Android.Media;

namespace Microsoft.NetConf2021.Maui.Platforms.Android;

public class AudioService : IAudioService
{
    MainActivity instance;

    private MediaPlayer mediaPlayer => (instance != null &&
        instance.binder.GetMediaPlayerService() != null ) ?
        instance.binder.GetMediaPlayerService().mediaPlayer : null;

    public bool IsPlaying => mediaPlayer?.IsPlaying ?? false;

    public double CurrentPosition => mediaPlayer?.CurrentPosition/1000 ?? 0;

    public async Task InitializeAsync(string audioURI)
    {
        if (this.instance == null)
        {
            this.instance = MainActivity.instance;
        }
        else
        {
            await this.instance.binder.GetMediaPlayerService().Stop();
        }

        this.instance.binder.GetMediaPlayerService().AudioUrl = audioURI;
    }

    public Task PauseAsync()
    {
        if (IsPlaying)
        {
            return this.instance.binder.GetMediaPlayerService().Pause();
        }

        return Task.CompletedTask;
    }

    public async Task PlayAsync(double position = 0)
    {
        await this.instance.binder.GetMediaPlayerService().Play();
        await this.instance.binder.GetMediaPlayerService().Seek((int)position * 1000);
    }
}
