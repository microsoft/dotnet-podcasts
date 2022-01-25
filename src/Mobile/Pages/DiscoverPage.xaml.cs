namespace Microsoft.NetConf2021.Maui.Pages;

public partial class DiscoverPage : BaseContentPage
{
    public DiscoverPage(DiscoverViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;    
    }

    protected override async void OnAppearing()
    {
        player.OnAppearing();
        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        player.OnDisappearing();
        base.OnDisappearing();
    }
}
