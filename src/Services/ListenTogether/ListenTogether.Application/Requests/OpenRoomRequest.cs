namespace ListenTogether.Application.Requests;

public class OpenRoomRequest : IRequest<Room>
{
    public OpenRoomRequest(Guid episodeId)
    {
        EpisodeId = episodeId;
    }

    public Guid EpisodeId { get; set; }
}
