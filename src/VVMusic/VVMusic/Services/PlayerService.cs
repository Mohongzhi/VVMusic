using System;
using System.Text;
using VVMusic.ViewModels;
using System.Threading.Tasks;
using Plugin.SimpleAudioPlayer;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace VVMusic.Services
{
    public class PlayerService : IPlayerService
    {
        public List<MusicListItemViewModel> MusicLists { get; set; } = new List<MusicListItemViewModel>();
        public List<string> Lyrics { get; set; } = new List<string>();
        public ISimpleAudioPlayer audioPlayer { get; set; }
        public SwitchMode SwitchMode { get; set; }

        public IServerStore ServerStore { get; }

        public PlayerService()
        {
            ServerStore = DependencyService.Get<IServerStore>();

            audioPlayer = CrossSimpleAudioPlayer.Current;
        }

        public async Task NextAsync()
        {

        }

        public async Task PauseAsync()
        {
            audioPlayer.Pause();
        }

        public async Task PlayAsync()
        {
            audioPlayer.Play();
        }

        public async Task PreviousAsync()
        {
        }

        public async Task StopAsync()
        {
            audioPlayer.Stop();
        }

        public async Task LoadMusicStreamAsync(MusicListItemViewModel model)
        {
            var musicStream = await ServerStore.DownloadMusicAsync(model.Name);

            audioPlayer.Load(musicStream);
        }
    }
}
