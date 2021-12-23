using System;
using System.Collections.Generic;
using System.Text;

namespace VVMusic.Models
{
    public class LinkItem
    {
        public string Name { get; set; }

        public string Href { get; set; }

        public bool IsFolder { get; set; } = false;
    }
}
