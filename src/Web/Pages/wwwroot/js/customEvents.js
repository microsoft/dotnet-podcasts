Blazor.registerCustomEventType('customdurationchange', {
    browserEventName: 'durationchange',
    createEventArgs: event => ({
        duration: event.srcElement.duration
    })
});

Blazor.registerCustomEventType('customtimeupdate', {
    browserEventName: 'timeupdate',
    createEventArgs: event => ({
        currentTime: event.srcElement.currentTime
    })
});

Blazor.registerCustomEventType('playbackratechange', {
    browserEventName: 'playbackratechange',
    createEventArgs: event => ({
        playbackRate: event.srcElement.playbackRate
    })
});