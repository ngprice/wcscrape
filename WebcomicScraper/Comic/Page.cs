using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebcomicScraper.Comic
{
    public class Page
    {
        public string Title { get; set; }
        public bool Downloaded { get; set; }

        public override string ToString()
        {
            return (String.Format("Page: {0}", Title));
        }
    }
}
