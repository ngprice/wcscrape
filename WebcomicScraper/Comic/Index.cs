using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Threading;

namespace WebcomicScraper.Comic
{
    public class Index
    {
        private Series ParentSeries;
        private bool AnalysisSuccess = false;

        public HtmlAgilityPack.HtmlDocument indexHtmlDocument { get; set; }

        public Index(Series parent, System.Windows.Forms.HtmlDocument doc)
        {
            this.ParentSeries = parent;
            indexHtmlDocument = new HtmlAgilityPack.HtmlDocument();
            indexHtmlDocument.LoadHtml(doc.Body.InnerHtml);
        }

        public void Analyze()
        {
            AnalysisSuccess = ParseSeries(indexHtmlDocument);
        }

        private bool ParseSeries(HtmlAgilityPack.HtmlDocument doc)
        {
            bool result = false;

            ParentSeries.Title = Find(FindTitle, doc);
            ParentSeries.Description = Find(FindDescription, doc);
            ParentSeries.Author = Find(FindAuthor, doc);
            ParentSeries.Artist = Find(FindArtist, doc);

            result = true;
            return result;
        }

        private delegate string FindOperation(HtmlAgilityPack.HtmlDocument doc);

        private string Find(FindOperation op, HtmlAgilityPack.HtmlDocument doc)
        {
            return op(doc);
        }

        private string FindTitle(HtmlAgilityPack.HtmlDocument doc)
        {
            var result = String.Empty;

            var titleElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article/div[1]/div/h1[@class='title']");
            if (titleElement != null)
                result = titleElement.InnerText;

            return result;
        }

        private string FindDescription(HtmlAgilityPack.HtmlDocument doc)
        {
            var result = String.Empty;

            var descriptionElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article/div[1]/div[@class='manga_detail']/div[1]/ul//p[@id='show']");
            result = descriptionElement.InnerText;

            return result;
        }

        private string FindAuthor(HtmlAgilityPack.HtmlDocument doc)
        {
            var result = String.Empty;

            var authorElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article/div[1]/div[@class='manga_detail']/div[1]/ul//label[text()='Author(s):']/../a[1]");
            result = authorElement.InnerText;

            return result;
        }

        private string FindArtist(HtmlAgilityPack.HtmlDocument doc)
        {
            var result = String.Empty;

            var authorElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article/div[1]/div[@class='manga_detail']/div[1]/ul//label[text()='Artist(s):']/../a[1]");
            result = authorElement.InnerText;

            return result;
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
