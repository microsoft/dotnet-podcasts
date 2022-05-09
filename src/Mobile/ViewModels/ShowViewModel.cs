namespace Microsoft.NetConf2021.Maui.ViewModels;

public class ShowViewModel : ObservableObject
{
    public Show Show { get; set; }

    private readonly SubscriptionsService subscriptionsService;

    private bool isSuscribed;

    public bool IsSubscribed 
    { 
        get => isSuscribed;
        set
        {
            isSuscribed = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<Episode> Episodes { get => Show?.Episodes; }

    public Uri Image { get => Show?.Image; }

    public string Author { get => Show?.Author; }

    public string Title { get => Show?.Title; }

    public string Description { get => Show?.Description; }

    public ICommand NavigateToDetailCommand { get; set; }

    public ShowViewModel(Show show, SubscriptionsService subs)
    {
        Show = show;
        subscriptionsService = subs;
        NavigateToDetailCommand = new AsyncCommand(NavigateToDetailCommandExecute);
        isSuscribed = subs.IsSubscribed(show.Id);
    }

    private Task NavigateToDetailCommandExecute() => Shell.Current.GoToAsync($"{nameof(ShowDetailPage)}?Id={Show.Id}");
}
