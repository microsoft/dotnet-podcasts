using ListenTogether.Application.Interfaces;
using ListenTogether.Domain;
using Microsoft.EntityFrameworkCore;

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
    private readonly IApplicationDbContext _dbContext;

    public UpdatePlayerRequestHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Room> HandleAsync(UpdatePlayerRequest request, CancellationToken cancellationToken)
    {
        var seconds = TimeSpan.FromSeconds(request.Progress);

        var room = await _dbContext.Rooms
            .Include(room => room.Users)
            .Include(room => room.Episode.Show)
            .FirstOrDefaultAsync(room => room.Code == request.RoomCode, cancellationToken);

        if (room == null)
        {
            throw new ArgumentNullException(nameof(Room));
        }

        room.UpdatePlayerState(seconds, request.State);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return room;
    }
}