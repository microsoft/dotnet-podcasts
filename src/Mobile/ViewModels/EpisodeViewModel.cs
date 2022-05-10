using MvvmHelpers.Interfaces;

namespace Microsoft.NetConf2021.Maui.ViewModels;

public class EpisodeViewModel : BaseViewModel
{
    private readonly PlayerService playerService;
    private Episode episode;

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

    public Episode Episode
    {
        get { return episode; }
        set { SetProperty(ref episode, value); }
    }

    public Show Show { get; set; }

    public IAsyncCommand PlayEpisodeCommand { get; private set; }

    public IAsyncCommand NavigateToDetailCommand { get; private set; }

    public EpisodeViewModel(
        Episode episode,
        Show show,
        PlayerService player)
    {
        playerService = player;

        Episode = episode;
        Show = show;
        PlayEpisodeCommand = new AsyncCommand(PlayEpisodeCommandExecute);
        NavigateToDetailCommand = new AsyncCommand(NavigateToDetailCommandExecute);
    }

    private Task PlayEpisodeCommandExecute() => playerService.PlayAsync(Episode, Show);

    private Task NavigateToDetailCommandExecute() => Shell.Current.GoToAsync($"{nameof(EpisodeDetailPage)}?Id={episode.Id}&ShowId={Show.Id}");
}
