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

        private void chkEpisodeWatchedHistory_CheckedChanged(object sender, EventArgs e)
        {
            ItemsToExport.WatchedHistoryEpisodes = chkEpisodeWatchedHistory.Checked;
        }

        private void chkMovieWatchedHistory_Click(object sender, EventArgs e)
        {
            ItemsToExport.WatchedHistoryMovies = chkMovieWatchedHistory.Checked;
        }

        private void chkEpisodeCollection_Click(object sender, EventArgs e)
        {
            ItemsToExport.CollectedEpisodes = chkEpisodeCollection.Checked;
        }

        private void chkMovieCollection_Click(object sender, EventArgs e)
        {
            ItemsToExport.CollectedMovies = chkMovieCollection.Checked;
        }

        private void chkEpisodeWatchlist_Click(object sender, EventArgs e)
        {
            ItemsToExport.WatchlistEpisodes = chkEpisodeWatchlist.Checked;
        }

        private void chkShowWatchlist_Click(object sender, EventArgs e)
        {
            ItemsToExport.WatchlistShows = chkShowWatchlist.Checked;
        }

        private void chkSeasonWatchlist_Click(object sender, EventArgs e)
        {
            ItemsToExport.WatchlistSeasons = chkSeasonWatchlist.Checked;
        }

        private void chkMovieWatchlist_Click(object sender, EventArgs e)
        {
            ItemsToExport.WatchlistMovies = chkMovieWatchlist.Checked;
        }

        private void chkEpisodeRatings_Click(object sender, EventArgs e)
        {
            ItemsToExport.RatedEpisodes = chkEpisodeRatings.Checked;
        }

        private void chkShowRatings_Click(object sender, EventArgs e)
        {
            ItemsToExport.RatedShows = chkShowRatings.Checked;
        }

        private void chkSeasonRatings_Click(object sender, EventArgs e)
        {
            ItemsToExport.RatedSeasons = chkSeasonRatings.Checked;
        }

        private void chkMovieRatings_Click(object sender, EventArgs e)
        {
            ItemsToExport.RatedMovies = chkMovieRatings.Checked;
        }

        private void chkEpisodePausedStates_Click(object sender, EventArgs e)
        {
            ItemsToExport.PausedEpisodes = chkEpisodePausedStates.Checked;
        }

        private void chkMoviePausedStates_Click(object sender, EventArgs e)
        {
            ItemsToExport.PausedMovies = chkMoviePausedStates.Checked;
        }
        #endregion
    }
}
