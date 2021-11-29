namespace Microsoft.NetConf2021.Maui.Pages;

public partial class CategoriesPage : ContentPage
{
    CategoriesViewModel vm => BindingContext as CategoriesViewModel;
    public CategoriesPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await vm.InitializeAsync();
    }
}
