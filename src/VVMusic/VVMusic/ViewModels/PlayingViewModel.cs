using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VVMusic.Services;
using VVMusic.StaticInfo;
using VVMusic.Views;
using Xamarin.Forms;

namespace VVMusic.ViewModels
{
    public class PlayingViewModel : VVBaseViewModel
    {
        public Command PlayCommand { get; }
               
        private string strCurrentPosition;

        public string StrCurrentPosition
        {
            get { return strCurrentPosition; }
            set { SetProperty(ref strCurrentPosition, value); }
        }

        private string strDuration;

        public string StrDuration
        {
            get { return strDuration; }
            set { SetProperty(ref strDuration, value); }
        }

        private double currentPosition = 0;

        public double CurrentPosition
        {
            get { return currentPosition; }
            set { SetProperty(ref currentPosition, value); }
        }

        private double duration = 1;

        public double Duration
        {
            get { return duration; }
            set { SetProperty(ref duration, value); }
        }

        internal void LoadLyrics()
        {
            Task.Run(async () =>
           {
               //while (!PlayerService.audioPlayer.IsPlaying)
               //{
               //    await Task.Delay(300);
               //}
               var lycList = await PlayerService.LoadLyrics();
               foreach (var item in lycList)
               {
                   Lyrics.Add(item);
               }

               ScrollLyrics();
           });
        }

        public ObservableCollection<LrcItemViewModel> Lyrics { get; set; }

        public IPlayerService PlayerService { get; set; }

        public IServerStore ServerStore { get; }

        public PlayingViewModel()
        {
            PlayerService = DependencyService.Get<IPlayerService>();
            ServerStore = DependencyService.Get<IServerStore>();

            PlayCommand = new Command(Play);
            Lyrics = new ObservableCollection<LrcItemViewModel>();
        }

        public async void Play(object obj)
        {
            PlayerService.PlayAsync();
        }

        private void ScrollLyrics()
        {
            Task.Run(async () =>
            {
                while (!PlayerService.audioPlayer.IsPlaying)
                {
                    await Task.Delay(300);
                }
                while (PlayerService.audioPlayer.IsPlaying)
                {
                    await PlayerService.GetTimeInfo();

                    StrCurrentPosition = PlayingInfo.StrCurrentPosition;
                    StrDuration = PlayingInfo.StrDuration;
                    CurrentPosition = PlayingInfo.CurrentPosition;
                    Duration = PlayingInfo.Duration;

                    SetLyrics(PlayingInfo.CurrentTime);
                    await Task.Delay(500);
                }
            });
        }


        void SetLyrics(TimeSpan ts)
        {
            var first = Lyrics.FirstOrDefault(x => x.ShowTime.TotalSeconds <= ts.TotalSeconds && x.IsShow == false);
            if (first != null)
            {
                foreach (var item in Lyrics)
                {
                    item.FontSize = 18;
                    item.TextColor = Color.Black;
                }
                first.TextColor = Color.LightBlue;
                first.FontSize = 20;
                first.IsShow = true;

                var id = Lyrics.IndexOf(first);

                if ((id + 3) < Lyrics.Count)
                {
                    OnScrollListView?.Invoke(id + 3);
                }
                if ((id + 2) < Lyrics.Count)
                {
                    OnScrollListView?.Invoke(id + 2);
                }
                else if ((id + 1) < Lyrics.Count)
                {
                    OnScrollListView?.Invoke(id + 1);
                }
                else
                {
                    OnScrollListView?.Invoke(id);
                }
            }
        }



        public Action<int> OnScrollListView;
    }
}
