using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebcomicScraper.Comic;
using HtmlAgilityPack;

namespace WebcomicScraper
{
    public partial class FillIndex : Form
    {
        private Series _series;
        private Index _index;
        public Index LoadedIndex 
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                DisplayIndex(_index);
            }
        }
        public FillIndex(Series series)
        {
            InitializeComponent();
            InitializeBackgroundWorkers();
            _series = series;
            LoadedIndex = series.Index;
        }

        public void InitializeBackgroundWorkers()
        {
            indexBackgroundWorker.DoWork += new DoWorkEventHandler(index_DoWork);
            indexBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(index_ReportProgress);
            indexBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(index_Completed);
        }

        public void DisplayIndex(Index index)
        {
            var source = new BindingSource();
            if (index.Chapters != null && index.Chapters.Any())
            {
                source.DataSource = index.Chapters;
                dgvIndex.DataSource = source;
                foreach (DataGridViewRow row in dgvIndex.Rows)
                {
                    var chapter = (Chapter)row.DataBoundItem;
                    row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
                    if (chapter.Downloaded)
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }
            else if (index.Pages != null && index.Pages.Any())
            {
                source.DataSource = index.Pages;
                dgvIndex.DataSource = source;

                foreach (DataGridViewRow row in dgvIndex.Rows)
                {
                    var page = (Page)row.DataBoundItem;
                    row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
                    if (page.Downloaded)
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }
        }

        private void btnFillForward_Click(object sender, EventArgs e)
        {
            indexBackgroundWorker.RunWorkerAsync(FillDirection.Forward);
        }

        private void btnFillBackward_Click(object sender, EventArgs e)
        {
            indexBackgroundWorker.RunWorkerAsync(FillDirection.Backward);
        }

        private void index_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var dir = (FillDirection)e.Argument;

            var page = Scraper.FillSeriesIndex(_series, dir);
            while (!(worker.CancellationPending || e.Cancel) && page != null)
            {
                worker.ReportProgress(0, String.Format("Added page {0}\n", page.Title));
                page = Scraper.FillSeriesIndex(_series, dir);
            }
        }

        private void index_ReportProgress(object sender, ProgressChangedEventArgs e)
        {
            Status(e.UserState as String);
        }

        private void index_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(String.Format("Error in background thread: {0}", e.Error.Message));
            }
            else if (e.Cancelled)
            {
                Status("Download canceled.");
            }
            else if (!(bool)e.Result)
            {
                Status("Download failed.");
            }
            else
            {
                Status("Download successful!");
            }
        }

        private void Status(string status)
        {
            txtStatus.AppendText(status + Environment.NewLine);
        }
    }
}
