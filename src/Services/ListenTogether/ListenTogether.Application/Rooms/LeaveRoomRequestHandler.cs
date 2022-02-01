using ListenTogether.Application.Interfaces;
using ListenTogether.Domain;
using Microsoft.EntityFrameworkCore;

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
    private readonly IApplicationDbContext _dbContext;

    public LeaveRoomRequestHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Room> HandleAsync(LeaveRoomRequest request, CancellationToken cancellationToken)
    {
        var room = await _dbContext.Rooms
            .Include(room => room.Users)
            .Include(room => room.Episode.Show)
            .FirstOrDefaultAsync(room => room.Code == request.RoomCode, cancellationToken);

        if (room == null)
        {
            throw new ArgumentNullException(nameof(Room));
        }

        var requestConnectionId = request.ConnectionId;
        room.RemoveUser(requestConnectionId);

        if (room.IsEmpty())
        {
            _dbContext.Rooms.Remove(room);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return room;
    }
}
