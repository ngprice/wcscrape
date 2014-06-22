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
        private Series _ParentSeries;
        public List<Chapter> Chapters { get; set; }

        public Index(Series parent)
        {
            this._ParentSeries = parent;
        }
    }
}
