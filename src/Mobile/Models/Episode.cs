using Microsoft.NetConf2021.Maui.Models.Responses;
using static Podcast.Components.ListenTogether.ListenTogether;

namespace Microsoft.NetConf2021.Maui.Models;

public class Episode : ObservableObject
{
    public Episode(RoomPlayerState playerState)
    {
        Id = playerState.Episode.Id;
        Title = playerState.Episode.Title;
        Description = playerState.Episode.Description;
        Published = playerState.Episode.Published;
        Duration = playerState.Episode.Duration.ToString();
        Url = new Uri(playerState.Episode.Url);
    }

    public Episode(EpisodeResponse response, ListenLaterService listenLater)
    {
        Id = response.Id;
        Title = response.Title;
        Description = response.Description;
        Published = response.Published;
        Duration = response.Duration;
        Url = response.Url;
        listenLaterService = listenLater;
        IsInListenLater = listenLaterService.IsInListenLater(this);
    }

    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime Published { get; set; }

    public string Duration { get; set; }

    public Uri Url { get; set; }

    private readonly ListenLaterService listenLaterService;
    private bool isInLisenLater;

    public bool IsInListenLater
    {
        get
        {
            return isInLisenLater;
        }
        set
        {
            SetProperty(ref isInLisenLater, value);
        }
    }
}
