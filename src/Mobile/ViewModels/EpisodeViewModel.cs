namespace Microsoft.NetConf2021.Maui.ViewModels;

public class EpisodeViewModel : BaseViewModel
{
    private readonly ListenLaterService listenLaterService;
    private readonly PlayerService playerService;
    private Episode episode;
    private bool isForListenLater;

    public bool IsForListenLater
    {
        get { return isForListenLater; }
        set { SetProperty(ref isForListenLater, value); }   
    }    

    public Episode Episode
    {
        get { return episode; }
        set {  SetProperty(ref episode, value); }
    }

    public Show Show { get; set; }

    public ICommand PlayEpisodeCommand => new AsyncCommand(PlayEpisodeCommandExecute);

    public EpisodeViewModel(Episode episode, Show show, ListenLaterService listen, PlayerService player)
    {
        listenLaterService = listen;
        playerService = player;

        Episode = episode;
        Show = show;
    }

    internal Task InitializeAsync()
    {
        this.IsForListenLater = listenLaterService.IsInListenLater(episode);
        return Task.CompletedTask;
    }

    private Task PlayEpisodeCommandExecute()
    {
        return playerService.PlayAsync(Episode, Show);
    }
}
