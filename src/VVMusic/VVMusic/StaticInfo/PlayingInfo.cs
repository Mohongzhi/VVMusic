using System;
using System.Collections.Generic;
using System.Text;
using VVMusic.ViewModels;

namespace VVMusic.StaticInfo
{
    public static class PlayingInfo
    {
        public static MusicListItemViewModel MusicListItem { get; set; }

        public static List<LrcItemViewModel> LyricsList { get; set; } = new List<LrcItemViewModel>();

        public static double CurrentPosition { get; set; }

        public static double Duration { get; set; }

        public static string StrCurrentPosition { get; set; }

        public static string StrDuration { get; set; }

        public static TimeSpan CurrentTime { get; set; }

    }
}
