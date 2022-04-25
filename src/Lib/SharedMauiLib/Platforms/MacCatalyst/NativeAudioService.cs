using AVFoundation;
using Foundation;

namespace SharedMauiLib.Platforms.MacCatalyst;

public class NativeAudioService : INativeAudioService
{
    AVPlayer avPlayer;
    string _uri;

    public bool IsPlaying => avPlayer != null
        ? avPlayer.TimeControlStatus == AVPlayerTimeControlStatus.Playing
        : false;

    public double CurrentPosition => avPlayer?.CurrentTime.Seconds ?? 0;
    public event EventHandler<bool> IsPlayingChanged;

    public async Task InitializeAsync(string audioURI)
    {
        _uri = audioURI;
        NSUrl fileURL = new NSUrl(_uri.ToString());

        if (avPlayer != null)
        {
            await PauseAsync();
        }

        avPlayer = new AVPlayer(fileURL);
    }

    public Task PauseAsync()
    {
        avPlayer?.Pause();

        return Task.CompletedTask;
    }

    public async Task PlayAsync(double position = 0)
    {
        await avPlayer.SeekAsync(new CoreMedia.CMTime((long)position, 1));
        avPlayer?.Play();
    }

    public Task SetCurrentTime(double value)
    {
        return avPlayer.SeekAsync(new CoreMedia.CMTime((long)value, 1));
    }

    public Task SetMuted(bool value)
    {
        if (avPlayer != null)
        {
            avPlayer.Muted = value;
        }

        return Task.CompletedTask;
    }

    public Task SetVolume(int value)
    {
        if (avPlayer != null)
        {
            avPlayer.Volume = value;
        }

        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        avPlayer?.Dispose();
        return ValueTask.CompletedTask;
    }
}
