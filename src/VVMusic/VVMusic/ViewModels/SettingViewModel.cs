using System;
using System.Text;
using Xamarin.Forms;
using VVMusic.Views;
using VVMusic.Models;
using VVMusic.Services;
using Xamarin.Essentials;
using System.Collections.Generic;

namespace VVMusic.ViewModels
{
    public class SettingViewModel : VVBaseViewModel
    {
        #region Commands
        public Command SaveCommand { get; }

        public Command TestCommand { get; }

        public Command SelectFileCommand { get; } 
        #endregion

        #region Properties
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

        private bool isEnableSave = false;

        public bool IsEnableSave
        {
            get { return isEnableSave; }
            set { SetProperty(ref isEnableSave, value); }
        }

        private string musicFolder;

        public string MusicFolder
        {
            get { return musicFolder; }
            set { SetProperty(ref musicFolder, value); }
        }

        #endregion

        public IConfigStore<ServerInfo> ConfigStore => DependencyService.Get<IConfigStore<ServerInfo>>();

        public IServerStore ServerStore { get; }

        public SettingViewModel()
        {
            ServerStore = DependencyService.Get<IServerStore>();

            SaveCommand = new Command(SaveButtonCommand);
            TestCommand = new Command(TestButtonCommand);
            SelectFileCommand = new Command(SelectFileButtonCommand);

            LoadConfig();
        }

        public void LoadConfig()
        {
            var serverInfo = ConfigStore.LoadConfigAsync().Result;
            if (serverInfo != null)
            {
                Password = serverInfo.Password;
                UserName = serverInfo.UserName;
                ServerAddress = serverInfo.ServerAddress;
                MusicFolder = serverInfo.MusicFolder;
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

        private async void TestButtonCommand(object obj)
        {
            var isConnected = await ServerStore.TryConnectAsync(ServerAddress, UserName, Password);
            IsEnableSave = isConnected;
        }

        private async void SelectFileButtonCommand(object obj)
        {
            if (ConfigStore.ServerInfo == null)
            {
                return;
            }
            await Shell.Current.GoToAsync($"{nameof(SelectFolderPage)}");
        }
    }
}
