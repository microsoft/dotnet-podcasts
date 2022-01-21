namespace Microsoft.NetConf2021.Maui.ViewModels;

public class SubscriptionsViewModel : BaseViewModel
{
    public bool HasData => SubscribedShows?.Any() ?? false;
    public bool HasNoData => !HasData;

    private readonly SubscriptionsService subscriptionsService;

    private ObservableCollection<ShowViewModel> subscribedShows;

    public ICommand NavigateToDiscoverCommand { get; internal set; }
    public ObservableCollection<ShowViewModel> SubscribedShows
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

    public ICommand SubscribeCommand => new AsyncCommand<ShowViewModel>(SubscribeCommandExecute);

    public SubscriptionsViewModel(SubscriptionsService subs)
    {
        subscriptionsService = subs;
        subscribedShows = new ObservableCollection<ShowViewModel>();
        NavigateToDiscoverCommand = new AsyncCommand(NavigateToDiscoverCommandExecute);
    }

    private Task NavigateToDiscoverCommandExecute()
    {
        return Shell.Current.GoToAsync($"{nameof(DiscoverPage)}");
    }

    public async Task InitializeAsync()
    {
        var podcasts = subscriptionsService.GetSubscribedShows();

        SubscribedShows.Clear();
        foreach (var podcast in podcasts)
        {
            var podcastViewModel = new ShowViewModel(podcast, subscriptionsService);
            await podcastViewModel.InitializeAsync();
            SubscribedShows.Add(podcastViewModel);
        }
        OnPropertyChanged(nameof(HasData));
        OnPropertyChanged(nameof(HasNoData));
    }

    private async Task SubscribeCommandExecute(ShowViewModel vm)
    {
        var podcastToRemove = SubscribedShows.Where(pod => pod.Show.Id == vm.Show.Id).FirstOrDefault();

        if (podcastToRemove != null)
        {
            await subscriptionsService.UnSubscribeFromShowAsync(vm.Show); 
            SubscribedShows.Remove(podcastToRemove);
            MessagingCenter.Instance.Send<string>(".NET Pods", "UnSubscribe");
        }
    }
}
