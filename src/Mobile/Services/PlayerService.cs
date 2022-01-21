namespace Microsoft.NetConf2021.Maui.Services;

public class PlayerService
{
    private readonly IAudioService audioService;
    private readonly WifiOptionsService wifiOptionsService;

    public Episode CurrentEpisode { get; set; }
    public Show CurrentShow { get; set; }

    public bool IsPlaying { get; set; }
    public double CurrentPosition => audioService.CurrentPosition;

    public event EventHandler NewEpisodeAdded;
    public event EventHandler IsPlayingChanged;

    public PlayerService(IAudioService audioService, WifiOptionsService wifiOptionsService)
    {
        this.audioService = audioService;
        this.wifiOptionsService = wifiOptionsService;
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

            if (isPlaying)
            {
                await InternalPlayAsync(initializePlayer: false, position);
            }
            else
            {
                await InternalPauseAsync();
            }

            NewEpisodeAdded?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            if (isPlaying)
            {
                await InternalPlayAsync(initializePlayer: false, position);
            }
            else
            {
                await InternalPauseAsync();
            }
        }

        IsPlayingChanged?.Invoke(this, EventArgs.Empty);
    }


    public Task PlayAsync(Episode episode, Show show)
    {
        var isOtherEpisode = CurrentEpisode?.Id != episode.Id;
        var isPlaying = isOtherEpisode || !audioService.IsPlaying;
        var position = isOtherEpisode ? 0 : CurrentPosition;

        return PlayAsync(episode, show, isPlaying, position);
    }

    private async Task InternalPauseAsync()
    {
        await audioService.PauseAsync();
        IsPlaying = false;
    }

    private async Task InternalPlayAsync(bool initializePlayer = false, double position = 0)
    {
        var canPlay = await wifiOptionsService.HasWifiOrCanPlayWithOutWifiAsync();

        if (!canPlay)
        {
            return;
        }

        if (initializePlayer)
        {
            await audioService.InitializeAsync(CurrentEpisode.Url.ToString());
        }

        await audioService.PlayAsync(position);
        IsPlaying = true;
    }
}
