namespace ListenTogether.Application.Requests;

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
