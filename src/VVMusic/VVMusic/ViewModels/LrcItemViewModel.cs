using System;
using System.Text;
using Xamarin.Forms;
using System.Collections.Generic;

namespace VVMusic.ViewModels
{
    public class LrcItemViewModel : VVBaseViewModel
    {
        private string lyrics;

        public string Lyrics
        {
            get { return lyrics; }
            set { SetProperty(ref lyrics, value); }
        }

        private TimeSpan showTime;

        public TimeSpan ShowTime
        {
            get { return showTime; }
            set { SetProperty(ref showTime, value); }
        }

        private double fontSize;

        public double FontSize
        {
            get { return fontSize; }
            set { SetProperty(ref fontSize, value); }
        }

        private Color textColor;

        public Color TextColor
        {
            get { return textColor; }
            set { SetProperty(ref textColor, value); }
        }

        public bool IsShow { get; set; } = false;
    }
}
