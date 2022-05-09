namespace Podcast.Shared;

public record Show(
    Guid Id,
    string Title,
    string Author,
    string Description,
    string Image,
    DateTime Updated,
    string Link,
    string Email,
    string Language,
    IEnumerable<Category> Categories,
    IEnumerable<Episode> Episodes,
    bool IsFeatured);
