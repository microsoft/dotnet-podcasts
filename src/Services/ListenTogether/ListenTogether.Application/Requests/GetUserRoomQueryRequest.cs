namespace ListenTogether.Application.Requests;

public class GetUserRoomQueryRequest : IRequest<Room>
{
    public string ConnectionId { get; }

    public GetUserRoomQueryRequest(string connectionId)
    {
        ConnectionId = connectionId;
    }
}
