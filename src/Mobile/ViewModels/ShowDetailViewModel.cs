using Microsoft.NetConf2021.Maui.Resources.Strings;
using MvvmHelpers;

namespace Microsoft.NetConf2021.Maui.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ShowDetailViewModel : ViewModelBase
{
    private readonly PlayerService playerService;
    private readonly SubscriptionsService subscriptionsService;
    private readonly ListenLaterService listenLaterService;
    private readonly ShowsService showsService;
    private readonly ImageProcessingService imageProcessingService;

    private Guid showId;

    public string Id { get; set; }

    [ObservableProperty]
    ShowViewModel show;

    [ObservableProperty]
    Episode episodeForPlaying;

    [ObservableProperty]
    ObservableRangeCollection<Episode> episodes;

    [ObservableProperty]
    bool isPlaying;

    [ObservableProperty]
    string textToSearch;

    public ShowDetailViewModel(ShowsService shows, PlayerService player, SubscriptionsService subs, ListenLaterService later, ImageProcessingService imageProcessing)
    {
        showsService = shows;
        playerService = player;
        subscriptionsService = subs;
        listenLaterService = later;
        imageProcessingService = imageProcessing;

        episodes = new ObservableRangeCollection<Episode>();
    }

    internal async Task InitializeAsync()
    {
        if (Id != null)
        {
            showId = new Guid(Id);
        }
        
        await FetchAsync();
    }

    async Task FetchAsync()
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

        var showVM = new ShowViewModel(show, subscriptionsService.IsSubscribed(show.Id), imageProcessingService);

        Show = showVM;
        Show.InitializeCommand.Execute(null);
        Episodes.ReplaceRange(show.Episodes.ToList());
    }

    [RelayCommand]
    void SearchEpisode()
    {
        var episodesList = show.Episodes
            .Where(ep => ep.Title.Contains(TextToSearch, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
        Episodes.ReplaceRange(episodesList);
    }

    [RelayCommand]
    Task TapEpisode(Episode episode) => Shell.Current.GoToAsync($"{nameof(EpisodeDetailPage)}?Id={episode.Id}&ShowId={showId}");

    [RelayCommand]
    async Task Subscribe()
    {
        if (Show is null || subscriptionsService is null)
            return;

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

    [RelayCommand]
    Task PlayEpisode(Episode episode) => playerService.PlayAsync(episode, Show.Show);

    [RelayCommand]
    Task AddToListenLater(Episode episode)
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
