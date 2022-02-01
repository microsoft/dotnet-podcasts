namespace Microsoft.NetConf2021.Maui.ViewModels;

public class ListenLaterViewModel : BaseViewModel
{
    public bool HasData => Episodes?.Any() ?? false;
    public bool HasNoData => !HasData;

    private readonly ListenLaterService listenLaterService;
    private readonly PlayerService playerService;

    private ObservableCollection<EpisodeViewModel> episodes;

    public ObservableCollection<EpisodeViewModel> Episodes
    {
        get { return episodes; }
        set {  SetProperty(ref episodes, value); }  
    }

    public ICommand RemoveCommand => new MvvmHelpers.Commands.Command<EpisodeViewModel>(RemoveCommandExecute);

    public ListenLaterViewModel(ListenLaterService listen, PlayerService player)
    {
        listenLaterService = listen;
        playerService = player;
        Episodes = new ObservableCollection<EpisodeViewModel>();
    }

    internal async Task InitializeAsync()
    {
        var episodes = listenLaterService.GetEpisodes();
        Episodes.Clear();
        foreach (var episode in episodes)
        {
            var episodeVM = new EpisodeViewModel(episode.Item1, episode.Item2, listenLaterService, playerService);
            await episodeVM.InitializeAsync();

            Episodes.Add(episodeVM);
        }
        OnPropertyChanged(nameof(HasData));
        OnPropertyChanged(nameof(HasNoData));
    }

    private void RemoveCommandExecute(EpisodeViewModel episode)
    {
        var episodeToRemove = Episodes.Where(ep => ep.Episode.Id == episode.Episode.Id).FirstOrDefault();
        if(episodeToRemove != null)
        {
            listenLaterService.Remove(episode.Episode);
            Episodes.Remove(episodeToRemove);
        }
        OnPropertyChanged(nameof(HasData));
        OnPropertyChanged(nameof(HasNoData));
    }
}

