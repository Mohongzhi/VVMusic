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
    public partial class SettingPage : ContentPage
    {
        SettingViewModel settingViewModel = new SettingViewModel();
        public SettingPage()
        {
            InitializeComponent();
            this.BindingContext = settingViewModel;
        }
    }
}