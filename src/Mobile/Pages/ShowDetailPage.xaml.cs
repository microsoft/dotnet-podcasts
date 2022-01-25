namespace Microsoft.NetConf2021.Maui.Pages
{
    public partial class ShowDetailPage : BaseContentPage
    {
        public ShowDetailPage(ShowDetailViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
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
