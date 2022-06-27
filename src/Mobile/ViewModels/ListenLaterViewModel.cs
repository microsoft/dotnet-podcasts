using MvvmHelpers;

namespace Microsoft.NetConf2021.Maui.ViewModels;

public partial class ListenLaterViewModel : ViewModelBase
{
    readonly ListenLaterService listenLaterService;
    readonly PlayerService playerService;

    [ObservableProperty]
    ObservableRangeCollection<EpisodeViewModel> episodes;

    public ListenLaterViewModel(ListenLaterService listen, PlayerService player)
    {
        listenLaterService = listen;
        playerService = player;
        Episodes = new ObservableRangeCollection<EpisodeViewModel>();
    }

    internal Task InitializeAsync()
    {
        var episodes = listenLaterService.GetEpisodes();
        var list = new List<EpisodeViewModel>();
        foreach (var episode in episodes)
        {
            var episodeVM = new EpisodeViewModel(episode.Item1, episode.Item2, playerService);

            list.Add(episodeVM);
        }
        Episodes.ReplaceRange(list);

        return Task.CompletedTask;
    }

    [RelayCommand]
    void Remove(EpisodeViewModel episode)
    {
        var episodeToRemove = Episodes
            .FirstOrDefault(ep => ep.Episode.Id == episode.Episode.Id);
        if(episodeToRemove != null)
        {
            listenLaterService.Remove(episode.Episode);
            Episodes.Remove(episodeToRemove);
            episode.IsInListenLater = false;
        }
    }
}

