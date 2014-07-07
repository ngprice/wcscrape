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
using HtmlAgilityPack;

namespace WebcomicScraper
{
    public partial class LearnNewSeries : Form
    {
        private bool _bLoaded;
        public Series NewSeries { get; set; }

        private readonly Dictionary<int, Link> _dicRowLink = new Dictionary<int, Link>() //got to be a better way
        {
            {5, new Link()}, //Next
            {6, new Link()}, //Prev
            {7, new Link()}, //First
            {8, new Link()}  //Last
        };
 
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

        private void analysis_DoWork(object sender, DoWorkEventArgs e)
        {
            _bLoaded = false;
            webBrowser1.Navigating -= webBrowser1_Navigating;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);

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

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = (WebBrowser)sender;
            var agilityDoc = new HtmlAgilityPack.HtmlDocument();
            agilityDoc.LoadHtml(browser.Document.Body.InnerHtml);

            NewSeries = Scraper.LoadSeries(browser.Url, agilityDoc);
            Scraper.AnalyzeSeries(NewSeries);

            browser.Document.MouseUp += new HtmlElementEventHandler(htmlDocument_Click);
            browser.DocumentCompleted -= webBrowser1_DocumentCompleted;
            browser.Navigating += new WebBrowserNavigatingEventHandler(webBrowser1_Navigating);
            browser.Stop();

            if (Scraper.KnowsSource(browser.Url.Host))
            {
                DisplaySeries(NewSeries);
                ToggleRadioButtons(false);
                Status(String.Format("Natively supported host detected: {0}", browser.Url.Host));
            }

            _bLoaded = true;
        }

        //http://stackoverflow.com/questions/20736331/get-xpath-from-clicked-htmlelement-in-webbrowsercontrol //isn't stackoverflow great
        private void htmlDocument_Click(object sender, HtmlElementEventArgs e)
        {
            var browserDoc = (System.Windows.Forms.HtmlDocument)sender;
            var element = browserDoc.GetElementFromPoint(e.ClientMousePosition);

            var uniqueId = Guid.NewGuid().ToString();
            element.Id = uniqueId;

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(element.Document.GetElementsByTagName("html")[0].OuterHtml);

            var node = doc.GetElementbyId(uniqueId);

            var cellPosition = tableLayoutPanel2.GetPositionFromControl(tableLayoutPanel2.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked));
            if (_dicRowLink.ContainsKey(cellPosition.Row))
                _dicRowLink[cellPosition.Row].XPath = node.XPath;
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (_bLoaded)
            {
                if (e.Url.Scheme == "http" || e.Url.Scheme == "https") //toss javascript, about, ftp, etc
                {
                    e.Cancel = true;
                    CycleRadioButtonURL(e.Url.ToString());
                }
            }
        }

        private void CycleRadioButtonURL(string url)
        {
            var cellPosition = tableLayoutPanel2.GetPositionFromControl(tableLayoutPanel2.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked));
            cellPosition.Column++;

            var txtBox = tableLayoutPanel2.GetControlFromPosition(cellPosition.Column, cellPosition.Row);
            txtBox.Text = url;

            if (_dicRowLink.ContainsKey(cellPosition.Row))
                _dicRowLink[cellPosition.Row].SampleURL = url;

            cellPosition.Column--; //check the next radiobutton
            cellPosition.Row++;
            var control = tableLayoutPanel2.GetControlFromPosition(cellPosition.Column, cellPosition.Row);
            if (control is RadioButton)
                ((RadioButton)control).Checked = true;
        }

        private void DisplaySeries(Series series)
        {
            txtTitle.Text = series.Title;
            txtAuthor.Text = series.Author;
            txtArtist.Text = series.Artist;
            txtSummary.Text = series.Summary;
            txtCoverURL.Text = series.CoverImageURL;
        }

        private void ToggleRadioButtons(bool toggle)
        {
            rdbNext.Enabled = toggle;
            rdbPrev.Enabled = toggle;
            rdbFirst.Enabled = toggle;
            rdbLast.Enabled = toggle;
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
