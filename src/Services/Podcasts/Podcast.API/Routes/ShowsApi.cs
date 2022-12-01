using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Podcast.API.Models;
using Podcast.Infrastructure.Data;
using Podcast.Infrastructure.Http;

namespace Podcast.API.Routes;

public static class ShowsApi
{
    public static RouteGroupBuilder MapShowsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllShows).WithName("GetShows");
        group.MapGet("/{id}", GetShowById).WithName("GetShowsById");
        return group;
    }

    public static async ValueTask<Ok<List<ShowDto>>> GetAllShows(int limit, string? term, Guid? categoryId, CancellationToken cancellationToken, PodcastDbContext podcastDbContext, ShowClient showClient)
    {
        var showsQuery = podcastDbContext.Shows.Include(show => show.Feed!.Categories)
        .ThenInclude(x => x.Category)
        .AsQueryable();

        if (!string.IsNullOrEmpty(term))
            showsQuery = showsQuery.Where(show => show.Title.Contains(term));

        if (categoryId is not null)
            showsQuery = showsQuery.Where(show =>
                show.Feed!.Categories.Any(y => y.CategoryId == categoryId));

        var shows = await showsQuery.OrderByDescending(show => show.Feed!.IsFeatured)
            .ThenBy(x => x.Title)
            .Take(limit)
            .Select(x => new ShowDto(x))
            .ToListAsync(cancellationToken);

        List<ShowDto> showsWithValidLinks = Task.WhenAll(shows.Select(async show => await showClient.CheckLink(show.Link) ? show : null)).Result.Where(show => show is not null).ToList()!;
        return TypedResults.Ok(showsWithValidLinks);
    }


    public static async ValueTask<Results<NotFound, Ok<ShowDto>>> GetShowById(PodcastDbContext podcastDbContext, Guid id, CancellationToken cancellationToken)
    {
        var show = await podcastDbContext.Shows
            .Include(show => show.Episodes.OrderByDescending(episode => episode.Published))
            .Include(show => show.Feed!.Categories)
            .ThenInclude(feed => feed.Category)
            .Where(x => x.Id == id)
            .Select(show => new ShowDto(show))
            .FirstOrDefaultAsync(cancellationToken);
        return show == null ? TypedResults.NotFound() : TypedResults.Ok(show);
    }
}
