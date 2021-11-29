namespace Microsoft.NetConf2021.Maui.Services;

public interface IShareService
{
    Task RequestAsync(ShareTextRequest request);
}
