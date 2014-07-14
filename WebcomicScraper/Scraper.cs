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
using System.Xml.Serialization;
using WebcomicScraper.Comic;
using WebcomicScraper.Sources;
using HtmlAgilityPack;
using Ionic.Zip;

namespace WebcomicScraper
{
    /*
    * TODO LIST:
    * --Index fill for next/prev style comics
    * --Option to trim page info from chapters that have been downloaded to reduce size of wclib.xml
    *   L--> store series XML in series folder
    * --Timers on status updates (e.g. Download successful! (00:01:30 s))
    * --Edit current series info (automatically load sample URL)
    * --Cache loaded cover images
    * 
    * PIE IN THE SKY:
    * --RSS feeds for comic updates
    * --Serialize extract methods (FileOperation delegates, etc) as XML for incremental updates of native sources
    *      L--> FTP for distribution? Mort's NAS?
    *      L--> other people's XML config files
    * --Multi-comic layout option when creating .CBR's... would need to create new image files
    * --Flash support (e.g. platinum grit)
    * */

    public delegate string FindStringDelegate(HtmlDocument doc);
    public delegate List<Chapter> FindChapterListDelegate(HtmlDocument doc);
    public delegate List<Page> FindPageListDelegate(HtmlDocument doc);
    public delegate Page FindPageDelegate(Link link, string url);

    public static class Scraper
    {
        private static readonly Dictionary<string, Source> _dicNativeSourceHosts
            = new Dictionary<string, Source>
            {
                { "", new SequentialSource()},
                { "www.mangahere.co", new MangaHere()}
            };

        public static Series LoadSeries(string URL, HtmlDocument doc)
        {
            return new Series(URL, doc);
        }

        public static bool AnalyzeSeries(Series series)
        {
            return (ParseSeries(series));
        }

        public static string FindString(FindStringDelegate fsd, HtmlDocument doc)
        {
            return fsd(doc);
        }

        public static List<Chapter> FindChapterList(FindChapterListDelegate fcld, HtmlDocument doc)
        {
            return fcld(doc);
        }

        public static List<Page> FindPageList(FindPageListDelegate fpld, HtmlDocument doc)
        {
            return fpld(doc);
        }

        public static Page FindPage(FindPageDelegate fpd, Link link, string url)
        {
            return fpd(link, url);
        }

        private static bool ParseSeries(Series series)
        {
            var hostUri = new Uri(series.SeedURL);
            if (KnowsSource(hostUri.Host))
            {
                var source = _dicNativeSourceHosts[hostUri.Host];
                series.Source = source;

                series.Title = FindString(source.FindTitle, series.Document);
                series.Summary = FindString(source.FindDescription, series.Document);
                series.Author = FindString(source.FindAuthor, series.Document);
                series.Artist = FindString(source.FindArtist, series.Document);
                series.CoverImageURL = FindString(source.FindCover, series.Document);

                return (!String.IsNullOrEmpty(series.Title) && ParseIndex(series)); //return false if we didn't find at least a title
            }
            else
            {
                series.Source = new SequentialSource();
                return true;
            }
        }

        private static bool ParseIndex(Series series)
        {
            if (!(series.Source is SequentialSource))
            {
                series.Index.Chapters = FindChapterList(series.Source.FindChapters, series.Document);
                return series.Index.Chapters != null;
            }
            return false; //generic sources don't have a parsable index
        }

        public static bool DownloadChapter(Chapter chapter, Series series, string saveDir, int? threads, bool convert, CancellationTokenSource cs)
        {
            if (!threads.HasValue)
                threads = Math.Min(Environment.ProcessorCount, 64);

            var seriesPath = Path.Combine(saveDir, CleanPath(series.Title));

            var doc = new HtmlDocument();
            doc.LoadHtml(GetHTML(chapter.SourceURL));

            chapter.Pages = FindPageList(series.Source.GetPages, doc);

            var chapterPath = Path.Combine(seriesPath, CleanPath(String.Join(" ", series.Title, chapter.Num.ToString("0000"), "-", chapter.Description).Trim()));
            if (chapterPath.Length > 260)
                throw new ApplicationException(String.Format("Save path cannot be longer than 260 characters: {0}", chapterPath));

            if (!Directory.Exists(chapterPath))
                Directory.CreateDirectory(chapterPath);

            if (chapter.Pages.Count > 0)
            {
                try
                {
                    chapter.Pages.AsParallel().WithDegreeOfParallelism(threads.Value).WithCancellation(cs.Token).ForAll(page =>
                    {
                        int tries = 0;
                        int maxTries = 5;
                        bool successful = false;
                        while (!successful)
                        {
                            try
                            {
                                cs.Token.ThrowIfCancellationRequested();

                                var pagePath = Path.Combine(chapterPath, page.Num.ToString("0000") + ".jpeg");
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
                }
                catch (OperationCanceledException) { throw; }
                catch (AggregateException) { }

                if (!IsDirectoryEmpty(chapterPath) && convert)
                {
                    var targetPath = Path.Combine(seriesPath, CleanPath(String.Join(" ", series.Title, chapter.Num.ToString("0000"), "-", chapter.Description)).Trim() + ".cbz");
                    if (File.Exists(targetPath))
                        File.Delete(targetPath); //overwrite

                    using (var zip = new ZipFile())
                    {
                        zip.AddDirectory(chapterPath);
                        zip.Save(targetPath); 
                    }
                    Directory.Delete(chapterPath, true);
                }
                chapter.Downloaded = true;
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

        public static string SerializeLibrary(Library lib, string path)
        {
            var serializer = new XmlSerializer(typeof(Library), Type.GetTypeArray(_dicNativeSourceHosts.Values.ToArray()));
            var filepath = path + @"\wclib.xml";
            var file = new StreamWriter(filepath);
            serializer.Serialize(file, lib);
            file.Close();

            return filepath;
        }

        public static Library DeserializeLibrary(string path)
        {
            var deserializer = new XmlSerializer(typeof(Library), Type.GetTypeArray(_dicNativeSourceHosts.Values.ToArray()));
            var filestream = new FileStream(path, FileMode.Open);
            var result = (Library)deserializer.Deserialize(filestream);
            filestream.Close();

            return result;
        }
    }
}
