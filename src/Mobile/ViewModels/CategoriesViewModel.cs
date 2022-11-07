namespace Microsoft.NetConf2021.Maui.ViewModels;

public partial class CategoriesViewModel : ViewModelBase
{
    private readonly ShowsService showsService;

    [ObservableProperty]
    List<Category> categories;

    public CategoriesViewModel(ShowsService shows)
    {
        showsService = shows;
    }

    public async Task InitializeAsync()
    {   
        var categories = await showsService.GetAllCategories();
        
        Categories = categories?.ToList();
    }

    [RelayCommand]
    Task Selected(Category category)
    {
        return Shell.Current.GoToAsync($"{nameof(CategoryPage)}?Id={category.Id}");
    }
}
