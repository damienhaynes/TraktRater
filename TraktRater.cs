using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using TraktRater.Sites;
using TraktRater.Extensions;
using TraktRater.TraktAPI.DataStructures;
using TraktRater.UI;
using TraktRater.Settings;

namespace TraktRater
{
    public partial class TraktRater : Form
    {
        #region UI Invoke Delegates
        delegate void SetControlStateDelegate(bool enable);
        delegate void SetTMDbControlStateDelegate();
        delegate void ClearProgressDelegate();
        #endregion

        #region Variables
        List<IRateSite> sites = new List<IRateSite>();
        static bool ImportRunning = false;
        static bool ImportCancelled = false;
        #endregion

        #region Constants
        const string cImportReady = "Start Ratings Import";
        const string cCancelImport = "Cancel Import";
        #endregion

        #region Constructor
        public TraktRater()
        {
            InitializeComponent();
        }
        #endregion

        #region Form Overrides
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AppSettings.Load();
            ClearProgress();

            // populate fields
            txtTraktUsername.Text = AppSettings.TraktUsername;
            txtTraktPassword.Text = AppSettings.TraktPassword;
            txtTVDbAccountId.Text = AppSettings.TVDbAccountIdentifier;
            txtImdbFilename.Text = AppSettings.IMDbFilename;
            SetTMDbControlState();

            // prevent re-hash and subscribe after setting password in box
            txtTraktPassword.TextChanged += new EventHandler(txtTraktPassword_TextChanged);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            CancelImport();
            AppSettings.Save();
            base.OnClosing(e);
        }
        #endregion

        #region UI Events
        private void btnImportRatings_Click(object sender, EventArgs e)
        {
            if (!ImportRunning)
                StartImport();
            else
                CancelImport();
        }

        private void txtTVDbAccountId_TextChanged(object sender, EventArgs e)
        {
            AppSettings.TVDbAccountIdentifier = txtTVDbAccountId.Text;
        }

        private void txtTraktPassword_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTraktPassword.Text))
            {
                AppSettings.TraktPassword = string.Empty;
                return;
            }
            AppSettings.TraktPassword = txtTraktPassword.Text.ToShaHash();
        }

        private void txtTraktUsername_TextChanged(object sender, EventArgs e)
        {
            AppSettings.TraktUsername = txtTraktUsername.Text;
        }

        private void btnImdbBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgFileOpen.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtImdbFilename.Text = dlgFileOpen.FileName;
            }
        }
        
        private void txtImdbFilename_TextChanged(object sender, EventArgs e)
        {
            AppSettings.IMDbFilename = txtImdbFilename.Text;
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

            Thread tokenThread = new Thread((o) =>
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

        #endregion

        #region Import Actions
        private void StartImport()
        {
            if (ImportRunning) return;

            if (string.IsNullOrEmpty(AppSettings.TraktUsername) || string.IsNullOrEmpty(AppSettings.TraktPassword))
            {
                UIUtils.UpdateStatus("You must enter in your trakt username and password!", true);
                return;
            }

            sites.Clear();

            // add import sites for processing
            sites.Add(new TMDb(AppSettings.TMDbRequestToken, AppSettings.TMDbSessionId));
            sites.Add(new TVDb(AppSettings.TVDbAccountIdentifier));
            sites.Add(new IMDb(AppSettings.IMDbFilename));

            if (sites.Where(s => s.Enabled).Count() == 0)
            {
                UIUtils.UpdateStatus("Incorrect site information supplied!", true);
                return;
            }

            #region Import
            Thread importThread = new Thread((o) =>
            {
                ImportRunning = true;

                // only one import at a time
                SetControlState(false);

                // Clear Progress
                ClearProgress();

                #region Login to trakt
                UIUtils.UpdateStatus("Logging in to trakt.tv...");
                var accountDetails = new TraktAuthentication { Username = AppSettings.TraktUsername, Password = AppSettings.TraktPassword };
                var response = TraktAPI.TraktAPI.TestAccount(accountDetails);
                if (response == null || response.Status != "success")
                {
                    UIUtils.UpdateStatus("Unable to login to trakt, check username and password!", true);
                    SetControlState(true);
                    ImportRunning = false;
                    ImportCancelled = false;
                    return;
                }
                #endregion

                // import ratings
                foreach (var site in sites.Where(s => s.Enabled))
                {
                    try
                    {
                        if (!ImportCancelled)
                            site.ImportRatings();
                    }
                    catch (Exception e)
                    {
                        UIUtils.UpdateStatus(string.Format("{0}:{1}", site.Name, e.Message), true);
                        Thread.Sleep(5000);
                    }
                }

                // finished
                SetControlState(true);
                UIUtils.UpdateStatus("Import Complete!");
                ImportRunning = false;
                ImportCancelled = false;
            });

            importThread.Start();
            #endregion
        }

        private void CancelImport()
        {
            if (!ImportRunning) return;

            UIUtils.UpdateStatus("Cancelling Import...");

            ImportCancelled = true;

            Thread cancelThread = new Thread((o) =>
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

        #region Private Methods
        private void SetControlState(bool enable)
        {
            if (this.InvokeRequired)
            {
                SetControlStateDelegate setControlState = new SetControlStateDelegate(SetControlState);
                this.Invoke(setControlState, enable);
                return;
            }

            txtTraktUsername.Enabled = enable;
            txtTraktPassword.Enabled = enable;
            txtTVDbAccountId.Enabled = enable;
            txtImdbFilename.Enabled = enable;
            btnImdbBrowse.Enabled = enable;
            lnkTMDbStart.Enabled = enable;

            btnImportRatings.Text = enable ? cImportReady : cCancelImport;
            pbrImportProgress.Style = enable ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;
        }

        private void ClearProgress()
        {
            if (this.InvokeRequired)
            {
                ClearProgressDelegate clearProgress = new ClearProgressDelegate(ClearProgress);
                this.Invoke(clearProgress);
                return;
            }
            
            lblStatusMessage.Text = "Ready for anything!";
            lblStatusMessage.ForeColor = Color.Black;
        }

        private void SetTMDbControlState()
        {
            if (this.InvokeRequired)
            {
                SetTMDbControlStateDelegate setTMDbState = new SetTMDbControlStateDelegate(SetTMDbControlState);
                this.Invoke(setTMDbState);
                return;
            }

            // we are either ready to get session id or we already have it
            if (!string.IsNullOrEmpty(AppSettings.TMDbRequestToken) || !string.IsNullOrEmpty(AppSettings.TMDbSessionId))
            {
                lblTMDbMessage.Text = "Request Token and/or Session Id is already found.";
                lnkTMDbStart.Text = "Disable TMDb Support";
            }
            else
            {
                lblTMDbMessage.Text = "To get user ratings from TMDb you must first allow this application to access your account details. This needs to be done by you in a webbrowser.";
                lnkTMDbStart.Text = "Start Request Process";
            }
        }
        #endregion

    }
}
