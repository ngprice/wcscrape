using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using HtmlAgilityPack;

namespace WebcomicScraper.Comic
{
    public class Page
    {
        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public HtmlDocument Document { get; set; }

        [Browsable(false)]
        public int Num { get; set; }

        public string Title { get; set; }
        public string PageURL { get; set; }
        public string ImageURL { get; set; }
        public bool Downloaded { get; set; }

        public Page() { }

        public override string ToString()
        {
            return Title;
        }
    }
}
