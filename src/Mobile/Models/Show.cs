using Microsoft.NetConf2021.Maui.Models.Responses;
using static Podcast.Components.ListenTogether.ListenTogether;

namespace Microsoft.NetConf2021.Maui.Models;

public class Show
{

    public Show(RoomPlayerState playerState)
    {
        Id = playerState.Episode.Show.Id;
        Title = playerState.Episode.Show.Title;
        Author = playerState.Episode.Show.Author;
        Image = new Uri(playerState.Episode.Show.Image);
    }

    public Show(ShowResponse response, ListenLaterService listenLaterService)
    {
        Id = response.Id;
        Title = response.Title;
        Author = response.Author;
        Description = response.Description;
        Image = response.Image;
        Updated = response.Updated;
        Episodes = response.Episodes?.Select(episode => new Episode(episode, listenLaterService.IsInListenLater(episode.Id)));
        Categories = response.Categories?.Select(category => new Category(category));
        IsFeatured = response.IsFeatured;
    }

    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public string Description { get; set; }

    public Uri Image { get; set; }

    public DateTime Updated { get; set; }

    public IEnumerable<Episode> Episodes { get; set; }

    public IEnumerable<Category> Categories { get; set; }

    public bool IsFeatured { get; set; }
}
