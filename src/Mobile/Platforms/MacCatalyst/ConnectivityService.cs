using System.Net.NetworkInformation;

namespace Microsoft.NetConf2021.Maui.Platforms.MacCatalyst;

public class ConnectivityService
{
    public ConnectivityService()
    {
    }

    public async Task<bool> IsConnected()
    {
        bool result = false;
        try
        {
            var hostUrl = "www.google.com";

            Ping ping = new Ping();

            PingReply pingReply = await ping.SendPingAsync(hostUrl);
            result = pingReply.Status == IPStatus.Success;
        }
        catch
        {
            result = false;
        }
        return result;
    }
}
