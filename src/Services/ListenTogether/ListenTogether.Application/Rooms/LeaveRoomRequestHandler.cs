namespace ListenTogether.Application.Rooms;

public class LeaveRoomRequestHandler : IRequestHandler<LeaveRoomRequest, Room>
{
    private readonly IGrainFactory _grainFactory;

    public LeaveRoomRequestHandler(IGrainFactory grainFactory) =>
        _grainFactory = grainFactory;

    public async Task<Room> HandleAsync(LeaveRoomRequest request, CancellationToken cancellationToken)
    {
        var roomGrain = _grainFactory.GetGrain<IRoomGrain>(request.RoomCode);
        var room = await roomGrain.LeaveRoom(request.ConnectionId);

        return room;
    }
}
