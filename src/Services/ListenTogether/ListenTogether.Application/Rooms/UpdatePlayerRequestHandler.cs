namespace ListenTogether.Application.Rooms;

public class UpdatePlayerRequestHandler : IRequestHandler<UpdatePlayerRequest, Room>
{
    private readonly IGrainFactory _grainFactory;

    public UpdatePlayerRequestHandler(IGrainFactory grainFactory) => 
        _grainFactory = grainFactory;

    public async Task<Room> HandleAsync(
        UpdatePlayerRequest request, CancellationToken cancellationToken)
    {
        var seconds = TimeSpan.FromSeconds(request.Progress);

        var roomGrain = _grainFactory.GetGrain<IRoomGrain>(request.RoomCode);
        var room = await roomGrain.UpdatePlayerState(seconds, request.State);

        return room;
    }
}