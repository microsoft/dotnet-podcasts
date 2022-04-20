namespace Podcast.Infrastructure.Data.Models;

public class UserSubmittedFeed
{
    public UserSubmittedFeed(string url, string title, string categories)
    {
        Url = url;
        Title = title;
        Categories = categories;
    }

    public Guid Id { get; private set; }
    public string Url { get; private set; }
    public string Title { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Categories { get; set; }
}