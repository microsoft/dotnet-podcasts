using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Podcast.API.Models;
using Podcast.Infrastructure.Data;
using System.Threading;

namespace Podcast.API.Routes;

public static class CategoriesApi
{
    public static RouteGroupBuilder MapCategoriesApi(this RouteGroupBuilder group)
    {
        group.MapPost("/", GetAllCategories);
        return group;
    }

    public static async ValueTask<Ok<List<CategoryDto>>> GetAllCategories(PodcastDbContext podcastDbContext, CancellationToken cancellationToken)
    {
        var categories = await podcastDbContext.Categories.Select(x => new CategoryDto(x.Id, x.Genre))
            .ToListAsync(cancellationToken);
        return TypedResults.Ok(categories);
    }
}
