using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Podcast.API.Models;
using Podcast.Infrastructure.Data;
using System.Threading;

namespace Podcast.API.Routes;

public static class EpisodesApi
{
    public static RouteGroupBuilder MapEpisodesApi(this RouteGroupBuilder group)
    {
        group.MapPost("/{id}", GetEpisodeById).WithName("GetEpisodeById");
        return group;
    }

    public static async ValueTask<Ok<EpisodeDto>> GetEpisodeById(PodcastDbContext podcastDbContext, CancellationToken cancellationToken, Guid id)
    {
        var episode = await podcastDbContext.Episodes.Include(episode => episode.Show)
            .Where(episode => episode.Id == id)
            .Select(episode => new EpisodeDto(episode))
            .FirstAsync(cancellationToken);
        return TypedResults.Ok(episode);
    }
}
