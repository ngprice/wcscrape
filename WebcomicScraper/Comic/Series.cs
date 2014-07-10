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
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public HtmlDocument Document { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public string Author { get; set; }
        public string Artist { get; set; }
        public string SeedURL { get; set; }
        public string CoverImageURL { get; set; }
        public Index Index { get; set; }
        public Source Source { get; set; }
        public Link NextLink { get; set; }
        public Link PrevLink { get; set; }
        public Link FirstLink { get; set; }
        public Link LastLink { get; set; }

        public Series() { }

        public Series(string url, HtmlDocument doc)
        {
            SeedURL = url;
            Document = doc;
            Index = new Index();
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
