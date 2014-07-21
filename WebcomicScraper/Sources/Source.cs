using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebcomicScraper.Comic;
using HtmlAgilityPack;

namespace WebcomicScraper.Sources
{
    public abstract class Source
    {
        protected Series ParentSeries;

        abstract public string FindTitle(HtmlDocument doc);
        abstract public string FindDescription(HtmlDocument doc);
        abstract public string FindAuthor(HtmlDocument doc);
        abstract public string FindArtist(HtmlDocument doc);
        abstract public string FindCover(HtmlDocument doc);
        abstract public List<Chapter> FindChapters(HtmlDocument doc);
        abstract public List<Page> GetPages(HtmlDocument doc);

        abstract public Page GetPage(Link imageLink, HtmlDocument doc);
        abstract public Page GetPageFromLink(Page start, Link direction);
    }
}
