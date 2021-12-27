using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using System.Threading;
using VVMusic.Services;
using VVMusic.StaticInfo;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace VVMusic.ViewModels
{
    public class PlayingViewModel : VVBaseViewModel
    {
        #region Commands
        public Command NextCommand { get; }

        public Command PreviousCommand { get; }

        public Command PlayCommand { get; }
        #endregion

        #region Mvvm
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
        #endregion

        public Action<int> OnScrollListView;

        CancellationTokenSource tokenSource = new CancellationTokenSource();
        CancellationToken token;

        internal void LoadLyrics()
        {
            Task.Run(async () =>
           {
               var lycList = await PlayerService.LoadLyrics();
               Lyrics.Clear();
               foreach (var item in lycList)
               {
                   Lyrics.Add(item);
               }

               ScrollLyrics(true);
           });
        }

        public ObservableCollection<LrcItemViewModel> Lyrics { get; set; }

        public IPlayerService PlayerService { get; set; }

        public IServerStore ServerStore { get; }

        public PlayingViewModel()
        {
            PlayerService = DependencyService.Get<IPlayerService>();
            ServerStore = DependencyService.Get<IServerStore>();

            CreateCancelToken();

            PlayCommand = new Command(Play);
            NextCommand = new Command(Next);
            PreviousCommand = new Command(Previous);

            Lyrics = new ObservableCollection<LrcItemViewModel>();
        }

        public async void Play(object obj)
        {
            if (PlayerService.audioPlayer.IsPlaying)
            {
                PlayerService.PauseAsync();
            }
            else
            {
                PlayerService.PlayAsync();
                ScrollLyrics();
            }
        }

        public async void Next(object obj)
        {
            CancelAndCreateToken();

            Lyrics.Clear();
            Lyrics.Add(new LrcItemViewModel()
            {
                FontSize = 22,
                Lyrics = "歌词加载中...",
                ShowTime = new TimeSpan(0, 0, 0),
                TextColor = Color.LightBlue
            });

            Task.Run(async () =>
            {
                await PlayerService.StopAsync();
                PlayerService.NextAsync();

                LoadLyrics();
            });
        }

        public async void Previous(object obj)
        {
            CancelAndCreateToken();

            Lyrics.Clear();
            Lyrics.Add(new LrcItemViewModel()
            {
                FontSize = 22,
                Lyrics = "歌词加载中...",
                ShowTime = new TimeSpan(0, 0, 0),
                TextColor = Color.LightBlue
            });

            Task.Run(async () =>
            {
                await PlayerService.StopAsync();
                PlayerService.PreviousAsync();

                LoadLyrics();
            });
        }

        private void ScrollLyrics(bool isWaitPlay = false)
        {
            Task.Run(async () =>
            {
                if (isWaitPlay)
                    while (!PlayerService.audioPlayer.IsPlaying)
                    {
                        if (token.IsCancellationRequested)
                            return;
                        await Task.Delay(300);
                    }
                while (PlayerService.audioPlayer.IsPlaying)
                {
                    if (token.IsCancellationRequested)
                        return;
                    await PlayerService.GetTimeInfo();

                    StrCurrentPosition = PlayingInfo.StrCurrentPosition;
                    StrDuration = PlayingInfo.StrDuration;
                    CurrentPosition = PlayingInfo.CurrentPosition;
                    Duration = PlayingInfo.Duration;

                    SetLyrics(PlayingInfo.CurrentTime);
                    await Task.Delay(100);
                }
                if (token.IsCancellationRequested)
                    return;
            }, token);
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

        void CreateCancelToken()
        {
            CancellationToken token = tokenSource.Token;
        }

        public void CancelAndCreateToken()
        {
            tokenSource.Cancel();
            CreateCancelToken();
        }

    }
}
