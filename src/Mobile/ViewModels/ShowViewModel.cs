namespace Microsoft.NetConf2021.Maui.ViewModels;

public class ShowViewModel : BaseViewModel
{
    public Show Show { get; set; }

    private readonly SubscriptionsService subscriptionsService;

    private bool isSuscribed;

    public bool IsSubscribed
    {
        get
        {
            return isSuscribed;
        }

        set
        {
            SetProperty(ref isSuscribed, value);
        }
    }

    public IEnumerable<Episode> Episodes { get => Show?.Episodes; }

    public Uri Image { get => Show?.Image; }

    public string Author { get => Show?.Author; }

    public new string Title { get => Show?.Title; }

    public string Description { get => Show?.Description; }

    public ICommand SubscribeCommand { get; internal set; }
    public ICommand NavigateToDetailCommand => new AsyncCommand(NavigateToDetailCommandExecute);

    public ShowViewModel(Show show, SubscriptionsService subs)
    {
        Show = show;
        subscriptionsService = subs;
    }

    internal Task InitializeAsync()
    {
        IsSubscribed = subscriptionsService.IsSubscribed(Show.Id);
        return Task.CompletedTask;
    }

    private Task NavigateToDetailCommandExecute()
    {
        return Shell.Current.GoToAsync($"{nameof(ShowDetailPage)}?Id={Show.Id}");
    }
}
