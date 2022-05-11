namespace ListenTogether.Application.Rooms;

public class GetUserRoomQueryHandler : IRequestHandler<GetUserRoomQueryRequest, Room?>
{
    private readonly IApplicationDbContext _dbContext;

    public GetUserRoomQueryHandler(IApplicationDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<Room?> HandleAsync(GetUserRoomQueryRequest request, CancellationToken cancellationToken)
    {
        var room = await _dbContext.Rooms
            .Include(room => room.Users)
            .Include(room => room.Episode.Show)
            .FirstOrDefaultAsync(
            room => room.Users.Any(user => user.ConnectionId == request.ConnectionId), cancellationToken);

        return room;
    }
}