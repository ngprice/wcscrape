using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WebcomicScraper.Comic
{
    class Index
    {
        private WebBrowser browser;
        private Series series;

        public string indexURL { get; set; }
        //private string indexHTML { get; set; }

        public Index(string sourceURL)
        {
            this.indexURL = sourceURL;
        }

        public bool Analyze()
        {
            bool result = false;

            //indexHTML = GetHTML(indexURL);
            browser = new WebBrowser();
            browser.Navigate(indexURL);

            series = ParseSeries(browser.Document);

            return result;
        }

        private Series ParseSeries(HtmlDocument doc)
        {
            var result = new Series();
            result.Title = Find(FindTitle, doc);
            result.Description = Find(FindDescription, doc);

            return result;
        }

        private delegate string FindOperation(HtmlDocument doc);

        private string Find(FindOperation op, HtmlDocument doc)
        {
            return op(doc);
        }

        private string FindTitle(HtmlDocument doc)
        {
            var result = String.Empty;

            IQueryable<HtmlElement> h1HEC = doc.GetElementsByTagName("h1").AsQueryable().Cast<HtmlElement>();
            var titleElement = h1HEC.First(e => e.GetAttribute("class") == "title");

            return result;
        }

        private string FindDescription(HtmlDocument doc)
        {
            var result = String.Empty;

            IQueryable<HtmlElement> h1HEC = doc.GetElementsByTagName("h1").AsQueryable().Cast<HtmlElement>();
            var titleElement = h1HEC.First(e => e.GetAttribute("class") == "title");

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
