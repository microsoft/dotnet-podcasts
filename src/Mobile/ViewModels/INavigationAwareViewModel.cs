namespace Microsoft.NetConf2021.Maui.ViewModels;

interface INavigationAwareViewModel
{
    Task OnAppearingAsync();
    Task OnDisappearingAsync();
}
