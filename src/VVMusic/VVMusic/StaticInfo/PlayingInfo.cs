using System;
using System.Collections.Generic;
using System.Text;
using VVMusic.ViewModels;

namespace VVMusic.StaticInfo
{
    public static class PlayingInfo
    {
        /// <summary>
        /// 当前播放项
        /// </summary>
        public static MusicListItemViewModel MusicListItem { get; set; }

        /// <summary>
        /// 歌词
        /// </summary>
        public static List<LrcItemViewModel> LyricsList { get; set; } = new List<LrcItemViewModel>();

        /// <summary>
        /// 当前位置
        /// </summary>
        public static double CurrentPosition { get; set; }

        /// <summary>
        /// 总时长
        /// </summary>
        public static double Duration { get; set; }

        /// <summary>
        /// 当前播放位置
        /// </summary>
        public static string StrCurrentPosition { get; set; }

        /// <summary>
        /// 总时长
        /// </summary>
        public static string StrDuration { get; set; }

        /// <summary>
        /// 当前播放的时间
        /// </summary>
        public static TimeSpan CurrentTime { get; set; }


        /// <summary>
        /// 播放列表
        /// </summary>
        public static List<MusicListItemViewModel> MusicLists { get; set; } = new List<MusicListItemViewModel>();

    }
}
