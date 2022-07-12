namespace Microsoft.NetConf2021.Maui.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    bool isDarkModeEnabled;

    [ObservableProperty]
    bool isWifiOnlyEnabled;

    partial void OnIsDarkModeEnabledChanged(bool value) => 
        ChangeUserAppTheme(value);
    partial void OnIsWifiOnlyEnabledChanged(bool value) =>
        Settings.IsWifiOnlyEnabled = value;

    public string AppVersion => AppInfo.VersionString;

    public SettingsViewModel()
    {
        isDarkModeEnabled = Settings.Theme == AppTheme.Dark;
        isWifiOnlyEnabled = Settings.IsWifiOnlyEnabled;
    }

    void ChangeUserAppTheme(bool activateDarkMode)
    {
        Settings.Theme = activateDarkMode 
            ? AppTheme.Dark
            : AppTheme.Light;

        TheTheme.SetTheme();
    }
}
