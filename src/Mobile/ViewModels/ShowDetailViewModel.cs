using Microsoft.NetConf2021.Maui.Resources.Strings;

namespace Microsoft.NetConf2021.Maui.ViewModels
{
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

        private List<Episode> episodes;

        public List<Episode> Episodes
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

        public ICommand PlayEpisodeCommand => new AsyncCommand<Episode>(PlayEpisodeCommandExecute);
        public ICommand TapEpisodeCommand => new AsyncCommand<Episode>(TapEpisodeCommandExecute);
        public ICommand SubscribeCommand => new AsyncCommand(SubscribeCommandExecute);
        public ICommand NavigateToPlayerCommand => new AsyncCommand<Episode>(NavigateToPlayerCommandExecute);
        public ICommand AddToListenLaterCommand => new AsyncCommand<Episode>(AddToListenLaterCommandExecute);
        public ICommand SearchEpisodeCommand => new MvvmHelpers.Commands.Command(OnSearchCommand);

        private void OnSearchCommand()
        {
            Episodes = show.Episodes.Where(ep=>ep.Title.Contains(TextToSearch, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public ShowDetailViewModel(ShowsService shows, PlayerService player, SubscriptionsService subs, ListenLaterService later)
        {
            showsService = shows;
            playerService = player;
            subscriptionsService = subs;
            listenLaterService = later;

            MessagingCenter.Instance.Subscribe<string>(".NET Pods", "UnSubscribe", async (sender) =>
            {
                await Show.InitializeAsync();

                OnPropertyChanged(nameof(Show));
            });
        }

        internal async Task InitializeAsync()
        {
            if (Show != null)
                return;

            showId = new Guid(Id);
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

            var showVM = new ShowViewModel(show, subscriptionsService);
            await showVM.InitializeAsync();

            Show = showVM;
            Episodes = show.Episodes.ToList();
        }

        private async Task NavigateToPlayerCommandExecute(Episode episode)
        {
          
        }

        private async Task TapEpisodeCommandExecute(Episode episode)
        {
            await Shell.Current.GoToAsync($"{nameof(EpisodeDetailPage)}?Id={episode.Id}&ShowId={showId}");
        }

        private async Task SubscribeCommandExecute()
        {
            await subscriptionsService.SubscribeToShowAsync(Show.Show);
            Show.IsSubscribed = subscriptionsService.IsSubscribed(Show.Show.Id);
        }

        private async Task PlayEpisodeCommandExecute(Episode episode)
        {   
            await playerService.PlayAsync(episode, Show.Show);
        }

        private Task AddToListenLaterCommandExecute(Episode episode)
        {
            if (listenLaterService.IsInListenLater(episode))
                listenLaterService.Remove(episode);
            else
                listenLaterService.Add(episode, Show.Show);

            episode.IsInListenLater = listenLaterService.IsInListenLater(episode);
            return Task.CompletedTask;
        }
    }
}
