using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebcomicScraper.Comic
{
    public class Page
    {
        public int PageNum { get; set; }
        public bool Downloaded { get; set; }
        public string ImageURL { get; set; }

        public Page()
        {
            Downloaded = false;
        }

        public override string ToString()
        {
            return (String.Format("Page {0}", PageNum));
        }
    }
}
