namespace Microsoft.NetConf2021.Maui.Helpers;

public static class Settings
{
    const OSAppTheme theme = OSAppTheme.Light;

    public static OSAppTheme Theme
    {
        get => Enum.Parse<OSAppTheme>(Preferences.Get(nameof(Theme), Enum.GetName(theme)));
        set => Preferences.Set(nameof(Theme), value.ToString());
    }
    
    public static bool IsWifiOnlyEnabled
    {
        get => Preferences.Get(nameof(IsWifiOnlyEnabled), false);
        set => Preferences.Set(nameof(IsWifiOnlyEnabled), value);
    }
}
