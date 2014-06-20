using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace WebcomicScraper.Comic
{
    public class Series
    {
        private HtmlDocument SeriesHtmlDocument;

        public string Title { get; set; }
        public string Summary { get; set; }
        public string Author { get; set; }
        public string Artist { get; set; }
        public string CoverImageURL { get; set; }

        public Index Index { get; set; }

        public Series(System.Windows.Forms.HtmlDocument doc)
        {
            SeriesHtmlDocument = new HtmlDocument();
            SeriesHtmlDocument.LoadHtml(doc.Body.InnerHtml);
            Index = new Index(this);
        }

        public bool Analyze()
        {
            bool result = true;

            result = (ParseSeries(SeriesHtmlDocument) && Index.Analyze());

            return result;
        }

        private bool ParseSeries(HtmlDocument doc)
        {
            bool result = false;

            Title = Find(FindTitle, doc);
            Summary = Find(FindDescription, doc);
            Author = Find(FindAuthor, doc);
            Artist = Find(FindArtist, doc);
            CoverImageURL = Find(FindCover, doc);

            result = true;
            return result;
        }

        private delegate string FindOperation(HtmlDocument doc);

        private string Find(FindOperation op, HtmlDocument doc)
        {
            return op(doc);
        }

        private string FindTitle(HtmlAgilityPack.HtmlDocument doc)
        {
            var result = String.Empty;

            var titleElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div/h1[@class='title']");
            if (titleElement != null)
                result = titleElement.InnerText;

            return result;
        }

        private string FindDescription(HtmlDocument doc)
        {
            var result = String.Empty;

            var descriptionElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[1]/ul//p[@id='show']");
            if (descriptionElement != null)
            {
                descriptionElement.RemoveChild(descriptionElement.LastChild);
                result = System.Net.WebUtility.HtmlDecode(descriptionElement.InnerText);
            }
            return result;
        }

        private string FindAuthor(HtmlDocument doc)
        {
            var result = String.Empty;

            var authorElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[1]/ul//label[text()='Author(s):']/../a[1]");
            if (authorElement != null)
                result = authorElement.InnerText;

            return result;
        }

        private string FindArtist(HtmlDocument doc)
        {
            var result = String.Empty;

            var artistElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[1]/ul//label[text()='Artist(s):']/../a[1]");
            if (artistElement != null)
                result = artistElement.InnerText;

            return result;
        }

        private string FindCover(HtmlDocument doc)
        {
            var result = String.Empty;

            var coverElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[@class='manga_detail_top clearfix']/img[@class='img' and contains(@src,'cover')]");
            if (coverElement != null)
                result = coverElement.GetAttributeValue("src", "");

            return result;
        }

        public override string ToString()
        {
            return String.Format("Series: {0} by {1}", Title, Author);
        }

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
