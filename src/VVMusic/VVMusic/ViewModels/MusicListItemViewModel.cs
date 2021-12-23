using System;
using System.Collections.Generic;
using System.Text;

namespace VVMusic.ViewModels
{
    public class MusicListItemViewModel : VVBaseViewModel
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string lyrics;

        public string Lyrics
        {
            get { return lyrics; }
            set { SetProperty(ref lyrics, value); }
        }
    }
}
