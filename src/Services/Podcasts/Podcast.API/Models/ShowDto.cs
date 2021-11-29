using System;
using System.Collections.Generic;
using System.Linq;
using Podcast.Infrastructure.Data.Models;

namespace Podcast.API.Models;

public record ShowDto
{
    public ShowDto(Show show)
    {
        Id = show.Id;
        Title = show.Title;
        Author = show.Author;
        Image = show.Image;
        Description = show.Description;
        Updated = show.Updated;
        Link = show.Link;
        Email = show.Email;
        Language = show.Language;
        IsFeatured = show.Feed!.IsFeatured;
        Categories = show.Feed.Categories
            .Select(category => new CategoryDto(category.Category!.Id, category.Category.Genre)).ToList();
        Episodes = show.Episodes.Select(episode =>
            new EpisodeDetailDto(episode.Id, episode.Title, episode.Published, episode.Url, episode.Description,
                episode.Duration?.ToString())).ToList();
    }

    public Guid Id { get; }
    public string Title { get; }
    public string Author { get; }
    public string Description { get; }
    public string Image { get; }
    public DateTime Updated { get; }
    public string Link { get; }
    public string Email { get; }
    public string Language { get; }
    public bool IsFeatured { get; set; }
    public List<CategoryDto> Categories { get; }
    public List<EpisodeDetailDto> Episodes { get; set; }

    public record EpisodeDetailDto(Guid Id, string Title, DateTime Published, string Url,
        string Description,
        string? Duration);
}