using ListenTogether.Application.Interfaces;
using ListenTogether.Domain;
using Microsoft.EntityFrameworkCore;
using Orleans;

namespace ListenTogether.Application.Rooms;

public class LeaveRoomRequest : IRequest<Room>
{
    public LeaveRoomRequest(string roomCode, string connectionId)
    {
        RoomCode = roomCode;
        ConnectionId = connectionId;
    }

    public string RoomCode { get; }
    public string ConnectionId { get; }
}

public class LeaveRoomRequestHandler : IRequestHandler<LeaveRoomRequest, Room>
{
    private readonly IGrainFactory _grainFactory;

    public LeaveRoomRequestHandler(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    public async Task<Room> HandleAsync(LeaveRoomRequest request, CancellationToken cancellationToken)
    {
        var roomGrain = _grainFactory.GetGrain<IRoomGrain>(request.RoomCode);
        var room = await roomGrain.LeaveRoom(request.ConnectionId);

        return room;
    }
}
