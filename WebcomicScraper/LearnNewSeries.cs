using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WebcomicScraper.Comic;

namespace WebcomicScraper
{
    public partial class LearnNewSeries : Form
    {
        private bool _bLoaded;
        public Series NewSeries { get; set; }
 
        public LearnNewSeries()
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

        private void DocumentLoaded(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = (WebBrowser)sender;
            NewSeries = Scraper.LoadSeries(browser.Url, browser.Document);
            Scraper.AnalyzeSeries(NewSeries);

            browser.DocumentCompleted -= DocumentLoaded;
            browser.Navigating += new WebBrowserNavigatingEventHandler(webBrowser1_Navigating);
            //browser.Stop();
            _bLoaded = true;
        }

        private void analysis_DoWork(object sender, DoWorkEventArgs e)
        {
            _bLoaded = false;
            webBrowser1.Navigating -= webBrowser1_Navigating;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocumentLoaded);
            //webBrowser1.ProgressChanged += new WebBrowserProgressChangedEventHandler(ProgressChanged);

            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(txtURL.Text);
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
                Status("Load canceled.");
            }
            else
            {
                Status("Load successful.");
            }
        }

        private void analysis_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Status(String.Format("Analysis {0}% complete.", e.ProgressPercentage));
        }

        //private void ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        //{
        //    if (e.CurrentProgress == e.MaximumProgress)
        //    {
        //        var browser = sender as WebBrowser;
        //        browser.Navigating += new WebBrowserNavigatingEventHandler(webBrowser1_Navigating);
        //    }
        //}

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (_bLoaded)
            {
                if (e.Url.Scheme == "http" || e.Url.Scheme == "https") //toss javascript, about, ftp, etc
                {
                    var cellPosition = tableLayoutPanel2.GetPositionFromControl(tableLayoutPanel2.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked));
                    cellPosition.Column++;
                    var txtBox = tableLayoutPanel2.GetControlFromPosition(cellPosition.Column, cellPosition.Row);
                    txtBox.Text = e.Url.ToString();

                    cellPosition.Column--; //try to check the next radiobutton
                    cellPosition.Row++;
                    var control = tableLayoutPanel2.GetControlFromPosition(cellPosition.Column, cellPosition.Row);
                    if (control is RadioButton)
                    {
                        ((RadioButton)control).Checked = true;
                    }
                    e.Cancel = true;
                }
            }
        }

        private void DisplaySeries(Series series)
        {
            txtTitle.Text = series.Title;
            txtAuthor.Text = series.Author;
            txtArtist.Text = series.Artist;
            txtSummary.Text = series.Summary;
            txtCoverURL.Text = series.CoverImageURL;
        }

        private void Status(string msg)
        {
            ToolStripStatusLabel1.Text = msg;
            statusStrip1.Refresh();
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtURL.Text))
            {
                Status("There's nothing there, idiot.");
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                rdbNext.Checked = true;
                Status("Loading...");
                analysisBackgroundWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            NewSeries.Title = txtTitle.Text;
            NewSeries.Author = txtAuthor.Text;
            NewSeries.Artist = txtArtist.Text;
            NewSeries.Summary = txtSummary.Text;
            NewSeries.CoverImageURL = txtCoverURL.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}
