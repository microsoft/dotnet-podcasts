namespace Microsoft.NetConf2021.Maui.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private bool isDarkModeEnabled;
    private bool isWifiOnlyEnabled;

    public bool IsDarkModeEnabled
    {
        get => isDarkModeEnabled;
        set
        {
            if (SetProperty(ref isDarkModeEnabled, value))
            {
                ChangeUserAppTheme(value);
            }
        }
    }
    
    public bool IsWifiOnlyEnabled
    {
        get => isWifiOnlyEnabled;
        set
        {
            if (SetProperty(ref isWifiOnlyEnabled, value))
            {
                Settings.IsWifiOnlyEnabled = value;
            }
        }
    }

    public static string AppVersion { get => AppInfo.VersionString; }

    public SettingsViewModel()
    {
        isDarkModeEnabled = Settings.Theme == AppTheme.Dark;
        isWifiOnlyEnabled = Settings.IsWifiOnlyEnabled;
    }

    private void ChangeUserAppTheme(bool activateDarkMode)
    {
        Settings.Theme = activateDarkMode 
            ? AppTheme.Dark
            : AppTheme.Light;

        TheTheme.SetTheme();
    }
}
