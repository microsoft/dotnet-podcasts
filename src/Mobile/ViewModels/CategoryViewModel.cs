namespace Microsoft.NetConf2021.Maui.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class CategoryViewModel : ViewModelBase
{
    private readonly ShowsService showsService;
    private readonly SubscriptionsService subscriptionsService;
    private readonly ImageProcessingService imageProcessingService;

    [ObservableProperty]
    string text;

    public string Id { get; set; }

    [ObservableProperty]
    Category category;

    [ObservableProperty]
    List<ShowViewModel> shows;


    public CategoryViewModel(ShowsService shows, SubscriptionsService subs, ImageProcessingService imageProcessing)
    {
        showsService = shows;
        subscriptionsService = subs;
        imageProcessingService = imageProcessing;
    }


    public async Task InitializeAsync()
    {
        await LoadCategoryAsync();
        var shows = await showsService.GetShowsByCategoryAsync(new Guid(Id));

        Shows = LoadShows(shows);
    }

    async Task LoadCategoryAsync()
    {
        var allCategories = await showsService.GetAllCategories();
        Category = allCategories?.First(c => c.Id == new Guid(Id));
    }

    [RelayCommand]
    async Task Subscribe(ShowViewModel vm)
    {
        await subscriptionsService.UnSubscribeFromShowAsync(vm.Show);
        OnPropertyChanged(nameof(vm.IsSubscribed));
    }

    [RelayCommand]
    async void Search()
    {
        var shows = await showsService.SearchShowsAsync(new Guid(Id), Text);
        Shows = LoadShows(shows);
    }

    List<ShowViewModel> LoadShows(IEnumerable<Show> shows)
    {
        var showList = new List<ShowViewModel>();
        if (shows == null)
        {
            return showList;
        }

        foreach (var show in shows)
        {
            var showVM = new ShowViewModel(show, subscriptionsService.IsSubscribed(show.Id), imageProcessingService);
            showList.Add(showVM);
        }

        return showList;
    }
}
