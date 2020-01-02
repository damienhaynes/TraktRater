namespace TraktRater
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;

    using global::TraktRater.Logger;
    using global::TraktRater.Settings;
    using global::TraktRater.Sites;
    using global::TraktRater.UI;
    
    public partial class TraktRater : Form
    {
        #region UI Invoke Delegates
        delegate void SetControlStateDelegate(bool enable);
        delegate void SetTMDbControlStateDelegate();
        delegate void ClearProgressDelegate();
        #endregion

        #region Variables

        readonly List<IRateSite> sites = new List<IRateSite>();
        static bool maintenanceRunning = false;
        static bool exportRunning = false;
        static bool importRunning = false;
        static bool importCancelled = false;
        static string pinCode = string.Empty;
        #endregion

        #region Constants
        const string cImportReadyText = "Start Import";
        const string cCancelText = "Cancel";
        const string cTraktAuthorise = "Click to authorise access to your account";
        const string cTraktUnAuthorise = "Click to remove current access token";
        const string cTraktPinCodeWaterMark = "Authorise and then enter pin code here...";
        #endregion

        #region Constructor
        public TraktRater()
        {
			FileLog.LogFileName = DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".log";
            InitializeComponent();
        }
        #endregion

        #region Form Overrides
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "TraktRater v" + Assembly.GetEntryAssembly().GetName().Version;
            AppSettings.Load();
            ClearProgress();

            // populate fields
            HideShowTraktAuthControls();
            
            txtTVDbAccountId.Text = AppSettings.TVDbAccountIdentifier;
            chkTMDbSyncWatchlist.Checked = AppSettings.TMDbSyncWatchlist;
            txtImdbRatingsFilename.Text = AppSettings.IMDbRatingsFilename;
            txtImdbWatchlistFile.Text = AppSettings.IMDbWatchlistFilename;
            txtImdbWebUsername.Text = AppSettings.IMDbUsername;
            chkImdbWebWatchlist.Checked = AppSettings.IMDbSyncWatchlist;
            lsImdbCustomLists.Items.AddRange(AppSettings.IMDbCustomLists.ToArray());
            chkListalWebWatchlist.Checked = AppSettings.ListalSyncWatchlist;
            txtListalMovieXMLExport.Text = AppSettings.ListalMovieFilename;
            txtListalShowXMLExport.Text = AppSettings.ListalShowFilename;
            txtCritickerCSVExportFile.Text = AppSettings.CritickerCSVFilename;
            txtToDoMovieExportFile.Text = AppSettings.ToDoMovieFilename;
            txtLetterboxdWatchedFile.Text = AppSettings.LetterboxdWatchedFilename;
            txtLetterboxdRatingsFile.Text = AppSettings.LetterboxdRatingsFilename;
            txtLetterboxdDiaryFile.Text = AppSettings.LetterboxdDiaryFilename;
            listLetterboxdCustomLists.Items.AddRange(AppSettings.LetterboxdCustomLists.ToArray() );
            txtFlixsterUserId.Text = AppSettings.FlixsterUserId;
            txtCheckMoviesCsvFile.Text = AppSettings.CheckMoviesFilename;
            chkFlixsterSyncWantToSee.Checked = AppSettings.FlixsterSyncWantToSee;
            cboCheckMoviesDelimiter.SelectedIndex = AppSettings.CheckMoviesDelimiter;
            chkMarkAsWatched.Checked = AppSettings.MarkAsWatched;
            chkIgnoreWatchedForWatchlists.Checked = AppSettings.IgnoreWatchedForWatchlist;
            chkTVDbEnabled.Checked = AppSettings.EnableTVDb;
            chkTMDbEnabled.Checked = AppSettings.EnableTMDb;
            chkIMDbEnabled.Checked = AppSettings.EnableIMDb;
            chkCheckMoviesEnabled.Checked = AppSettings.EnableCheckMovies;
            chkListalEnabled.Checked = AppSettings.EnableListal;
            chkCritickerEnabled.Checked = AppSettings.EnableCriticker;
            chkToDoMoviesEnabled.Checked = AppSettings.EnableToDoMovies;
            chkLetterboxdEnabled.Checked = AppSettings.EnableLetterboxd;
            chkFlixsterEnabled.Checked = AppSettings.EnableFlixster;
            chkCheckMoviesAddWatchedToWatchlist.Checked = AppSettings.CheckMoviesAddWatchedMoviesToWatchlist;
            chkCheckMoviesUpdateWatchedStatus.Checked = AppSettings.CheckMoviesUpdateWatchedHistory;
            chkCheckMoviesAddMoviesToCollection.Checked = AppSettings.CheckMoviesAddToCollection;
            chkSetWatchedOnReleaseDay.Checked = AppSettings.WatchedOnReleaseDay;
            nudBatchSize.Value = AppSettings.BatchSize;

            SetTMDbControlState();

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

            EnableExternalSourceControlsInGroupBoxes();

            if (AppSettings.EnableIMDb)
            {
                EnableImdbCSVControls(rdnImdbCSV.Checked);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            CancelImport();
            AppSettings.Save();
            base.OnClosing(e);
        }
        #endregion

        #region UI Events

        private void pbPaypal_Click( object sender, EventArgs e )
        {
            Process.Start( @"https://www.paypal.me/damienlhaynes" );
        }

        private void lnkPaypal_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            Process.Start( @"https://www.paypal.me/damienlhaynes" );
        }

        private void lnkTraktOAuth_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // if we have already authorised, un-register so can sign-in and authorise again
            if (!string.IsNullOrEmpty(AppSettings.TraktOAuthToken))
            {
                AppSettings.TraktOAuthToken = null;

                lnkTraktOAuth.Text = cTraktAuthorise;
                txtTraktPinCode.Visible = true;
                txtTraktPinCode.Text = cTraktPinCodeWaterMark;
                txtTraktPinCode.ForeColor = SystemColors.GrayText;
                pinCode = string.Empty;

                lblWarnPeriod.Visible = false;
            }
            else
            {
                // sign-in to authorise
                Process.Start(string.Format(TraktAPI.TraktURIs.PinUrl, TraktAPI.TraktAPI.AppId));

                lblWarnPeriod.Visible = true;
            }
        }
        
        private void txtTraktPinCode_Click(object sender, EventArgs e)
        {
            if (txtTraktPinCode.Text == cTraktPinCodeWaterMark)
            {
                txtTraktPinCode.Text = string.Empty;
                txtTraktPinCode.ForeColor = SystemColors.WindowText;
                pinCode = string.Empty;
            }
        }

        private void txtTraktPinCode_TextChanged(object sender, EventArgs e)
        {
            if (txtTraktPinCode.Text != cTraktPinCodeWaterMark)
            {
                pinCode = txtTraktPinCode.Text;
                txtTraktPinCode.ForeColor = SystemColors.WindowText;
            }
        }

        private void chkMarkAsWatched_Click(object sender, EventArgs e)
        {
            AppSettings.MarkAsWatched = !AppSettings.MarkAsWatched;
        }

        private void btnStartSync_Click(object sender, EventArgs e)
        {
            if (importRunning)
            {
                CancelImport();
            }
            else if (maintenanceRunning)
            {
                CancelMaintenance();
            }
            else
            {
                StartImport();
            }
        }

        private void txtTVDbAccountId_TextChanged(object sender, EventArgs e)
        {
            AppSettings.TVDbAccountIdentifier = txtTVDbAccountId.Text;
        }
        
        private void chkImdbWatchlist_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.IMDbSyncWatchlist = chkImdbWebWatchlist.Checked;
        }

        private void chkIgnoreWatchedForWatchlists_Click(object sender, EventArgs e)
        {
            AppSettings.IgnoreWatchedForWatchlist = chkIgnoreWatchedForWatchlists.Checked;
        }

        private void rdnImdbCSV_CheckedChanged(object sender, EventArgs e)
        {
            if (rdnImdbCSV.Checked)
                EnableImdbCSVControls(true);
            else
                EnableImdbCSVControls(false);
        }

        private void btnImdbBrowse_Click(object sender, EventArgs e)
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

        private void btnListalMovieXMLExport_Click(object sender, EventArgs e)
        {
            dlgFileOpen.Filter = "XML files|*.xml";
            DialogResult result = dlgFileOpen.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtListalMovieXMLExport.Text = dlgFileOpen.FileName;
            }
        }

        private void btnListalShowXMLExport_Click(object sender, EventArgs e)
        {
            dlgFileOpen.Filter = "XML files|*.xml";
            DialogResult result = dlgFileOpen.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtListalShowXMLExport.Text = dlgFileOpen.FileName;
            }
        }

        private void btnCritickerCSVExportBrowse_Click(object sender, EventArgs e)
        {
            dlgFileOpen.Filter = "CSV files|*.csv";
            DialogResult result = dlgFileOpen.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtCritickerCSVExportFile.Text = dlgFileOpen.FileName;
            }
        }

        private void txtImdbFilename_TextChanged(object sender, EventArgs e)
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

        private void txtImdbUsername_TextChanged(object sender, EventArgs e)
        {
            AppSettings.IMDbUsername = txtImdbWebUsername.Text;
        }

        private void txtListalMovieXMLExport_TextChanged(object sender, EventArgs e)
        {
            AppSettings.ListalMovieFilename = txtListalMovieXMLExport.Text;
        }

        private void txtListalShowXMLExport_TextChanged(object sender, EventArgs e)
        {
            AppSettings.ListalShowFilename = txtListalShowXMLExport.Text;
        }

        private void txtCritickerMovieExportFile_TextChanged(object sender, EventArgs e)
        {
            AppSettings.CritickerCSVFilename = txtCritickerCSVExportFile.Text;
        }

        private void chkListalWebWatchlist_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.ListalSyncWatchlist = chkListalWebWatchlist.Checked;
        }

        private void lnkTMDbStart_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AppSettings.TMDbRequestToken) || !string.IsNullOrEmpty(AppSettings.TMDbSessionId))
            {
                AppSettings.TMDbRequestToken = string.Empty;
                AppSettings.TMDbSessionId = string.Empty;
                SetTMDbControlState();
                return;
            }

            Thread tokenThread = new Thread(o =>
                {
                    // store token and parse into tmdb object later
                    // for request session id
                    string requestToken = TMDb.RequestToken();

                    if (!string.IsNullOrEmpty(requestToken))
                    {
                        TMDb.RequestAuthorization(requestToken);
                        AppSettings.TMDbRequestToken = requestToken;
                        SetTMDbControlState();
                    }
                });

            tokenThread.Start();
        }

        private void lnkListalExport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.listal.com/user/export");
        }

        private void chkTVDbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.EnableTVDb = chkTVDbEnabled.Checked;
            EnableTvdbControls(AppSettings.EnableTVDb);
        }

        private void chkIMDbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.EnableIMDb = chkIMDbEnabled.Checked;
            EnableImdbControls(AppSettings.EnableIMDb);
            if (AppSettings.EnableIMDb)
            {
                EnableImdbCSVControls(rdnImdbCSV.Checked);
            }
        }

        private void chkTMDbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.EnableTMDb = chkTMDbEnabled.Checked;
            EnableTmdbControls(AppSettings.EnableTMDb);
        }

        private void chkTMDbSyncWatchlist_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.TMDbSyncWatchlist = chkTMDbSyncWatchlist.Checked;
        }

        private void chkListalEnabled_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.EnableListal = chkListalEnabled.Checked;
            EnableListalControls(AppSettings.EnableListal);
        }

        private void chkCritickerEnabled_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.EnableCriticker = chkCritickerEnabled.Checked;
            EnableCritickerControls(AppSettings.EnableCriticker);
        }

        private void btnLetterboxdRatingsBrowse_Click(object sender, EventArgs e)
        {
            dlgFileOpen.Filter = "CSV files|*.csv";
            DialogResult result = dlgFileOpen.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtLetterboxdRatingsFile.Text = dlgFileOpen.FileName;
            }
        }

        private void btnLetterboxdWatchedBrowse_Click(object sender, EventArgs e)
        {
            dlgFileOpen.Filter = "CSV files|*.csv";
            DialogResult result = dlgFileOpen.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtLetterboxdWatchedFile.Text = dlgFileOpen.FileName;
            }
        }

        private void btnLetterboxdDiaryBrowse_Click(object sender, EventArgs e)
        {
            dlgFileOpen.Filter = "CSV files|*.csv";
            DialogResult result = dlgFileOpen.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtLetterboxdDiaryFile.Text = dlgFileOpen.FileName;
            }
        }

        private void txtLetterboxdRatingsFile_TextChanged(object sender, EventArgs e)
        {
            AppSettings.LetterboxdRatingsFilename = txtLetterboxdRatingsFile.Text;
        }

        private void txtLetterboxdWatchedFile_TextChanged(object sender, EventArgs e)
        {
            AppSettings.LetterboxdWatchedFilename = txtLetterboxdWatchedFile.Text;
        }

        private void txtLetterboxdDiaryFile_TextChanged(object sender, EventArgs e)
        {
            AppSettings.LetterboxdDiaryFilename = txtLetterboxdDiaryFile.Text;
        }

        private void chkLetterboxdEnabled_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.EnableLetterboxd = chkLetterboxdEnabled.Checked;
            EnableLetterboxdControls(AppSettings.EnableLetterboxd);
        }

        private void btnLetterBoxAddList_Click( object sender, EventArgs e )
        {
            dlgFileOpen.Filter = "CSV files|*.csv";
            DialogResult result = dlgFileOpen.ShowDialog( this );
            if ( result == DialogResult.OK )
            {
                if ( AppSettings.LetterboxdCustomLists.Contains( dlgFileOpen.FileName ) )
                    return;

                listLetterboxdCustomLists.Items.Add( dlgFileOpen.FileName );
                AppSettings.LetterboxdCustomLists.Add( dlgFileOpen.FileName );
            }
        }

        private void btnLetterBoxRemoveList_Click( object sender, EventArgs e )
        {
            var lSelectedItem = listLetterboxdCustomLists.SelectedItem;
            if ( lSelectedItem == null ) return;

            listLetterboxdCustomLists.Items.Remove( lSelectedItem );

            if ( AppSettings.LetterboxdCustomLists.Contains( lSelectedItem.ToString() ) )
                AppSettings.LetterboxdCustomLists.Remove( lSelectedItem.ToString() );
        }

        private void chkFlixsterEnabled_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.EnableFlixster = chkFlixsterEnabled.Checked;
            EnableFlixsterControls(AppSettings.EnableFlixster);
        }

        private void txtFlixsterUserId_TextChanged(object sender, EventArgs e)
        {
            AppSettings.FlixsterUserId = txtFlixsterUserId.Text;
        }

        private void chkFlixsterSyncWantToSee_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.FlixsterSyncWantToSee = chkFlixsterSyncWantToSee.Checked;            
        }

        private void chkCheckMoviesEnabled_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.EnableCheckMovies = chkCheckMoviesEnabled.Checked;
            EnableCheckMoviesControls(AppSettings.EnableCheckMovies);
        }

        private void btnCheckMoviesBrowse_Click(object sender, EventArgs e)
        {
            dlgFileOpen.Filter = "CSV files|*.csv";
            DialogResult result = dlgFileOpen.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtCheckMoviesCsvFile.Text = dlgFileOpen.FileName;
            }
        }
        
        private void txtCheckMoviesCsvFile_TextChanged(object sender, EventArgs e)
        {
            AppSettings.CheckMoviesFilename = txtCheckMoviesCsvFile.Text;
        }

        private void chkCheckMoviesAddWatchedToWatchlist_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.CheckMoviesAddWatchedMoviesToWatchlist = chkCheckMoviesAddWatchedToWatchlist.Checked;
        }

        private void chkCheckMoviesUpdateWatchedStatus_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.CheckMoviesUpdateWatchedHistory = chkCheckMoviesUpdateWatchedStatus.Checked;
        }
        
        private void cboCheckMoviesDelimiter_SelectedIndexChanged(object sender, EventArgs e)
        {
            AppSettings.CheckMoviesDelimiter = cboCheckMoviesDelimiter.SelectedIndex;
        }

        private void chkCheckMoviesAddMoviesToCollection_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.CheckMoviesAddToCollection = chkCheckMoviesAddMoviesToCollection.Checked;
        }

        private void chkToDoMoviesEnabled_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.EnableToDoMovies = chkToDoMoviesEnabled.Checked;
            EnableToDoMoviesControls(AppSettings.EnableToDoMovies);
        }

        private void txtToDoMovieExportFile_TextChanged(object sender, EventArgs e)
        {
            AppSettings.ToDoMovieFilename = txtToDoMovieExportFile.Text;
        }

        private void btnToDoMoviesExportBrowse_Click(object sender, EventArgs e)
        {
            dlgFileOpen.Filter = "csv files|*.csv;*.txt";
            DialogResult result = dlgFileOpen.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtToDoMovieExportFile.Text = dlgFileOpen.FileName;
            }
        }

        private void lnkLogFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(FileLog.LogDirectory);
        }

        private void chkSetWatchedOnReleaseDay_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.WatchedOnReleaseDay = chkSetWatchedOnReleaseDay.Checked;
        }

        private void nudBatchSize_ValueChanged(object sender, EventArgs e)
        {
            AppSettings.BatchSize = (int)nudBatchSize.Value;
        }

        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            var maintenanceDlg = new MaintenanceDialog();
            DialogResult result = maintenanceDlg.ShowDialog(this);

            if (result != DialogResult.OK)
                return;

            // perform maintenance for user
            StartMaintenance(maintenanceDlg.Settings);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var exportDlg = new ExportDialog(AppSettings.CsvExportPath, AppSettings.CsvExportItems);

            DialogResult result = exportDlg.ShowDialog(this);

            if (result != DialogResult.OK)
                return;

            // save settings
            AppSettings.CsvExportPath = exportDlg.ExportPath;
            AppSettings.CsvExportItems = exportDlg.ItemsToExport;

            // perform export for user
            StartExport(exportDlg.ItemsToExport);
        }
        #endregion

        #region Import Actions
        private void StartImport()
        {
            if (!CheckAccountDetails() || importRunning)
                return;

            // update file log with new name
            FileLog.LogFileName = DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".log";

            sites.Clear();

            // add import sites for processing
            if (AppSettings.EnableTMDb)             sites.Add(new TMDb(AppSettings.TMDbRequestToken, AppSettings.TMDbSessionId));
            if (AppSettings.EnableTVDb)             sites.Add(new TVDb(AppSettings.TVDbAccountIdentifier));
            if (AppSettings.EnableIMDb)             sites.Add(new IMDb(AppSettings.IMDbRatingsFilename, AppSettings.IMDbWatchlistFilename, AppSettings.IMDbCustomLists, rdnImdbCSV.Checked));
            if (AppSettings.EnableIMDb)             sites.Add(new IMDbWeb(AppSettings.IMDbUsername, rdnImdbUsername.Checked));
            if (AppSettings.EnableListal)           sites.Add(new Listal(AppSettings.ListalMovieFilename, AppSettings.ListalShowFilename, AppSettings.ListalSyncWatchlist));
            if (AppSettings.EnableCriticker)        sites.Add(new Criticker(AppSettings.CritickerCSVFilename));
            if (AppSettings.EnableLetterboxd)       sites.Add(new Letterboxd(AppSettings.LetterboxdRatingsFilename, AppSettings.LetterboxdWatchedFilename, AppSettings.LetterboxdDiaryFilename, AppSettings.LetterboxdCustomLists) );
            if (AppSettings.EnableFlixster)         sites.Add(new Flixster(AppSettings.FlixsterUserId, AppSettings.FlixsterSyncWantToSee));
            if (AppSettings.EnableCheckMovies)      sites.Add(new CheckMovies(AppSettings.CheckMoviesFilename, AppSettings.CheckMoviesDelimiter));
            if (AppSettings.EnableToDoMovies)       sites.Add(new ToDoMovies(AppSettings.ToDoMovieFilename));

            if (!sites.Any(s => s.Enabled))
            {
                UIUtils.UpdateStatus("No sites enabled or incorrect site information supplied!", true);
                return;
            }

            #region Import
            var importThread = new Thread(o =>
            {
                importRunning = true;

                // only one import at a time
                SetControlState(false);

                // Clear Progress
                ClearProgress();

                // Login to trakt.tv
                if (!Login())
                    return;

                // import ratings
                foreach (var site in sites.Where(s => s.Enabled))
                {
                    UIUtils.UpdateStatus(string.Format("Starting import from {0}", site.Name));
                    try
                    {   
                        if (!importCancelled)
                            site.ImportRatings();
                    }
                    catch (Exception e)
                    {
                        UIUtils.UpdateStatus(string.Format("{0}:{1}", site.Name, e.Message), true);
                        Thread.Sleep(5000);
                    }
                    UIUtils.UpdateStatus("Finished import from {0}", site.Name);
                }

                // finished
                SetControlState(true);
                UIUtils.UpdateStatus("Import Complete!");
                importRunning = false;
                importCancelled = false;
            });

            importThread.Start();
            #endregion
        }

        private void CancelImport()
        {
            if (!importRunning) return;

            UIUtils.UpdateStatus("Cancelling Import...");

            importCancelled = true;

            Thread cancelThread = new Thread(o =>
            {
                // cancel import
                foreach (var site in sites.Where(s => s.Enabled))
                {
                    site.Cancel();
                }
            });

            cancelThread.Start();
        }
        #endregion

        #region Maintenance Actions
        private void StartMaintenance(MaintenanceSettings settings)
        {
            if (!CheckAccountDetails() || maintenanceRunning)
                return;

            // update file log with new name
            FileLog.LogFileName = DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".log";

            var maintThread = new Thread(o =>
            {
                maintenanceRunning = true;
                Maintenance.Cancel = false;

                // only one import at a time
                SetControlState(false);

                // Clear Progress
                ClearProgress();

                // Login to trakt.tv
                if (!Login())
                    return;

                // Cleanup user data from trakt.tv
                if (settings.WatchedHistoryEpisodes)
                {
                    Maintenance.RemoveEpisodesFromWatchedHistory();
                }
                if (settings.WatchedHistoryMovies)
                {
                    Maintenance.RemoveMoviesFromWatchedHistory();
                }
                if (settings.CollectedEpisodes)
                {
                    Maintenance.RemoveEpisodesFromCollection();
                }
                if (settings.CollectedMovies)
                {
                    Maintenance.RemoveMoviesFromCollection();
                }
                if (settings.RatedEpisodes)
                {
                    Maintenance.RemoveEpisodesFromRatings();
                }
                if (settings.RatedShows)
                {
                    Maintenance.RemoveShowsFromRatings();
                }
                if (settings.RatedSeasons)
                {
                    Maintenance.RemoveSeasonsFromRatings();
                }
                if (settings.RatedMovies)
                {
                    Maintenance.RemoveMoviesFromRatings();
                }
                if (settings.WatchlistEpisodes)
                {
                    Maintenance.RemoveEpisodesFromWatchlist();
                }
                if (settings.WatchlistShows)
                {
                    Maintenance.RemoveShowsFromWatchlist();
                }
                if (settings.WatchlistSeasons)
                {
                    Maintenance.RemoveSeasonsFromWatchlist();
                }
                if (settings.WatchlistMovies)
                {
                    Maintenance.RemoveMoviesFromWatchlist();
                }
                if (settings.PausedEpisodes)
                {
                    Maintenance.RemoveEpisodePausedState();
                }
                if (settings.PausedMovies)
                {
                    Maintenance.RemoveMoviePausedState();
                }
                if (settings.CustomLists)
                {
                    Maintenance.RemoveCustomLists();
                }

                // finished
                SetControlState(true);
                UIUtils.UpdateStatus("Maintenance Complete!");
                maintenanceRunning = false;
            });

            maintThread.Start();
        }

        private void CancelMaintenance()
        {
            if (!maintenanceRunning) return;

            UIUtils.UpdateStatus("Cancelling Maintenance...");
            Maintenance.Cancel = true;
        }
        #endregion

        #region Export to CSV
        private void StartExport(ExportItems items)
        {
            if (!CheckAccountDetails() || exportRunning)
                return;

            // update file log with new name
            FileLog.LogFileName = DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".log";

            var mainThread = new Thread(o =>
            {
                exportRunning = true;
                Export.Cancel = false;

                // only one import/export at a time
                SetControlState(false);

                // Clear Progress
                ClearProgress();

                // Login to trakt.tv
                if (!Login())
                    return;

                // export user data from trakt.tv
                if (AppSettings.CsvExportItems.WatchedHistoryEpisodes)
                {
                    Export.CreateWatchedEpisodeCsv();
                }
                if (AppSettings.CsvExportItems.WatchedHistoryMovies)
                {
                    Export.CreateWatchedMoviesCsv();
                }
                if (AppSettings.CsvExportItems.CollectedEpisodes)
                {
                    Export.CreateCollectedEpisodeCsv();
                }
                if (AppSettings.CsvExportItems.CollectedMovies)
                {
                    Export.CreateCollectedMoviesCsv();
                }
                if (AppSettings.CsvExportItems.RatedEpisodes)
                {
                    Export.CreateRatedEpisodeCsv();
                }
                if (AppSettings.CsvExportItems.RatedSeasons)
                {
                    Export.CreateRatedSeasonsCsv();
                }
                if (AppSettings.CsvExportItems.RatedShows)
                {
                    Export.CreateRatedShowsCsv();
                }
                if (AppSettings.CsvExportItems.RatedMovies)
                {
                    Export.CreateRatedMoviesCsv();
                }
                if (AppSettings.CsvExportItems.WatchlistEpisodes)
                {
                    Export.CreateWatchlistEpisodesCsv();
                }
                if (AppSettings.CsvExportItems.WatchlistSeasons)
                {
                    Export.CreateWatchlistSeasonsCsv();
                }
                if (AppSettings.CsvExportItems.WatchlistShows)
                {
                    Export.CreateWatchlistShowsCsv();
                }
                if (AppSettings.CsvExportItems.WatchlistMovies)
                {
                    Export.CreateWatchlistMoviesCsv();
                }
                if (AppSettings.CsvExportItems.PausedEpisodes)
                {
                    Export.CreatePausedEpisodesCsv();
                }
                if (AppSettings.CsvExportItems.PausedMovies)
                {
                    Export.CreatePausedMoviesCsv();
                }
                if (AppSettings.CsvExportItems.CustomLists)
                {
                    Export.CreateCustomListsCsv();
                }
                if (AppSettings.CsvExportItems.CommentedEpisodes)
                {
                    Export.CreateCommentedEpisodesCsv();
                }
                if (AppSettings.CsvExportItems.CommentedSeasons)
                {
                    Export.CreateCommentedSeasonsCsv();
                }
                if (AppSettings.CsvExportItems.CommentedShows)
                {
                    Export.CreateCommentedShowsCsv();
                }
                if (AppSettings.CsvExportItems.CommentedMovies)
                {
                    Export.CreateCommentedMoviesCsv();
                }
                if (AppSettings.CsvExportItems.CommentedLists)
                {
                    Export.CreateCommentedListsCsv();
                }
                if (AppSettings.CsvExportItems.LikedComments)
                {
                    Export.CreateLikedCommentsCsv();
                }
                if (AppSettings.CsvExportItems.LikedLists)
                {
                    Export.CreateLikedListsCsv();
                }

                // finished
                SetControlState(true);
                UIUtils.UpdateStatus("Export Complete!");
                exportRunning = false;

                // open folder to exported files
                try
                {
                    Process.Start("explorer.exe", AppSettings.CsvExportPath);
                }
                catch
                {
                    UIUtils.UpdateStatus("Failed to open folder to exported files", true);
                }

            });

            mainThread.Start();
        }

        private void CancelExport()
        {
            if (!exportRunning) return;

            UIUtils.UpdateStatus("Cancelling CSV Export...");
            Export.Cancel = true;
        }
        #endregion

        #region Private Methods

        private bool Login()
        {
            // exchange pin-code for access token or refresh existing token
            UIUtils.UpdateStatus("Exchanging {0} for access-token...", pinCode.Length == 8 ? "pin-code" : "refresh-token");
            var response = TraktAPI.TraktAPI.GetOAuthToken(pinCode.Length == 8 ? pinCode : AppSettings.TraktOAuthToken);
            if (response == null || string.IsNullOrEmpty(response.AccessToken))
            {
                UIUtils.UpdateStatus("Unable to login to trakt, check log for details", true);
                SetControlState(true);
                importRunning = false;
                importCancelled = false;
                maintenanceRunning = false;
                pinCode = string.Empty;
                return false;
            }

            // save the refresh-token for next time
            AppSettings.TraktOAuthToken = response.RefreshToken;
            pinCode = string.Empty;

            return true;
        }

        private bool CheckAccountDetails()
        {
            if (string.IsNullOrEmpty(AppSettings.TraktOAuthToken))
            {
                if (string.IsNullOrEmpty(pinCode) || pinCode.Length != 8)
                {
                    UIUtils.UpdateStatus("You must authorise TraktRater to access your trakt.tv account and enter the 8 character pin code with-in 15 minutes of starting an import", true);
                    return false;
                }
            }
            return true;
        }

        private void HideShowTraktAuthControls()
        {
            // if we have a access token then allow user un-register
            lnkTraktOAuth.Text = string.IsNullOrEmpty(AppSettings.TraktOAuthToken) ? cTraktAuthorise : cTraktUnAuthorise;

            // show pin code text box if we have not authorised yet
            txtTraktPinCode.Text = cTraktPinCodeWaterMark;
            txtTraktPinCode.ForeColor = SystemColors.GrayText;
            txtTraktPinCode.Visible = string.IsNullOrEmpty(AppSettings.TraktOAuthToken);

            // only show 15min warning when pin code is entered
            lblWarnPeriod.Visible = false;
        }

        private void SetControlState(bool enable)
        {
            if (InvokeRequired)
            {
                SetControlStateDelegate setControlState = SetControlState;
                Invoke(setControlState, enable);
                return;
            }

            grbOptions.Enabled = enable;
            grbTrakt.Enabled = enable;

            grbImdb.Enabled = enable;
            grbTMDb.Enabled = enable;
            grbTVDb.Enabled = enable;
            grbListal.Enabled = enable;
            grbCriticker.Enabled = enable;
            grbLetterboxd.Enabled = enable;
            grbFlixster.Enabled = enable;
            grbCheckMovies.Enabled = enable;
            grbToDoMovies.Enabled = enable;

            HideShowTraktAuthControls();

            btnStartSync.Text = enable ? cImportReadyText : cCancelText;
            pbrImportProgress.Style = enable ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;
        }

        private void ClearProgress()
        {
            if (InvokeRequired)
            {
                ClearProgressDelegate clearProgress = ClearProgress;
                Invoke(clearProgress);
                return;
            }
            
            lblStatusMessage.Text = "Ready for anything!";
            lblStatusMessage.ForeColor = Color.Black;
        }

        private void SetTMDbControlState()
        {
            if (InvokeRequired)
            {
                SetTMDbControlStateDelegate setTMDbState = SetTMDbControlState;
                Invoke(setTMDbState);
                return;
            }

            // we are either ready to get session id or we already have it
            if (!string.IsNullOrEmpty(AppSettings.TMDbRequestToken) || !string.IsNullOrEmpty(AppSettings.TMDbSessionId))
            {
                lblTMDbMessage.Text = "Request Token and/or Session Id has already been found.";
                lnkTMDbStart.Text = "Disable TMDb Support";
            }
            else
            {
                lblTMDbMessage.Text = "To get user ratings from TMDb you must first allow this application to access your account details. This needs to be done by you in a webbrowser.";
                lnkTMDbStart.Text = "Start Request Process";
            }
        }

        private void EnableTvdbControls(bool enableState)
        {
            lblTVDbAccountId.Enabled = enableState;
            txtTVDbAccountId.Enabled = enableState;
        }

        private void EnableImdbControls(bool enableState)
        {
            lblImdbDescription.Enabled = enableState;
            rdnImdbCSV.Enabled = enableState;
            rdnImdbUsername.Enabled = enableState;
            txtImdbRatingsFilename.Enabled = enableState;
            txtImdbWatchlistFile.Enabled = enableState;
            btnImdbRatingsBrowse.Enabled = enableState;
            btnImdbWatchlistBrowse.Enabled = enableState;
            lblImdbRatingsFile.Enabled = enableState;
            lblImdbWatchlistFile.Enabled = enableState;
            lblImdbCustomLists.Enabled = enableState;
            lsImdbCustomLists.Enabled = enableState;
            btnImdbAddList.Enabled = enableState;
            btnImdbDeleteList.Enabled = enableState;
            txtImdbWebUsername.Enabled = enableState;
            chkImdbWebWatchlist.Enabled = enableState;
            lblImdbNote.Enabled = enableState;
        }

        private void EnableImdbCSVControls(bool isCSV)
        {
            lblImdbRatingsFile.Enabled = isCSV;
            txtImdbRatingsFilename.Enabled = isCSV;
            btnImdbRatingsBrowse.Enabled = isCSV;

            txtImdbWatchlistFile.Enabled = isCSV;
            btnImdbWatchlistBrowse.Enabled = isCSV;
            lblImdbWatchlistFile.Enabled = isCSV;

            lblImdbCustomLists.Enabled = isCSV;
            lsImdbCustomLists.Enabled = isCSV;
            btnImdbAddList.Enabled = isCSV;
            btnImdbDeleteList.Enabled = isCSV;

            txtImdbWebUsername.Enabled = !isCSV;
            chkImdbWebWatchlist.Enabled = !isCSV;
        }

        private void EnableTmdbControls(bool enableState)
        {
            lblTMDbMessage.Enabled = enableState;
            lnkTMDbStart.Enabled = enableState;
            chkTMDbSyncWatchlist.Enabled = enableState;
        }

        private void EnableListalControls(bool enableState)
        {
            lblListalMovieExportFile.Enabled = enableState;
            lblListalShowExportFile.Enabled = enableState;
            txtListalMovieXMLExport.Enabled = enableState;
            txtListalShowXMLExport.Enabled = enableState;
            lblListalLinkInfo.Enabled = enableState;
            lnkListalExport.Enabled = enableState;
            chkListalWebWatchlist.Enabled = enableState;
        }

        private void EnableCritickerControls(bool enableState)
        {
            lblCritickerCSVExportFile.Enabled = enableState;
            txtCritickerCSVExportFile.Enabled = enableState;
            btnCritickerCSVExportBrowse.Enabled = enableState;
        }

        private void EnableLetterboxdControls(bool enableState)
        {
            lblLetterboxdDiary.Enabled = enableState;
            lblLetterboxdRatingsFile.Enabled = enableState;
            lblLetterboxdWatched.Enabled = enableState;
            btnLetterboxdDiaryBrowse.Enabled = enableState;
            btnLetterboxdRatingsBrowse.Enabled = enableState;
            btnLetterboxdWatchedBrowse.Enabled = enableState;
            txtLetterboxdDiaryFile.Enabled = enableState;
            txtLetterboxdRatingsFile.Enabled = enableState;
            txtLetterboxdWatchedFile.Enabled = enableState;
            lblLetterboxdCustomList.Enabled = enableState;
            listLetterboxdCustomLists.Enabled = enableState;
            btnLetterBoxAddList.Enabled = enableState;
            btnLetterBoxRemoveList.Enabled = enableState;
        }

        private void EnableFlixsterControls(bool enableState)
        {            
            lblFlisterUserId.Enabled = enableState;
            txtFlixsterUserId.Enabled = enableState;
            chkFlixsterSyncWantToSee.Enabled = enableState;
            lblFlixsterUserIdDesc.Enabled = enableState;
        }

        private void EnableExternalSourceControlsInGroupBoxes()
        {
            EnableImdbControls(AppSettings.EnableIMDb);
            EnableTmdbControls(AppSettings.EnableTMDb);
            EnableCheckMoviesControls(AppSettings.EnableCheckMovies);
            EnableTvdbControls(AppSettings.EnableTVDb);
            EnableListalControls(AppSettings.EnableListal);
            EnableCritickerControls(AppSettings.EnableCriticker);
            EnableLetterboxdControls(AppSettings.EnableLetterboxd);
            EnableFlixsterControls(AppSettings.EnableFlixster);
            EnableToDoMoviesControls(AppSettings.EnableToDoMovies);
        }

        private void EnableCheckMoviesControls(bool enableState)
        {
            btnCheckMoviesExportBrowse.Enabled = enableState;
            lblCheckMoviesFile.Enabled = enableState;
            lblCheckMoviesDelimiter.Enabled = enableState;
            txtCheckMoviesCsvFile.Enabled = enableState;
            chkCheckMoviesAddWatchedToWatchlist.Enabled = enableState;
            chkCheckMoviesUpdateWatchedStatus.Enabled = enableState;
            chkCheckMoviesUpdateWatchedStatus.Enabled = enableState;
            chkCheckMoviesAddMoviesToCollection.Enabled = enableState;
            cboCheckMoviesDelimiter.Enabled = enableState;
        }

        private void EnableToDoMoviesControls(bool enableState)
        {
            btnToDoMoviesExportBrowse.Enabled = enableState;
            lblToDoMovieExportFile.Enabled = enableState;
            txtToDoMovieExportFile.Enabled = enableState;
        }

        #endregion

    }
}
