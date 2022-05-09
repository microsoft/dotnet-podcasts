using Microsoft.JSInterop;

namespace Podcast.Components;

public class ClipboardInterop 
{
    private readonly IJSRuntime _jsRuntime;

    public ClipboardInterop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public ValueTask<string> ReadTextAsync() =>
        _jsRuntime.InvokeAsync<string>("navigator.clipboard.readText");

    public ValueTask WriteTextAsync(string text) =>
        _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
}