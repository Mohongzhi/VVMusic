using System;
using System.Collections.Generic;
using System.Text;

namespace VVMusic.ViewModels
{
    public class FolderItemViewModel : VVBaseViewModel
    {
        private string folderName;

        public string FolderName
        {
            get { return folderName; }
            set { SetProperty(ref folderName, value); }
        }

        private string href;

        public string Href
        {
            get { return href; }
            set { SetProperty(ref href, value); }
        }


    }
}
