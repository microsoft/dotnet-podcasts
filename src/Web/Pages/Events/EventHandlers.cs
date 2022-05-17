using Microsoft.AspNetCore.Components;

namespace Podcast.Pages.Events;

[EventHandler("oncustomdurationchange", typeof(DurationChangeEventArgs),
    enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("oncustomtimeupdate", typeof(TimeUpdateEventArgs),
    enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("playbackratechange", typeof(TimeUpdateEventArgs),
    enableStopPropagation: true, enablePreventDefault: true)]
public static class EventHandlers
{
}
