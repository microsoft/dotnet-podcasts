namespace ListenTogether.Application.Rooms;

public class GetRoomsQueryQueryHandler : IRequestHandler<GetRoomsRequest, IList<Room>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetRoomsQueryQueryHandler(IApplicationDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<IList<Room>> HandleAsync(GetRoomsRequest request, CancellationToken cancellationToken)
    {
        var rooms = await _dbContext.Rooms
            .Include(room => room.Users)
            .Include(room => room.Episode.Show).ToListAsync(cancellationToken);
        return rooms;
    }
}