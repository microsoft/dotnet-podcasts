using SharedMauiLib;

namespace Microsoft.NetConf2021.Maui.Services;

public class PlayerService
{
    private readonly INativeAudioService audioService;
    private readonly WifiOptionsService wifiOptionsService;

    public Episode CurrentEpisode { get; set; }
    public Show CurrentShow { get; set; }

    public bool IsPlaying { get; set; }
    public double CurrentPosition => audioService.CurrentPosition;

    public event EventHandler NewEpisodeAdded;
    public event EventHandler IsPlayingChanged;

    public PlayerService(INativeAudioService audioService, WifiOptionsService wifiOptionsService)
    {
        this.audioService = audioService;
        this.wifiOptionsService = wifiOptionsService;

        this.audioService.IsPlayingChanged += (object sender, bool e) =>
        {
            IsPlaying = e;
            IsPlayingChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    public async Task PlayAsync(Episode episode, Show show, bool isPlaying, double position = 0)
    {
        if (episode == null) { return; }

        var isOtherEpisode = CurrentEpisode?.Id != episode.Id;

        CurrentShow = show;

        if (isOtherEpisode)
        {
            CurrentEpisode = episode;

            if (audioService.IsPlaying)
            {
                await InternalPauseAsync();
            }

            await audioService.InitializeAsync(CurrentEpisode.Url.ToString());

            await InternalPlayPauseAsync(isPlaying, position);

            NewEpisodeAdded?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            await InternalPlayPauseAsync(isPlaying, position);
        }

        IsPlayingChanged?.Invoke(this, EventArgs.Empty);
    }


    public Task PlayAsync(Episode episode, Show show)
    {
        var isOtherEpisode = CurrentEpisode?.Id != episode.Id;
        var isPlaying = isOtherEpisode || !audioService.IsPlaying;
        var position = isOtherEpisode ? 0 : CurrentPosition;

        if (CurrentEpisode != null)
        {
            if (isPlaying)
            {
                SemanticScreenReader.Announce(string.Format("Episode with title {0} will start playing", CurrentEpisode.Title));
            }
            else
            {
                SemanticScreenReader.Announce(string.Format("Episode with title {0} will be paused", CurrentEpisode.Title));
            }
        }

        return PlayAsync(episode, show, isPlaying, position);
    }

    public async Task resumeEpisode(double position)
    {
        await audioService.InitializeAsync(CurrentEpisode.Url.ToString());

        await InternalPlayPauseAsync(true, position);

        IsPlayingChanged?.Invoke(this, EventArgs.Empty);
    }

    private async Task InternalPlayPauseAsync(bool isPlaying, double position)
    {
        if (isPlaying)
        {
            await InternalPlayAsync(position);
        }
        else
        {
            await InternalPauseAsync();
        }
    }

    private async Task InternalPauseAsync()
    {
        await audioService.PauseAsync();
        IsPlaying = false;
    }

    private async Task InternalPlayAsync(double position = 0)
    {
        var canPlay = await wifiOptionsService.HasWifiOrCanPlayWithOutWifiAsync();

        if (!canPlay)
        {
            return;
        }

        await audioService.PlayAsync(position);
        IsPlaying = true;
    }
}
