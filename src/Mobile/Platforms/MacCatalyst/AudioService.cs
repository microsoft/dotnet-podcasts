using AVFoundation;
using Foundation;

namespace Microsoft.NetConf2021.Maui.Platforms.MacCatalyst;

public class AudioService : IAudioService
{
    AVPlayer avPlayer;
    string _uri;

    public bool IsPlaying => avPlayer != null
        ? avPlayer.TimeControlStatus == AVPlayerTimeControlStatus.Playing
        : false; //TODO
        
    public double CurrentPosition => avPlayer?.CurrentTime.Seconds ?? 0;

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
}
