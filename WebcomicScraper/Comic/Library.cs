using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebcomicScraper.Comic
{
    public class Library
    {
        public List<Series> lstSeries { get; private set; }

        public Library()
        {
            lstSeries = new List<Series>();
        }

        public void AddSeries(Series series)
        {
            lstSeries.Add(series);
            lstSeries = lstSeries.OrderBy(s => s.Title).ToList();
        }

        public void RemoveSeries(Series series)
        {
            lstSeries.Remove(series);
        }
    }
}
