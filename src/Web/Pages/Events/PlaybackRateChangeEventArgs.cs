namespace Podcast.Pages.Events;

public class PlaybackRateChangeEventArgs : EventArgs
{
    public double PlaybackRate { get; set; }
}
