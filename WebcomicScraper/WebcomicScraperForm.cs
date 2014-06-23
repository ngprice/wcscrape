using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WebcomicScraper.Comic;
using System.Threading;

namespace WebcomicScraper
{
    public partial class WebcomicScraperForm : Form
    {
        private WebBrowser browser;
        public Series LoadedSeries { get; set; }

        public WebcomicScraperForm()
        {
            InitializeComponent();
            InitializeBackgroundWorkers();
        }

        private void InitializeBackgroundWorkers()
        {
            analysisBackgroundWorker.DoWork += new DoWorkEventHandler(analysis_DoWork);
            analysisBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(analysis_Completed);
            analysisBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(analysis_ProgressChanged);

            downloadBackgroundWorker.DoWork += new DoWorkEventHandler(download_DoWork);
            downloadBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(download_Completed);
            downloadBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(download_ProgressChanged);
        }

        /*
         * TODO:
         * --Resource list for saving URLs
         * */

        private void btnScrape_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtIndex.Text))
            {
                Status("There's nothing there, idiot.");
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                browser = new WebBrowser();
                browser.ScriptErrorsSuppressed = true;
                browser.Navigate(txtIndex.Text);

                browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocumentLoaded);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DocumentLoaded(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = (WebBrowser)sender;
            analysisBackgroundWorker.RunWorkerAsync(browser.Document);

            browser.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(DocumentLoaded);
            browser.Stop();
        }

        private void analysis_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadedSeries = Scraper.LoadSeries((HtmlDocument)e.Argument);
            e.Result = Scraper.AnalyzeSeries(LoadedSeries);
        }

        private void analysis_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            
            if (e.Error != null)
            {
                MessageBox.Show(String.Format("Error in background thread: {0}", e.Error.Message));
            }
            else if (e.Cancelled)
            {
                Status("Analysis canceled.");
            }
            else if (!(bool)e.Result)
            {
                Status("Analysis failed.");
            }
            else
            {
                DisplaySeries(LoadedSeries);
                Status("Analysis successful!");
            }
        }

        private void analysis_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Status(String.Format("Analysis {0}% complete.", e.ProgressPercentage));
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            var rows = dgvIndex.SelectedRows;
            if (rows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                Status("Download started.");

                downloadBackgroundWorker.RunWorkerAsync(rows);
            }
            else Status("Must select at least 1 row.");
        }

        private void download_DoWork(object sender, DoWorkEventArgs e)
        {
            var rows = e.Argument as DataGridViewSelectedRowCollection;
            var seriesPath = Path.Combine(txtSaveDir.Text, LoadedSeries.Title);
            foreach (DataGridViewRow row in rows) //TODO: PLINQ here
            {
                e.Result = Scraper.DownloadChapter(row.DataBoundItem as Chapter, seriesPath);
            }
        }

        private void download_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Cursor.Current = Cursors.Default;

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

        private void download_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Status(String.Format("Download {0}% complete.", e.ProgressPercentage));
        }

        private void DisplaySeries(Series series)
        {
            txtTitle.Text = series.Title;
            txtAuthor.Text = series.Author;
            txtArtist.Text = series.Artist;
            txtSummary.Text = series.Summary;

            previewPictureBox.WaitOnLoad = false;
            previewPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            previewPictureBox.LoadAsync(series.CoverImageURL);

            var source = new BindingSource();
            source.DataSource = series.Index.Chapters;
            dgvIndex.DataSource = source;
            dgvIndex.Refresh();
        }

        private void Status(string msg)
        {
            ToolStripStatusLabel1.Text = msg;
            statusStrip1.Refresh();
        }
    }
}
