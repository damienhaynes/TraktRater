using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraktRater.Settings;

namespace TraktRater.UI.Controls
{
    public partial class IMDbUserControl : UserControl
    {
        public IMDbUserControl()
        {
            InitializeComponent();
        }

        public void Initialise()
        {
            chkIMDbEnabled.Checked = AppSettings.EnableIMDb;
            lsImdbCustomLists.Items.AddRange(AppSettings.IMDbCustomLists.ToArray());
            txtImdbRatingsFilename.Text = AppSettings.IMDbRatingsFilename;
            txtImdbWatchlistFile.Text = AppSettings.IMDbWatchlistFilename;
            txtImdbWebUsername.Text = AppSettings.IMDbUsername;
            chkImdbWebWatchlist.Checked = AppSettings.IMDbSyncWatchlist;

            // enable relavent IMDb option
            if (!string.IsNullOrEmpty(AppSettings.IMDbRatingsFilename) || !string.IsNullOrEmpty(AppSettings.IMDbWatchlistFilename) || AppSettings.IMDbCustomLists.Count > 0)
            {
                rdnImdbCSV.Checked = true;
            }
            else if (!string.IsNullOrEmpty(AppSettings.IMDbUsername))
            {
                rdnImdbUsername.Checked = true;
            }
            else
            {
                rdnImdbCSV.Checked = true;
            }

            if (AppSettings.EnableIMDb)
            {
                EnableCsvControls(rdnImdbCSV.Checked);
            }
        }

        private void chkIMDbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.EnableIMDb = chkIMDbEnabled.Checked;
            EnableCsvControls(AppSettings.EnableIMDb);
            if (AppSettings.EnableIMDb)
            {
                EnableCsvControls(rdnImdbCSV.Checked);
            }
        }

        public void EnableCsvControls(bool aIsCsv)
        {
            lblImdbRatingsFile.Enabled = aIsCsv;
            txtImdbRatingsFilename.Enabled = aIsCsv;
            btnImdbRatingsBrowse.Enabled = aIsCsv;

            txtImdbWatchlistFile.Enabled = aIsCsv;
            btnImdbWatchlistBrowse.Enabled = aIsCsv;
            lblImdbWatchlistFile.Enabled = aIsCsv;

            lblImdbCustomLists.Enabled = aIsCsv;
            lsImdbCustomLists.Enabled = aIsCsv;
            btnImdbAddList.Enabled = aIsCsv;
            btnImdbDeleteList.Enabled = aIsCsv;

            txtImdbWebUsername.Enabled = !aIsCsv;
            chkImdbWebWatchlist.Enabled = !aIsCsv;
        }

        public void EnableControls(bool aState)
        {
            lblImdbDescription.Enabled = aState;
            rdnImdbCSV.Enabled = aState;
            rdnImdbUsername.Enabled = aState;
            txtImdbRatingsFilename.Enabled = aState;
            txtImdbWatchlistFile.Enabled = aState;
            btnImdbRatingsBrowse.Enabled = aState;
            btnImdbWatchlistBrowse.Enabled = aState;
            lblImdbRatingsFile.Enabled = aState;
            lblImdbWatchlistFile.Enabled = aState;
            lblImdbCustomLists.Enabled = aState;
            lsImdbCustomLists.Enabled = aState;
            btnImdbAddList.Enabled = aState;
            btnImdbDeleteList.Enabled = aState;
            txtImdbWebUsername.Enabled = aState;
            chkImdbWebWatchlist.Enabled = aState;
            lblImdbNote.Enabled = aState;
        }

        public void SetControlState(bool aEnabled)
        {
            grbImdb.Enabled = aEnabled;
        }

        private void rdnImdbCSV_CheckedChanged(object sender, EventArgs e)
        {
            if (rdnImdbCSV.Checked)
                EnableCsvControls(true);
            else
                EnableCsvControls(false);
        }

        private void chkImdbWebWatchlist_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.IMDbSyncWatchlist = chkImdbWebWatchlist.Checked;
        }

        private void btnImdbRatingsBrowse_Click(object sender, EventArgs e)
        {
            dlgFileOpen.Filter = "CSV files|*.csv";
            DialogResult result = dlgFileOpen.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtImdbRatingsFilename.Text = dlgFileOpen.FileName;
            }
        }

        private void btnImdbWatchlistBrowse_Click(object sender, EventArgs e)
        {
            dlgFileOpen.Filter = "CSV files|*.csv";
            DialogResult result = dlgFileOpen.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtImdbWatchlistFile.Text = dlgFileOpen.FileName;
            }
        }

        private void txtImdbRatingsFilename_TextChanged(object sender, EventArgs e)
        {
            AppSettings.IMDbRatingsFilename = txtImdbRatingsFilename.Text;
        }

        private void txtImdbWatchlistFile_TextChanged(object sender, EventArgs e)
        {
            AppSettings.IMDbWatchlistFilename = txtImdbWatchlistFile.Text;
        }

        private void btnImdbAddList_Click(object sender, EventArgs e)
        {
            dlgFileOpen.Filter = "CSV files|*.csv";
            DialogResult result = dlgFileOpen.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                if (AppSettings.IMDbCustomLists.Contains(dlgFileOpen.FileName))
                    return;

                lsImdbCustomLists.Items.Add(dlgFileOpen.FileName);
                AppSettings.IMDbCustomLists.Add(dlgFileOpen.FileName);
            }
        }

        private void btnImdbDeleteList_Click(object sender, EventArgs e)
        {
            var lSelectedItem = lsImdbCustomLists.SelectedItem;
            if (lSelectedItem == null) return;

            lsImdbCustomLists.Items.Remove(lSelectedItem);

            if (AppSettings.IMDbCustomLists.Contains(lSelectedItem.ToString()))
                AppSettings.IMDbCustomLists.Remove(lSelectedItem.ToString());
        }

        private void txtImdbWebUsername_TextChanged(object sender, EventArgs e)
        {
            AppSettings.IMDbUsername = txtImdbWebUsername.Text;
        }

        public bool IsCsvEnabled
        {
            get
            {
                return rdnImdbCSV.Checked;
            }
        }

        public bool IsWebScraperEnabled
        {
            get
            {
                return rdnImdbUsername.Checked;
            }
        }
    }
}
