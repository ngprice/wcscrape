using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebcomicScraper.Comic;
using HtmlAgilityPack;

namespace WebcomicScraper
{
    public static class Scraper
    {
        public static Series LoadSeries(System.Windows.Forms.HtmlDocument doc)
        {
            var agilityDoc = new HtmlDocument();
            agilityDoc.LoadHtml(doc.Body.InnerHtml);
            return new Series(agilityDoc);
        }

        public static bool AnalyzeSeries(Series series)
        {
            return (ParseSeries(series) && ParseIndex(series));
        }

        private static bool ParseSeries(Series series)
        {
            series.Title = FindString(FindTitle, series.Document);
            series.Summary = FindString(FindDescription, series.Document);
            series.Author = FindString(FindAuthor, series.Document);
            series.Artist = FindString(FindArtist, series.Document);
            series.CoverImageURL = FindString(FindCover, series.Document);

            return !String.IsNullOrEmpty(series.Title); //return false if we didn't find at least a title
        }

        private static bool ParseIndex(Series series)
        {
            var index = new Index(series);
            index.Chapters = FindChapters(series.Document);

            return index.Chapters.Count() > 0; //return false if we didn't find any chapters
        }

        private delegate object FindOperation(HtmlDocument doc);

        private static string FindString(FindOperation op, HtmlDocument doc)
        {
            return (string)op(doc);
        }

        private static string FindTitle(HtmlDocument doc)
        {
            var result = String.Empty;

            var titleElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div/h1[@class='title']");
            if (titleElement != null)
                result = titleElement.InnerText.Trim();

            return result;
        }

        private static string FindDescription(HtmlDocument doc)
        {
            var result = String.Empty;

            var descriptionElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[1]/ul//p[@id='show']");
            if (descriptionElement != null)
            {
                descriptionElement.RemoveChild(descriptionElement.LastChild);
                result = System.Net.WebUtility.HtmlDecode(descriptionElement.InnerText.Trim());
            }
            return result;
        }

        private static string FindAuthor(HtmlDocument doc)
        {
            var result = String.Empty;

            var authorElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[1]/ul//label[text()='Author(s):']/../a[1]");
            if (authorElement != null)
                result = authorElement.InnerText.Trim();

            return result;
        }

        private static string FindArtist(HtmlDocument doc)
        {
            var result = String.Empty;

            var artistElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[1]/ul//label[text()='Artist(s):']/../a[1]");
            if (artistElement != null)
                result = artistElement.InnerText.Trim();

            return result;
        }

        private static string FindCover(HtmlDocument doc)
        {
            var result = String.Empty;

            var coverElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[@class='manga_detail_top clearfix']/img[@class='img' and contains(@src,'cover')]");
            if (coverElement != null)
                result = coverElement.GetAttributeValue("src", "");

            return result;
        }

        private static List<Chapter> FindChapters(HtmlDocument doc)
        {
            var result = new List<Chapter>();

            foreach (var node in doc.DocumentNode.SelectNodes("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[@class='detail_list']/ul[1]/li"))
            {
                var chapter = new Chapter();
                var date = new DateTime();

                var anchorNode = node.SelectSingleNode("span/a[@class][@href]");
                if (anchorNode != null)
                {
                    chapter.Title = anchorNode.InnerText;
                    chapter.SourceURL = anchorNode.GetAttributeValue("href", "");
                }

                var newNode = node.SelectSingleNode("i[@class='new']");
                var titleNode = node.SelectSingleNode("span[@class='left']");
                if (newNode != null)
                    chapter.Description = newNode.InnerText;
                else if (titleNode != null)
                    chapter.Description = titleNode.LastChild.InnerText;

                var dateNode = node.SelectSingleNode("span[@class='right']");
                if (dateNode != null)
                    DateTime.TryParse(dateNode.InnerText, out date);

                result.Add(chapter);
            }

            return result;
        }

        ////uhh is this thread safe?
        //private string GetHTML(string fromURL)
        //{
        //    string data = String.Empty;
        //    var request = (HttpWebRequest)WebRequest.Create(fromURL);
        //    var response = (HttpWebResponse)request.GetResponse();

        //    if (response.StatusCode == HttpStatusCode.OK)
        //    {
        //        Stream receiveStream = response.GetResponseStream();
        //        StreamReader readStream = null;

        //        if (response.CharacterSet == null || response.CharacterSet == "utf8")
        //            readStream = new StreamReader(receiveStream);
        //        else
        //            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

        //        data = readStream.ReadToEnd();
        //        response.Close();
        //        readStream.Close();
        //    }
        //    else throw new ApplicationException(String.Format("Bad response from index: {0}, {1}", System.Enum.GetName(typeof(HttpStatusCode), response.StatusCode), response.StatusDescription));

        //    return data;
        //}
    }
}
