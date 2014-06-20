using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
        }

        /*
         * TODO:
         * --Resource list for saving URLs
         * */

        private void btnAnalyze_Click(object sender, EventArgs e)
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

                //var series = new Series(txtIndex.Text);
                
                //threading here for asynch update status bar
                //series.Analyze();

                //DisplaySeries(series);

                //if (!String.IsNullOrEmpty(series.Title))
                //    Status("Analysis successful!");
                //else Status("Analysis failed!");
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
            LoadedSeries = new Series((HtmlDocument)e.Argument);
            e.Result = LoadedSeries.Analyze();
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

        private void DisplaySeries(Series series)
        {
            txtTitle.Text = series.Title;
            txtAuthor.Text = series.Author;
            txtArtist.Text = series.Artist;
            txtSummary.Text = series.Summary;

            previewPictureBox.WaitOnLoad = false;
            previewPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            previewPictureBox.LoadAsync(series.CoverImageURL);
        }

        private void Status(string msg)
        {
            ToolStripStatusLabel1.Text = msg;
            statusStrip1.Refresh();
        }
    }
}
