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
        private bool _bDownloading;
        private CancellationTokenSource _cs;

        public Library LoadedLibrary 
        { 
            get
            {
                return _activeLibrary;
            }
            set
            {
                _activeLibrary = value;
                libraryToolStripMenuItem.Enabled = (value != null);
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
                seriesToolStripMenuItem.Enabled = (value != null);
                DisplaySeries(_activeSeries);
            }
        }

        public WebcomicScraperForm()
        {
            InitializeComponent();
            InitializeBackgroundWorkers();

            nudThreads.Value = Math.Min(Environment.ProcessorCount, 64);

            if (!String.IsNullOrEmpty(Properties.Settings.Default.LibraryPath) && File.Exists(Properties.Settings.Default.LibraryPath))
            {
                try
                {
                    LoadedLibrary = Scraper.DeserializeLibrary(Properties.Settings.Default.LibraryPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Error loading library file: {0}", ex.Message));
                }
            }
            else
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
            if (!_bDownloading)
            {
                _cs = new CancellationTokenSource();
                progressBar1.Value = 0;
                var rows = dgvIndex.SelectedRows;
                if (rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Status("Download started.");
                    _bDownloading = true;
                    btnDownload.Text = "Cancel";
                    downloadBackgroundWorker.RunWorkerAsync(rows);
                }
                else Status("Must select at least 1 row.");
            }
            else if (!downloadBackgroundWorker.CancellationPending)
            {
                try
                {
                    Status("Canceling download...");
                    downloadBackgroundWorker.CancelAsync();
                    _cs.Cancel();
                    _bDownloading = false;
                    btnDownload.Enabled = false;
                    progressBar1.Value = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void download_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var rows = e.Argument as DataGridViewSelectedRowCollection;
            var ctr = 0;
            int? threads = (int?)nudThreads.Value;

            e.Result = true;

            Parallel.ForEach(rows.Cast<DataGridViewRow>(), currentRow =>
                {
                    try
                    {
                        if (!worker.CancellationPending)
                        {
                            var chapter = (Chapter)currentRow.DataBoundItem;
                            currentRow.DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF77");

                            if (!Scraper.DownloadChapter(chapter, LoadedSeries, txtSaveDir.Text, threads, chkConvert.Checked, _cs))
                                e.Result = false;
                            else
                            {
                                currentRow.DefaultCellStyle.BackColor = Color.LightGreen;
                                _bLibraryDirty = true;
                                worker.ReportProgress((++ctr * 100) / rows.Count);
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        e.Cancel = true;
                    }
                });
            }

        private void download_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            btnDownload.Text = "Download";
            btnDownload.Enabled = true;
            _bDownloading = false;

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
            progressBar1.Value = e.ProgressPercentage;
            Status(String.Format("Download in progress: {0}%", e.ProgressPercentage));
        }

        private void DisplaySeries(Series series)
        {
            if (series != null)
            {
                txtTitle.Text = series.Title;
                txtAuthor.Text = series.Author;
                txtArtist.Text = series.Artist;
                txtSummary.Text = series.Summary;
                txtURL.Text = series.SeedURL.ToString();

                if (!String.IsNullOrEmpty(series.CoverImageURL))
                {
                    previewPictureBox.WaitOnLoad = false;
                    previewPictureBox.LoadAsync(series.CoverImageURL);
                }

                var source = new BindingSource();
                if (series.Index.Chapters != null && series.Index.Chapters.Any())
                {
                    source.DataSource = series.Index.Chapters;
                    dgvIndex.DataSource = source;
                    foreach (DataGridViewRow row in dgvIndex.Rows)
                    {
                        var chapter = (Chapter)row.DataBoundItem;
                        row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
                        if (chapter.Downloaded)
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                        else if (chapter.Downloading)
                            row.DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF77");
                    }
                }
                else if (series.Index.Pages != null && series.Index.Pages.Any())
                {
                    source.DataSource = series.Index.Pages;
                    dgvIndex.DataSource = source;

                    foreach (DataGridViewRow row in dgvIndex.Rows)
                    {
                        var page = (Page)row.DataBoundItem;
                        row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
                        if (page.Downloaded)
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                }
                else
                {
                    dgvIndex.DataSource = null;
                    dgvIndex.Refresh();
                }
            }
            else //null series
            {
                ClearControls();
            }
        }

        private void ClearControls()
        {
            txtTitle.Text = String.Empty;
            txtAuthor.Text = String.Empty;
            txtArtist.Text = String.Empty;
            txtSummary.Text = String.Empty;
            txtURL.Text = String.Empty;

            previewPictureBox.Image = null;

            dgvIndex.DataSource = null;
            dgvIndex.Refresh();

            listBoxLibrary.SelectedIndex = -1;
        }

        private void DisplayLibrary(Library library)
        {
            var source = new BindingSource();
            source.DataSource = library.lstSeries;
            listBoxLibrary.DataSource = source;
            listBoxLibrary.Refresh();
        }

        private void SaveLibrary()
        {
            string libraryPath = Scraper.SerializeLibrary(LoadedLibrary, txtSaveDir.Text);
            Properties.Settings.Default.LibraryPath = libraryPath;
            _bLibraryDirty = false;
            Status("Library saved.");
        }

        private void Status(string msg)
        {
            ToolStripStatusLabel1.Text = msg;
            statusStrip1.Refresh();
        }

        private void addNewSeriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var addNewSeriesForm = new AddNewSeries())
            {
                if (addNewSeriesForm.ShowDialog() == DialogResult.OK) //Save
                {
                    var newSeries = addNewSeriesForm.NewSeries;
                    LoadedLibrary.AddSeries(newSeries);
                    DisplayLibrary(LoadedLibrary);
                    LoadedSeries = newSeries;
                    listBoxLibrary.SelectedItem = LoadedSeries;
                    _bLibraryDirty = true;
                }
                this.BringToFront();
            }
        }

        private void openLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = txtSaveDir.Text;
            ofd.Filter = "XML files (*.xml)|*.xml";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var filepath = ofd.FileName;
                LoadedLibrary = Scraper.DeserializeLibrary(filepath);
                Status("Library loaded.");
            }
        }

        private void saveLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveLibrary();
        }

        private void WebcomicScraperForm_Load(object sender, EventArgs e)
        {
            txtSaveDir.Text = Properties.Settings.Default.SaveDir;
        }

        private void listBoxLibrary_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBoxLibrary.SelectedIndex >= 0)
                LoadedSeries = (Series)listBoxLibrary.SelectedItem;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WebcomicScraperForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_bLibraryDirty)
            {
                var choice = MessageBox.Show("Save changes to the current library?", "Save Library", MessageBoxButtons.YesNoCancel);
                switch (choice)
                {
                    case (DialogResult.Yes):
                        SaveLibrary();
                        break;
                    case (DialogResult.Cancel):
                        e.Cancel = true;
                        break;
                    case (DialogResult.No):
                    default:
                        break;
                }
            }
        }

        private void WebcomicScraperForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtSaveDir.Text))
            {
                Properties.Settings.Default.SaveDir = txtSaveDir.Text;
                var libPath = txtSaveDir.Text + @"\wclib.xml";
                if (File.Exists(libPath))
                    Properties.Settings.Default.LibraryPath = libPath;

                Properties.Settings.Default.Save();
            }
        }

        private void deleteCurrentSeriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var choice = MessageBox.Show("Are you sure you want to delete the current series?", "Delete Series", MessageBoxButtons.YesNo);
            switch (choice)
            {
                case (DialogResult.Yes):
                    LoadedLibrary.RemoveSeries(LoadedSeries);
                    DisplayLibrary(LoadedLibrary);
                    LoadedSeries = null;
                    _bLibraryDirty = true;
                    break;
                case (DialogResult.No):
                default:
                    break;
            }
        }

        private void fillIndexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fillIndexForm = new FillIndex(LoadedSeries))
            {
                if (fillIndexForm.ShowDialog() == DialogResult.OK) //Save
                {
                    LoadedSeries = fillIndexForm.LoadedSeries;
                    listBoxLibrary.SelectedItem = LoadedSeries;
                    _bLibraryDirty = true;
                }
                this.BringToFront();
            }
        }
    }
}
