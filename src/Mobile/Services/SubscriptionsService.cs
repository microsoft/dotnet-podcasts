namespace Microsoft.NetConf2021.Maui.Services;

public class SubscriptionsService
{
    private List<Show> subscribedShows;

    public SubscriptionsService()
    {
        this.subscribedShows = new List<Show>();
    }

    public IEnumerable<Show> GetSubscribedShows()
    {
        return this.subscribedShows;
    }

    public void SubscribeToShow(Show show)
    {
        if (show == null)
            return ;

        SemanticScreenReader.Announce(string.Format("Subscribe to show {0}", show.Title));
        this.subscribedShows.Add(show);
    }

    public async Task<bool> UnSubscribeFromShowAsync(Show podcast)
    {
        var isUnsubcribed = false;
        var userWantUnsubscribe = await App.Current.MainPage.DisplayAlert(
                    $"Do you want to unsubscribe from {podcast.Title} ?",
                    string.Empty,
                    "Yes, unsubcribe",
                    "Cancel");

        if (userWantUnsubscribe)
        {
            var showToRemove = this.subscribedShows
                .FirstOrDefault(p => p.Id == podcast.Id);
            if (showToRemove != null)
            {
                this.subscribedShows.Remove(showToRemove);
                isUnsubcribed = true;
            }
        }

        return isUnsubcribed;
    }

    internal bool IsSubscribed(Guid id)
    {
        return this.subscribedShows.Any(p => p.Id == id);
    }
}
