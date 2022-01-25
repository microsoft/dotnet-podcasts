
namespace Microsoft.NetConf2021.Maui.Pages;

public class BaseContentPage:ContentPage
{
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is INavigationAwareViewModel)
            await ((INavigationAwareViewModel)BindingContext).OnAppearingAsync();
    }
    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is INavigationAwareViewModel)
            await ((INavigationAwareViewModel)BindingContext).OnDisappearingAsync();
    }
}
