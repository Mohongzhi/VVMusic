using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VVMusic.Models;
using VVMusic.ViewModels;

namespace VVMusic.Services
{
    /// <summary>
    /// 播放器服务
    /// </summary>
    public interface IPlayerService
    {
        SwitchMode SwitchMode { get; set; }

        /// <summary>
        /// 播放列表
        /// </summary>
        List<MusicListItemViewModel> MusicLists { get; set; }

        /// <summary>
        /// 歌词列表
        /// </summary>
        List<string> Lyrics { get; set; }

        /// <summary>
        /// 播放器
        /// </summary>
        ISimpleAudioPlayer audioPlayer { get; set; }

        Task PlayAsync();

        Task StopAsync();

        Task PauseAsync();

        Task NextAsync();

        Task PreviousAsync();

        Task LoadMusicStreamAsync(MusicListItemViewModel model);

        Task<List<LrcItemViewModel>> LoadLyrics();

        Task GetTimeInfo();

    }

    public enum SwitchMode
    {
        Random,
        Sequence,
        Loop
    }
}
