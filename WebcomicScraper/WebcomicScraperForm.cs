using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using WebcomicScraper.Comic;
using System.Threading;

namespace WebcomicScraper
{
    public partial class WebcomicScraperForm : Form
    {
        private Library _activeLibrary;
        private Series _activeSeries;
        private bool _bLibraryDirty;
        public Library LoadedLibrary 
        { 
            get
            {
                return _activeLibrary;
            }
            set
            {
                _activeLibrary = value;
                DisplayLibrary(_activeLibrary);
            }
        }
        public Series LoadedSeries 
        { 
            get
            {
                return _activeSeries;
            }
            set
            {
                _activeSeries = value;
                DisplaySeries(_activeSeries);
            }
        }

        public WebcomicScraperForm()
        {
            InitializeComponent();
            InitializeBackgroundWorkers();

            nudThreads.Value = Math.Min(Environment.ProcessorCount, 64);
            LoadedLibrary = new Library();
        }

        private void InitializeBackgroundWorkers()
        {
            downloadBackgroundWorker.DoWork += new DoWorkEventHandler(download_DoWork);
            downloadBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(download_Completed);
            downloadBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(download_ProgressChanged);
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
            var seriesPath = Path.Combine(txtSaveDir.Text, Scraper.CleanPath(LoadedSeries.Title));
            int? threads = (int?)nudThreads.Value;

            e.Result = true;

            Parallel.ForEach(rows.Cast<DataGridViewRow>(), currentRow =>
                {
                    if (!Scraper.DownloadChapter(currentRow.DataBoundItem as Chapter, LoadedSeries.Title, seriesPath, threads))
                        e.Result = false;
                });
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

            if (!String.IsNullOrEmpty(series.CoverImageURL))
            {
                previewPictureBox.WaitOnLoad = false;
                previewPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                previewPictureBox.LoadAsync(series.CoverImageURL);
            }

            if (series.Index.Chapters.Count() > 0)
            {
                var source = new BindingSource();
                source.DataSource = series.Index.Chapters;
                dgvIndex.DataSource = source;
                dgvIndex.Columns[4].Visible = false;
                foreach (DataGridViewRow row in dgvIndex.Rows)
                {
                    row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
                }
                dgvIndex.Refresh();
            }
        }

        private void DisplayLibrary(Library library)
        {

        }

        private void Status(string msg)
        {
            ToolStripStatusLabel1.Text = msg;
            statusStrip1.Refresh();
        }

        private void teachNewSeriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var teachNewSeriesForm = new LearnNewSeries())
            {
                if (teachNewSeriesForm.ShowDialog() == DialogResult.OK) //Save
                {
                    var newSeries = teachNewSeriesForm.NewSeries;
                    LoadedSeries = newSeries;
                    LoadedLibrary.AddSeries(newSeries);
                }
            }
        }

        private void importConfigxmlToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToConfigxmlToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
