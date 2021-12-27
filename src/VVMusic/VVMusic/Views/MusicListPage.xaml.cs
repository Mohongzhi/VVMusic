using Xamarin.Forms;
using VVMusic.ViewModels;
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

            viewModel.LoadMusicAsync();
        }
    }
}