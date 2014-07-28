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
        private Series _series;
        private Index _index;
        private bool _browserCompleted;
        private HtmlAgilityPack.HtmlDocument _doc;

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
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);

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

            while (!(worker.CancellationPending || e.Cancel))
            {
                Page contextPage = null;
                Link contextLink = null;
                switch (dir)
                {
                    case (FillDirection.Forward):
                        contextPage = _series.Index.Pages.Last();
                        contextLink = _series.NextLink;
                        break;
                    case (FillDirection.Backward):
                        contextPage = _series.Index.Pages.First();
                        contextLink = _series.PrevLink;
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
                    string newPageUrl = newPageLink.GetAttributeValue("href", "");

                    if (!String.IsNullOrEmpty(newPageUrl))
                    {
                        GetBrowserAgilityDoc(newPageUrl);
                        while (!_browserCompleted)
                            Thread.Sleep(100);

                        Page newPage = _series.Source.GetPage(_series.ComicLink, _doc);

                        switch (dir)
                        {
                            case (FillDirection.Forward):
                                _series.Index.Pages.Add(newPage);
                                break;
                            case (FillDirection.Backward):
                                _series.Index.Pages.Insert(0, newPage);
                                break;
                            default:
                                break;
                        }

                        worker.ReportProgress(0, String.Format("Added page {0}\n", newPage.Title));
                    }
                }
            }
        }

        private void GetBrowserAgilityDoc(string url) //use WebBrowser to retrieve HTML, as javascript may change the document body.
        {
            _browserCompleted = false;
            webBrowser1.Navigate(url);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var browser = sender as WebBrowser;
            _doc = new HtmlAgilityPack.HtmlDocument();
            _doc.LoadHtml(browser.Document.Body.Parent.OuterHtml);
            _browserCompleted = true;
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
