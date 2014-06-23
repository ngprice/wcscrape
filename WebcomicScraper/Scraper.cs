using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Drawing;
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
            series.Index.Chapters = FindChapters(series.Document);
            return series.Index.Chapters.Count() > 0; //return false if we didn't find any chapters
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
                {
                    DateTime.TryParse(dateNode.InnerText, out date);
                    chapter.DatePublished = date;
                }

                result.Add(chapter);
            }

            return result;
        }

        public static bool DownloadChapter(Chapter chapter, string seriesPath)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(GetHTML(chapter.SourceURL));

            //table of contents
            var contentsNodes = doc.DocumentNode.SelectNodes("html/body/section[1]/div[@class='go_page clearfix']/span[@class='right']/select[1]/option");
            chapter.Pages = Scraper.GetPages(contentsNodes);

            var chapterPath = Path.Combine(seriesPath, chapter.Title.Trim());
            if (!Directory.Exists(chapterPath))
                Directory.CreateDirectory(chapterPath);

            if (chapter.Pages.Count > 0)
            {
                foreach (Page page in chapter.Pages)
                {
                    var pagePath = Path.Combine(chapterPath, page.PageNum.ToString() + ".jpeg");
                    page.Image.Save(pagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                //chapter.Pages.AsParallel().ForAll(page =>
                //{
                //    var pagePath = Path.Combine(chapterPath, page.PageNum.ToString());
                //    page.Image.Save(pagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                //});
            }

            return true;
        }

        private static List<Page> GetPages(HtmlNodeCollection nodes)
        {
            var result = new List<Page>();

            nodes.AsParallel().ForAll(node =>
            {
                using (WebClient webClient = new WebClient()) {
                    try
                    {
                        string pageHtml = webClient.DownloadString(node.GetAttributeValue("value", ""));

                        var doc = new HtmlDocument();
                        doc.LoadHtml(pageHtml);

                        var imgElement = doc.DocumentNode.SelectSingleNode("html/body/section[@class='read_img'][@id='viewer']/a[1]/img");
                        if (imgElement != null)
                        {
                            var page = new Page();
                            page.PageNum = int.Parse(node.NextSibling.InnerText);
                            page.ImageURL = imgElement.GetAttributeValue("src", "");
                            page.Image = GetImage(page.ImageURL);

                            result.Add(page);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error getting pages: " + ex.Message);
                    }
                };
            });

            return result.OrderBy(p => p.PageNum).ToList();
        }

        //uhh is this thread safe?
        private static string GetHTML(string fromURL)
        {
            string data = String.Empty;
            var request = (HttpWebRequest)WebRequest.Create(fromURL);
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null || response.CharacterSet == "utf8")
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
            else throw new ApplicationException(String.Format("Bad response from index: {0}, {1}", System.Enum.GetName(typeof(HttpStatusCode), response.StatusCode), response.StatusDescription));

            return data;
        }

        private static Image GetImage(string url)
        {
            Image result = null;

            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();

            Stream stream = httpWebReponse.GetResponseStream();
            result = Image.FromStream(stream);

            return result;
        }

        /* Note: none of this shit worked, there are stupid combined pages masquerading as single pages. so we do things the hard way
         * 
         * 
        //there's a pattern to these image names. in each thread try to guess the image name, then use xpath if that fails
        //http://z.mhcdn.net/store/manga/11912/001.0/compressed/lopm_001_001.jpg?v=11350898681
        //http://z.mhcdn.net/store/manga/11912/001.0/compressed/lopm_001_002.jpg?v=11350898681
        //http://z.mhcdn.net/store/manga/11912/001.0/compressed/lopm_001_003-004.jpg?v=11350898681 //combined pages
        //http://z.mhcdn.net/store/manga/11912/001.0/compressed/lopm_001_019-020.jpg?v=11350898681 //page 17 is actually "numbered" 19-20. annoying.

        //start with two pages, compare the image names. 
        var imageElement = doc.DocumentNode.SelectSingleNode("html/body/section[@class='read_img'][@id='viewer']/a[1]/img");
        if (imageElement != null)
        {
            Page firstPage = new Page();
            firstPage.ImageURL = imageElement.GetAttributeValue("src", "");

            var secondPageLink = doc.DocumentNode.SelectSingleNode("html/body/section[1]/div[@class='go_page clearfix']/span[@class='right']/a[@class='next_page']");
            if (secondPageLink != null && !String.IsNullOrEmpty(secondPageLink.GetAttributeValue("href","")))
            {
                var doc2 = new HtmlDocument();
                doc2.LoadHtml(GetHTML(secondPageLink.GetAttributeValue("href","")));

                Page secondPage = new Page();
                var imageElement2 = doc2.DocumentNode.SelectSingleNode("html/body/section[@class='read_img'][@id='viewer']/a[1]/img");
                if (imageElement2 != null)
                {
                    secondPage.ImageURL = imageElement2.GetAttributeValue("src", "");

                    //try to guess the pattern & read images directly without having to fetch whole html document for every page
                    List<Page> predictedPages = Scraper.PredictPages(contentsNodes.Count(), firstPage.ImageURL, secondPage.ImageURL);

                    if (predictedPages.Count > 0)
                    {
                        return DownloadPages(predictedPages);
                    }
                }
            }
        }
            

        return true;
    }

    private static List<Page> PredictPages(int iterations, params string[] urls)
    {
        var result = new List<Page>();

        for (int i = 0; i < urls.Length; i++)
        {
            var first = urls[i++];
            var second = urls[i--];
            var diff = DiffIndex(first, second);

            if (diff > 0) //potential match
            {
                var firstChar = first[diff];
                var secondChar = second[diff];
                int firstNum, secondNum;

                if (int.TryParse(firstChar.ToString(), out firstNum) && int.TryParse(secondChar.ToString(), out secondNum))
                {
                    if (secondNum - firstNum == 1) //iterating by 1
                    {
                        for (int j = 1; j <= urls.Count(); j++) //create pages for the URLs we were given
                        {
                            var page = new Page();
                            page.PageNum = j;
                            page.ImageURL = urls[j - 1];

                            result.Add(page);
                        }

                        int pageNumLength = 1;
                        for (int k = diff - 1; k >= 0; k--) //work backwards through the string
                        {
                            int output;
                            if (int.TryParse(first[k].ToString(), out output))
                                pageNumLength++;
                            else break;
                        }

                        var formatString = new String('0', pageNumLength);
                        var predictionTemplate = first.Substring(0, diff - pageNumLength + 1) + "{0}" + first.Substring(diff + 1, first.Length - diff - 1);

                        for (int l = result.Count + 1; l <= iterations; l++) //prediction loop
                        {
                            var predictedPage = new Page();
                            predictedPage.PageNum = l;
                            predictedPage.ImageURL = String.Format(predictionTemplate, l.ToString(formatString));

                            result.Add(predictedPage);
                        }

                        return result;
                    }
                }
            }
        }

        return result;
    }

    private static int DiffIndex(string first, string second)
    {
        if (first == second)
            return -1;

        for (int i = 0; i < first.Length; i++)
            if (first[i] != second[i])
                return i;

        return 0;
    }

    private static bool DownloadPages(List<Page> pages)
    {
        return true;
    }
    */
    }
}
