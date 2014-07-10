using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using WebcomicScraper.Comic;
using HtmlAgilityPack;

namespace WebcomicScraper.Sources
{
    public sealed class MangaHere : Source
    {
        public override string FindTitle(HtmlDocument doc)
        {
            var result = String.Empty;

            var titleElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div/h1[@class='title']");
            if (titleElement != null)
                result = titleElement.InnerText.Trim();

            return result;
        }

        public override string FindDescription(HtmlDocument doc)
        {
            var result = String.Empty;

            var descriptionElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[1]/ul//p[@id='show']");
            if (descriptionElement != null)
            {
                descriptionElement.RemoveChild(descriptionElement.LastChild);
                result = WebUtility.HtmlDecode(descriptionElement.InnerText.Trim());
            }
            return result;
        }

        public override string FindAuthor(HtmlDocument doc)
        {
            var result = String.Empty;

            var authorElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[1]/ul//label[text()='Author(s):']/../a[1]");
            if (authorElement != null)
                result = authorElement.InnerText.Trim();

            return result;
        }

        public override string FindArtist(HtmlDocument doc)
        {
            var result = String.Empty;

            var artistElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[1]/ul//label[text()='Artist(s):']/../a[1]");
            if (artistElement != null)
                result = artistElement.InnerText.Trim();

            return result;
        }

        public override string FindCover(HtmlDocument doc)
        {
            var result = String.Empty;

            var coverElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[@class='manga_detail_top clearfix']/img[@class='img' and contains(@src,'cover')]");
            if (coverElement != null)
                result = coverElement.GetAttributeValue("src", "");

            return result;
        }

        public override List<Chapter> FindChapters(HtmlDocument doc)
        {
            var result = new List<Chapter>();
            int ctr = 0;

            foreach (var node in doc.DocumentNode.SelectNodes("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[@class='detail_list']/ul[1]/li").Reverse())
            {
                ctr++;
                var chapter = new Chapter();
                var date = new DateTime();

                var anchorNode = node.SelectSingleNode("span/a[@class][@href]");
                if (anchorNode != null)
                {
                    chapter.Num = ctr;
                    chapter.Title = WebUtility.HtmlDecode(anchorNode.InnerText.Trim());
                    chapter.SourceURL = anchorNode.GetAttributeValue("href", "");
                }

                var newNode = node.SelectSingleNode("i[@class='new']");
                var titleNode = node.SelectSingleNode("span[@class='left']");
                if (newNode != null)
                    chapter.Description = WebUtility.HtmlDecode(newNode.InnerText.Trim());
                else if (titleNode != null)
                    chapter.Description = WebUtility.HtmlDecode(titleNode.LastChild.InnerText.Trim());

                var dateNode = node.SelectSingleNode("span[@class='right']");
                if (dateNode != null)
                {
                    DateTime.TryParse(dateNode.InnerText, out date);
                    chapter.DatePublished = date;
                }

                result.Add(chapter);
            }
            return result;
        }

        public override List<Page> GetPages(HtmlDocument chapterDoc)
        {
            var result = new List<Page>();

            //table of contents
            var contentsNodes = chapterDoc.DocumentNode.SelectNodes("html[1]/body[1]/section[1]/div[@class='go_page clearfix']/span[@class='right']/select[1]/option");

            contentsNodes.AsParallel().AsOrdered().ForAll(node =>
            {
                using (WebClient webClient = new WebClient())
                {
                    try
                    {
                        string pageHtml = webClient.DownloadString(node.GetAttributeValue("value", ""));

                        var pageDoc = new HtmlDocument();
                        pageDoc.LoadHtml(pageHtml);

                        var imgElement = pageDoc.DocumentNode.SelectSingleNode("html[1]/body[1]/section[@class='read_img'][@id='viewer']/a[1]/img");
                        if (imgElement != null)
                        {
                            var page = new Page();
                            page.Num = int.Parse(node.NextSibling.InnerText);
                            page.ImageURL = imgElement.GetAttributeValue("src", "");

                            result.Add(page);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error getting pages: " + ex.Message);
                    }
                };
            });

            return result.OrderBy(p => p.Num).ToList();
        }
    }
}
