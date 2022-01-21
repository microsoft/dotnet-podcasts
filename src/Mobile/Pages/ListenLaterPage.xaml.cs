namespace Microsoft.NetConf2021.Maui.Pages;

public partial class ListenLaterPage : ContentPage
{
    ListenLaterViewModel viewModel => BindingContext as ListenLaterViewModel;

    public ListenLaterPage(ListenLaterViewModel vm)
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
        this.player.OnDisappearing();
        base.OnDisappearing();
    }
}

