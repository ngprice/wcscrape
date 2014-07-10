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
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DatePublished { get; set; }
        public string SourceURL { get; set; }
        public List<Page> Pages { get; set; }
        public int Num { get; set; }
        public bool Downloaded { get; set; }

        public Chapter() { }

        public override string ToString()
        {
            return String.Format("Chapter: {0}, {1}", Title, Description);
        }
    }
}
