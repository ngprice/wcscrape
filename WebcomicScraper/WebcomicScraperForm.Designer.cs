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
            this.label1 = new System.Windows.Forms.Label();
            this.txtIndex = new System.Windows.Forms.TextBox();
            this.btnScrape = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSummary = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtArtist = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAuthor = new System.Windows.Forms.TextBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.previewPictureBox = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.analysisBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvIndex = new System.Windows.Forms.DataGridView();
            this.btnDownload = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSaveDir = new System.Windows.Forms.TextBox();
            this.downloadBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.label7 = new System.Windows.Forms.Label();
            this.nudThreads = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreads)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Series URL:";
            // 
            // txtIndex
            // 
            this.txtIndex.Location = new System.Drawing.Point(82, 10);
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.Size = new System.Drawing.Size(261, 20);
            this.txtIndex.TabIndex = 1;
            this.txtIndex.Text = "http://www.mangahere.co/manga/onepunch_man/";
            // 
            // btnScrape
            // 
            this.btnScrape.Location = new System.Drawing.Point(349, 7);
            this.btnScrape.Name = "btnScrape";
            this.btnScrape.Size = new System.Drawing.Size(75, 23);
            this.btnScrape.TabIndex = 2;
            this.btnScrape.Text = "Analyze";
            this.btnScrape.UseVisualStyleBackColor = true;
            this.btnScrape.Click += new System.EventHandler(this.btnScrape_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSummary);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtArtist);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtAuthor);
            this.groupBox1.Controls.Add(this.txtTitle);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.previewPictureBox);
            this.groupBox1.Location = new System.Drawing.Point(11, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(413, 299);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Series Info";
            // 
            // txtSummary
            // 
            this.txtSummary.Location = new System.Drawing.Point(63, 94);
            this.txtSummary.Multiline = true;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.ReadOnly = true;
            this.txtSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSummary.Size = new System.Drawing.Size(169, 199);
            this.txtSummary.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Summary:";
            // 
            // txtArtist
            // 
            this.txtArtist.Location = new System.Drawing.Point(63, 67);
            this.txtArtist.Name = "txtArtist";
            this.txtArtist.ReadOnly = true;
            this.txtArtist.Size = new System.Drawing.Size(169, 20);
            this.txtArtist.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Artist:";
            // 
            // txtAuthor
            // 
            this.txtAuthor.Location = new System.Drawing.Point(63, 41);
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.ReadOnly = true;
            this.txtAuthor.Size = new System.Drawing.Size(169, 20);
            this.txtAuthor.TabIndex = 12;
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(63, 16);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.ReadOnly = true;
            this.txtTitle.Size = new System.Drawing.Size(169, 20);
            this.txtTitle.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Author:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Title:";
            // 
            // previewPictureBox
            // 
            this.previewPictureBox.Location = new System.Drawing.Point(238, 16);
            this.previewPictureBox.Name = "previewPictureBox";
            this.previewPictureBox.Size = new System.Drawing.Size(169, 277);
            this.previewPictureBox.TabIndex = 6;
            this.previewPictureBox.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 351);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(744, 22);
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
            this.groupBox2.Location = new System.Drawing.Point(430, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(307, 258);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Index";
            // 
            // dgvIndex
            // 
            this.dgvIndex.AllowUserToAddRows = false;
            this.dgvIndex.AllowUserToDeleteRows = false;
            this.dgvIndex.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvIndex.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvIndex.Location = new System.Drawing.Point(3, 16);
            this.dgvIndex.Name = "dgvIndex";
            this.dgvIndex.ReadOnly = true;
            this.dgvIndex.Size = new System.Drawing.Size(301, 239);
            this.dgvIndex.TabIndex = 0;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(433, 312);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 23);
            this.btnDownload.TabIndex = 7;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(430, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Save Directory:";
            // 
            // txtSaveDir
            // 
            this.txtSaveDir.Location = new System.Drawing.Point(516, 10);
            this.txtSaveDir.Name = "txtSaveDir";
            this.txtSaveDir.Size = new System.Drawing.Size(221, 20);
            this.txtSaveDir.TabIndex = 9;
            this.txtSaveDir.Text = "C:\\Users\\ngprice\\Downloads";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(513, 317);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Threads:";
            // 
            // nudThreads
            // 
            this.nudThreads.Location = new System.Drawing.Point(564, 315);
            this.nudThreads.Name = "nudThreads";
            this.nudThreads.Size = new System.Drawing.Size(34, 20);
            this.nudThreads.TabIndex = 11;
            // 
            // WebcomicScraperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 373);
            this.Controls.Add(this.nudThreads);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtSaveDir);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnScrape);
            this.Controls.Add(this.txtIndex);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(760, 411);
            this.Name = "WebcomicScraperForm";
            this.Text = "Webcomic Scraper";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreads)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIndex;
        private System.Windows.Forms.Button btnScrape;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel1;
        private System.ComponentModel.BackgroundWorker analysisBackgroundWorker;
        private System.Windows.Forms.PictureBox previewPictureBox;
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
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSaveDir;
        private System.ComponentModel.BackgroundWorker downloadBackgroundWorker;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudThreads;
    }
}

