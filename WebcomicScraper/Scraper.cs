using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Drawing;
using System.Threading;
using System.Text.RegularExpressions;
using System.ComponentModel;
using WebcomicScraper.Comic;
using WebcomicScraper.Sources;
using HtmlAgilityPack;
using Ionic.Zip;

namespace WebcomicScraper
{
    /*
    * TODO LIST:
    * --cancel toggle on download button
    *      L--> analyze disabled during this too
    * --Resource list for saving URLs
    *      L--> save learned domains/series
    *      L--> XML config for the learned xpath links, etc
    * --Save series metadata in resources (pages downloaded, file locations, table of contents, last extraction, etc)
    *      L--> serialize Series objects as XML, save to file? http://msdn.microsoft.com/en-us/library/ms172873.aspx
    * 
    * PIE IN THE SKY:
    * --RSS feeds for comic updates
    * --Serialize extract methods (FileOperation delegates, etc) as XML for incremental updates of native sources
    *      L--> FTP for distribution? Mort's NAS?
    *      L--> other people's XML config files
    * --Multi-comic layout option when creating .CBR's... would need to create new image files
    * --Flash support (e.g. platinum grit)
    * */

    public static class Scraper
    {
        private static readonly Dictionary<string, ISource> _dicNativeSourceHosts
            = new Dictionary<string, ISource>
            {
                { "www.mangahere.co", new MangaHere()}
            };

        public static Series LoadSeries(Uri URL, HtmlDocument doc)
        {
            return new Series(URL, doc);
        }

        public static bool AnalyzeSeries(Series series)
        {
            return (ParseSeries(series));
        }

        private static bool ParseSeries(Series series)
        {
            if (_dicNativeSourceHosts.ContainsKey(series.SeedURL.Host))
            {
                var source = _dicNativeSourceHosts[series.SeedURL.Host];
                series.Source = source;

                series.Title = source.FindTitle(series.Document);
                series.Summary = source.FindDescription(series.Document);
                series.Author = source.FindAuthor(series.Document);
                series.Artist = source.FindArtist(series.Document);
                series.CoverImageURL = source.FindCover(series.Document);

                return (!String.IsNullOrEmpty(series.Title) && ParseIndex(series)); //return false if we didn't find at least a title
            }
            else return true; //new series
        }

        private static bool ParseIndex(Series series)
        {
            series.Index.Chapters = series.Source.FindChapters(series.Document);
            return series.Index.Chapters.Count() > 0; //return false if we didn't find any chapters
        }

        public static bool DownloadChapter(Chapter chapter, Series series, string saveDir, int? threads)
        {
            if (!threads.HasValue)
                threads = Math.Min(Environment.ProcessorCount, 64);

            var seriesPath = Path.Combine(saveDir, CleanPath(series.Title));

            var doc = new HtmlDocument();
            doc.LoadHtml(GetHTML(chapter.SourceURL));

            chapter.Pages = series.Source.GetPages(doc);

            var chapterPath = Path.Combine(seriesPath, CleanPath(String.Join(" ", series.Title, chapter.Num.ToString("0000"), "-", chapter.Description).Trim()));
            if (chapterPath.Length > 260)
                throw new ApplicationException(String.Format("Save path cannot be longer than 260 characters: {0}", chapterPath));

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
                            if (tries++ > maxTries)
                                throw;
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
                    var targetPath = Path.Combine(seriesPath, CleanPath(String.Join(" ", series.Title, chapter.Num.ToString("0000"), "-", chapter.Description)).Trim() + ".cbz");
                    if (File.Exists(targetPath))
                        File.Delete(targetPath); //overwrite

                    using (var zip = new ZipFile())
                    {
                        zip.AddDirectory(chapterPath);
                        zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                        zip.Save(targetPath); 
                    }
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

        private static string CleanPath(string path) //it is ridiculous that Path.Combine does not do this
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(String.Format("[{0}]", Regex.Escape(regexSearch)));

            return r.Replace(path, "");
        }

        public static bool KnowsSource(string sourceDomain)
        {
            return _dicNativeSourceHosts.ContainsKey(sourceDomain);
        }
    }
}
