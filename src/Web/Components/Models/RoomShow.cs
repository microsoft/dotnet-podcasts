namespace Podcast.Components;

public readonly record struct RoomShow(
    Guid Id,
    string Title, 
    string Author, 
    string Image);
