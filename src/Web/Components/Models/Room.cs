namespace Podcast.Components;

public readonly record struct Room(
    TimeSpan Progress,
    PlayerState PlayerState,
    string Code,
    RoomEpisode Episode,
    List<User> Users);
