using MvvmHelpers.Interfaces;

namespace Microsoft.NetConf2021.Maui.ViewModels;

public class SubscriptionsViewModel : BaseViewModel
{
    private readonly SubscriptionsService subscriptionsService;

    private ObservableRangeCollection<ShowViewModel> subscribedShows;

    public ObservableRangeCollection<ShowViewModel> SubscribedShows
    {
        get
        {
            return this.subscribedShows;
        }
        set
        {
            this.SetProperty(ref this.subscribedShows, value);
        }
    }

    public IAsyncCommand<ShowViewModel> SubscribeCommand { get; private set; }

    public IAsyncCommand NavigateToDiscoverCommand { get; private set; }

    public SubscriptionsViewModel(SubscriptionsService subs)
    {
        subscriptionsService = subs;
        subscribedShows = new ObservableRangeCollection<ShowViewModel>();
        NavigateToDiscoverCommand = new AsyncCommand(NavigateToDiscoverCommandExecute);
        SubscribeCommand = new AsyncCommand<ShowViewModel>(SubscribeCommandExecute);
    }

    private Task NavigateToDiscoverCommandExecute()
    {
        return Shell.Current.GoToAsync($"{nameof(DiscoverPage)}");
    }

    public Task InitializeAsync()
    {
        var shows = subscriptionsService.GetSubscribedShows();

        var list = new List<ShowViewModel>();
        foreach (var show in shows)
        {
            var showViewModel = new ShowViewModel(show, subscriptionsService.IsSubscribed(show.Id));
            list.Add(showViewModel);
        }
        SubscribedShows.ReplaceRange(list);

        return Task.CompletedTask;
    }

    private async Task SubscribeCommandExecute(ShowViewModel showViewModel)
    {
        var podcastToRemove = SubscribedShows
            .FirstOrDefault(pod => pod.Show.Id == showViewModel.Show.Id);

        if (podcastToRemove != null)
        {
            var isUnsubscribe = await subscriptionsService.UnSubscribeFromShowAsync(showViewModel.Show); 
            if (isUnsubscribe)
            {
                SubscribedShows.Remove(podcastToRemove);
                showViewModel.IsSubscribed = false;
            }
        }
    }
}
