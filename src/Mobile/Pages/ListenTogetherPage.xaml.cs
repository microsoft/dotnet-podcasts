﻿using CommunityToolkit.Mvvm.Messaging;

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
            WeakReferenceMessenger.Default.Send<string, string>(".NET Pods", "LeaveRoom");
            base.OnDisappearing();
        }
    }
}
