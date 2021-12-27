using System;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using VVMusic.Models;
using VVMusic.Views;
using VVMusic.Services;
using VVMusic.StaticInfo;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace VVMusic.ViewModels
{
    public class MusicListViewModel : VVBaseViewModel
    {
        public Command OpenPlayingCommand { get; }

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
            OpenPlayingCommand = new Command(OnOpenPlayingCommand);
        }

        /// <summary>
        /// 加载音乐
        /// </summary>
        /// <returns></returns>
        public async Task LoadMusicAsync()
        {
            MusicListItemViewModels.Clear();
            PlayingInfo.MusicLists.Clear();

            var listItems = ServerStore.GetLinkItemsAsync(ConfigStore.ServerInfo.MusicFolder).Result;
            var allMusic = listItems.Where(x => x.IsFolder == false).ToList();
            foreach (var item in allMusic)
            {
                if (item.Name.Contains("mp3") || item.Name.Contains("flac") || item.Name.Contains("wav"))
                {//音乐
                    var musicItem = new MusicListItemViewModel();
                    musicItem.Name = item.Name;
                    PlayingInfo.MusicLists.Add(musicItem);
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

        public async void OnSelectChangedCommand(object obj)
        {
            if (CurrentSelectItem != null)
            {
                Task.Run(async () =>
                {
                    if (PlayingInfo.MusicListItem != null && PlayingInfo.MusicListItem.Name == CurrentSelectItem.Name)
                    {//同一首歌跳过播放，直接打开界面
                        return;
                    }

                    PlayingInfo.MusicListItem = CurrentSelectItem;

                    await PlayerService.LoadMusicStreamAsync(CurrentSelectItem);

                    PlayerService.PlayAsync();
                });

                await Shell.Current.GoToAsync($"{nameof(PlayingPage)}");
            }
        }

        public async void OnOpenPlayingCommand(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(PlayingPage)}");
        }
    }
}
