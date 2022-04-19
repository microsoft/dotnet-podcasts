namespace Microsoft.NetConf2021.Maui.Helpers;

public static class Settings
{
    const AppTheme theme = AppTheme.Light;

    public static AppTheme Theme
    {
        get => Enum.Parse<AppTheme>(Preferences.Get(nameof(Theme), Enum.GetName(theme)));
        set => Preferences.Set(nameof(Theme), value.ToString());
    }
    
    public static bool IsWifiOnlyEnabled
    {
        get => Preferences.Get(nameof(IsWifiOnlyEnabled), false);
        set => Preferences.Set(nameof(IsWifiOnlyEnabled), value);
    }
}
