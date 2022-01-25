namespace Microsoft.NetConf2021.Maui.Pages
{
    public partial class EpisodeDetailPage
    {
        public EpisodeDetailPage(EpisodeDetailViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            this.player.OnAppearing();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            this.player.OnDisappearing();
            base.OnDisappearing();
        }
    }
}
