using System;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using VVMusic.Models;
using VVMusic.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VVMusic.ViewModels
{
    public class MusicListViewModel : VVBaseViewModel
    {
        public Command SelectChangedCommand { get; }
        public ObservableCollection<MusicListItemViewModel> MusicListItemViewModels { get; set; }

        private MusicListItemViewModel currentSelectItem;

        public MusicListItemViewModel CurrentSelectItem
        {
            get { return currentSelectItem; }
            set { SetProperty(ref currentSelectItem, value); }
        }


        public IServerStore ServerStore { get; }

        public IConfigStore<ServerInfo> ConfigStore { get; }

        public IPlayerService PlayerService { get; }

        public MusicListViewModel()
        {
            ServerStore = DependencyService.Get<IServerStore>();
            PlayerService = DependencyService.Get<PlayerService>();
            ConfigStore = DependencyService.Get<IConfigStore<ServerInfo>>();

            MusicListItemViewModels = new ObservableCollection<MusicListItemViewModel>();

            SelectChangedCommand = new Command(OnSelectChangedCommand);
        }        

        public async Task LoadMusic()
        {
           MusicListItemViewModels.Clear();

            var listItems = ServerStore.GetLinkItemsAsync(ConfigStore.ServerInfo.MusicFolder).Result;
            var allMusic = listItems.Where(x => x.IsFolder == false).ToList();
            foreach (var item in allMusic)
            {
                if (item.Name.Contains("mp3") || item.Name.Contains("flac") || item.Name.Contains("wav"))
                {//音乐
                    var musicItem = new MusicListItemViewModel();
                    musicItem.Name = item.Name;
                    PlayerService.MusicLists.Add(musicItem);
                    MusicListItemViewModels.Add(musicItem);
                    var str = item.Name.Remove(item.Name.LastIndexOf("."));
                    var lrc = PlayerService.Lyrics.FirstOrDefault(x => x.Contains(str));
                    if (lrc != null)
                    {
                        musicItem.Lyrics = lrc;
                    }
                }
                if (item.Name.Contains(".lrc"))
                {//歌词
                    var str = item.Name.Remove(item.Name.IndexOf(".lrc"));
                    var first = MusicListItemViewModels.FirstOrDefault(x => x.Name.Contains(str));
                    if (first != null)
                    {
                        first.Lyrics = str + ".lrc";
                        continue;
                    }
                    if (!PlayerService.Lyrics.Any(x => x.Contains(str)))
                    {
                        PlayerService.Lyrics.Add(str + ".lrc");
                    }
                }
            }
        }

        public void OnSelectChangedCommand(object obj)
        {
            if (CurrentSelectItem != null)
            {
                Task.Run(async () =>
                {
                    await PlayerService.LoadMusicStreamAsync(CurrentSelectItem);

                    PlayerService.PlayAsync();
                });
            }
        }
    }
}
