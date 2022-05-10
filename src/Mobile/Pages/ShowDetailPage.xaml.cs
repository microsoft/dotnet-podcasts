namespace Microsoft.NetConf2021.Maui.Pages;

public partial class ShowDetailPage : ContentPage
{
    private ShowDetailViewModel viewModel => BindingContext as ShowDetailViewModel;

    public ShowDetailPage(ShowDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        this.player.OnAppearing();
        await viewModel.InitializeAsync();
    }

    protected override void OnDisappearing()
    {
        this.player.OnDisappearing();
        base.OnDisappearing();
    }
}
