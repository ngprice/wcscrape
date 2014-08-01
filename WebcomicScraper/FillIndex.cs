using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using WebcomicScraper.Comic;
using HtmlAgilityPack;

namespace WebcomicScraper
{
    public partial class FillIndex : Form
    {
        private Index _index;
        private bool _browserCompleted;
        private HtmlAgilityPack.HtmlDocument _doc;
        private bool _downloading;

        public Series LoadedSeries { get; set; }

        private Index LoadedIndex 
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
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);

            _downloading = false;
            LoadedSeries = series;
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
                    else if (chapter.Downloading)
                        row.DefaultCellStyle.BackColor = Color.LightYellow;
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
            if (!_downloading)
            {
                _downloading = true;
                txtStatus.ResetText();
                ToggleCancelButton((Button)sender);
                indexBackgroundWorker.RunWorkerAsync(FillDirection.Forward);
            }
            else
            {
                Status("\nCanceling...\n");
                indexBackgroundWorker.CancelAsync();
            }
        }

        private void btnFillBackward_Click(object sender, EventArgs e)
        {
            if (!_downloading)
            {
                _downloading = true;
                txtStatus.ResetText();
                ToggleCancelButton((Button)sender);
                indexBackgroundWorker.RunWorkerAsync(FillDirection.Backward);
            }
            else
            {
                Status("\nCanceling...\n");
                indexBackgroundWorker.CancelAsync();
            }
        }

        private void ToggleCancelButton(Button btn)
        {
            btn.Text = "Cancel";
            btnFillBackward.Enabled = (btn == btnFillBackward);
            btnFillForward.Enabled = (btn == btnFillForward);

            btnCancel.Enabled = false;
            btnSave.Enabled = false;
        }

        private void RestoreAllButtons()
        {
            btnFillBackward.Text = "Fill Backward";
            btnFillBackward.Enabled = true;

            btnFillForward.Text = "Fill Forward";
            btnFillForward.Enabled = true;

            btnCancel.Enabled = true;
            btnSave.Enabled = true;
        }

        private void index_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var dir = (FillDirection)e.Argument;
            int tries = 0;
            int maxTries = 5;
            int invalidLinkCtr = 0;

            while (tries < maxTries && !(worker.CancellationPending || e.Cancel))
            {
                try
                {
                    Page contextPage = null;
                    Link contextLink = null;
                    switch (dir)
                    {
                        case (FillDirection.Forward):
                            contextPage = LoadedSeries.Index.Pages.Last();
                            contextLink = LoadedSeries.NextLink;
                            break;
                        case (FillDirection.Backward):
                            contextPage = LoadedSeries.Index.Pages.First();
                            contextLink = LoadedSeries.PrevLink;
                            break;
                        default:
                            break;
                    }

                    if (contextPage != null)
                    {
                        if (contextPage.Document == null)
                        {
                            GetBrowserAgilityDoc(contextPage.PageURL);
                            while (!_browserCompleted)
                                Thread.Sleep(100);
                            contextPage.Document = _doc;
                        }

                        var newPageLink = contextPage.Document.DocumentNode.SelectSingleNode(contextLink.XPath);
                        if (newPageLink == null)
                        {
                            invalidLinkCtr++;
                            contextPage.Document = null;
                            throw new ApplicationException(String.Format("Bad link XPath: {0}", contextLink.XPath));
                        }

                        string newPageUrl = newPageLink.GetAttributeValue("href", "");

                        if (ValidPageUrl(newPageUrl))
                        {
                            invalidLinkCtr = 0;
                            GetBrowserAgilityDoc(newPageUrl);
                            while (!_browserCompleted)
                                Thread.Sleep(100);

                            Page newPage = LoadedSeries.Source.GetPage(LoadedSeries.ComicLink, newPageUrl, _doc);

                            switch (dir)
                            {
                                case (FillDirection.Forward):
                                    LoadedSeries.Index.Pages.Add(newPage);
                                    break;
                                case (FillDirection.Backward):
                                    LoadedSeries.Index.Pages.Insert(0, newPage);
                                    break;
                                default:
                                    break;
                            }
                            tries = 0;
                            worker.ReportProgress(0, String.Format("Added page {0}\n", newPage.Title));
                        }
                        else throw new ApplicationException(String.Format("Bad page link: {0}", newPageLink.InnerHtml));
                    }
                }
                catch (Exception ex)
                {
                    if (invalidLinkCtr >= maxTries)
                    {
                        worker.ReportProgress(0, "End of series reached.");
                        e.Result = true;
                        return;
                    }
                    if (tries++ <= maxTries)
                    {
                        worker.ReportProgress(0, ".");
                        Thread.Sleep(100);
                    }
                    else
                    {
                        e.Result = false;
                        throw new ApplicationException(String.Format("Retry #{0}: {1}\n", tries, ex.Message));
                    }
                }
            }

            if (worker.CancellationPending)
            {
                e.Cancel = true;
                e.Result = false;
            }
            else
                e.Result = false;
            return;
        }

        private bool ValidPageUrl(string pageUrl)
        {
            var seedUri = new Uri(LoadedSeries.SeedURL);
            var pageUri = new Uri(pageUrl);

            if (String.IsNullOrEmpty(pageUrl))
                return false;
            else if (seedUri.Host != pageUri.Host)
                return false;
            else return true;
        }

        private void GetBrowserAgilityDoc(string url) //use WebBrowser to retrieve HTML, as javascript may change the document body.
        {
            _browserCompleted = false;
            webBrowser1.Navigate(url);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var browser = sender as WebBrowser;
            if (browser.ReadyState != WebBrowserReadyState.Complete)
                return;

            _doc = new HtmlAgilityPack.HtmlDocument();
            _doc.LoadHtml(browser.Document.Body.Parent.OuterHtml);
            _browserCompleted = true;
        }

        private void index_ReportProgress(object sender, ProgressChangedEventArgs e)
        {
            Status(e.UserState as String);
            DisplayIndex(LoadedIndex);
        }

        private void index_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            _downloading = false;
            RestoreAllButtons();

            if (e.Error != null)
            {
                MessageBox.Show(String.Format("Error in background thread: {0}\n", e.Error.Message));
            }
            else if (e.Cancelled)
            {
                Status("Download canceled.\n");
            }
            else if (!(bool)e.Result)
            {
                Status("Download stopped.\n");
            }
            else
            {
                Status("Download successful.\n");
            }

            DisplayIndex(LoadedIndex);
        }

        private void Status(string status)
        {
            txtStatus.AppendText(status.Replace("\n", Environment.NewLine));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
