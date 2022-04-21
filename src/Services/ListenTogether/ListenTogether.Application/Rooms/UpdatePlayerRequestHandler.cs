using ListenTogether.Application.Interfaces;
using ListenTogether.Domain;
using Microsoft.EntityFrameworkCore;
using Orleans;

namespace ListenTogether.Application.Rooms;

public class UpdatePlayerRequest : IRequest<Room>
{
    public UpdatePlayerRequest(string roomCode, long progress, PlayerState state)
    {
        RoomCode = roomCode;
        Progress = progress;
        State = state;
    }

    public string RoomCode { get; set; }
    public float Progress { get; set; }
    public PlayerState State { get; set; }
}

public class UpdatePlayerRequestHandler : IRequestHandler<UpdatePlayerRequest, Room>
{
    private readonly IGrainFactory _grainFactory;

    public UpdatePlayerRequestHandler(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    public async Task<Room> HandleAsync(UpdatePlayerRequest request, CancellationToken cancellationToken)
    {
        var seconds = TimeSpan.FromSeconds(request.Progress);

        var roomGrain = _grainFactory.GetGrain<IRoomGrain>(request.RoomCode);
        var room = await roomGrain.UpdatePlayerState(seconds, request.State);

        return room;
    }
}