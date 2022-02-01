using Windows.Media.Core;
using Windows.Media.Playback;

namespace Microsoft.NetConf2021.Maui.Platforms.Windows;

public class AudioService : IAudioService
{
    string _uri;
    MediaPlayer mediaPlayer;

    public bool IsPlaying => mediaPlayer != null
        && mediaPlayer.CurrentState == MediaPlayerState.Playing;

    public double CurrentPosition => (long)mediaPlayer?.Position.TotalSeconds;

    public async Task InitializeAsync(string audioURI)
    {
        _uri = audioURI;

        if(this.mediaPlayer == null)
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
}
