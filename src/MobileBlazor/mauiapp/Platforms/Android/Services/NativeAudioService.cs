using NetPodsMauiBlazor.Services;
using Android.Media;

namespace NetPodsMauiBlazor.Platforms.Android.Services;

internal class NativeAudioService : INativeAudioService
{
    MainActivity instance;

    private MediaPlayer mediaPlayer => instance != null &&
        instance.binder.GetMediaPlayerService() != null ?
        instance.binder.GetMediaPlayerService().mediaPlayer : null;

    public bool IsPlaying => mediaPlayer?.IsPlaying ?? false;

    public double CurrentPosition => mediaPlayer?.CurrentPosition / 1000 ?? 0;

    public Task InitializeAsync(string audioURI)
    {
        if (instance == null)
        {
            instance = MainActivity.instance;
        }
        else
        {
            instance.binder.GetMediaPlayerService().isCurrentEpisode = false;
            instance.binder.GetMediaPlayerService().UpdatePlaybackStateStopped();
        }

        instance.binder.GetMediaPlayerService().AudioUrl = audioURI;

        return Task.CompletedTask;
    }

    public Task PauseAsync()
    {
        if (IsPlaying)
        {
            return instance.binder.GetMediaPlayerService().Pause();
        }

        return Task.CompletedTask;
    }

    public async Task PlayAsync(double position = 0)
    {
        await instance.binder.GetMediaPlayerService().Play();
        await instance.binder.GetMediaPlayerService().Seek((int)position * 1000);
    }

    public Task SetMuted(bool value)
    {
        instance?.binder.GetMediaPlayerService().SetMuted(value);

        return Task.CompletedTask;
    }

    public Task SetVolume(int value)
    {
        instance?.binder.GetMediaPlayerService().SetVolume(value);

        return Task.CompletedTask;
    }

    public Task SetCurrentTime(double position)
    {
        return instance.binder.GetMediaPlayerService().Seek((int)position * 1000);
    }

    public ValueTask DisposeAsync()
    {
        instance.binder?.Dispose();
        return ValueTask.CompletedTask;
    }
}
