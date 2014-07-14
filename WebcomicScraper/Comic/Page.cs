using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WebcomicScraper.Comic
{
    public class Page
    {
        public int Num { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public bool Downloaded { get; set; }

        public Page() { }

        public override string ToString()
        {
            return (String.Format("Page {0}", Num));
        }
    }
}
