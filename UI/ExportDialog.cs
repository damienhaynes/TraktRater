using System;
using System.Windows.Forms;
using TraktRater.Settings;

namespace TraktRater.UI
{
    public partial class ExportDialog : Form
    {
        #region Properties
        public ExportItems ItemsToExport { get; set; }
        public string ExportPath { get; set; }
        #endregion

        #region Constructor
        public ExportDialog(string path, ExportItems items)
        {
            InitializeComponent();

            ExportPath = path;
            ItemsToExport = items;

            #region Set UI Properties
            chkMoviePausedStates.Checked = ItemsToExport.PausedMovies;
            chkEpisodePausedStates.Checked = ItemsToExport.PausedEpisodes;
            chkMovieWatchlist.Checked = ItemsToExport.WatchlistMovies;
            chkSeasonWatchlist.Checked = ItemsToExport.WatchlistSeasons;
            chkShowWatchlist.Checked = ItemsToExport.WatchlistShows;
            chkEpisodeWatchlist.Checked = ItemsToExport.WatchlistEpisodes;
            chkMovieRatings.Checked = ItemsToExport.RatedMovies;
            chkSeasonRatings.Checked = ItemsToExport.RatedSeasons;
            chkShowRatings.Checked = ItemsToExport.RatedShows;
            chkEpisodeRatings.Checked = ItemsToExport.RatedEpisodes;
            chkMovieCollection.Checked = ItemsToExport.CollectedMovies;
            chkEpisodeCollection.Checked = ItemsToExport.CollectedEpisodes;
            chkMovieWatchedHistory.Checked = ItemsToExport.WatchedHistoryMovies;
            chkEpisodeWatchedHistory.Checked = ItemsToExport.WatchedHistoryEpisodes;
            chkCustomLists.Checked = ItemsToExport.CustomLists;
            chkEpisodeComments.Checked = ItemsToExport.CommentedEpisodes;
            chkSeasonComments.Checked = ItemsToExport.CommentedSeasons;
            chkShowComments.Checked = ItemsToExport.CommentedShows;
            chkMovieComments.Checked = ItemsToExport.CommentedMovies;
            chkListComments.Checked = ItemsToExport.CommentedLists;
            chkLikedLists.Checked = ItemsToExport.LikedLists;
            chkLikedComments.Checked = ItemsToExport.LikedComments;

            txtExportPath.Text = ExportPath;
            #endregion
        }
        #endregion

        #region Form Events

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnBrowsePath_Click(object sender, EventArgs e)
        {
            DialogResult result = fbdExportPath.ShowDialog(this);
              
            if (result == DialogResult.OK)
            {
                ExportPath = txtExportPath.Text = fbdExportPath.SelectedPath;
            }
        }
        
        private void lnkSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // iterate through each checkbox in the groupbox and check it
            foreach(var control in grbExport.Controls)
            {
                if (control is CheckBox)
                {
                    ((CheckBox)control).Checked = true;
                }
            }
        }
        
        private void lnkUncheckAll_Click(object sender, EventArgs e)
        {
            // iterate through each checkbox in the groupbox and uncheck it
            foreach (var control in grbExport.Controls)
            {
                if (control is CheckBox)
                {
                    ((CheckBox)control).Checked = false;
                }
            }
        }

        private void chkEpisodeWatchedHistory_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.WatchedHistoryEpisodes = chkEpisodeWatchedHistory.Checked;
        }

        private void chkMovieWatchedHistory_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.WatchedHistoryMovies = chkMovieWatchedHistory.Checked;
        }

        private void chkEpisodeCollection_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.CollectedEpisodes = chkEpisodeCollection.Checked;
        }

        private void chkMovieCollection_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.CollectedMovies = chkMovieCollection.Checked;
        }

        private void chkEpisodeWatchlist_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.WatchlistEpisodes = chkEpisodeWatchlist.Checked;
        }

        private void chkShowWatchlist_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.WatchlistShows = chkShowWatchlist.Checked;
        }

        private void chkSeasonWatchlist_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.WatchlistSeasons = chkSeasonWatchlist.Checked;
        }

        private void chkMovieWatchlist_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.WatchlistMovies = chkMovieWatchlist.Checked;
        }

        private void chkEpisodeRatings_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.RatedEpisodes = chkEpisodeRatings.Checked;
        }

        private void chkShowRatings_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.RatedShows = chkShowRatings.Checked;
        }

        private void chkSeasonRatings_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.RatedSeasons = chkSeasonRatings.Checked;
        }

        private void chkMovieRatings_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.RatedMovies = chkMovieRatings.Checked;
        }

        private void chkEpisodePausedStates_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.PausedEpisodes = chkEpisodePausedStates.Checked;
        }

        private void chkMoviePausedStates_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.PausedMovies = chkMoviePausedStates.Checked;
        }

        private void chkCustomLists_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.CustomLists = chkCustomLists.Checked;
        }

        private void chkEpisodeComments_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.CommentedEpisodes = chkEpisodeComments.Checked;
        }

        private void chkSeasonComments_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.CommentedSeasons = chkSeasonComments.Checked;
        }

        private void chkShowComments_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.CommentedShows = chkShowComments.Checked;
        }

        private void chkMovieComments_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.CommentedMovies = chkMovieComments.Checked;
        }

        private void chkListComments_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.CommentedLists = chkListComments.Checked;
        }

        private void chkLikedComments_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.LikedComments = chkLikedComments.Checked;
        }

        private void chkLikedLists_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.LikedLists = chkLikedLists.Checked;
        }
        #endregion
    }
}
