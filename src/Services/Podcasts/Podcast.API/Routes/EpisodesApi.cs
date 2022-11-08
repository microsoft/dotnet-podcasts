namespace Podcast.API.Routes;

public static class EpisodesApi
{
    public static RouteGroupBuilder MapEpisodesApi(this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", GetEpisodeById).WithName("GetEpisodeById");
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
