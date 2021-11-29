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

    public async Task SubscribeToShowAsync(Show show)
    {
        if (show == null)
            return;

        if (IsSubscribed(show.Id))
        {
            await UnSubscribeFromShowAsync(show);
            return;
        }

        this.subscribedShows.Add(show);
    }

    public async Task UnSubscribeFromShowAsync(Show podcast)
    {
        var userWantUnsubscribe = await App.Current.MainPage.DisplayAlert(
                    $"Do you want to unsubscribe from {podcast.Title} ?",
                    string.Empty,
                    "Yes, unsubcribe",
                    "Cancel");

        if (userWantUnsubscribe)
        {
            var showToRemove = this.subscribedShows.FirstOrDefault(p => p.Id == podcast.Id);
            if (showToRemove != null)
            {
                this.subscribedShows.Remove(showToRemove);
            }
        }
    }

    internal bool IsSubscribed(Guid id)
    {
        return this.subscribedShows.Any(p => p.Id == id);
    }
}
