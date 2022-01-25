namespace Microsoft.NetConf2021.Maui.ViewModels;

public class CategoriesViewModel :AppBaseViewModel
{
    private readonly ShowsService showsService;

    List<Category> categories;

    public List<Category> Categories
    {
        get { return categories; }
        set { SetProperty(ref categories, value); } 
    }

    public ICommand SelectedCommand => new AsyncCommand<Category>(SelectedCommandExecute);

    public CategoriesViewModel(ShowsService shows)
    {
        showsService = shows;
    }

    private Task SelectedCommandExecute(Category category)
    {
        return Shell.Current.GoToAsync($"{nameof(CategoryPage)}?Id={category.Id}");
    }

    
    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        var categories = await showsService.GetAllCategories();
        Categories = categories?.ToList();
    }


}
