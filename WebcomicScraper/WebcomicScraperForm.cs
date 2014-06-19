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
    public partial class WebcomicScraperForm : Form
    {
        public WebcomicScraperForm()
        {
            InitializeComponent();
        }

        /*
         * TODO:
         * --Resource list for saving URLs
         * */

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtIndex.Text))
            {
                Status("There's nothing there, idiot.");
                return;
            }

            try
            {
                var index = new Index(txtIndex.Text);
                if (index.Analyze())
                    Status("Analysis successful!");
                else Status("Analysis failed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Status(string msg)
        {
            ToolStripStatusLabel1.Text = msg;
            statusStrip1.Refresh();
        }
    }
}
