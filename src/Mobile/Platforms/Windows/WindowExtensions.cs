using System.Runtime.InteropServices;
using WinRT;


namespace Microsoft.NetConf2021.Maui.Platforms.Windows;

public static class WindowExtensions
{
    public static IntPtr GetNativeWindowHandle(this UI.Xaml.Window window)
    {
        var nativeWindow = window.As<IWindowNative>();
        return nativeWindow.WindowHandle;
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
    internal interface IWindowNative
    {
        IntPtr WindowHandle { get; }
    }
}
