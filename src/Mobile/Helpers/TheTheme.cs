using CommunityToolkit.Mvvm.Messaging;
using Microsoft.NetConf2021.Maui.Messaging;

namespace Microsoft.NetConf2021.Maui.Helpers;

public static class TheTheme
{
    public static void SetTheme()
    {
        switch (Settings.Theme)
        {
            default:
            case AppTheme.Light:
                App.Current.UserAppTheme = AppTheme.Light;
                break;
            case AppTheme.Dark:
                App.Current.UserAppTheme = AppTheme.Dark;
                break;

        }

        WeakReferenceMessenger.Default.Send<ChangeThemeNotification>();
    }
}
