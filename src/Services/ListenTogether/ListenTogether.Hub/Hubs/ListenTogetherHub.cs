using ListenTogether.Application.Interfaces;
using ListenTogether.Application.Rooms;
using ListenTogether.Domain;

namespace ListenTogether.Hub.Hubs;

public class ListenTogetherHub : Microsoft.AspNetCore.SignalR.Hub
{
    private const string UpdateRoomMethod = "UpdateRoom";
    private const string RoomOpenedMethod = "RoomOpened";
    private const string ReceiveMessageMethod = "ReceiveMessage";
    private const string UpdatePlayerStateMethod = "UpdatePlayerState";
    private readonly IRequestHandler<GetUserRoomQuery, Room?> _getUserRoomHandler;
    private readonly IRequestHandler<JoinRoomRequest, Room> _joinRoomHandler;
    private readonly IRequestHandler<LeaveRoomRequest, Room> _leaveRoomHandler;
    private readonly IRequestHandler<OpenRoomRequest, Room> _openRoomHandler;
    private readonly IRequestHandler<UpdatePlayerRequest, Room> _updatePlayerHandler;

    public ListenTogetherHub(IRequestHandler<OpenRoomRequest, Room> openRoomHandler,
        IRequestHandler<JoinRoomRequest, Room> joinRoomHandler,
        IRequestHandler<LeaveRoomRequest, Room> leaveRoomHandler,
        IRequestHandler<UpdatePlayerRequest, Room> updatePlayerHandler,
        IRequestHandler<GetUserRoomQuery, Room?> getUserRoomHandler)
    {
        _openRoomHandler = openRoomHandler;
        _joinRoomHandler = joinRoomHandler;
        _leaveRoomHandler = leaveRoomHandler;
        _updatePlayerHandler = updatePlayerHandler;
        _getUserRoomHandler = getUserRoomHandler;
    }

    public async Task OpenRoom(string userName, Guid episodeId)
    {
        var room = await _openRoomHandler.HandleAsync(new OpenRoomRequest(episodeId), Context.ConnectionAborted);
        await Clients.Caller.SendAsync(RoomOpenedMethod, room);
        await JoinRoom(userName, room.Code);
    }

    public async Task JoinRoom(string userName, string roomCode)
    {
        var room = await _joinRoomHandler.HandleAsync(new JoinRoomRequest(roomCode, Context.ConnectionId, userName),
            Context.ConnectionAborted);
        await Groups.AddToGroupAsync(Context.ConnectionId, room.Code);
        await Clients.Group(roomCode).SendAsync(UpdateRoomMethod, room);
    }

    public async Task LeaveRoom(string roomCode)
    {
        await LeaveRoom(roomCode, Context.ConnectionAborted);
    }

    public async Task SendMessage(string userName, string message, string roomCode)
    {
        await Clients.Group(roomCode).SendAsync(ReceiveMessageMethod, userName, message);
    }

    public async Task UpdatePlayerState(long progress, PlayerState state, string roomCode)
    {
        var room = await _updatePlayerHandler.HandleAsync(new UpdatePlayerRequest(roomCode, progress, state),
            Context.ConnectionAborted);
        await Clients.OthersInGroup(room.Code).SendAsync(UpdatePlayerStateMethod, room);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;

        var room = await _getUserRoomHandler.HandleAsync(new GetUserRoomQuery(Context.ConnectionId), cancellationToken);
        if (room != null) await LeaveRoom(room.Code, cancellationToken);

        await base.OnDisconnectedAsync(exception);
    }

    private async Task LeaveRoom(string roomCode, CancellationToken cancellationToken)
    {
        var room = await _leaveRoomHandler.HandleAsync(new LeaveRoomRequest(roomCode, Context.ConnectionId),
            cancellationToken);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.Code, cancellationToken);
        await Clients.Group(room.Code).SendAsync(UpdateRoomMethod, room, cancellationToken);
    }
}