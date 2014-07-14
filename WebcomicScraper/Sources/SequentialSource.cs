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

        public override Page GetPage(Link imageLink, string Url)
        {
            using (WebClient webClient = new WebClient())
            {
                var pageHtml = webClient.DownloadString(Url);
                var doc = new HtmlDocument();
                doc.LoadHtml(pageHtml);

                var img = doc.DocumentNode.SelectSingleNode(imageLink.XPath);

                var result = new Page();
                result.ImageURL = img.GetAttributeValue("src", "");
                result.Title = String.IsNullOrEmpty(img.GetAttributeValue("title", "")) ? img.GetAttributeValue("alt", "") : img.GetAttributeValue("title", "");

                return result;
            }
        }
    }
}
