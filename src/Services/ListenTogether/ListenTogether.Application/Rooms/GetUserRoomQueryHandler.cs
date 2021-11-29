using ListenTogether.Application.Interfaces;
using ListenTogether.Domain;
using Microsoft.EntityFrameworkCore;

namespace ListenTogether.Application.Rooms;
public class GetUserRoomQuery : IRequest<Room>
{
    public string ConnectionId { get; }

    public GetUserRoomQuery(string connectionId)
    {
        ConnectionId = connectionId;
    }
}

public class GetUserRoomQueryHandler : IRequestHandler<GetUserRoomQuery, Room?>
{
    private readonly IApplicationDbContext _dbContext;

    public GetUserRoomQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Room?> HandleAsync(GetUserRoomQuery request, CancellationToken cancellationToken)
    {
        var room = await _dbContext.Rooms
            .Include(room => room.Users)
            .Include(room => room.Episode.Show)
            .FirstOrDefaultAsync(
            room => room.Users.Any(user => user.ConnectionId == request.ConnectionId), cancellationToken);

        return room;
    }
}