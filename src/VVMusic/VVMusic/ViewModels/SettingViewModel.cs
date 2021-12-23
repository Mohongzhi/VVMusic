using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace VVMusic.ViewModels
{
    public class SettingViewModel : VVBaseViewModel
    {
        public Command SaveCommand { get; }

        private string serverAddress;

        public string ServerAddress
        {
            get { return serverAddress; }
            set { SetProperty(ref serverAddress, value); }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }
    }
}
