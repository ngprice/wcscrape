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
    public partial class AddNewSeries : Form
    {
        private bool _bLoaded;
        public Series NewSeries { get; set; }

        private readonly Dictionary<int, Link> _dicRowLink;
 
        public AddNewSeries()
        {
            InitializeComponent();
            InitializeBackgroundWorkers();

            _dicRowLink = new Dictionary<int, Link>();
            _dicRowLink.Add(tableLayoutPanel2.GetPositionFromControl(txtThisComic).Row, new Link());
            _dicRowLink.Add(tableLayoutPanel2.GetPositionFromControl(txtNextLink).Row, new Link());
            _dicRowLink.Add(tableLayoutPanel2.GetPositionFromControl(txtPrevLink).Row, new Link());
            _dicRowLink.Add(tableLayoutPanel2.GetPositionFromControl(txtFirstLink).Row, new Link());
            _dicRowLink.Add(tableLayoutPanel2.GetPositionFromControl(txtLastLink).Row, new Link());
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
            if (browser.Document != null)
            {
                agilityDoc.LoadHtml(browser.Document.Body.InnerHtml);

                NewSeries = Scraper.LoadSeries(browser.Url.ToString(), agilityDoc);
                Scraper.AnalyzeSeries(NewSeries);

                browser.Document.MouseUp += new HtmlElementEventHandler(htmlDocument_Click);
                browser.DocumentCompleted -= webBrowser1_DocumentCompleted;
                browser.Navigating += new WebBrowserNavigatingEventHandler(webBrowser1_Navigating);
                //browser.Stop(); //sometimes javascript triggers click events, filling in radio button fields. Stop() sometimes prevents loading of jscript-dependent elements though (e.g. penny arcade). unsure what best fix is

                if (Scraper.KnowsSource(browser.Url.Host))
                {
                    DisplaySeries(NewSeries);
                    ToggleRadioButtons(false);
                    Status(String.Format("Known host detected: {0}", browser.Url.Host));
                }

                _bLoaded = true;
            }
            else Status("Failed to load page: " + browser.Url);
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
            var control = tableLayoutPanel2.GetControlFromPosition(cellPosition.Column, cellPosition.Row);
            if (control.Enabled)
            {
                if (_dicRowLink.ContainsKey(cellPosition.Row))
                {
                    _dicRowLink[cellPosition.Row].XPath = node.XPath;

                    if (control == rdbThisComic) //get img/@src instead of a/@href
                    {
                        CycleRadioButtonURL(node.GetAttributeValue("src", ""));
                    }
                    else
                    {
                        CycleRadioButtonURL(node.GetAttributeValue("href",""));
                    }
                }
            }
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (_bLoaded)
            {
                if (e.Url.Scheme == "http" || e.Url.Scheme == "https")
                {
                    if (ModifierKeys != Keys.Control) //allow navigation if CTRL is pressed
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void CycleRadioButtonURL(string url)
        {
            var cellPosition = tableLayoutPanel2.GetPositionFromControl(tableLayoutPanel2.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked));
            cellPosition.Column++;

            var txtBox = tableLayoutPanel2.GetControlFromPosition(cellPosition.Column, cellPosition.Row);
            if (txtBox.Enabled)
            {
                txtBox.Text = url;

                if (_dicRowLink.ContainsKey(cellPosition.Row))
                    _dicRowLink[cellPosition.Row].SampleURL = url;

                cellPosition.Column--; //check the next radiobutton
                cellPosition.Row++;
                var control = tableLayoutPanel2.GetControlFromPosition(cellPosition.Column, cellPosition.Row);
                if (control is RadioButton)
                    ((RadioButton)control).Checked = true;
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

        private void ToggleRadioButtons(bool toggle)
        {
            rdbThisComic.Enabled = toggle;
            rdbNext.Enabled = toggle;
            rdbPrev.Enabled = toggle;
            rdbFirst.Enabled = toggle;
            rdbLast.Enabled = toggle;

            txtThisComic.Enabled = toggle;
            txtNextLink.Enabled = toggle;
            txtPrevLink.Enabled = toggle;
            txtFirstLink.Enabled = toggle;
            txtLastLink.Enabled = toggle;
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
            NewSeries.Title = txtTitle.Text;
            NewSeries.Author = txtAuthor.Text;
            NewSeries.Artist = txtArtist.Text;
            NewSeries.Summary = txtSummary.Text;
            NewSeries.CoverImageURL = txtCoverURL.Text;

            if (NewSeries.Source == null)
            {
                NewSeries.SampleComic = _dicRowLink[tableLayoutPanel2.GetPositionFromControl(txtThisComic).Row];
                NewSeries.NextLink = _dicRowLink[tableLayoutPanel2.GetPositionFromControl(txtNextLink).Row];
                NewSeries.PrevLink = _dicRowLink[tableLayoutPanel2.GetPositionFromControl(txtPrevLink).Row];
                NewSeries.FirstLink = _dicRowLink[tableLayoutPanel2.GetPositionFromControl(txtFirstLink).Row];
                NewSeries.LastLink = _dicRowLink[tableLayoutPanel2.GetPositionFromControl(txtLastLink).Row];

                var page = new Page();//add sample page to index
                page.Num = 1;
                page.ImageURL = NewSeries.SampleComic.SampleURL;

                NewSeries.Index.Pages = new List<Page>();
                NewSeries.Index.Pages.Add(page);
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}
