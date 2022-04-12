using ListenTogether.Application.Interfaces;
using ListenTogether.Domain;
using Orleans;

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
    private readonly IGrainFactory _grainFactory;
    private readonly IRoomGrain _roomGrain;

    public OpenRoomRequestHandler(IEpisodesClient episodesClient, IGrainFactory grainFactory)
    {
        _episodesClient = episodesClient;
        _grainFactory = grainFactory;
    }

    public async Task<Room> HandleAsync(OpenRoomRequest request, CancellationToken cancellationToken)
    {
        var episode = await _episodesClient.GetEpisodeByIdAsync(request.EpisodeId, cancellationToken);
        var room = new Room(episode);

        var roomGrain = _grainFactory.GetGrain<IRoomGrain>(room.Code);
        await roomGrain.SetRoom(room);

        return room;
    }
}
