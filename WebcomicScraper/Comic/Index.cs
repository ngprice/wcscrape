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
        private Series ParentSeries;

        public Index(Series parent)
        {
            this.ParentSeries = parent;
        }

        public bool Analyze()
        {
            return true;
        }
    }
}
