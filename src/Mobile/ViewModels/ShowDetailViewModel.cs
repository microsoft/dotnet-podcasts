using Microsoft.NetConf2021.Maui.Resources.Strings;
using MvvmHelpers.Interfaces;

namespace Microsoft.NetConf2021.Maui.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public class ShowDetailViewModel : BaseViewModel
{
    public string Id { get; set; }
    private Guid showId;

    private readonly PlayerService playerService;
    private readonly SubscriptionsService subscriptionsService;
    private readonly ListenLaterService listenLaterService;
    private readonly ShowsService showsService;

    private ShowViewModel show;
    public ShowViewModel Show
    {
        get => show;
        set => SetProperty(ref show, value);
    }

    private Episode episodeForPlaying;
    public Episode EpisodeForPlaying
    {
        get => episodeForPlaying;
        set => SetProperty(ref episodeForPlaying, value);
    }

    private ObservableRangeCollection<Episode> episodes;

    public ObservableRangeCollection<Episode> Episodes
    {
        get => episodes;
        set => SetProperty(ref episodes, value);
    }

    bool isPlaying;
    private string textToSearch;

    public bool IsPlaying
    {
        get => isPlaying;
        set => SetProperty(ref isPlaying, value);
    }

    public string TextToSearch
    {
        get { return textToSearch; }
        set
        {
            SetProperty(ref textToSearch, value);
        }
    }

    public IAsyncCommand<Episode> PlayEpisodeCommand { get; private set; }
    public IAsyncCommand<Episode> TapEpisodeCommand { get; private set; }
    public IAsyncCommand SubscribeCommand { get; private set; }
    public IAsyncCommand<Episode> AddToListenLaterCommand { get; private set; }
    public ICommand SearchEpisodeCommand { get; private set; }

    private void OnSearchCommand()
    {
        var episodesList = show.Episodes
            .Where(ep => ep.Title.Contains(TextToSearch, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
        Episodes.ReplaceRange(episodesList);
    }

    public ShowDetailViewModel(ShowsService shows, PlayerService player, SubscriptionsService subs, ListenLaterService later)
    {
        showsService = shows;
        playerService = player;
        subscriptionsService = subs;
        listenLaterService = later;
        episodes = new ObservableRangeCollection<Episode>();

        PlayEpisodeCommand = new AsyncCommand<Episode>(PlayEpisodeCommandExecute);
        TapEpisodeCommand = new AsyncCommand<Episode>(TapEpisodeCommandExecute);
        SubscribeCommand = new AsyncCommand(SubscribeCommandExecute);
        AddToListenLaterCommand = new AsyncCommand<Episode>(AddToListenLaterCommandExecute);
        SearchEpisodeCommand = new MvvmHelpers.Commands.Command(OnSearchCommand);
    }

    internal async Task InitializeAsync()
    {
        if (Id != null)
        {
            showId = new Guid(Id);
        }
        
        await FetchAsync();
    }

    private async Task FetchAsync()
    {
        var show = await showsService.GetShowByIdAsync(showId);

        if (show == null)
        {
            await Shell.Current.DisplayAlert(
                      AppResource.Error_Title,
                      AppResource.Error_Message,
                      AppResource.Close);

            return;
        }

        var showVM = new ShowViewModel(show, subscriptionsService.IsSubscribed(show.Id));

        Show = showVM;
        Episodes.ReplaceRange(show.Episodes.ToList());
    }

    private async Task TapEpisodeCommandExecute(Episode episode)
    {
        await Shell.Current.GoToAsync($"{nameof(EpisodeDetailPage)}?Id={episode.Id}&ShowId={showId}");
    }

    private async Task SubscribeCommandExecute()
    {
        if (Show.IsSubscribed)
        {
            var isUnsubcribe = await subscriptionsService.UnSubscribeFromShowAsync(Show.Show);
            Show.IsSubscribed = !isUnsubcribe;
        }
        else
        {
            subscriptionsService.SubscribeToShow(Show.Show);
            Show.IsSubscribed = true;
        }
    }

    private async Task PlayEpisodeCommandExecute(Episode episode)
    {
        await playerService.PlayAsync(episode, Show.Show);
    }

    private Task AddToListenLaterCommandExecute(Episode episode)
    {
        var itemHasInListenLaterList = listenLaterService.IsInListenLater(episode);
        if (itemHasInListenLaterList)
        {
            listenLaterService.Remove(episode);
        }
        else
        {
            listenLaterService.Add(episode, Show.Show);
        }

        episode.IsInListenLater = !itemHasInListenLaterList;

        return Task.CompletedTask;
    }
}
