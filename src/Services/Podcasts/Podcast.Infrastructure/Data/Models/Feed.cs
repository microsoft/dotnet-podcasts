namespace Podcast.Infrastructure.Data.Models;

public class Feed
{
    public Feed(Guid id, string url, bool isFeatured = false)
    {
        Id = id;
        Url = url;
        IsFeatured = isFeatured;
    }

    public Guid Id { get; private set; }
    public string Url { get; private set; }
    public bool IsFeatured { get; private set; }
    public Show? Show { get; set; }
    public ICollection<FeedCategory> Categories { get; private set; } = new List<FeedCategory>();
}