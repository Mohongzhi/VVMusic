using System;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using VVMusic.Models;
using VVMusic.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VVMusic.ViewModels
{
    public class SelectFolderViewModel : VVBaseViewModel
    {
        public Command ConfirmCommand { get; }

        public Command OnSelectFolderCommand { get; }

        private string folderTitle;

        public string FolderTitle
        {
            get { return folderTitle; }
            set { SetProperty(ref folderTitle, value); }
        }

        private FolderItemViewModel selectionItem;

        public FolderItemViewModel SelectionItem
        {
            get { return selectionItem; }
            set { SetProperty(ref selectionItem, value); }
        }


        public ObservableCollection<FolderItemViewModel> FolderItemViewModels { get; set; }

        public IServerStore ServerStore { get; set; }

        public IConfigStore<ServerInfo> ConfigStore { get; set; }

        public SelectFolderViewModel()
        {
            ServerStore = DependencyService.Get<IServerStore>();
            ConfigStore = DependencyService.Get<IConfigStore<ServerInfo>>();

            FolderItemViewModels = new ObservableCollection<FolderItemViewModel>();

            LoadLinkItems("");

            OnSelectFolderCommand = new Command(OnSelectFolderItemCommand);
            ConfirmCommand = new Command(OnConfirmButtonCommand);
        }

        public void OnSelectFolderItemCommand(object obj)
        {
            var model = obj as SelectFolderViewModel;
            if (model != null)
            {
                var it = model.SelectionItem;
                FolderTitle = it.FolderName;
                LoadLinkItems(it.Href);
            }
        }

        public async void OnConfirmButtonCommand(object obj)
        {
            var serverInfo = ConfigStore.LoadConfigAsync().Result;
            if (serverInfo != null)
            {
                serverInfo.MusicFolder = SelectionItem.Href;
                await ConfigStore.SaveConfigAsync(serverInfo);
                await Shell.Current.GoToAsync("..");
            }
        }

        private void LoadLinkItems(string href)
        {
            try
            {
                FolderItemViewModels.Clear();
                var listLinkItems = ServerStore.GetLinkItemsAsync(href).Result.Where(x => x.IsFolder).ToList();
                foreach (var item in listLinkItems)
                {
                    FolderItemViewModels.Add(new FolderItemViewModel()
                    {
                        FolderName = item.Name,
                        Href = item.Href
                    });
                }
            }
            catch
            {

            }            
        }
    }
}
