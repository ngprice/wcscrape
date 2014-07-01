using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebcomicScraper.Comic;

namespace WebcomicScraper
{
    public partial class TeachNewSeries : Form
    {
        public Series NewSeries { get; set; }
 
        public TeachNewSeries()
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

            browser.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(DocumentLoaded);
            browser.Stop();
        }

        private void analysis_DoWork(object sender, DoWorkEventArgs e)
        {
            //webBrowser1 = new WebBrowser();
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(txtURL.Text);

            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocumentLoaded);
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
            else
            {
                Status("Analysis successful!");
            }
        }

        private void analysis_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Status(String.Format("Analysis {0}% complete.", e.ProgressPercentage));
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
            this.DialogResult = DialogResult.OK;
        }
    }
}
