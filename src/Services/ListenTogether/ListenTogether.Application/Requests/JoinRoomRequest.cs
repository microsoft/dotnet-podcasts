namespace ListenTogether.Application.Requests;

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
