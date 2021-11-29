using Android.App;
using Android.Runtime;
using Microsoft.Maui.Hosting;

namespace Microsoft.NetConf2021.Maui
{
    [Application]
	public class MainApplication : MauiApplication
	{
		public MainApplication(IntPtr handle, JniHandleOwnership ownership)
			: base(handle, ownership)
		{
		}

		protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
	}
}
