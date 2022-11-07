using Microsoft.NetConf2021.Maui.Resources.Strings;

namespace Microsoft.NetConf2021.Maui.ViewModels;

public class ShellViewModel : ViewModelBase
{
    public AppSection Discover { get; set; }
    public AppSection Subscriptions { get; set; }
    public AppSection Lists { get; set; }
    public AppSection ListenLater { get; set; }
    public AppSection Settings { get; set; }
    public AppSection ListenTogether { get; set; }

    public ShellViewModel()
    {
        Discover = new AppSection() { Title = AppResource.Discover , Icon = "discover.png", IconDark="discover_dark.png", TargetType = typeof(DiscoverPage) };
        Subscriptions = new AppSection() { Title = AppResource.Subscriptions, Icon = "subscriptions.png", IconDark="subscriptions_dark.png", TargetType = typeof(SubscriptionsPage) };
        ListenLater = new AppSection() { Title = Config.Desktop ? AppResource.Listen_Later : AppResource.Listen_Later_Short, Icon = "clock.png", IconDark="clock_dark.png", TargetType = typeof(ListenLaterPage) };
        ListenTogether = new AppSection() { Title = Config.Desktop ? AppResource.Listen_Together : AppResource.Listen_Together_Short, Icon = "listentogether.png", IconDark = "listentogether_dark.png",  TargetType = typeof(SettingsPage) };
        Settings = new AppSection() { Title = AppResource.Settings, Icon = "settings.png", IconDark="settings_dark.png", TargetType = typeof(SettingsPage) };
    }
}
