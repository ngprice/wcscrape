using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using WebcomicScraper.Comic;
using HtmlAgilityPack;

namespace WebcomicScraper.Sources
{
    public class SequentialSource : Source
    {

        public override string FindTitle(HtmlDocument doc)
        {
            throw new NotImplementedException();
        }

        public override string FindDescription(HtmlDocument doc)
        {
            throw new NotImplementedException();
        }

        public override string FindAuthor(HtmlDocument doc)
        {
            throw new NotImplementedException();
        }

        public override string FindArtist(HtmlDocument doc)
        {
            throw new NotImplementedException();
        }

        public override string FindCover(HtmlDocument doc)
        {
            throw new NotImplementedException();
        }

        public override List<Chapter> FindChapters(HtmlDocument doc)
        {
            throw new NotImplementedException();
        }

        public override List<Page> GetPages(HtmlDocument doc)
        {
            throw new NotImplementedException();
        }

        public override Page GetPage(Link imageLink, HtmlDocument doc)
        {
            var img = doc.DocumentNode.SelectSingleNode(imageLink.XPath);

            if (img == null)
                throw new ApplicationException(String.Format("Unable to find image from this XPath: {0}", imageLink.XPath));

            var result = new Page();
            result.ImageURL = img.GetAttributeValue("src", "");
            result.Title = String.IsNullOrEmpty(img.GetAttributeValue("title", "")) ? img.GetAttributeValue("alt", "") : img.GetAttributeValue("title", "");

            return result;
        }

        public override void FillIndex(Series series, Page start, Link direction)
        {
            throw new NotImplementedException();
        }
    }
}
