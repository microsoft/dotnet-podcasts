using Microsoft.AspNetCore.Components;

namespace Podcast.Pages.Events;

[EventHandler("oncustomdurationchange", typeof(DurationChangeEventArgs),
    enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("oncustomtimeupdate", typeof(TimeUpdateEventArgs),
    enableStopPropagation: true, enablePreventDefault: true)]
public static class EventHandlers
{
}

public class DurationChangeEventArgs : EventArgs
{
    public double Duration { get; set; }
}

public class TimeUpdateEventArgs : EventArgs
{
    public double CurrentTime { get; set; }
}