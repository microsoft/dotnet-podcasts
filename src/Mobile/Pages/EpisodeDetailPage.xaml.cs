namespace Microsoft.NetConf2021.Maui.Pages
{
    public partial class EpisodeDetailPage
    {
        private EpisodeDetailViewModel viewModel => BindingContext as EpisodeDetailViewModel;

        public EpisodeDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.player.OnAppearing();
            await viewModel.InitializeAsync();
        }

        protected override void OnDisappearing()
        {
            this.player.OnDisappearing();
            base.OnDisappearing();
        }
    }
}
