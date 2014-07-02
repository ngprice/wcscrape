using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebcomicScraper.Comic
{
    public class Library
    {
        public List<Series> lstSeries { get; set; }

        public Library()
        {
            lstSeries = new List<Series>();
        }
        public void AddSeries(Series series)
        {

        }

        public void RemoveSeries(Series series)
        {

        }
    }
}
