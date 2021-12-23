using System;
using VVMusic.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VVMusic
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<ServerConfigStore>();
            DependencyService.Register<ServerStore>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
