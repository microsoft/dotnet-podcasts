using Microsoft.NetConf2021.Maui.Resources.Strings;

namespace Microsoft.NetConf2021.Maui.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    [QueryProperty(nameof(ShowId), nameof(ShowId))]
    public class EpisodeDetailViewModel : BaseViewModel
    {
        public string Id { get; set; }
        public string ShowId { get; set; }

        Episode episode;
        public Episode Episode
        {
            get => episode;
            set => SetProperty(ref episode, value);
        }

        private bool isInLisenLater;

        public bool IsInListenLater
        {
            get
            {
                return isInLisenLater;
            }
            set
            {
                SetProperty(ref isInLisenLater, value);
            }
        }

        Uri image;
        private Show show;

        public Show Show
        {
            get => show;
            set => SetProperty(ref show, value);
        }
        public Uri Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        public ICommand ShareCommand { get; set; }

        public ICommand PlayCommand { get; set; }

        public ICommand ListenLaterCommand { get; set; }

        private readonly ListenLaterService listenLaterService;
        private readonly ShowsService podcastService;
        private readonly PlayerService playerService;

        public EpisodeDetailViewModel(ListenLaterService listen, ShowsService shows, PlayerService player)
        {
            listenLaterService = listen;
            podcastService = shows;
            playerService = player;
            ShareCommand = new AsyncCommand(ShareCommandExecuteAsync);
            PlayCommand = new AsyncCommand(PlayCommandExecute);
            ListenLaterCommand = new AsyncCommand(ListenLaterCommandExecuteAsync);
        }

        internal async Task InitializeAsync()
        {
            if (Episode != null)
                return;

            await FetchAsync();
        }

        private async Task FetchAsync()
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

        private Task ListenLaterCommandExecuteAsync()
        {
            if (listenLaterService.IsInListenLater(episode))
                listenLaterService.Remove(episode);
            else
                listenLaterService.Add(episode, show);

            IsInListenLater = listenLaterService.IsInListenLater(episode);
            return Task.CompletedTask;
        }

        private Task PlayCommandExecute()
        {
            return playerService.PlayAsync(Episode, Show);
        }

        private Task ShareCommandExecuteAsync()
        {
            var request = new ShareTextRequest
            {
                Text = $"{Config.BaseWeb}show/{show.Id}",
                Title = "Share the episode uri"
            };

            return Share.RequestAsync(request);
        }
    }
}
