namespace Podcast.Infrastructure.Data.Models;

public class Category
{
    public Category(Guid id, string genre)
    {
        Id = id;
        Genre = genre;
    }

    public Guid Id { get; private set; }
    public string Genre { get; private set; }
}