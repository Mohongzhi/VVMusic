using VVMusic.ViewModels;
using System.Threading.Tasks;
using Plugin.SimpleAudioPlayer;
using System.Collections.Generic;

namespace VVMusic.Services
{
    /// <summary>
    /// 播放器服务
    /// </summary>
    public interface IPlayerService
    {
        /// <summary>
        /// 切歌模式
        /// </summary>
        SwitchMode SwitchMode { get; set; }

        /// <summary>
        /// 歌词列表
        /// </summary>
        List<string> Lyrics { get; set; }

        /// <summary>
        /// 播放器
        /// </summary>
        ISimpleAudioPlayer audioPlayer { get; set; }

        /// <summary>
        /// 播放
        /// </summary>
        /// <returns></returns>
        Task PlayAsync();

        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        Task StopAsync();

        /// <summary>
        /// 暂停
        /// </summary>
        /// <returns></returns>
        Task PauseAsync();

        /// <summary>
        /// 下一曲
        /// </summary>
        /// <returns></returns>
        Task NextAsync();

        /// <summary>
        /// 上一曲
        /// </summary>
        /// <returns></returns>
        Task PreviousAsync();

        /// <summary>
        /// 加载音乐
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task LoadMusicStreamAsync(MusicListItemViewModel model);

        /// <summary>
        /// 加载歌词
        /// </summary>
        /// <returns></returns>
        Task<List<LrcItemViewModel>> LoadLyrics();

        /// <summary>
        /// 获取播放器时间信息
        /// </summary>
        /// <returns></returns>
        Task GetTimeInfo();

    }

    public enum SwitchMode
    {
        Random,
        Sequence,
        Loop
    }
}
