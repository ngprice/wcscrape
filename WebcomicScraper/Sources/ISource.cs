using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebcomicScraper.Comic;
using HtmlAgilityPack;

namespace WebcomicScraper.Sources
{
    public interface ISource
    {
        string FindTitle(HtmlDocument doc);
        string FindDescription(HtmlDocument doc);
        string FindAuthor(HtmlDocument doc);
        string FindArtist(HtmlDocument doc);
        string FindCover(HtmlDocument doc);
        List<Chapter> FindChapters(HtmlDocument doc);
        List<Page> GetPages(HtmlNodeCollection nodes);
    }
}
