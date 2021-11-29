namespace Podcast.Infrastructure.Data.Models;

public class FeedCategory
{
    public FeedCategory(Guid categoryId, Guid feedId)
    {
        CategoryId = categoryId;
        FeedId = feedId;
    }

    public Guid FeedId { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category? Category { get; private set; }
}