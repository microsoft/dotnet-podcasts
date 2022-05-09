using Microsoft.NetConf2021.Maui.Resources.Strings;

namespace Microsoft.NetConf2021.Maui.ViewModels;

public class DiscoverViewModel : BaseViewModel
{
    private readonly ShowsService showsService;
    private readonly SubscriptionsService subscriptionsService;
    private IEnumerable<ShowViewModel> shows;
    private CategoriesViewModel categoriesVM;
    private string text;

    public ObservableRangeCollection<ShowGroup> PodcastsGroup { get; private set; } = new ObservableRangeCollection<ShowGroup>();

    public ICommand SearchCommand { get; }

    public ICommand SubscribeCommand => new AsyncCommand<ShowViewModel>(SubscribeCommandExecute);

    public ICommand SeeAllCategoriesCommand => new AsyncCommand(SeeAllCategoriesCommandExecute);

    public string Text
    {
        get { return text; }
        set 
        {
            SetProperty(ref text, value);
        }
    }  

    public CategoriesViewModel CategoriesVM
    {
        get { return categoriesVM; }      
        set {  SetProperty(ref categoriesVM, value); }
    }

    public DiscoverViewModel(ShowsService shows, SubscriptionsService subs, CategoriesViewModel categories)
    {
        showsService = shows;
        subscriptionsService = subs;

        SearchCommand = new AsyncCommand(OnSearchCommandAsync);
        categoriesVM = categories;
    }

    internal async Task InitializeAsync()
    {
        await FetchAsync();
    }

    private async Task FetchAsync()
    {
        var podcastsModels = await showsService.GetShowsAsync();

        if (podcastsModels == null)
        {
            await Shell.Current.DisplayAlert(
                AppResource.Error_Title,
                AppResource.Error_Message,
                AppResource.Close);

            return;
        }

        await CategoriesVM.InitializeAsync();
        shows = ConvertToViewModels(podcastsModels);
        UpdatePodcasts(shows);
    }

    private List<ShowViewModel> ConvertToViewModels(IEnumerable<Show> podcasts)
    {
        var viewmodels = new List<ShowViewModel>();
        foreach (var podcast in podcasts)
        {
            var podcastViewModel = new ShowViewModel(podcast, subscriptionsService);
            viewmodels.Add(podcastViewModel);
        }

        return viewmodels;
    }

    private void UpdatePodcasts(IEnumerable<ShowViewModel> listPodcasts)
    {
        var groupedShows = listPodcasts
            .GroupBy(podcasts => podcasts.Show.IsFeatured)
            .Where(group => group.Any())
            .ToDictionary(group => group.Key ? AppResource.Whats_New : AppResource.Specially_For_You, group => group.ToList())
            .Select(dictionary => new ShowGroup(dictionary.Key, dictionary.Value));

        PodcastsGroup.ReplaceRange(groupedShows);
    }

    private async Task OnSearchCommandAsync()
    {
        IEnumerable<Show> list;
        if (string.IsNullOrWhiteSpace(Text))
        {
            list = await showsService.GetShowsAsync();
        }
        else
        {
            list = await showsService.SearchShowsAsync(Text);
        }

        if (list != null)
        {
            UpdatePodcasts(ConvertToViewModels(list));
        }
    }

    private async Task SubscribeCommandExecute(ShowViewModel showViewModel)
    {
        showViewModel.IsSubscribed = await subscriptionsService.UnSubscribeFromShowAsync(showViewModel.Show);
    }

    private Task SeeAllCategoriesCommandExecute()
    {
        return Shell.Current.GoToAsync($"{nameof(CategoriesPage)}");
    }
}
