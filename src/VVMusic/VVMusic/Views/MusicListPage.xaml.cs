using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVMusic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VVMusic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MusicListPage : ContentPage
    {
        MusicListViewModel viewModel = new MusicListViewModel();
        public MusicListPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadMusic();
        }
    }
}