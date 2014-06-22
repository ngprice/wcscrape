using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace WebcomicScraper.Comic
{
    public class Series
    {
        public HtmlDocument Document;

        public string Title { get; set; }
        public string Summary { get; set; }
        public string Author { get; set; }
        public string Artist { get; set; }
        public string CoverImageURL { get; set; }

        public Index Index { get; set; }

        public Series(HtmlDocument doc)
        {
            Document = doc;
            Index = new Index();
        }

        public override string ToString()
        {
            return String.Format("Series: {0} by {1}", Title, Author);
        }
    }
}
