namespace TraktRater.UI
{
    using System;
    using System.Windows.Forms;

    using global::TraktRater.Settings;

    public partial class MaintenanceDialog : Form
    {
        #region Properties
        public MaintenanceSettings Settings = null;
        #endregion

        #region Constructor
        public MaintenanceDialog()
        {
            InitializeComponent();
            Settings = new MaintenanceSettings();
        }
        #endregion

        #region Form Events
        private void btnStart_Click(object sender, EventArgs e)
        {
            string warning = "Are you really sure you want to remove the selected items from your trakt.tv account?";

            // give user a warning about what they about to do!
            if (MessageBox.Show(warning, "Maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void chkEpisodeWatchedHistory_Click(object sender, EventArgs e)
        {
            Settings.WatchedHistoryEpisodes = chkEpisodeWatchedHistory.Checked;
        }

        private void chkMovieWatchedHistory_Click(object sender, EventArgs e)
        {
            Settings.WatchedHistoryMovies = chkMovieWatchedHistory.Checked;
        }

        private void chkEpisodeCollection_Click(object sender, EventArgs e)
        {
            Settings.CollectedEpisodes = chkEpisodeCollection.Checked;
        }

        private void chkMovieCollection_Click(object sender, EventArgs e)
        {
            Settings.CollectedMovies = chkMovieCollection.Checked;
        }

        private void chkEpisodeWatchlist_Click(object sender, EventArgs e)
        {
            Settings.WatchlistEpisodes = chkEpisodeWatchlist.Checked;
        }

        private void chkShowWatchlist_Click(object sender, EventArgs e)
        {
            Settings.WatchlistShows = chkShowWatchlist.Checked;
        }

        private void chkSeasonWatchlist_Click(object sender, EventArgs e)
        {
            Settings.WatchlistSeasons = chkSeasonWatchlist.Checked;
        }

        private void chkMovieWatchlist_Click(object sender, EventArgs e)
        {
            Settings.WatchlistMovies = chkMovieWatchlist.Checked;
        }

        private void chkEpisodeRatings_Click(object sender, EventArgs e)
        {
            Settings.RatedEpisodes = chkEpisodeRatings.Checked;
        }

        private void chkShowRatings_Click(object sender, EventArgs e)
        {
            Settings.RatedShows = chkShowRatings.Checked;
        }

        private void chkSeasonRatings_Click(object sender, EventArgs e)
        {
            Settings.RatedSeasons = chkSeasonRatings.Checked;
        }

        private void chkMovieRatings_Click(object sender, EventArgs e)
        {
            Settings.RatedMovies = chkMovieRatings.Checked;
        }

        private void chkEpisodeResumeTimes_Click(object sender, EventArgs e)
        {
            Settings.PausedEpisodes = chkEpisodePausedStates.Checked;
        }

        private void chkMovieResumeTimes_Click(object sender, EventArgs e)
        {
            Settings.PausedMovies = chkMoviePausedStates.Checked;
        }

        private void chkCustomLists_Click(object sender, EventArgs e)
        {
            Settings.CustomLists = chkCustomLists.Checked;
        }
        #endregion

    }
}
