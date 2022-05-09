namespace Microsoft.NetConf2021.Maui.ViewModels;

public class ListenLaterViewModel : BaseViewModel
{
    private readonly ListenLaterService listenLaterService;
    private readonly PlayerService playerService;

    private ObservableRangeCollection<EpisodeViewModel> episodes;

    public ObservableRangeCollection<EpisodeViewModel> Episodes
    {
        get { return episodes; }
        set {  SetProperty(ref episodes, value); }  
    }

    public ICommand RemoveCommand { get; private set; }

    public ListenLaterViewModel(ListenLaterService listen, PlayerService player)
    {
        listenLaterService = listen;
        playerService = player;
        Episodes = new ObservableRangeCollection<EpisodeViewModel>();
        RemoveCommand = new MvvmHelpers.Commands.Command<EpisodeViewModel>(RemoveCommandExecute);
    }

    internal Task InitializeAsync()
    {
        var episodes = listenLaterService.GetEpisodes();
        var list = new List<EpisodeViewModel>();
        foreach (var episode in episodes)
        {
            var episodeVM = new EpisodeViewModel(episode.Item1, episode.Item2, listenLaterService, playerService);

            list.Add(episodeVM);
        }
        Episodes.ReplaceRange(list);

        return Task.CompletedTask;
    }

    private void RemoveCommandExecute(EpisodeViewModel episode)
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

