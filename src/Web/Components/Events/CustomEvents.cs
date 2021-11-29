using Microsoft.AspNetCore.Components;

namespace Podcast.Components.Events;

[EventHandler("onanimationend", typeof(EventArgs),
    enableStopPropagation: true, enablePreventDefault: false)]
public static class EventHandlers
{
}
