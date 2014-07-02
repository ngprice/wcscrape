using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebcomicScraper.Sources;
using HtmlAgilityPack;

namespace WebcomicScraper.Comic
{
    public class Series
    {
        public HtmlDocument Document { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public string Author { get; set; }
        public string Artist { get; set; }
        public Uri SeedURL { get; set; }
        public string CoverImageURL { get; set; }
        public Index Index { get; set; }
        public ISource Source { get; set; }
        public Link NextLink { get; set; }
        public Link PrevLink { get; set; }
        public Link FirstLink { get; set; }
        public Link LastLink { get; set; }

        public Series(Uri url, HtmlDocument doc)
        {
            SeedURL = url;
            Document = doc;
            Index = new Index();
        }

        public override string ToString()
        {
            return String.Format("Series: {0} by {1}", Title, Author);
        }
    }
}
