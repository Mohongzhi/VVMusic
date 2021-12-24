using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using VVMusic.ViewModels;
using Xamarin.Forms.Xaml;
using VVMusic.Services;

namespace VVMusic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayingPage : ContentPage
    {
        PlayingViewModel PlayingViewModel;

        public PlayingPage()
        {
            PlayingViewModel = new PlayingViewModel();
            PlayingViewModel.OnScrollListView += (id) =>
            {
                LrcListView.ScrollTo(id);
            };

            InitializeComponent();
            BindingContext = PlayingViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            PlayingViewModel.LoadLyrics();
        }
    }
}