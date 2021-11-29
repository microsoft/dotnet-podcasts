using ListenTogether.Application.Interfaces;
using ListenTogether.Domain;
using Microsoft.EntityFrameworkCore;

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
    private readonly IApplicationDbContext _dbContext;

    public JoinRoomRequestHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Room> HandleAsync(JoinRoomRequest request, CancellationToken cancellationToken)
    {
        var room = await _dbContext.Rooms
            .Include(room => room.Users)
            .Include(room => room.Episode.Show)
            .FirstOrDefaultAsync(room => room.Code == request.RoomCode, cancellationToken);

        if (room == null)
        {
            throw new ArgumentNullException(nameof(Room));
        }

        var user = new User(request.ConnectionId, request.UserName);
        room.AddUser(user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return room;
    }
}