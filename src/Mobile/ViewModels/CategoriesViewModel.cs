namespace Microsoft.NetConf2021.Maui.ViewModels;

public class CategoriesViewModel : BaseViewModel
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

    public async Task InitializeAsync()
    {   
        var categories = await showsService.GetAllCategories();
        
        Categories = categories?.ToList();
    }

    private Task SelectedCommandExecute(Category category)
    {
        return Shell.Current.GoToAsync($"{nameof(CategoryPage)}?Id={category.Id}");
    }
}
