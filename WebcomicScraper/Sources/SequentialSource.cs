using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using WebcomicScraper.Comic;
using HtmlAgilityPack;

namespace WebcomicScraper.Sources
{
    public class SequentialSource : Source
    {
        public override Page GetPage(Link imageLink, string fromUrl, HtmlDocument doc)
        {
            var img = doc.DocumentNode.SelectSingleNode(imageLink.XPath);

            if (img == null)
                throw new ApplicationException(String.Format("Unable to find image from this XPath: {0}", imageLink.XPath));

            var result = new Page();
            result.ImageURL = img.GetAttributeValue("src", "");
            result.Title = String.IsNullOrEmpty(img.GetAttributeValue("title", "")) ? img.GetAttributeValue("alt", "") : img.GetAttributeValue("title", "");
            result.Document = doc;
            result.PageURL = fromUrl;

            return result;
        }

        public override Page GetPageFromLink(Page start, Link direction)
        {
            var pageLink = start.Document.DocumentNode.SelectSingleNode(direction.XPath);

            if (pageLink == null || String.IsNullOrEmpty(pageLink.GetAttributeValue("href", "")))
                return null;

            var result = new Page();
            result.PageURL = pageLink.GetAttributeValue("href", "");

            return result;
        }

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
    }
}
