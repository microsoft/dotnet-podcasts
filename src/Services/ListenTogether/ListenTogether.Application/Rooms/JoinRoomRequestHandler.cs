using ListenTogether.Application.Interfaces;
using ListenTogether.Domain;
using Microsoft.EntityFrameworkCore;
using Orleans;

namespace ListenTogether.Application.Rooms;

public class JoinRoomRequest : IRequest<Room>
{
    public JoinRoomRequest(string roomCode, string connectionId, string userName)
    {
        RoomCode = roomCode;
        ConnectionId = connectionId;
        UserName = userName;
    }

    public string RoomCode { get; set; }
    public string ConnectionId { get; set; }
    public string UserName { get; set; }
}

public class JoinRoomRequestHandler : IRequestHandler<JoinRoomRequest, Room>
{
    private readonly IGrainFactory _grainFactory;

    public JoinRoomRequestHandler(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    public async Task<Room> HandleAsync(JoinRoomRequest request, CancellationToken cancellationToken)
    {
        var roomGrain = _grainFactory.GetGrain<IRoomGrain>(request.RoomCode);
        var room = await roomGrain.JoinRoom(request.ConnectionId, request.UserName);

        return room;
    }
}