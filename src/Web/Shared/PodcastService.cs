using System.Net.Http.Json;

namespace Podcast.Shared;

public class PodcastService
{
    private readonly HttpClient _httpClient;

    public PodcastService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<Category[]?> GetCategories() =>
        _httpClient.GetFromJsonAsync<Category[]>("v1/categories");

    public Task<Show[]?> GetShows(int limit, string? term = null) =>
        _httpClient.GetFromJsonAsync<Show[]>($"/v1/shows?limit={limit}&term={term}");

    public Task<Show[]?> GetShows(int limit, string? term = null, Guid? categoryId = null) =>
    _httpClient.GetFromJsonAsync<Show[]>($"/v1/shows?limit={limit}&term={term}&categoryId={categoryId}");

    public Task<Show?> GetShow(Guid id) =>
        _httpClient.GetFromJsonAsync<Show>($"v1/shows/{id}");
}

public record Category(Guid Id, string Genre);

public record Episode(
    Guid Id,
    string Title,
    string Description,
    string Explicit,
    DateTime Published,
    TimeSpan? Duration,
    string Url);

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
