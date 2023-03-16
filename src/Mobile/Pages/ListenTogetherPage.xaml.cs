
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.NetConf2021.Maui.Messaging;

namespace Microsoft.NetConf2021.Maui.Pages
{
    public partial class ListenTogetherPage
    {
        public ListenTogetherPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            player.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            player.OnDisappearing();

            WeakReferenceMessenger.Default.Send<LeaveRoomNotification>();

            base.OnDisappearing();
        }
    }
}
