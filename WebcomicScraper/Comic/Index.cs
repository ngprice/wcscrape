using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Threading;

namespace WebcomicScraper.Comic
{
    public class Index
    {
        public List<Chapter> Chapters { get; set; }
        public List<Page> Pages { get; set; }

        public Index() { }
    }
}
