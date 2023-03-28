using Microsoft.NetConf2021.Maui.Resources.Strings;
using MvvmHelpers;

namespace Microsoft.NetConf2021.Maui.ViewModels;

public partial class DiscoverViewModel : ViewModelBase
{
    private readonly ShowsService showsService;
    private readonly SubscriptionsService subscriptionsService;
    private readonly ImageProcessingService imageProcessingService;

    private IEnumerable<ShowViewModel> shows;

    [ObservableProperty]
    CategoriesViewModel categoriesVM;

    [ObservableProperty]
    string text;

    [ObservableProperty]
    ObservableRangeCollection<ShowGroup> podcastsGroup;

    public DiscoverViewModel(ShowsService shows, SubscriptionsService subs, CategoriesViewModel categories, ImageProcessingService imageProcessing)
    {
        showsService = shows;
        subscriptionsService = subs;
        imageProcessingService = imageProcessing;

        PodcastsGroup = new ObservableRangeCollection<ShowGroup>();
        categoriesVM = categories;
    }

    internal async Task InitializeAsync()
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
            await FetchAsync();
    }

    private async Task FetchAsync()
    {
        if(shows?.Any() ??  false)
            return;

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

    private List<ShowViewModel> ConvertToViewModels(IEnumerable<Show> shows)
    {
        var viewmodels = new List<ShowViewModel>();
        foreach (var show in shows)
        {
            var showViewModel = new ShowViewModel(show, subscriptionsService.IsSubscribed(show.Id), imageProcessingService);
            viewmodels.Add(showViewModel);
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

    [RelayCommand]
    async Task Search()
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

    [RelayCommand]
    async Task Subscribe(ShowViewModel showViewModel) => 
        showViewModel.IsSubscribed = await subscriptionsService.UnSubscribeFromShowAsync(showViewModel.Show);

    [RelayCommand]
    Task SeeAllCategories() => Shell.Current.GoToAsync($"{nameof(CategoriesPage)}");
}
