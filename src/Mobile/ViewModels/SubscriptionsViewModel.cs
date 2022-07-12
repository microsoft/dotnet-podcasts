using MvvmHelpers;

namespace Microsoft.NetConf2021.Maui.ViewModels;

public partial class SubscriptionsViewModel : ViewModelBase
{
    readonly SubscriptionsService subscriptionsService;

    [ObservableProperty]
    ObservableRangeCollection<ShowViewModel> subscribedShows;


    public SubscriptionsViewModel(SubscriptionsService subs)
    {
        subscriptionsService = subs;
        SubscribedShows = new ObservableRangeCollection<ShowViewModel>();
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

    [RelayCommand]
    Task NavigateToDiscover() => Shell.Current.GoToAsync($"{nameof(DiscoverPage)}");


    [RelayCommand]
    async Task Subscribe(ShowViewModel showViewModel)
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
