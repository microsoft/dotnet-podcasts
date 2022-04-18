using AVFoundation;
using Foundation;
using NetPodsMauiBlazor.Services;

namespace NetPodsMauiBlazor.Platforms.iOS.Services
{
    internal class NativeAudioService : INativeAudioService
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
}
