namespace Microsoft.NetConf2021.Maui.ViewModels;

public class AppBaseViewModel : BaseViewModel, INavigationAwareViewModel
{
    public virtual Task OnAppearingAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task OnDisappearingAsync()
    {
        return Task.CompletedTask;
    }
}
