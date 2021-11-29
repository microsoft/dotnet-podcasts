using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Podcast.Components;

public class AudioInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public AudioInterop(IJSRuntime jsRuntime)
    {
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
           "import", "./_content/Podcast.Components/audioJsInterop.js").AsTask());
    }

    public async Task Play(ElementReference element)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("play", element);
    }

    public async Task Pause(ElementReference element)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("pause", element);
    }

    public async Task Stop(ElementReference element)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("stop", element);
    }

    public async Task SetMuted(ElementReference element, bool value)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("setMuted", element, value);
    }

    public async Task SetVolume(ElementReference element, int value)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("setVolume", element, value / 100d);
    }

    public async Task SetCurrentTime(ElementReference element, double value)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("setCurrentTime", element, value);
    }

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}