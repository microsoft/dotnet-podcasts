namespace Microsoft.NetConf2021.Maui.Pages
{
    public partial class SubscriptionsPage: BaseContentPage
    {
        public SubscriptionsPage(SubscriptionsViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            player.OnAppearing();
            subscribedPodcasts.SelectedItem = null;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            this.player.OnDisappearing();
            base.OnDisappearing();
        }
    }
}
