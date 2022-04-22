using Android.Media;
using AndroidApp = Android.App;

namespace SharedMauiLib.Platforms.Android
{
    public class NativeAudioService : INativeAudioService
    {
        IAudioActivity instance;

        private MediaPlayer mediaPlayer => instance != null &&
            instance.Binder.GetMediaPlayerService() != null ?
            instance.Binder.GetMediaPlayerService().mediaPlayer : null;

        public bool IsPlaying => mediaPlayer?.IsPlaying ?? false;

        public double CurrentPosition => mediaPlayer?.CurrentPosition / 1000 ?? 0;
        public event EventHandler<bool> IsPlayingChanged;

        public Task InitializeAsync(string audioURI)
        {
            if (instance == null)
            {
                var activity = CurrentActivity.CrossCurrentActivity.Current;
                instance = activity.Activity as IAudioActivity;
            }
            else
            {
                instance.Binder.GetMediaPlayerService().isCurrentEpisode = false;
                instance.Binder.GetMediaPlayerService().UpdatePlaybackStateStopped();
            }

            this.instance.Binder.GetMediaPlayerService().PlayingChanged += (object sender, bool e) =>
            {
                Task.Run(async () => {
                    if (e)
                    {
                        await this.PlayAsync();
                    }
                    else
                    {
                        await this.PauseAsync();
                    }
                });
                IsPlayingChanged?.Invoke(this, e);
            };

            instance.Binder.GetMediaPlayerService().AudioUrl = audioURI;

            return Task.CompletedTask;
        }

        public Task PauseAsync()
        {
            if (IsPlaying)
            {
                return instance.Binder.GetMediaPlayerService().Pause();
            }

            return Task.CompletedTask;
        }

        public async Task PlayAsync(double position = 0)
        {
            await instance.Binder.GetMediaPlayerService().Play();
            await instance.Binder.GetMediaPlayerService().Seek((int)position * 1000);
        }

        public Task SetMuted(bool value)
        {
            instance?.Binder.GetMediaPlayerService().SetMuted(value);

            return Task.CompletedTask;
        }

        public Task SetVolume(int value)
        {
            instance?.Binder.GetMediaPlayerService().SetVolume(value);

            return Task.CompletedTask;
        }

        public Task SetCurrentTime(double position)
        {
            return instance.Binder.GetMediaPlayerService().Seek((int)position * 1000);
        }

        public ValueTask DisposeAsync()
        {
            instance.Binder?.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
