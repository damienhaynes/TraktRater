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

namespace TraktRater
{
    public partial class TraktRater : Form
    {
        #region UI Invoke Delegates
        delegate void SetControlStateDelegate(bool enable);
        delegate void ClearProgressDelegate();
        #endregion

        #region Variables
        List<IRateSite> sites = new List<IRateSite>();
        static bool ImportRunning = false;
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
            ClearProgress();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            CancelImport();
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
            Settings.TVDbAccountIdentifier = txtTVDbAccountId.Text;
        }

        private void txtTraktPassword_TextChanged(object sender, EventArgs e)
        {
            Settings.TraktPassword = txtTraktPassword.Text.ToShaHash();
        }

        private void txtTraktUsername_TextChanged(object sender, EventArgs e)
        {
            Settings.TraktUsername = txtTraktUsername.Text;
        }
        #endregion

        #region Import Actions
        private void StartImport()
        {
            if (ImportRunning) return;

            if (string.IsNullOrEmpty(Settings.TraktUsername) || string.IsNullOrEmpty(Settings.TraktPassword))
            {
                UIUtils.UpdateStatus("You must enter in your trakt username and password!", true);
                return;
            }

            sites.Clear();

            // add import sites for processing
            sites.Add(new TVDb(Settings.TVDbAccountIdentifier));

            if (sites.Where(s => s.Enabled).Count() == 0)
            {
                UIUtils.UpdateStatus("Incorrect site information supplied!", true);
                return;
            }

            Thread importThread = new Thread((o) =>
            {
                ImportRunning = true;

                // only one import at a time
                SetControlState(false);

                // Clear Progress
                ClearProgress();

                #region Login to trakt
                UIUtils.UpdateStatus("Logging in to trakt.tv...");
                var accountDetails = new TraktAuthentication { Username = Settings.TraktUsername, Password = Settings.TraktPassword };
                var response = TraktAPI.TraktAPI.TestAccount(accountDetails);
                if (response == null || response.Status != "success")
                {
                    UIUtils.UpdateStatus("Unable to login to trakt, check username and password!", true);
                    SetControlState(true);
                    ImportRunning = false;
                    return;
                }
                #endregion

                // import ratings
                foreach (var site in sites.Where(s => s.Enabled))
                {
                    try
                    {
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
            });

            importThread.Start();
        }

        private void CancelImport()
        {
            if (!ImportRunning) return;

            UIUtils.UpdateStatus("Cancelling Import...");

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
        #endregion

    }
}
