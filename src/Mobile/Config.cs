namespace Microsoft.NetConf2021.Maui;

public static class Config
{
    public static bool ListenTogetherIsVisible => true;

    public static bool Desktop
    {
        get
        {
#if WINDOWS || MACCATALYST
            return true;
#else
            return false;
#endif
        }
    }

    public static string BaseWeb = $"{Base}:5002/";
    public static string Base = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2" : "http://localhost";
    public static string APIUrl = $"{Base}:5003/";
    public static string ListenTogetherUrl = $"{Base}:5001/listentogether";
}
