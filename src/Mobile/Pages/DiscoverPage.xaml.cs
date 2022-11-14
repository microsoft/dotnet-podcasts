namespace Microsoft.NetConf2021.Maui.Pages;

public partial class DiscoverPage : ContentPage
{
    private DiscoverViewModel viewModel => BindingContext as DiscoverViewModel;

    public DiscoverPage(DiscoverViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;    
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        player.OnAppearing();
        await viewModel.InitializeAsync();
    }


    protected override void OnDisappearing()
    {
        player.OnDisappearing();
        base.OnDisappearing();
    }
}
