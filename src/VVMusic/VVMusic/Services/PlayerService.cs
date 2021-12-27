using System;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using VVMusic.ViewModels;
using VVMusic.StaticInfo;
using System.Threading.Tasks;
using Plugin.SimpleAudioPlayer;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VVMusic.Services
{
    public class PlayerService : IPlayerService
    {
        public List<MusicListItemViewModel> MusicLists { get; set; } = new List<MusicListItemViewModel>();
        public List<string> Lyrics { get; set; } = new List<string>();

        public ISimpleAudioPlayer audioPlayer { get; set; }
        public SwitchMode SwitchMode { get; set; } = SwitchMode.Loop;

        public IServerStore ServerStore { get; }

        public PlayerService()
        {
            ServerStore = DependencyService.Get<IServerStore>();

            audioPlayer = CrossSimpleAudioPlayer.Current;
        }

        public async Task NextAsync()
        {
            var currentIndex = PlayingInfo.MusicLists.IndexOf(PlayingInfo.MusicListItem);
            MusicListItemViewModel nextMusicItem = null;
            if (currentIndex != -1 && currentIndex != PlayingInfo.MusicLists.Count - 1)
            {
                switch (SwitchMode)
                {
                    case SwitchMode.Loop:
                    case SwitchMode.Sequence:
                        {
                            nextMusicItem = PlayingInfo.MusicLists[currentIndex + 1];
                            break;
                        }
                    case SwitchMode.Random:
                        {
                            var rd = new Random(DateTime.Now.Millisecond);
                            nextMusicItem = PlayingInfo.MusicLists[rd.Next(rd.Next(0, PlayingInfo.MusicLists.Count))];
                            break;
                        }
                }
            }
            else if (currentIndex == PlayingInfo.MusicLists.Count - 1)
            {
                switch (SwitchMode)
                {
                    case SwitchMode.Loop:
                        {
                            nextMusicItem = PlayingInfo.MusicLists.FirstOrDefault();
                            break;
                        }
                    case SwitchMode.Sequence:
                        break;
                    case SwitchMode.Random:
                        {
                            var rd = new Random(DateTime.Now.Millisecond);
                            nextMusicItem = PlayingInfo.MusicLists[rd.Next(rd.Next(0, PlayingInfo.MusicLists.Count))];
                            break;
                        }
                }
            }
            if (nextMusicItem != null)
            {
                PlayingInfo.MusicListItem = nextMusicItem;
                await LoadMusicStreamAsync(nextMusicItem);
                PlayAsync();
            }
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
            var currentIndex = PlayingInfo.MusicLists.IndexOf(PlayingInfo.MusicListItem);
            MusicListItemViewModel nextMusicItem = null;
            if (currentIndex > 0)
            {
                switch (SwitchMode)
                {
                    case SwitchMode.Loop:
                    case SwitchMode.Sequence:
                        {
                            nextMusicItem = PlayingInfo.MusicLists[currentIndex - 1];
                            break;
                        }
                    case SwitchMode.Random:
                        {
                            var rd = new Random(DateTime.Now.Millisecond);
                            nextMusicItem = PlayingInfo.MusicLists[rd.Next(rd.Next(0, PlayingInfo.MusicLists.Count))];
                            break;
                        }
                }
            }
            else if (currentIndex == 0)
            {
                switch (SwitchMode)
                {
                    case SwitchMode.Loop:
                        {
                            nextMusicItem = PlayingInfo.MusicLists.LastOrDefault();
                            break;
                        }
                    case SwitchMode.Sequence:
                        break;
                    case SwitchMode.Random:
                        {
                            var rd = new Random(DateTime.Now.Millisecond);
                            nextMusicItem = PlayingInfo.MusicLists[rd.Next(rd.Next(0, PlayingInfo.MusicLists.Count))];
                            break;
                        }
                }
            }
            if (nextMusicItem != null)
            {
                PlayingInfo.MusicListItem = nextMusicItem;
                await LoadMusicStreamAsync(nextMusicItem);
                PlayAsync();
            }
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

        public async Task<List<LrcItemViewModel>> LoadLyrics()
        {
            PlayingInfo.LyricsList.Clear();
            if (PlayingInfo.MusicListItem != null)
            {
                var lrcs = await ServerStore.DownloadMusicLrcAsync(PlayingInfo.MusicListItem.Lyrics);
                StreamReader sr = new StreamReader(lrcs, Encoding.UTF8);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("[ti:"))
                    {
                        //PlayingService.Title = SplitInfo(line);
                    }
                    else if (line.StartsWith("[ar:"))
                    {
                        //PlayingService.Artist = SplitInfo(line);
                    }
                    else if (line.StartsWith("[al:"))
                    {
                        //PlayingService.Album = SplitInfo(line);
                    }
                    else if (line.StartsWith("[by:"))
                    {
                        //PlayingService.LrcBy = SplitInfo(line);
                    }
                    else if (line.StartsWith("[offset:"))
                    {
                        //PlayingService.Offset = SplitInfo(line);
                    }
                    else
                    {
                        Regex regex = new Regex(@"\[([0-9.:]*)\]+(.*)", RegexOptions.Compiled);
                        MatchCollection mc = regex.Matches(line);
                        double time = TimeSpan.Parse("00:" + mc[0].Groups[1].Value).TotalSeconds;
                        string word = mc[0].Groups[2].Value;
                        PlayingInfo.LyricsList.Add(new LrcItemViewModel()
                        {
                            Lyrics = word,
                            FontSize = 18,
                            ShowTime = TimeSpan.Parse("00:" + mc[0].Groups[1].Value),
                            TextColor = Color.Black
                        });
                    }
                }
                if (PlayingInfo.LyricsList.Count == 0)
                    PlayingInfo.LyricsList.Add(new LrcItemViewModel()
                    {
                        Lyrics = "暂无歌词",
                        FontSize = 25,
                        ShowTime = new TimeSpan(0),
                        TextColor = Color.Black
                    });

            }
            return PlayingInfo.LyricsList;
        }

        public async Task GetTimeInfo()
        {
            var intPosition = Convert.ToInt32(audioPlayer.CurrentPosition);
            var intDuration = Convert.ToInt32(audioPlayer.Duration);
            var tsCurrent = new TimeSpan(0, 0, intPosition);
            var tsDuration = new TimeSpan(0, 0, intDuration);
            PlayingInfo.StrCurrentPosition = $"{tsCurrent.Hours.ToString().PadLeft(2, '0')}:{tsCurrent.Minutes.ToString().PadLeft(2, '0')}:{tsCurrent.Seconds.ToString().PadLeft(2, '0')}";
            PlayingInfo.StrDuration = $"{tsDuration.Hours.ToString().PadLeft(2, '0')}:{tsDuration.Minutes.ToString().PadLeft(2, '0')}:{tsDuration.Seconds.ToString().PadLeft(2, '0')}";
            PlayingInfo.CurrentPosition = audioPlayer.CurrentPosition;
            PlayingInfo.Duration = audioPlayer.Duration;
            PlayingInfo.CurrentTime = tsCurrent;
        }

    }
}
