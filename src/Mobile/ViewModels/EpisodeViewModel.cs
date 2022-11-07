namespace Microsoft.NetConf2021.Maui.ViewModels;

public partial class EpisodeViewModel : ViewModelBase
{
    readonly PlayerService playerService;

    [ObservableProperty]
    Episode episode;

    public bool IsInListenLater
    {
        get
        {
            return episode.IsInListenLater;
        }
        set
        {
            episode.IsInListenLater = value;
            OnPropertyChanged();
        }
    }

    public Show Show { get; set; }

    public EpisodeViewModel(
        Episode episode,
        Show show,
        PlayerService player)
    {
        playerService = player;

        Episode = episode;
        Show = show;
    }

    [RelayCommand]
    Task PlayEpisode() => playerService.PlayAsync(Episode, Show);

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"{nameof(EpisodeDetailPage)}?Id={episode.Id}&ShowId={Show.Id}");
}
