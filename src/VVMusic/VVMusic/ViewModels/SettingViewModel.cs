using System;
using System.Text;
using Xamarin.Forms;
using VVMusic.Models;
using VVMusic.Services;
using Xamarin.Essentials;
using System.Collections.Generic;

namespace VVMusic.ViewModels
{
    public class SettingViewModel : VVBaseViewModel
    {
        public Command SaveCommand { get; }

        public Command TestCommand { get; }

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

        public IConfigStore<ServerInfo> ConfigStore => DependencyService.Get<IConfigStore<ServerInfo>>();

        public SettingViewModel()
        {
            SaveCommand = new Command(SaveButtonCommand);
            TestCommand = new Command(TestButtonCommand);

            var serverInfo = ConfigStore.LoadConfigAsync().Result;
            if (serverInfo != null)
            {
                Password = serverInfo.Password;
                UserName = serverInfo.UserName;
                ServerAddress = serverInfo.ServerAddress;
            }            
        }

        private void SaveButtonCommand(object obj)
        {
            var serverInfo = new ServerInfo()
            {
                 Password = Password,
                  UserName = UserName,
                   ServerAddress = ServerAddress
            };
            ConfigStore.SaveConfigAsync(serverInfo).Wait();
        }

        private void TestButtonCommand(object obj)
        {

        }
    }
}
