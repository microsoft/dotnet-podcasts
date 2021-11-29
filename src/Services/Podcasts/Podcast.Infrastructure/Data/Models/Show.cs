namespace Podcast.Infrastructure.Data.Models;

public class Show
{
    public Show(string author, string description, string email, string language, string title, string link, string image, DateTime updated)
    {
        Author = author;
        Description = description;
        Email = email;
        Language = language;
        Title = title;
        Link = link;
        Image = image;
        Updated = updated;
    }

    public Guid Id { get; private set;  }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string Description { get; private set; }
    public string Image { get; private set; }
    public DateTime Updated { get; private set; }
    public string Link { get; private set; }
    public string Email { get; private set;}
    public string Language { get; private set; }
    public Guid FeedId { get; private set; }
    public Feed? Feed { get; private set; }
    public ICollection<Episode> Episodes { get; private set; } = new List<Episode>();
}