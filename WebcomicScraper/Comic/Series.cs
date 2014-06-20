using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebcomicScraper.Comic
{
    public class Series
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Artist { get; set; }
        public Index Index { get; set; }

        public Series(HtmlDocument doc)
        {
            Index = new Index(this, doc);
        }

        public bool Analyze()
        {
            bool result = true;

            Index.Analyze();

            //wait for a bit

            return result;
        }
    }
}
