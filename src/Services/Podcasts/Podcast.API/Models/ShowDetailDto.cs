namespace Podcast.API.Models;

public sealed record ShowDetailDto(
    Guid Id, 
    string Title, 
    string Author, 
    string Image);