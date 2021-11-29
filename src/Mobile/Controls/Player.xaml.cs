namespace Microsoft.NetConf2021.Maui.Controls;

public partial class Player : ContentView
{   
    private readonly PlayerService playerService;
    
    public Player()
    {
        InitializeComponent();
        this.playerService = ServicesProvider.GetService<PlayerService>();
        this.IsVisible = false;
    }

    private async void PlayGesture_Tapped(object sender, EventArgs e)
    {
        await playerService.PlayAsync(playerService.CurrentEpisode, playerService.CurrentShow);
    }

    internal void OnAppearing()
    {
        this.playerService.IsPlayingChanged += PlayerService_IsPlayingChanged;
        IsVisible = playerService.CurrentEpisode != null;
        if (this.playerService.CurrentEpisode != null)
            UpdatePlayPause();
    }

    private void UpdatePlayPause()
    {
        this.IsVisible = true;

        this.playButton.Source = this.playerService.IsPlaying ? "player_pause.png" : "player_play.png";

        epiosdeTitle.Text = this.playerService.CurrentEpisode.Title;
        authorText.Text = $"{this.playerService.CurrentShow?.Author} - {this.playerService.CurrentEpisode?.Published.ToString("MMM, d yyy")}";

        podcastImage.Source = this.playerService.CurrentShow?.Image;
        duration.Text = this.playerService.CurrentEpisode?.Duration.ToString();
    }

    private void PlayerService_IsPlayingChanged(object sender, EventArgs e)
    {
        if (this.playerService.CurrentEpisode == null)
        {
            IsVisible = false;
        }
        else
        {
            UpdatePlayPause();
        }
    }

    internal void OnDisappearing()
    {
        this.playerService.IsPlayingChanged -= PlayerService_IsPlayingChanged;
    }
}

