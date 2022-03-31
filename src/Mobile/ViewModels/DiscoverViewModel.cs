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
        shows = await ConvertToViewModels(podcastsModels);
        UpdatePodcasts(shows);
    }

    private async Task<List<ShowViewModel>> ConvertToViewModels(IEnumerable<Show> podcasts)
    {
        var viewmodels = new List<ShowViewModel>();
        foreach (var podcast in podcasts)
        {
            var podcastViewModel = new ShowViewModel(podcast, subscriptionsService);
            await podcastViewModel.InitializeAsync();
            viewmodels.Add(podcastViewModel);
        }

        return viewmodels;
    }

    private void UpdatePodcasts(IEnumerable<ShowViewModel> listPodcasts)
    {
        var list = new ObservableRangeCollection<ShowGroup>
        {
            new ShowGroup(AppResource.Whats_New, listPodcasts.Take(3).ToList()),
            new ShowGroup(AppResource.Specially_For_You, listPodcasts.Take(3..).ToList())
        };

        PodcastsGroup.ReplaceRange(list);
    }

    private async Task OnSearchCommandAsync()
    {     
        var list = await showsService.SearchShowsAsync(Text);
        if (string.IsNullOrWhiteSpace(Text))
        {
            list = await showsService.GetShowsAsync();
        }
        if (list != null)
        {
            UpdatePodcasts(await ConvertToViewModels(list));
        }
    }

    private async Task SubscribeCommandExecute(ShowViewModel vm)
    {
        await subscriptionsService.UnSubscribeFromShowAsync(vm.Show);
        vm.IsSubscribed = subscriptionsService.IsSubscribed(vm.Show.Id);
    }

    private Task SeeAllCategoriesCommandExecute()
    {
        return Shell.Current.GoToAsync($"{nameof(CategoriesPage)}");
    }
}
