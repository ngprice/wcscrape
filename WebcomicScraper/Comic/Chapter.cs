using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace WebcomicScraper.Comic
{
    public class Chapter
    {
        private bool _forceDownloaded;
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DatePublished { get; set; }
        public string SourceURL { get; set; }
        public List<Page> Pages { get; set; }
        public int Num { get; set; }

        public bool Downloaded
        {
            get
            {
                if (_forceDownloaded) 
                    return true;
                else if (Pages != null)
                    return Pages.Count > 0 && !Pages.Any(p => p.Downloaded == false);
                else return false;
            }
        }

        public Chapter()
        {
            _forceDownloaded = false;
        }

        public Chapter(bool download)
        {
            _forceDownloaded = download;
        }

        public override string ToString()
        {
            return String.Format("Chapter: {0}, {1}", Title, Description);
        }
    }
}
