using Podcast.Infrastructure.Data.Models;

namespace Podcast.API.Models;

public record EpisodeDto
{
    public EpisodeDto(Episode episode)
    {
        Id = episode.Id;
        Title = episode.Title;
        Published = episode.Published;
        Url = episode.Url;
        Show = new ShowDetailDto(episode.Show!.Id, episode.Show.Title, episode.Show.Author,
            episode.Show.Image);
        Description = episode.Description;
        Duration = episode.Duration?.ToString();
    }

    public Guid Id { get; }
    public string Title { get; }
    public string Url { get; }
    public DateTime Published { get; }
    public string? Duration { get; set; }
    public string Description { get; set; }
    public ShowDetailDto Show { get; }

    public record ShowDetailDto(Guid Id, string Title, string Author, string Image);
}