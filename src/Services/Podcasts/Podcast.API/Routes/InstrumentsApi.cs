using Podcast.Infrastructure.Data.DTOs;

namespace Podcast.API.Routes;

public static class InstrumentsApi
{
    public static RouteGroupBuilder MapInstrumentsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", GetInstrumentById).WithName("GetInstrumentById");
        return group;
    }

    public static Ok<InstrumentDto> GetInstrumentById(PodcastDbContext podcastDbContext, CancellationToken cancellationToken, Guid id)
    {
        var instrument = new InstrumentDto { Id = Guid.NewGuid().ToString(), InstrumentName = "Test123"  };
        return TypedResults.Ok(instrument);
    }
}
