using Foundation;
using Microsoft.Maui.Hosting;

namespace Microsoft.NetConf2021.Maui;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
