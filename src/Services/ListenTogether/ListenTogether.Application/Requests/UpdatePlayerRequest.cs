namespace ListenTogether.Application.Requests;

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
