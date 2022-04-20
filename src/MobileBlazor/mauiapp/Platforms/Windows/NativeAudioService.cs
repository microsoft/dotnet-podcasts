using NetPodsMauiBlazor.Services;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace NetPodsMauiBlazor.Platforms.Windows;

internal class NativeAudioService : INativeAudioService
{
    string _uri;
    MediaPlayer mediaPlayer;

    public bool IsPlaying => mediaPlayer != null
        && mediaPlayer.CurrentState == MediaPlayerState.Playing;

    public double CurrentPosition => (long)mediaPlayer?.Position.TotalSeconds;

    public async Task InitializeAsync(string audioURI)
    {
        _uri = audioURI;

        if (this.mediaPlayer == null)
        {
            this.mediaPlayer = new MediaPlayer
            {
                Source = MediaSource.CreateFromUri(new Uri(_uri)),
                AudioCategory = MediaPlayerAudioCategory.Media
            };
        }
        if (this.mediaPlayer != null)
        {
            await PauseAsync();
            this.mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(_uri));
        }

    }

    public Task PauseAsync()
    {
        this.mediaPlayer?.Pause();
        return Task.CompletedTask;
    }

    public Task PlayAsync(double position = 0)
    {
        if (this.mediaPlayer != null)
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(position);
            mediaPlayer.Play();
        }

        return Task.CompletedTask;
    }

    public Task SetCurrentTime(double value)
    {
        if (this.mediaPlayer != null)
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(value);
        }

        return Task.CompletedTask;
    }

    public Task SetMuted(bool value)
    {
        if (this.mediaPlayer != null)
        {
            this.mediaPlayer.IsMuted = value;
        }

        return Task.CompletedTask;
    }

    public Task SetVolume(int value)
    {
        if (this.mediaPlayer != null)
        {
            this.mediaPlayer.Volume = value != 0
                ? value / 100d
                : 0;
        }

        return Task.CompletedTask;
    }
}
