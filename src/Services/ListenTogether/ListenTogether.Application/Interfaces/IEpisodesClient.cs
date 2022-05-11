namespace ListenTogether.Application.Interfaces;

public interface IEpisodesClient
{
    Task<Episode> GetEpisodeByIdAsync(Guid episodeId, CancellationToken cancellationToken);
}