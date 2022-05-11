namespace ListenTogether.Application.Rooms;

public class JoinRoomRequestHandler : IRequestHandler<JoinRoomRequest, Room>
{
    private readonly IGrainFactory _grainFactory;

    public JoinRoomRequestHandler(IGrainFactory grainFactory) => 
        _grainFactory = grainFactory;

    public async Task<Room> HandleAsync(JoinRoomRequest request, CancellationToken cancellationToken)
    {
        var roomGrain = _grainFactory.GetGrain<IRoomGrain>(request.RoomCode);
        var room = await roomGrain.JoinRoom(request.ConnectionId, request.UserName);

        return room;
    }
}