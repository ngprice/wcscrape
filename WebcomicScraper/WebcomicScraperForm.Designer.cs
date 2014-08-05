namespace WebcomicScraper
{
    partial class WebcomicScraperForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebcomicScraperForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtArtist = new System.Windows.Forms.TextBox();
            this.txtAuthor = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSummary = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblURL = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvIndex = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtSaveDir = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnDownload = new System.Windows.Forms.Button();
            this.threadPanel = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.nudThreads = new System.Windows.Forms.NumericUpDown();
            this.downloadBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBoxLibrary = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.previewPictureBox = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToConfigxmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importConfigxmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.libraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewSeriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshCurrentSeriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCurrentSeriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCurrentSeriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fillIndexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trimToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkConvert = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudChapterSize = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndex)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.threadPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreads)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudChapterSize)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(144, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 273);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Series Info";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtArtist, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtAuthor, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtTitle, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtSummary, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.lblURL, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.txtURL, 1, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(217, 254);
            this.tableLayoutPanel3.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 20);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label3.Size = new System.Drawing.Size(41, 19);
            this.label3.TabIndex = 10;
            this.label3.Text = "Author:";
            // 
            // txtArtist
            // 
            this.txtArtist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtArtist.Location = new System.Drawing.Point(62, 43);
            this.txtArtist.Name = "txtArtist";
            this.txtArtist.ReadOnly = true;
            this.txtArtist.Size = new System.Drawing.Size(152, 20);
            this.txtArtist.TabIndex = 14;
            // 
            // txtAuthor
            // 
            this.txtAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAuthor.Location = new System.Drawing.Point(62, 23);
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.ReadOnly = true;
            this.txtAuthor.Size = new System.Drawing.Size(152, 20);
            this.txtAuthor.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 40);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label4.Size = new System.Drawing.Size(33, 19);
            this.label4.TabIndex = 13;
            this.label4.Text = "Artist:";
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.Location = new System.Drawing.Point(62, 3);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.ReadOnly = true;
            this.txtTitle.Size = new System.Drawing.Size(152, 20);
            this.txtTitle.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label2.Size = new System.Drawing.Size(30, 19);
            this.label2.TabIndex = 9;
            this.label2.Text = "Title:";
            // 
            // txtSummary
            // 
            this.txtSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSummary.Location = new System.Drawing.Point(62, 83);
            this.txtSummary.Multiline = true;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.ReadOnly = true;
            this.txtSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSummary.Size = new System.Drawing.Size(152, 168);
            this.txtSummary.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 80);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label5.Size = new System.Drawing.Size(53, 19);
            this.label5.TabIndex = 15;
            this.label5.Text = "Summary:";
            // 
            // lblURL
            // 
            this.lblURL.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(24, 60);
            this.lblURL.Name = "lblURL";
            this.lblURL.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblURL.Size = new System.Drawing.Size(32, 19);
            this.lblURL.TabIndex = 17;
            this.lblURL.Text = "URL:";
            // 
            // txtURL
            // 
            this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtURL.Location = new System.Drawing.Point(62, 63);
            this.txtURL.Name = "txtURL";
            this.txtURL.ReadOnly = true;
            this.txtURL.Size = new System.Drawing.Size(152, 20);
            this.txtURL.TabIndex = 18;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 303);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(883, 22);
            this.statusStrip1.TabIndex = 4;
            // 
            // ToolStripStatusLabel1
            // 
            this.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1";
            this.ToolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvIndex);
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(558, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(322, 273);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Index";
            // 
            // dgvIndex
            // 
            this.dgvIndex.AllowUserToAddRows = false;
            this.dgvIndex.AllowUserToDeleteRows = false;
            this.dgvIndex.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvIndex.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvIndex.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvIndex.Location = new System.Drawing.Point(3, 16);
            this.dgvIndex.Name = "dgvIndex";
            this.dgvIndex.ReadOnly = true;
            this.dgvIndex.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dgvIndex.Size = new System.Drawing.Size(316, 176);
            this.dgvIndex.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.txtSaveDir, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.progressBar1, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnDownload, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.threadPanel, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 192);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(316, 78);
            this.tableLayoutPanel2.TabIndex = 20;
            // 
            // txtSaveDir
            // 
            this.txtSaveDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSaveDir.Location = new System.Drawing.Point(103, 3);
            this.txtSaveDir.Name = "txtSaveDir";
            this.txtSaveDir.Size = new System.Drawing.Size(210, 20);
            this.txtSaveDir.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Save Directory:";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(103, 29);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(210, 19);
            this.progressBar1.TabIndex = 18;
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.Location = new System.Drawing.Point(3, 29);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(94, 20);
            this.btnDownload.TabIndex = 13;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // threadPanel
            // 
            this.threadPanel.Controls.Add(this.label7);
            this.threadPanel.Controls.Add(this.nudThreads);
            this.threadPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.threadPanel.Location = new System.Drawing.Point(3, 55);
            this.threadPanel.Name = "threadPanel";
            this.threadPanel.Size = new System.Drawing.Size(94, 20);
            this.threadPanel.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Threads:";
            // 
            // nudThreads
            // 
            this.nudThreads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nudThreads.Location = new System.Drawing.Point(57, 0);
            this.nudThreads.Name = "nudThreads";
            this.nudThreads.Size = new System.Drawing.Size(34, 20);
            this.nudThreads.TabIndex = 17;
            // 
            // downloadBackgroundWorker
            // 
            this.downloadBackgroundWorker.WorkerReportsProgress = true;
            this.downloadBackgroundWorker.WorkerSupportsCancellation = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(883, 279);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBoxLibrary);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(135, 273);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Library";
            // 
            // listBoxLibrary
            // 
            this.listBoxLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLibrary.FormattingEnabled = true;
            this.listBoxLibrary.Location = new System.Drawing.Point(3, 16);
            this.listBoxLibrary.Name = "listBoxLibrary";
            this.listBoxLibrary.Size = new System.Drawing.Size(129, 254);
            this.listBoxLibrary.TabIndex = 0;
            this.listBoxLibrary.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBoxLibrary_MouseClick);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.previewPictureBox);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(373, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(179, 273);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Cover Art";
            // 
            // previewPictureBox
            // 
            this.previewPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewPictureBox.Location = new System.Drawing.Point(3, 16);
            this.previewPictureBox.Margin = new System.Windows.Forms.Padding(10);
            this.previewPictureBox.Name = "previewPictureBox";
            this.previewPictureBox.Size = new System.Drawing.Size(173, 254);
            this.previewPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.previewPictureBox.TabIndex = 7;
            this.previewPictureBox.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.libraryToolStripMenuItem,
            this.seriesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.ShowItemToolTips = true;
            this.menuStrip1.Size = new System.Drawing.Size(883, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToConfigxmlToolStripMenuItem,
            this.importConfigxmlToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // saveToConfigxmlToolStripMenuItem
            // 
            this.saveToConfigxmlToolStripMenuItem.Name = "saveToConfigxmlToolStripMenuItem";
            this.saveToConfigxmlToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToConfigxmlToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.saveToConfigxmlToolStripMenuItem.Text = "&Save Library";
            this.saveToConfigxmlToolStripMenuItem.Click += new System.EventHandler(this.saveLibraryToolStripMenuItem_Click);
            // 
            // importConfigxmlToolStripMenuItem
            // 
            this.importConfigxmlToolStripMenuItem.Name = "importConfigxmlToolStripMenuItem";
            this.importConfigxmlToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.importConfigxmlToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.importConfigxmlToolStripMenuItem.Text = "&Open Library";
            this.importConfigxmlToolStripMenuItem.Click += new System.EventHandler(this.openLibraryToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // libraryToolStripMenuItem
            // 
            this.libraryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewSeriesToolStripMenuItem});
            this.libraryToolStripMenuItem.Enabled = false;
            this.libraryToolStripMenuItem.Name = "libraryToolStripMenuItem";
            this.libraryToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.libraryToolStripMenuItem.Text = "&Library";
            // 
            // addNewSeriesToolStripMenuItem
            // 
            this.addNewSeriesToolStripMenuItem.Name = "addNewSeriesToolStripMenuItem";
            this.addNewSeriesToolStripMenuItem.ShowShortcutKeys = false;
            this.addNewSeriesToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.addNewSeriesToolStripMenuItem.Text = "&Add New Series";
            this.addNewSeriesToolStripMenuItem.Click += new System.EventHandler(this.addNewSeriesToolStripMenuItem_Click);
            // 
            // seriesToolStripMenuItem
            // 
            this.seriesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshCurrentSeriesToolStripMenuItem,
            this.editCurrentSeriesToolStripMenuItem,
            this.deleteCurrentSeriesToolStripMenuItem,
            this.indexToolStripMenuItem});
            this.seriesToolStripMenuItem.Enabled = false;
            this.seriesToolStripMenuItem.Name = "seriesToolStripMenuItem";
            this.seriesToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.seriesToolStripMenuItem.Text = "&Series";
            // 
            // refreshCurrentSeriesToolStripMenuItem
            // 
            this.refreshCurrentSeriesToolStripMenuItem.Name = "refreshCurrentSeriesToolStripMenuItem";
            this.refreshCurrentSeriesToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.refreshCurrentSeriesToolStripMenuItem.Text = "&Refresh";
            // 
            // editCurrentSeriesToolStripMenuItem
            // 
            this.editCurrentSeriesToolStripMenuItem.Name = "editCurrentSeriesToolStripMenuItem";
            this.editCurrentSeriesToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.editCurrentSeriesToolStripMenuItem.Text = "&Edit";
            // 
            // deleteCurrentSeriesToolStripMenuItem
            // 
            this.deleteCurrentSeriesToolStripMenuItem.Name = "deleteCurrentSeriesToolStripMenuItem";
            this.deleteCurrentSeriesToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.deleteCurrentSeriesToolStripMenuItem.Text = "&Delete";
            this.deleteCurrentSeriesToolStripMenuItem.Click += new System.EventHandler(this.deleteCurrentSeriesToolStripMenuItem_Click);
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fillIndexToolStripMenuItem,
            this.trimToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // fillIndexToolStripMenuItem
            // 
            this.fillIndexToolStripMenuItem.Name = "fillIndexToolStripMenuItem";
            this.fillIndexToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.fillIndexToolStripMenuItem.Text = "&Fill...";
            this.fillIndexToolStripMenuItem.Click += new System.EventHandler(this.fillIndexToolStripMenuItem_Click);
            // 
            // trimToolStripMenuItem
            // 
            this.trimToolStripMenuItem.Name = "trimToolStripMenuItem";
            this.trimToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.trimToolStripMenuItem.Text = "Trim";
            this.trimToolStripMenuItem.ToolTipText = "Removes page info from downloaded chapters; use if your library is loading slowly" +
                "";
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Enabled = false;
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.ToolTipText = "Only available for natively supported sources with a chapter index";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.nudChapterSize);
            this.panel1.Controls.Add(this.chkConvert);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(103, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(210, 20);
            this.panel1.TabIndex = 19;
            // 
            // chkConvert
            // 
            this.chkConvert.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkConvert.AutoSize = true;
            this.chkConvert.Checked = true;
            this.chkConvert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkConvert.Location = new System.Drawing.Point(3, 0);
            this.chkConvert.Name = "chkConvert";
            this.chkConvert.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.chkConvert.Size = new System.Drawing.Size(84, 20);
            this.chkConvert.TabIndex = 20;
            this.chkConvert.Text = "Create .CBZ";
            this.chkConvert.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Chapter size:";
            // 
            // nudChapterSize
            // 
            this.nudChapterSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nudChapterSize.Location = new System.Drawing.Point(165, 0);
            this.nudChapterSize.Name = "nudChapterSize";
            this.nudChapterSize.Size = new System.Drawing.Size(34, 20);
            this.nudChapterSize.TabIndex = 22;
            // 
            // WebcomicScraperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 325);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(899, 363);
            this.Name = "WebcomicScraperForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Webcomic Scraper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WebcomicScraperForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WebcomicScraperForm_FormClosed);
            this.Load += new System.EventHandler(this.WebcomicScraperForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndex)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.threadPanel.ResumeLayout(false);
            this.threadPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreads)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudChapterSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel1;
        private System.Windows.Forms.TextBox txtSummary;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtArtist;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAuthor;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvIndex;
        private System.ComponentModel.BackgroundWorker downloadBackgroundWorker;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToConfigxmlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importConfigxmlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem libraryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewSeriesToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtSaveDir;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Panel threadPanel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudThreads;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.PictureBox previewPictureBox;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.ListBox listBoxLibrary;
        private System.Windows.Forms.ToolStripMenuItem seriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshCurrentSeriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fillIndexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteCurrentSeriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trimToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editCurrentSeriesToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudChapterSize;
        private System.Windows.Forms.CheckBox chkConvert;
    }
}

