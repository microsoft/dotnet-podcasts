namespace Microsoft.NetConf2021.Maui.Services;

public class ListenLaterService
{
    List<Tuple<Episode, Show>> episodes;

    public ListenLaterService()
    {
        episodes = new List<Tuple<Episode, Show>>();
    }

    public List<Tuple<Episode, Show>> GetEpisodes()
    {
        return episodes;
    }

    public void Add(Episode episode, Show Show)
    {
        if (episodes.Any(ep => ep.Item1.Id == episode.Id))
            
            return;
        
        episodes.Add(new Tuple<Episode, Show>(episode, Show));
    }

    public void Remove(Episode episode)
    {
        var episodeToRemove = episodes.First(ep => ep.Item1.Id == episode.Id);
        if (episodeToRemove != null)
        {
            episodes.Remove(episodeToRemove);
        }
    }

    public bool IsInListenLater(Episode episode)
    {
        return episodes.Any(ep => ep.Item1.Id == episode.Id);
    }
    
    public bool IsInListenLater(Guid id)
    {
        return episodes.Any(ep => ep.Item1.Id == id);
    }
}
