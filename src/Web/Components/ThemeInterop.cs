using Microsoft.JSInterop;

namespace Podcast.Components;

public enum Theme
{
    Dark,
    Light
}

public class ThemeInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public ThemeInterop(IJSRuntime jsRuntime)
    {
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
           "import", "./_content/Podcast.Components/themeJsInterop.js").AsTask());
    }

    public async ValueTask<Theme> GetTheme()
    {
        var module = await moduleTask.Value;
        var theme = await module.InvokeAsync<string>("getTheme");
        return theme != null ? (Theme)Enum.Parse(typeof(Theme), theme) : Theme.Light;
    }

    public async Task SetTheme(Theme theme)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("setTheme", Enum.GetName(typeof(Theme), theme));
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