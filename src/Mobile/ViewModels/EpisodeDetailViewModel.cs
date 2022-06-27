using Microsoft.NetConf2021.Maui.Resources.Strings;

namespace Microsoft.NetConf2021.Maui.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
[QueryProperty(nameof(ShowId), nameof(ShowId))]
public partial class EpisodeDetailViewModel : ViewModelBase
{
    public string Id { get; set; }
    public string ShowId { get; set; }


    [ObservableProperty]
    Episode episode;

    public bool IsInListenLater
    {
        get => episode.IsInListenLater;
        set
        {
            episode.IsInListenLater = value;
            OnPropertyChanged();
        }
    }

    [ObservableProperty]
    Uri image;

    [ObservableProperty]
    private Show show;

    private readonly ListenLaterService listenLaterService;
    private readonly ShowsService podcastService;
    private readonly PlayerService playerService;

    public EpisodeDetailViewModel(ListenLaterService listen, ShowsService shows, PlayerService player)
    {
        listenLaterService = listen;
        podcastService = shows;
        playerService = player;
    }

    internal async Task InitializeAsync()
    {
        if (Episode != null)
            return;

        await FetchAsync();
    }

    async Task FetchAsync()
    {
        Show = await podcastService.GetShowByIdAsync(new Guid(ShowId));
        var eId = new Guid(Id);
        Episode = Show.Episodes.FirstOrDefault(e => e.Id == eId);

        if (Show == null || Episode == null)
        {
            await Shell.Current.DisplayAlert(
                AppResource.Error_Title,
                AppResource.Error_Message,
                AppResource.Close);

            return;
        }

        Image = Show.Image;
        IsInListenLater = listenLaterService.IsInListenLater(Episode);
    }

    [RelayCommand]
    Task ListenLater()
    {
        if (listenLaterService.IsInListenLater(episode))
            listenLaterService.Remove(episode);
        else
            listenLaterService.Add(episode, show);

        IsInListenLater = listenLaterService.IsInListenLater(episode);
        Show.Episodes.FirstOrDefault(x => x.Id == episode.Id).IsInListenLater = IsInListenLater;
        return Task.CompletedTask;
    }

    [RelayCommand]
    Task Play() => playerService.PlayAsync(Episode, Show);

    [RelayCommand]
    Task Share() => 
        Microsoft.Maui.ApplicationModel.DataTransfer.Share.RequestAsync(new ShareTextRequest
    {
        Text = $"{Config.BaseWeb}show/{show.Id}",
        Title = "Share the episode uri"
    });
}
