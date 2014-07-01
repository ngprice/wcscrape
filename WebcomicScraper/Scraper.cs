using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Drawing;
using System.IO.Compression;
using System.Threading;
using System.Text.RegularExpressions;
using WebcomicScraper.Comic;
using WebcomicScraper.Sources;
using HtmlAgilityPack;

namespace WebcomicScraper
{
     /*
     * TODO LIST:
     * --WebBrowser not thread-safe; use webclient requests instead (like in Scraper.cs) and shove the work into the analyzebackgroundworker
     *      L--> cancel toggle on download button
     *          L--> analyze disabled during this too
     * --Classes of delegates for populating index data, scraping pages, etc for each series/domain(PA, mangahere, etc).
     *      L--> Try all prior delegates when running a new comic, log which ones work in resources
     * --Stupid chapternames aren't distinct; get better naming convention for storing chapters (ordinal? index in the list?)
     * --Resource list for saving URLs
     *      L--> save learned domains/series
     *      L--> XML config for the learned xpath links, etc
     * --Learning/teaching section: feed in a comic URL and the link to the next button, it does the rest
     *      L--> perhaps implement chapter table of contents identification too?
     * --scraper types: index (table of contents), browser (next buttons)
     * --Save series metadata in resources (pages downloaded, file locations, table of contents, last extraction, etc)
     *      L--> serialize Series objects as XML, save to file? http://msdn.microsoft.com/en-us/library/ms172873.aspx
     * 
     * PIE IN THE SKY:
     * --RSS feeds for comic updates
     * --Serialize extract methods (FileOperation delegates, etc) as XML, for incremental updates?
     *      L--> FTP for distribution? Mort's NAS?
     *      L--> other people's XML config files
     * --Multi-comic layout option when creating .CBR's... would need to create new image files
     * */

    public delegate object FindOperation(HtmlDocument doc);

    public static class Scraper
    {
        private static readonly Dictionary<string, ISource> _dicNativeSourceHosts
            = new Dictionary<string, ISource>
            {
                { "www.mangahere.co", new MangaHere()}
            };

        public static Series LoadSeries(Uri URL, System.Windows.Forms.HtmlDocument doc)
        {
            var agilityDoc = new HtmlDocument();
            agilityDoc.LoadHtml(doc.Body.InnerHtml);
            return new Series(URL, agilityDoc);
        }

        public static bool AnalyzeSeries(Series series)
        {
            return (ParseSeries(series));
        }

        private static bool ParseSeries(Series series)
        {
            if (_dicNativeSourceHosts.ContainsKey(series.URL.Host))
            {
                var source = _dicNativeSourceHosts[series.URL.Host];

                series.Title = FindString(source.FindTitle, series.Document);
                series.Summary = FindString(source.FindDescription, series.Document);
                series.Author = FindString(source.FindAuthor, series.Document);
                series.Artist = FindString(source.FindArtist, series.Document);
                series.CoverImageURL = FindString(source.FindCover, series.Document);

                return (!String.IsNullOrEmpty(series.Title) && ParseIndex(series)); //return false if we didn't find at least a title
            }
            else return true; //new series
        }

        private static bool ParseIndex(Series series)
        {
            series.Index.Chapters = series.Source.FindChapters(series.Document);
            return series.Index.Chapters.Count() > 0; //return false if we didn't find any chapters
        }

        private static string FindString(FindOperation op, HtmlDocument doc)
        {
            return (string)op(doc);
        }

        public static bool DownloadChapter(Chapter chapter, string seriesPath, int? threads)
        {
            if (!threads.HasValue)
                threads = Math.Min(Environment.ProcessorCount, 64);

            var doc = new HtmlDocument();
            doc.LoadHtml(GetHTML(chapter.SourceURL));

            var chapterPath = Path.Combine(seriesPath, CleanPath(chapter.Title.Trim()));
            if (!Directory.Exists(chapterPath))
                Directory.CreateDirectory(chapterPath);

            if (chapter.Pages.Count > 0)
            {
                chapter.Pages.AsParallel().WithDegreeOfParallelism(threads.Value).ForAll(page =>
                {
                    int tries = 0;
                    int maxTries = 5;
                    bool successful = false;
                    while (!successful)
                    {
                        try
                        {
                            var pagePath = Path.Combine(chapterPath, page.PageNum.ToString("0000") + ".jpeg");
                            if (!File.Exists(pagePath))
                            {
                                using (var img = new Bitmap(GetImage(page.ImageURL)))
                                    img.Save(pagePath, System.Drawing.Imaging.ImageFormat.Jpeg); //Bitmap is workaround for GDI+ error in Image.Save()
                                successful = true;
                            }
                            else successful = true;
                        }
                        catch (System.Net.WebException) //timed out
                        {
                            if (tries > maxTries)
                                throw;
                            tries++;
                            Thread.Sleep(333); //looks like butts
                        }
                        catch
                        {
                            throw;
                        }
                    }
                });

                if (!IsDirectoryEmpty(chapterPath))
                {
                    var targetPath = Path.Combine(seriesPath, CleanPath(chapter.Title.Trim()) + ".cbz");
                    if (File.Exists(targetPath))
                        File.Delete(targetPath); //overwrite

                    var zipStream = new GZipStream(new FileStream(chapterPath, FileMode.Open), CompressionMode.Compress);
                    zipStream.CompressDirectory(chapterPath, targetPath);
                    Directory.Delete(chapterPath, true);
                }
            }

            return true;
        }

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

        private static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static string CleanPath(string path) //it is ridiculous that Path.Combine does not do this
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(String.Format("[{0}]", Regex.Escape(regexSearch)));

            return r.Replace(path, "");
        }
    }
}
