namespace Microsoft.NetConf2021.Maui.Pages;

public partial class SubscriptionsPage: ContentPage
{
    SubscriptionsViewModel viewModel => BindingContext as SubscriptionsViewModel;
    public SubscriptionsPage(SubscriptionsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.InitializeAsync();
        this.player.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        this.player.OnDisappearing();
        base.OnDisappearing();
    }
}
