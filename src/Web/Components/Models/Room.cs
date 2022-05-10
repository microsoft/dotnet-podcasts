namespace Podcast.Components;

public record Room(
    TimeSpan Progress,
    PlayerState PlayerState,
    string Code,
    RoomEpisode Episode,
    List<User> Users);
