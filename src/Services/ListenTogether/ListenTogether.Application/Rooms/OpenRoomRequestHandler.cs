namespace ListenTogether.Application.Rooms;

public class OpenRoomRequestHandler : IRequestHandler<OpenRoomRequest, Room>
{
    private readonly IEpisodesClient _episodesClient;
    private readonly IGrainFactory _grainFactory;

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
