using System;
using System.Collections.Generic;
using VVMusic.Views;
using Xamarin.Forms;

namespace VVMusic
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SelectFolderPage),typeof(SelectFolderPage));
            Routing.RegisterRoute(nameof(PlayingPage), typeof(PlayingPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
