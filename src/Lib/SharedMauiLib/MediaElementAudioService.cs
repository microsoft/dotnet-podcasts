using CommunityToolkit.Maui.Views;

namespace SharedMauiLib;

public class MediaElementAudioService : INativeAudioService
{
    public static MediaElement MediaElement { get; set; }
    double volume = 1.0;
    public bool IsPlaying => MediaElement?.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing;

    public double CurrentPosition => MediaElement?.Position.Seconds ?? 0;

    public event EventHandler<bool> IsPlayingChanged;

    public ValueTask DisposeAsync()
    {
        MediaElement?.Handler?.DisconnectHandler();
        MediaElement = null;
        return ValueTask.CompletedTask;
    }

    public Task InitializeAsync(string audioURI)
    {

        if (MediaElement is not null)
        {
            MediaElement.Source = UriMediaSource.FromUri(audioURI);
            MediaElement.StateChanged += MediaElement_StateChanged;
        }

        return Task.CompletedTask;
    }

    private void MediaElement_StateChanged(object sender, CommunityToolkit.Maui.Core.Primitives.MediaStateChangedEventArgs e)
    {
        IsPlayingChanged?.Invoke(this, IsPlaying);
    }

    public Task PauseAsync()
    {
        MediaElement?.Pause();
        return Task.CompletedTask;
    }

    public Task PlayAsync(double position = 0)
    {
        MediaElement?.SeekTo(TimeSpan.FromSeconds(position));
        MediaElement?.Play();

        return Task.CompletedTask;
    }

    public Task SetCurrentTime(double value)
    {
        MediaElement?.SeekTo(TimeSpan.FromSeconds(value));
        return Task.CompletedTask;
    }

    
    public Task SetMuted(bool value)
    {
        if (MediaElement is not null)
        {
            if (MediaElement.Volume != 0)
                volume = MediaElement.Volume;

            MediaElement.Volume = value ? 0 : volume;
        }
        return Task.CompletedTask;
    }

    public Task SetVolume(int value)
    {
        if (MediaElement is not null)
            MediaElement.Volume = value / 100d;
        return Task.CompletedTask;
    }
}
