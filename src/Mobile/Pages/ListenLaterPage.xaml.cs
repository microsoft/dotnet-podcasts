namespace Microsoft.NetConf2021.Maui.Pages;

public partial class ListenLaterPage : BaseContentPage
{
    public ListenLaterPage(ListenLaterViewModel vm)
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
        this.player.OnDisappearing();
        base.OnDisappearing();
    }
}

