using ListenTogether.Application.Interfaces;
using ListenTogether.Domain;

namespace ListenTogether.Application.Rooms;

public class OpenRoomRequest : IRequest<Room>
{
    public OpenRoomRequest(Guid episodeId)
    {
        EpisodeId = episodeId;
    }

    public Guid EpisodeId { get; set; }
}

public class OpenRoomRequestHandler : IRequestHandler<OpenRoomRequest, Room>
{
    private readonly IEpisodesClient _episodesClient;
    private readonly IApplicationDbContext _dbContext;

    public OpenRoomRequestHandler(IEpisodesClient episodesClient, IApplicationDbContext dbContext)
    {
        _episodesClient = episodesClient;
        _dbContext = dbContext;
    }

    public async Task<Room> HandleAsync(OpenRoomRequest request, CancellationToken cancellationToken)
    {
        var episode = await _episodesClient.GetEpisodeByIdAsync(request.EpisodeId, cancellationToken);
        var room = new Room(episode);

        _dbContext.Rooms.Add(room);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return room;
    }
}
