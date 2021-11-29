namespace ListenTogether.Domain;

public class Episode
{
    protected Episode() { }

    public Episode(Guid id, string title, string description, string url, DateTime published, TimeSpan? duration, Show show)
    {
        Id = Guid.NewGuid();
        EpisodeId = id;
        Title = title;
        Description = description;
        Url = url;
        Published = published;
        Duration = duration;
        Show = show;
    }
    public Guid Id { get; private set; }
    public Guid EpisodeId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Url { get; private set; }
    public DateTime Published { get; private set; }
    public TimeSpan? Duration { get; private set; }
    public Show Show { get; private set; }
}