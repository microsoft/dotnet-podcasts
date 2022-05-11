namespace Podcast.Pages.Events;

public class TimeUpdateEventArgs : EventArgs
{
    public double CurrentTime { get; set; }
}