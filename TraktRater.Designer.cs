namespace TraktRater
{
    partial class TraktRater
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TraktRater));
            this.grbTrakt = new System.Windows.Forms.GroupBox();
            this.txtTraktPinCode = new System.Windows.Forms.TextBox();
            this.lnkTraktOAuth = new System.Windows.Forms.LinkLabel();
            this.radTraktPinCode = new System.Windows.Forms.RadioButton();
            this.radTraktUserPass = new System.Windows.Forms.RadioButton();
            this.lblTraktAuthMethod = new System.Windows.Forms.Label();
            this.btnMaintenance = new System.Windows.Forms.Button();
            this.txtTraktPassword = new System.Windows.Forms.TextBox();
            this.lblTraktPassword = new System.Windows.Forms.Label();
            this.lblTraktUser = new System.Windows.Forms.Label();
            this.txtTraktUser = new System.Windows.Forms.TextBox();
            this.grbTVDb = new System.Windows.Forms.GroupBox();
            this.chkTVDbEnabled = new System.Windows.Forms.CheckBox();
            this.txtTVDbAccountId = new System.Windows.Forms.TextBox();
            this.lblTVDbAccountId = new System.Windows.Forms.Label();
            this.lblDetails = new System.Windows.Forms.Label();
            this.btnStartSync = new System.Windows.Forms.Button();
            this.pbrImportProgress = new System.Windows.Forms.ProgressBar();
            this.grbReport = new System.Windows.Forms.GroupBox();
            this.lnkLogFolder = new System.Windows.Forms.LinkLabel();
            this.lblStatusMessage = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.txtImdbRatingsFilename = new System.Windows.Forms.TextBox();
            this.txtImdbWebUsername = new System.Windows.Forms.TextBox();
            this.txtImdbWatchlistFile = new System.Windows.Forms.TextBox();
            this.nudBatchSize = new System.Windows.Forms.NumericUpDown();
            this.grbImdb = new System.Windows.Forms.GroupBox();
            this.chkIMDbEnabled = new System.Windows.Forms.CheckBox();
            this.btnImdbWatchlistBrowse = new System.Windows.Forms.Button();
            this.lblImdbWatchlistFile = new System.Windows.Forms.Label();
            this.lblImdbRatingsFile = new System.Windows.Forms.Label();
            this.lblImdbDescription = new System.Windows.Forms.Label();
            this.rdnImdbUsername = new System.Windows.Forms.RadioButton();
            this.rdnImdbCSV = new System.Windows.Forms.RadioButton();
            this.chkImdbWebWatchlist = new System.Windows.Forms.CheckBox();
            this.btnImdbRatingsBrowse = new System.Windows.Forms.Button();
            this.dlgFileOpen = new System.Windows.Forms.OpenFileDialog();
            this.grbTMDb = new System.Windows.Forms.GroupBox();
            this.chkTMDbSyncWatchlist = new System.Windows.Forms.CheckBox();
            this.chkTMDbEnabled = new System.Windows.Forms.CheckBox();
            this.lnkTMDbStart = new System.Windows.Forms.LinkLabel();
            this.lblTMDbMessage = new System.Windows.Forms.Label();
            this.grbOptions = new System.Windows.Forms.GroupBox();
            this.lblBatchImportSize = new System.Windows.Forms.Label();
            this.chkIgnoreWatchedForWatchlists = new System.Windows.Forms.CheckBox();
            this.chkMarkAsWatched = new System.Windows.Forms.CheckBox();
            this.grbListal = new System.Windows.Forms.GroupBox();
            this.chkListalEnabled = new System.Windows.Forms.CheckBox();
            this.lblListalShowExportFile = new System.Windows.Forms.Label();
            this.btnListalShowXMLExport = new System.Windows.Forms.Button();
            this.txtListalShowXMLExport = new System.Windows.Forms.TextBox();
            this.lblListalLinkInfo = new System.Windows.Forms.Label();
            this.lnkListalExport = new System.Windows.Forms.LinkLabel();
            this.chkListalWebWatchlist = new System.Windows.Forms.CheckBox();
            this.btnListalMovieXMLExport = new System.Windows.Forms.Button();
            this.txtListalMovieXMLExport = new System.Windows.Forms.TextBox();
            this.lblListalMovieExportFile = new System.Windows.Forms.Label();
            this.grbCriticker = new System.Windows.Forms.GroupBox();
            this.btnCritickerMovieExportBrowse = new System.Windows.Forms.Button();
            this.chkCritickerEnabled = new System.Windows.Forms.CheckBox();
            this.txtCritickerMovieExportFile = new System.Windows.Forms.TextBox();
            this.lblCritickerMovieExportFile = new System.Windows.Forms.Label();
            this.lblWarnPeriod = new System.Windows.Forms.Label();
            this.grbTrakt.SuspendLayout();
            this.grbTVDb.SuspendLayout();
            this.grbReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBatchSize)).BeginInit();
            this.grbImdb.SuspendLayout();
            this.grbTMDb.SuspendLayout();
            this.grbOptions.SuspendLayout();
            this.grbListal.SuspendLayout();
            this.grbCriticker.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbTrakt
            // 
            this.grbTrakt.Controls.Add(this.lblWarnPeriod);
            this.grbTrakt.Controls.Add(this.txtTraktPinCode);
            this.grbTrakt.Controls.Add(this.lnkTraktOAuth);
            this.grbTrakt.Controls.Add(this.radTraktPinCode);
            this.grbTrakt.Controls.Add(this.radTraktUserPass);
            this.grbTrakt.Controls.Add(this.lblTraktAuthMethod);
            this.grbTrakt.Controls.Add(this.txtTraktPassword);
            this.grbTrakt.Controls.Add(this.lblTraktPassword);
            this.grbTrakt.Controls.Add(this.lblTraktUser);
            this.grbTrakt.Controls.Add(this.txtTraktUser);
            this.grbTrakt.Location = new System.Drawing.Point(12, 12);
            this.grbTrakt.Name = "grbTrakt";
            this.grbTrakt.Size = new System.Drawing.Size(443, 121);
            this.grbTrakt.TabIndex = 0;
            this.grbTrakt.TabStop = false;
            this.grbTrakt.Text = "Trakt";
            // 
            // txtTraktPinCode
            // 
            this.txtTraktPinCode.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtTraktPinCode.Location = new System.Drawing.Point(178, 64);
            this.txtTraktPinCode.Name = "txtTraktPinCode";
            this.txtTraktPinCode.Size = new System.Drawing.Size(244, 20);
            this.txtTraktPinCode.TabIndex = 16;
            this.txtTraktPinCode.Text = "Authorise and then enter pin code here...";
            this.txtTraktPinCode.Visible = false;
            this.txtTraktPinCode.Click += new System.EventHandler(this.txtTraktPinCode_Click);
            this.txtTraktPinCode.TextChanged += new System.EventHandler(this.txtTraktPinCode_TextChanged);
            // 
            // lnkTraktOAuth
            // 
            this.lnkTraktOAuth.AutoSize = true;
            this.lnkTraktOAuth.Location = new System.Drawing.Point(178, 44);
            this.lnkTraktOAuth.Name = "lnkTraktOAuth";
            this.lnkTraktOAuth.Size = new System.Drawing.Size(203, 13);
            this.lnkTraktOAuth.TabIndex = 15;
            this.lnkTraktOAuth.TabStop = true;
            this.lnkTraktOAuth.Text = "Click to Authorise access to your account";
            this.lnkTraktOAuth.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTraktOAuth_LinkClicked);
            // 
            // radTraktPinCode
            // 
            this.radTraktPinCode.AutoSize = true;
            this.radTraktPinCode.Location = new System.Drawing.Point(297, 14);
            this.radTraktPinCode.Name = "radTraktPinCode";
            this.radTraktPinCode.Size = new System.Drawing.Size(105, 17);
            this.radTraktPinCode.TabIndex = 14;
            this.radTraktPinCode.TabStop = true;
            this.radTraktPinCode.Text = "Pin Code (oAuth)";
            this.radTraktPinCode.UseVisualStyleBackColor = true;
            this.radTraktPinCode.Click += new System.EventHandler(this.radTraktPinCode_Click);
            // 
            // radTraktUserPass
            // 
            this.radTraktUserPass.AutoSize = true;
            this.radTraktUserPass.Location = new System.Drawing.Point(178, 14);
            this.radTraktUserPass.Name = "radTraktUserPass";
            this.radTraktUserPass.Size = new System.Drawing.Size(75, 17);
            this.radTraktUserPass.TabIndex = 13;
            this.radTraktUserPass.TabStop = true;
            this.radTraktUserPass.Text = "User/Pass";
            this.radTraktUserPass.UseVisualStyleBackColor = true;
            this.radTraktUserPass.Click += new System.EventHandler(this.radTraktUserPass_Click);
            // 
            // lblTraktAuthMethod
            // 
            this.lblTraktAuthMethod.AutoSize = true;
            this.lblTraktAuthMethod.Location = new System.Drawing.Point(22, 19);
            this.lblTraktAuthMethod.Name = "lblTraktAuthMethod";
            this.lblTraktAuthMethod.Size = new System.Drawing.Size(117, 13);
            this.lblTraktAuthMethod.TabIndex = 12;
            this.lblTraktAuthMethod.Text = "Authentication Method:";
            // 
            // btnMaintenance
            // 
            this.btnMaintenance.Location = new System.Drawing.Point(21, 89);
            this.btnMaintenance.Name = "btnMaintenance";
            this.btnMaintenance.Size = new System.Drawing.Size(276, 26);
            this.btnMaintenance.TabIndex = 11;
            this.btnMaintenance.Text = "Cleanup / Maintenance...";
            this.btnMaintenance.UseVisualStyleBackColor = true;
            this.btnMaintenance.Click += new System.EventHandler(this.btnMaintenance_Click);
            // 
            // txtTraktPassword
            // 
            this.txtTraktPassword.Location = new System.Drawing.Point(178, 64);
            this.txtTraktPassword.Name = "txtTraktPassword";
            this.txtTraktPassword.Size = new System.Drawing.Size(244, 20);
            this.txtTraktPassword.TabIndex = 3;
            this.txtTraktPassword.UseSystemPasswordChar = true;
            this.txtTraktPassword.TextChanged += new System.EventHandler(this.txtTraktPassword_TextChanged);
            // 
            // lblTraktPassword
            // 
            this.lblTraktPassword.AutoSize = true;
            this.lblTraktPassword.Location = new System.Drawing.Point(22, 67);
            this.lblTraktPassword.Name = "lblTraktPassword";
            this.lblTraktPassword.Size = new System.Drawing.Size(56, 13);
            this.lblTraktPassword.TabIndex = 2;
            this.lblTraktPassword.Text = "Password:";
            // 
            // lblTraktUser
            // 
            this.lblTraktUser.AutoSize = true;
            this.lblTraktUser.Location = new System.Drawing.Point(22, 44);
            this.lblTraktUser.Name = "lblTraktUser";
            this.lblTraktUser.Size = new System.Drawing.Size(58, 13);
            this.lblTraktUser.TabIndex = 0;
            this.lblTraktUser.Text = "Username:";
            // 
            // txtTraktUser
            // 
            this.txtTraktUser.Location = new System.Drawing.Point(178, 41);
            this.txtTraktUser.Name = "txtTraktUser";
            this.txtTraktUser.Size = new System.Drawing.Size(244, 20);
            this.txtTraktUser.TabIndex = 1;
            this.txtTraktUser.TextChanged += new System.EventHandler(this.txtTraktUsername_TextChanged);
            // 
            // grbTVDb
            // 
            this.grbTVDb.Controls.Add(this.chkTVDbEnabled);
            this.grbTVDb.Controls.Add(this.txtTVDbAccountId);
            this.grbTVDb.Controls.Add(this.lblTVDbAccountId);
            this.grbTVDb.Location = new System.Drawing.Point(12, 152);
            this.grbTVDb.Name = "grbTVDb";
            this.grbTVDb.Size = new System.Drawing.Size(443, 81);
            this.grbTVDb.TabIndex = 3;
            this.grbTVDb.TabStop = false;
            this.grbTVDb.Text = "TVDb";
            // 
            // chkTVDbEnabled
            // 
            this.chkTVDbEnabled.AutoSize = true;
            this.chkTVDbEnabled.Location = new System.Drawing.Point(19, 19);
            this.chkTVDbEnabled.Name = "chkTVDbEnabled";
            this.chkTVDbEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkTVDbEnabled.TabIndex = 0;
            this.chkTVDbEnabled.Text = "Enabled";
            this.chkTVDbEnabled.UseVisualStyleBackColor = true;
            this.chkTVDbEnabled.CheckedChanged += new System.EventHandler(this.chkTVDbEnabled_CheckedChanged);
            // 
            // txtTVDbAccountId
            // 
            this.txtTVDbAccountId.Location = new System.Drawing.Point(175, 46);
            this.txtTVDbAccountId.Name = "txtTVDbAccountId";
            this.txtTVDbAccountId.Size = new System.Drawing.Size(244, 20);
            this.txtTVDbAccountId.TabIndex = 2;
            this.tipHelp.SetToolTip(this.txtTVDbAccountId, "The Account Identifier can be found in the account tab of\r\nhttp://thetvdb.com web" +
        "site. It is 16 hexidecimal characters e.g.\r\n\r\nEB6D329D10E3835A");
            this.txtTVDbAccountId.TextChanged += new System.EventHandler(this.txtTVDbAccountId_TextChanged);
            // 
            // lblTVDbAccountId
            // 
            this.lblTVDbAccountId.AutoSize = true;
            this.lblTVDbAccountId.Location = new System.Drawing.Point(16, 49);
            this.lblTVDbAccountId.Name = "lblTVDbAccountId";
            this.lblTVDbAccountId.Size = new System.Drawing.Size(93, 13);
            this.lblTVDbAccountId.TabIndex = 1;
            this.lblTVDbAccountId.Text = "Account Identifier:";
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Location = new System.Drawing.Point(9, 136);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(550, 13);
            this.lblDetails.TabIndex = 2;
            this.lblDetails.Text = "Enter in account details for each external source you wish to transfer ratings, w" +
    "atched and watchlist data to trakt.tv:";
            // 
            // btnStartSync
            // 
            this.btnStartSync.Location = new System.Drawing.Point(12, 595);
            this.btnStartSync.Name = "btnStartSync";
            this.btnStartSync.Size = new System.Drawing.Size(887, 26);
            this.btnStartSync.TabIndex = 8;
            this.btnStartSync.Text = "Start Import";
            this.btnStartSync.UseVisualStyleBackColor = true;
            this.btnStartSync.Click += new System.EventHandler(this.btnStartSync_Click);
            // 
            // pbrImportProgress
            // 
            this.pbrImportProgress.Location = new System.Drawing.Point(13, 627);
            this.pbrImportProgress.Name = "pbrImportProgress";
            this.pbrImportProgress.Size = new System.Drawing.Size(886, 23);
            this.pbrImportProgress.TabIndex = 9;
            // 
            // grbReport
            // 
            this.grbReport.Controls.Add(this.lnkLogFolder);
            this.grbReport.Controls.Add(this.lblStatusMessage);
            this.grbReport.Controls.Add(this.label5);
            this.grbReport.Location = new System.Drawing.Point(12, 660);
            this.grbReport.Name = "grbReport";
            this.grbReport.Size = new System.Drawing.Size(887, 49);
            this.grbReport.TabIndex = 10;
            this.grbReport.TabStop = false;
            this.grbReport.Text = "Report";
            // 
            // lnkLogFolder
            // 
            this.lnkLogFolder.AutoSize = true;
            this.lnkLogFolder.Location = new System.Drawing.Point(795, 20);
            this.lnkLogFolder.Name = "lnkLogFolder";
            this.lnkLogFolder.Size = new System.Drawing.Size(86, 13);
            this.lnkLogFolder.TabIndex = 2;
            this.lnkLogFolder.TabStop = true;
            this.lnkLogFolder.Text = "Open Log Folder";
            this.lnkLogFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLogFolder_LinkClicked);
            // 
            // lblStatusMessage
            // 
            this.lblStatusMessage.Location = new System.Drawing.Point(77, 20);
            this.lblStatusMessage.Name = "lblStatusMessage";
            this.lblStatusMessage.Size = new System.Drawing.Size(712, 23);
            this.lblStatusMessage.TabIndex = 1;
            this.lblStatusMessage.Text = "Ready for anything!";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(7, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 23);
            this.label5.TabIndex = 0;
            this.label5.Text = "Status:";
            // 
            // tipHelp
            // 
            this.tipHelp.AutoPopDelay = 10000;
            this.tipHelp.InitialDelay = 500;
            this.tipHelp.IsBalloon = true;
            this.tipHelp.ReshowDelay = 100;
            this.tipHelp.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.tipHelp.ToolTipTitle = "Help";
            // 
            // txtImdbRatingsFilename
            // 
            this.txtImdbRatingsFilename.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtImdbRatingsFilename.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtImdbRatingsFilename.Enabled = false;
            this.txtImdbRatingsFilename.Location = new System.Drawing.Point(175, 91);
            this.txtImdbRatingsFilename.Name = "txtImdbRatingsFilename";
            this.txtImdbRatingsFilename.Size = new System.Drawing.Size(208, 20);
            this.txtImdbRatingsFilename.TabIndex = 4;
            this.tipHelp.SetToolTip(this.txtImdbRatingsFilename, "You can export your ratings history to csv from your IMDb account settings.\r\nOnce" +
        " you have downloaded file, you can specify filename in this textbox.");
            this.txtImdbRatingsFilename.TextChanged += new System.EventHandler(this.txtImdbFilename_TextChanged);
            // 
            // txtImdbWebUsername
            // 
            this.txtImdbWebUsername.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtImdbWebUsername.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtImdbWebUsername.Location = new System.Drawing.Point(175, 179);
            this.txtImdbWebUsername.Name = "txtImdbWebUsername";
            this.txtImdbWebUsername.Size = new System.Drawing.Size(244, 20);
            this.txtImdbWebUsername.TabIndex = 10;
            this.tipHelp.SetToolTip(this.txtImdbWebUsername, resources.GetString("txtImdbWebUsername.ToolTip"));
            this.txtImdbWebUsername.TextChanged += new System.EventHandler(this.txtImdbUsername_TextChanged);
            // 
            // txtImdbWatchlistFile
            // 
            this.txtImdbWatchlistFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtImdbWatchlistFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtImdbWatchlistFile.Location = new System.Drawing.Point(175, 136);
            this.txtImdbWatchlistFile.Name = "txtImdbWatchlistFile";
            this.txtImdbWatchlistFile.Size = new System.Drawing.Size(208, 20);
            this.txtImdbWatchlistFile.TabIndex = 7;
            this.tipHelp.SetToolTip(this.txtImdbWatchlistFile, "Leave field blank if you\'re not interested in importing a watchlist from IMDb to " +
        "trakt.tv.");
            this.txtImdbWatchlistFile.TextChanged += new System.EventHandler(this.txtImdbWatchlistFile_TextChanged);
            // 
            // nudBatchSize
            // 
            this.nudBatchSize.Location = new System.Drawing.Point(133, 61);
            this.nudBatchSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudBatchSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBatchSize.Name = "nudBatchSize";
            this.nudBatchSize.Size = new System.Drawing.Size(104, 20);
            this.nudBatchSize.TabIndex = 3;
            this.tipHelp.SetToolTip(this.nudBatchSize, "Set the size of the batch when importing items to trakt.tv. Set lower if having i" +
        "ssues with the server.");
            this.nudBatchSize.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudBatchSize.ValueChanged += new System.EventHandler(this.nudBatchSize_ValueChanged);
            // 
            // grbImdb
            // 
            this.grbImdb.Controls.Add(this.chkIMDbEnabled);
            this.grbImdb.Controls.Add(this.btnImdbWatchlistBrowse);
            this.grbImdb.Controls.Add(this.txtImdbWatchlistFile);
            this.grbImdb.Controls.Add(this.lblImdbWatchlistFile);
            this.grbImdb.Controls.Add(this.lblImdbRatingsFile);
            this.grbImdb.Controls.Add(this.lblImdbDescription);
            this.grbImdb.Controls.Add(this.rdnImdbUsername);
            this.grbImdb.Controls.Add(this.rdnImdbCSV);
            this.grbImdb.Controls.Add(this.chkImdbWebWatchlist);
            this.grbImdb.Controls.Add(this.txtImdbWebUsername);
            this.grbImdb.Controls.Add(this.btnImdbRatingsBrowse);
            this.grbImdb.Controls.Add(this.txtImdbRatingsFilename);
            this.grbImdb.Location = new System.Drawing.Point(12, 239);
            this.grbImdb.Name = "grbImdb";
            this.grbImdb.Size = new System.Drawing.Size(443, 241);
            this.grbImdb.TabIndex = 5;
            this.grbImdb.TabStop = false;
            this.grbImdb.Text = "IMDb";
            // 
            // chkIMDbEnabled
            // 
            this.chkIMDbEnabled.AutoSize = true;
            this.chkIMDbEnabled.Location = new System.Drawing.Point(19, 19);
            this.chkIMDbEnabled.Name = "chkIMDbEnabled";
            this.chkIMDbEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkIMDbEnabled.TabIndex = 0;
            this.chkIMDbEnabled.Text = "Enabled";
            this.chkIMDbEnabled.UseVisualStyleBackColor = true;
            this.chkIMDbEnabled.CheckedChanged += new System.EventHandler(this.chkIMDbEnabled_CheckedChanged);
            // 
            // btnImdbWatchlistBrowse
            // 
            this.btnImdbWatchlistBrowse.Location = new System.Drawing.Point(390, 133);
            this.btnImdbWatchlistBrowse.Name = "btnImdbWatchlistBrowse";
            this.btnImdbWatchlistBrowse.Size = new System.Drawing.Size(28, 23);
            this.btnImdbWatchlistBrowse.TabIndex = 8;
            this.btnImdbWatchlistBrowse.Text = "...";
            this.btnImdbWatchlistBrowse.UseVisualStyleBackColor = true;
            this.btnImdbWatchlistBrowse.Click += new System.EventHandler(this.btnImdbWatchlistBrowse_Click);
            // 
            // lblImdbWatchlistFile
            // 
            this.lblImdbWatchlistFile.AutoSize = true;
            this.lblImdbWatchlistFile.Location = new System.Drawing.Point(175, 118);
            this.lblImdbWatchlistFile.Name = "lblImdbWatchlistFile";
            this.lblImdbWatchlistFile.Size = new System.Drawing.Size(73, 13);
            this.lblImdbWatchlistFile.TabIndex = 6;
            this.lblImdbWatchlistFile.Text = "Watchlist File:";
            // 
            // lblImdbRatingsFile
            // 
            this.lblImdbRatingsFile.AutoSize = true;
            this.lblImdbRatingsFile.Location = new System.Drawing.Point(172, 75);
            this.lblImdbRatingsFile.Name = "lblImdbRatingsFile";
            this.lblImdbRatingsFile.Size = new System.Drawing.Size(65, 13);
            this.lblImdbRatingsFile.TabIndex = 3;
            this.lblImdbRatingsFile.Text = "Ratings File:";
            // 
            // lblImdbDescription
            // 
            this.lblImdbDescription.AutoSize = true;
            this.lblImdbDescription.Location = new System.Drawing.Point(18, 45);
            this.lblImdbDescription.Name = "lblImdbDescription";
            this.lblImdbDescription.Size = new System.Drawing.Size(347, 13);
            this.lblImdbDescription.TabIndex = 1;
            this.lblImdbDescription.Text = "Select \'CSV Import\' for static file import or \'Web Scrape\' for web retrieval:";
            // 
            // rdnImdbUsername
            // 
            this.rdnImdbUsername.AutoSize = true;
            this.rdnImdbUsername.Checked = true;
            this.rdnImdbUsername.Location = new System.Drawing.Point(19, 182);
            this.rdnImdbUsername.Name = "rdnImdbUsername";
            this.rdnImdbUsername.Size = new System.Drawing.Size(88, 17);
            this.rdnImdbUsername.TabIndex = 9;
            this.rdnImdbUsername.TabStop = true;
            this.rdnImdbUsername.Text = "Web Scrape:";
            this.rdnImdbUsername.UseVisualStyleBackColor = true;
            // 
            // rdnImdbCSV
            // 
            this.rdnImdbCSV.AutoSize = true;
            this.rdnImdbCSV.Location = new System.Drawing.Point(19, 73);
            this.rdnImdbCSV.Name = "rdnImdbCSV";
            this.rdnImdbCSV.Size = new System.Drawing.Size(81, 17);
            this.rdnImdbCSV.TabIndex = 2;
            this.rdnImdbCSV.Text = "CSV Import:";
            this.rdnImdbCSV.UseVisualStyleBackColor = true;
            this.rdnImdbCSV.CheckedChanged += new System.EventHandler(this.rdnImdbCSV_CheckedChanged);
            // 
            // chkImdbWebWatchlist
            // 
            this.chkImdbWebWatchlist.AutoSize = true;
            this.chkImdbWebWatchlist.Location = new System.Drawing.Point(175, 205);
            this.chkImdbWebWatchlist.Name = "chkImdbWebWatchlist";
            this.chkImdbWebWatchlist.Size = new System.Drawing.Size(97, 17);
            this.chkImdbWebWatchlist.TabIndex = 11;
            this.chkImdbWebWatchlist.Text = "Sync Watchlist";
            this.chkImdbWebWatchlist.UseVisualStyleBackColor = true;
            this.chkImdbWebWatchlist.CheckedChanged += new System.EventHandler(this.chkImdbWatchlist_CheckedChanged);
            // 
            // btnImdbRatingsBrowse
            // 
            this.btnImdbRatingsBrowse.Enabled = false;
            this.btnImdbRatingsBrowse.Location = new System.Drawing.Point(389, 89);
            this.btnImdbRatingsBrowse.Name = "btnImdbRatingsBrowse";
            this.btnImdbRatingsBrowse.Size = new System.Drawing.Size(29, 23);
            this.btnImdbRatingsBrowse.TabIndex = 5;
            this.btnImdbRatingsBrowse.Text = "...";
            this.btnImdbRatingsBrowse.UseVisualStyleBackColor = true;
            this.btnImdbRatingsBrowse.Click += new System.EventHandler(this.btnImdbBrowse_Click);
            // 
            // dlgFileOpen
            // 
            this.dlgFileOpen.DefaultExt = "csv";
            // 
            // grbTMDb
            // 
            this.grbTMDb.Controls.Add(this.chkTMDbSyncWatchlist);
            this.grbTMDb.Controls.Add(this.chkTMDbEnabled);
            this.grbTMDb.Controls.Add(this.lnkTMDbStart);
            this.grbTMDb.Controls.Add(this.lblTMDbMessage);
            this.grbTMDb.Location = new System.Drawing.Point(461, 152);
            this.grbTMDb.Name = "grbTMDb";
            this.grbTMDb.Size = new System.Drawing.Size(443, 126);
            this.grbTMDb.TabIndex = 4;
            this.grbTMDb.TabStop = false;
            this.grbTMDb.Text = "TMDb";
            // 
            // chkTMDbSyncWatchlist
            // 
            this.chkTMDbSyncWatchlist.AutoSize = true;
            this.chkTMDbSyncWatchlist.Location = new System.Drawing.Point(20, 42);
            this.chkTMDbSyncWatchlist.Name = "chkTMDbSyncWatchlist";
            this.chkTMDbSyncWatchlist.Size = new System.Drawing.Size(97, 17);
            this.chkTMDbSyncWatchlist.TabIndex = 1;
            this.chkTMDbSyncWatchlist.Text = "Sync Watchlist";
            this.chkTMDbSyncWatchlist.UseVisualStyleBackColor = true;
            this.chkTMDbSyncWatchlist.CheckedChanged += new System.EventHandler(this.chkTMDbSyncWatchlist_CheckedChanged);
            // 
            // chkTMDbEnabled
            // 
            this.chkTMDbEnabled.AutoSize = true;
            this.chkTMDbEnabled.Location = new System.Drawing.Point(20, 19);
            this.chkTMDbEnabled.Name = "chkTMDbEnabled";
            this.chkTMDbEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkTMDbEnabled.TabIndex = 0;
            this.chkTMDbEnabled.Text = "Enabled";
            this.chkTMDbEnabled.UseVisualStyleBackColor = true;
            this.chkTMDbEnabled.CheckedChanged += new System.EventHandler(this.chkTMDbEnabled_CheckedChanged);
            // 
            // lnkTMDbStart
            // 
            this.lnkTMDbStart.AutoSize = true;
            this.lnkTMDbStart.Location = new System.Drawing.Point(18, 98);
            this.lnkTMDbStart.Name = "lnkTMDbStart";
            this.lnkTMDbStart.Size = new System.Drawing.Size(113, 13);
            this.lnkTMDbStart.TabIndex = 3;
            this.lnkTMDbStart.TabStop = true;
            this.lnkTMDbStart.Text = "Start Request Process";
            this.lnkTMDbStart.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTMDbStart_LinkClicked);
            // 
            // lblTMDbMessage
            // 
            this.lblTMDbMessage.Location = new System.Drawing.Point(17, 64);
            this.lblTMDbMessage.Name = "lblTMDbMessage";
            this.lblTMDbMessage.Size = new System.Drawing.Size(406, 44);
            this.lblTMDbMessage.TabIndex = 2;
            this.lblTMDbMessage.Text = "To get user ratings from TMDb you must first allow this application to access you" +
    "r account details. This needs to be done by you in a webbrowser.";
            // 
            // grbOptions
            // 
            this.grbOptions.Controls.Add(this.nudBatchSize);
            this.grbOptions.Controls.Add(this.lblBatchImportSize);
            this.grbOptions.Controls.Add(this.chkIgnoreWatchedForWatchlists);
            this.grbOptions.Controls.Add(this.chkMarkAsWatched);
            this.grbOptions.Controls.Add(this.btnMaintenance);
            this.grbOptions.Location = new System.Drawing.Point(461, 12);
            this.grbOptions.Name = "grbOptions";
            this.grbOptions.Size = new System.Drawing.Size(443, 121);
            this.grbOptions.TabIndex = 1;
            this.grbOptions.TabStop = false;
            this.grbOptions.Text = "Options";
            // 
            // lblBatchImportSize
            // 
            this.lblBatchImportSize.AutoSize = true;
            this.lblBatchImportSize.Location = new System.Drawing.Point(17, 66);
            this.lblBatchImportSize.Name = "lblBatchImportSize";
            this.lblBatchImportSize.Size = new System.Drawing.Size(93, 13);
            this.lblBatchImportSize.TabIndex = 2;
            this.lblBatchImportSize.Text = "Batch Import Size:";
            // 
            // chkIgnoreWatchedForWatchlists
            // 
            this.chkIgnoreWatchedForWatchlists.AutoSize = true;
            this.chkIgnoreWatchedForWatchlists.Location = new System.Drawing.Point(21, 39);
            this.chkIgnoreWatchedForWatchlists.Name = "chkIgnoreWatchedForWatchlists";
            this.chkIgnoreWatchedForWatchlists.Size = new System.Drawing.Size(216, 17);
            this.chkIgnoreWatchedForWatchlists.TabIndex = 1;
            this.chkIgnoreWatchedForWatchlists.Text = "Ignore watched items for Watchlist Sync";
            this.chkIgnoreWatchedForWatchlists.UseVisualStyleBackColor = true;
            this.chkIgnoreWatchedForWatchlists.Click += new System.EventHandler(this.chkIgnoreWatchedForWatchlists_Click);
            // 
            // chkMarkAsWatched
            // 
            this.chkMarkAsWatched.AutoSize = true;
            this.chkMarkAsWatched.Location = new System.Drawing.Point(21, 19);
            this.chkMarkAsWatched.Name = "chkMarkAsWatched";
            this.chkMarkAsWatched.Size = new System.Drawing.Size(276, 17);
            this.chkMarkAsWatched.TabIndex = 0;
            this.chkMarkAsWatched.Text = "Mark episodes and movies as watched if rated online";
            this.chkMarkAsWatched.UseVisualStyleBackColor = true;
            this.chkMarkAsWatched.Click += new System.EventHandler(this.chkMarkAsWatched_Click);
            // 
            // grbListal
            // 
            this.grbListal.Controls.Add(this.chkListalEnabled);
            this.grbListal.Controls.Add(this.lblListalShowExportFile);
            this.grbListal.Controls.Add(this.btnListalShowXMLExport);
            this.grbListal.Controls.Add(this.txtListalShowXMLExport);
            this.grbListal.Controls.Add(this.lblListalLinkInfo);
            this.grbListal.Controls.Add(this.lnkListalExport);
            this.grbListal.Controls.Add(this.chkListalWebWatchlist);
            this.grbListal.Controls.Add(this.btnListalMovieXMLExport);
            this.grbListal.Controls.Add(this.txtListalMovieXMLExport);
            this.grbListal.Controls.Add(this.lblListalMovieExportFile);
            this.grbListal.Location = new System.Drawing.Point(462, 284);
            this.grbListal.Name = "grbListal";
            this.grbListal.Size = new System.Drawing.Size(442, 196);
            this.grbListal.TabIndex = 6;
            this.grbListal.TabStop = false;
            this.grbListal.Text = "Listal";
            // 
            // chkListalEnabled
            // 
            this.chkListalEnabled.AutoSize = true;
            this.chkListalEnabled.Location = new System.Drawing.Point(20, 26);
            this.chkListalEnabled.Name = "chkListalEnabled";
            this.chkListalEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkListalEnabled.TabIndex = 0;
            this.chkListalEnabled.Text = "Enabled";
            this.chkListalEnabled.UseVisualStyleBackColor = true;
            this.chkListalEnabled.CheckedChanged += new System.EventHandler(this.chkListalEnabled_CheckedChanged);
            // 
            // lblListalShowExportFile
            // 
            this.lblListalShowExportFile.AutoSize = true;
            this.lblListalShowExportFile.Location = new System.Drawing.Point(16, 89);
            this.lblListalShowExportFile.Name = "lblListalShowExportFile";
            this.lblListalShowExportFile.Size = new System.Drawing.Size(106, 13);
            this.lblListalShowExportFile.TabIndex = 4;
            this.lblListalShowExportFile.Text = "TV Show Export File:";
            // 
            // btnListalShowXMLExport
            // 
            this.btnListalShowXMLExport.Location = new System.Drawing.Point(392, 80);
            this.btnListalShowXMLExport.Name = "btnListalShowXMLExport";
            this.btnListalShowXMLExport.Size = new System.Drawing.Size(29, 23);
            this.btnListalShowXMLExport.TabIndex = 6;
            this.btnListalShowXMLExport.Text = "...";
            this.btnListalShowXMLExport.UseVisualStyleBackColor = true;
            this.btnListalShowXMLExport.Click += new System.EventHandler(this.btnListalShowXMLExport_Click);
            // 
            // txtListalShowXMLExport
            // 
            this.txtListalShowXMLExport.Location = new System.Drawing.Point(155, 82);
            this.txtListalShowXMLExport.Name = "txtListalShowXMLExport";
            this.txtListalShowXMLExport.Size = new System.Drawing.Size(231, 20);
            this.txtListalShowXMLExport.TabIndex = 5;
            this.txtListalShowXMLExport.TextChanged += new System.EventHandler(this.txtListalShowXMLExport_TextChanged);
            // 
            // lblListalLinkInfo
            // 
            this.lblListalLinkInfo.AutoSize = true;
            this.lblListalLinkInfo.Location = new System.Drawing.Point(17, 149);
            this.lblListalLinkInfo.Name = "lblListalLinkInfo";
            this.lblListalLinkInfo.Size = new System.Drawing.Size(346, 13);
            this.lblListalLinkInfo.TabIndex = 8;
            this.lblListalLinkInfo.Text = "Logon into the Listal website, then download export files from link below:";
            // 
            // lnkListalExport
            // 
            this.lnkListalExport.AutoSize = true;
            this.lnkListalExport.Location = new System.Drawing.Point(17, 169);
            this.lnkListalExport.Name = "lnkListalExport";
            this.lnkListalExport.Size = new System.Drawing.Size(157, 13);
            this.lnkListalExport.TabIndex = 9;
            this.lnkListalExport.TabStop = true;
            this.lnkListalExport.Text = "Get your Listal export file(s) here";
            this.lnkListalExport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkListalExport_LinkClicked);
            // 
            // chkListalWebWatchlist
            // 
            this.chkListalWebWatchlist.AutoSize = true;
            this.chkListalWebWatchlist.Location = new System.Drawing.Point(155, 116);
            this.chkListalWebWatchlist.Name = "chkListalWebWatchlist";
            this.chkListalWebWatchlist.Size = new System.Drawing.Size(150, 17);
            this.chkListalWebWatchlist.TabIndex = 7;
            this.chkListalWebWatchlist.Text = "Sync Wantlist to Watchlist";
            this.chkListalWebWatchlist.UseVisualStyleBackColor = true;
            this.chkListalWebWatchlist.CheckedChanged += new System.EventHandler(this.chkListalWebWatchlist_CheckedChanged);
            // 
            // btnListalMovieXMLExport
            // 
            this.btnListalMovieXMLExport.Location = new System.Drawing.Point(392, 54);
            this.btnListalMovieXMLExport.Name = "btnListalMovieXMLExport";
            this.btnListalMovieXMLExport.Size = new System.Drawing.Size(29, 23);
            this.btnListalMovieXMLExport.TabIndex = 3;
            this.btnListalMovieXMLExport.Text = "...";
            this.btnListalMovieXMLExport.UseVisualStyleBackColor = true;
            this.btnListalMovieXMLExport.Click += new System.EventHandler(this.btnListalMovieXMLExport_Click);
            // 
            // txtListalMovieXMLExport
            // 
            this.txtListalMovieXMLExport.Location = new System.Drawing.Point(155, 56);
            this.txtListalMovieXMLExport.Name = "txtListalMovieXMLExport";
            this.txtListalMovieXMLExport.Size = new System.Drawing.Size(231, 20);
            this.txtListalMovieXMLExport.TabIndex = 2;
            this.txtListalMovieXMLExport.TextChanged += new System.EventHandler(this.txtListalMovieXMLExport_TextChanged);
            // 
            // lblListalMovieExportFile
            // 
            this.lblListalMovieExportFile.AutoSize = true;
            this.lblListalMovieExportFile.Location = new System.Drawing.Point(16, 60);
            this.lblListalMovieExportFile.Name = "lblListalMovieExportFile";
            this.lblListalMovieExportFile.Size = new System.Drawing.Size(91, 13);
            this.lblListalMovieExportFile.TabIndex = 1;
            this.lblListalMovieExportFile.Text = "Movie Export File:";
            // 
            // grbCriticker
            // 
            this.grbCriticker.Controls.Add(this.btnCritickerMovieExportBrowse);
            this.grbCriticker.Controls.Add(this.chkCritickerEnabled);
            this.grbCriticker.Controls.Add(this.txtCritickerMovieExportFile);
            this.grbCriticker.Controls.Add(this.lblCritickerMovieExportFile);
            this.grbCriticker.Location = new System.Drawing.Point(13, 487);
            this.grbCriticker.Name = "grbCriticker";
            this.grbCriticker.Size = new System.Drawing.Size(442, 97);
            this.grbCriticker.TabIndex = 7;
            this.grbCriticker.TabStop = false;
            this.grbCriticker.Text = "Criticker";
            // 
            // btnCritickerMovieExportBrowse
            // 
            this.btnCritickerMovieExportBrowse.Location = new System.Drawing.Point(388, 53);
            this.btnCritickerMovieExportBrowse.Name = "btnCritickerMovieExportBrowse";
            this.btnCritickerMovieExportBrowse.Size = new System.Drawing.Size(29, 23);
            this.btnCritickerMovieExportBrowse.TabIndex = 3;
            this.btnCritickerMovieExportBrowse.Text = "...";
            this.btnCritickerMovieExportBrowse.UseVisualStyleBackColor = true;
            this.btnCritickerMovieExportBrowse.Click += new System.EventHandler(this.btnCritickerMovieExportBrowse_Click);
            // 
            // chkCritickerEnabled
            // 
            this.chkCritickerEnabled.AutoSize = true;
            this.chkCritickerEnabled.Location = new System.Drawing.Point(20, 29);
            this.chkCritickerEnabled.Name = "chkCritickerEnabled";
            this.chkCritickerEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkCritickerEnabled.TabIndex = 0;
            this.chkCritickerEnabled.Text = "Enabled";
            this.chkCritickerEnabled.UseVisualStyleBackColor = true;
            this.chkCritickerEnabled.CheckedChanged += new System.EventHandler(this.chkCritickerEnabled_CheckedChanged);
            // 
            // txtCritickerMovieExportFile
            // 
            this.txtCritickerMovieExportFile.Location = new System.Drawing.Point(151, 55);
            this.txtCritickerMovieExportFile.Name = "txtCritickerMovieExportFile";
            this.txtCritickerMovieExportFile.Size = new System.Drawing.Size(231, 20);
            this.txtCritickerMovieExportFile.TabIndex = 2;
            this.txtCritickerMovieExportFile.TextChanged += new System.EventHandler(this.txtCritickerMovieExportFile_TextChanged);
            // 
            // lblCritickerMovieExportFile
            // 
            this.lblCritickerMovieExportFile.AutoSize = true;
            this.lblCritickerMovieExportFile.Location = new System.Drawing.Point(17, 58);
            this.lblCritickerMovieExportFile.Name = "lblCritickerMovieExportFile";
            this.lblCritickerMovieExportFile.Size = new System.Drawing.Size(91, 13);
            this.lblCritickerMovieExportFile.TabIndex = 1;
            this.lblCritickerMovieExportFile.Text = "Movie Export File:";
            // 
            // lblWarnPeriod
            // 
            this.lblWarnPeriod.AutoSize = true;
            this.lblWarnPeriod.Location = new System.Drawing.Point(181, 91);
            this.lblWarnPeriod.Name = "lblWarnPeriod";
            this.lblWarnPeriod.Size = new System.Drawing.Size(211, 13);
            this.lblWarnPeriod.TabIndex = 17;
            this.lblWarnPeriod.Text = "You have 15 mins enter pin and start import";
            // 
            // TraktRater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 716);
            this.Controls.Add(this.grbCriticker);
            this.Controls.Add(this.grbListal);
            this.Controls.Add(this.grbOptions);
            this.Controls.Add(this.grbTMDb);
            this.Controls.Add(this.grbImdb);
            this.Controls.Add(this.grbReport);
            this.Controls.Add(this.pbrImportProgress);
            this.Controls.Add(this.btnStartSync);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.grbTVDb);
            this.Controls.Add(this.grbTrakt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TraktRater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trakt Rater";
            this.grbTrakt.ResumeLayout(false);
            this.grbTrakt.PerformLayout();
            this.grbTVDb.ResumeLayout(false);
            this.grbTVDb.PerformLayout();
            this.grbReport.ResumeLayout(false);
            this.grbReport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBatchSize)).EndInit();
            this.grbImdb.ResumeLayout(false);
            this.grbImdb.PerformLayout();
            this.grbTMDb.ResumeLayout(false);
            this.grbTMDb.PerformLayout();
            this.grbOptions.ResumeLayout(false);
            this.grbOptions.PerformLayout();
            this.grbListal.ResumeLayout(false);
            this.grbListal.PerformLayout();
            this.grbCriticker.ResumeLayout(false);
            this.grbCriticker.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grbTrakt;
        private System.Windows.Forms.TextBox txtTraktPassword;
        private System.Windows.Forms.Label lblTraktPassword;
        private System.Windows.Forms.Label lblTraktUser;
        private System.Windows.Forms.TextBox txtTraktUser;
        private System.Windows.Forms.GroupBox grbTVDb;
        private System.Windows.Forms.TextBox txtTVDbAccountId;
        private System.Windows.Forms.Label lblTVDbAccountId;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Button btnStartSync;
        private System.Windows.Forms.ProgressBar pbrImportProgress;
        private System.Windows.Forms.GroupBox grbReport;
        public System.Windows.Forms.Label lblStatusMessage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolTip tipHelp;
        private System.Windows.Forms.GroupBox grbImdb;
        private System.Windows.Forms.Button btnImdbRatingsBrowse;
        private System.Windows.Forms.TextBox txtImdbRatingsFilename;
        private System.Windows.Forms.OpenFileDialog dlgFileOpen;
        private System.Windows.Forms.GroupBox grbTMDb;
        private System.Windows.Forms.LinkLabel lnkTMDbStart;
        private System.Windows.Forms.Label lblTMDbMessage;
        private System.Windows.Forms.GroupBox grbOptions;
        private System.Windows.Forms.CheckBox chkMarkAsWatched;
        private System.Windows.Forms.TextBox txtImdbWebUsername;
        private System.Windows.Forms.CheckBox chkImdbWebWatchlist;
        private System.Windows.Forms.RadioButton rdnImdbUsername;
        private System.Windows.Forms.RadioButton rdnImdbCSV;
        private System.Windows.Forms.Label lblImdbDescription;
        private System.Windows.Forms.Button btnImdbWatchlistBrowse;
        private System.Windows.Forms.TextBox txtImdbWatchlistFile;
        private System.Windows.Forms.Label lblImdbWatchlistFile;
        private System.Windows.Forms.Label lblImdbRatingsFile;
        private System.Windows.Forms.CheckBox chkIgnoreWatchedForWatchlists;
        private System.Windows.Forms.GroupBox grbListal;
        private System.Windows.Forms.TextBox txtListalMovieXMLExport;
        private System.Windows.Forms.Label lblListalMovieExportFile;
        private System.Windows.Forms.Button btnListalMovieXMLExport;
        private System.Windows.Forms.CheckBox chkListalWebWatchlist;
        private System.Windows.Forms.LinkLabel lnkListalExport;
        private System.Windows.Forms.Label lblListalLinkInfo;
        private System.Windows.Forms.Label lblListalShowExportFile;
        private System.Windows.Forms.Button btnListalShowXMLExport;
        private System.Windows.Forms.TextBox txtListalShowXMLExport;
        private System.Windows.Forms.CheckBox chkTVDbEnabled;
        private System.Windows.Forms.CheckBox chkIMDbEnabled;
        private System.Windows.Forms.CheckBox chkTMDbEnabled;
        private System.Windows.Forms.CheckBox chkListalEnabled;
        private System.Windows.Forms.CheckBox chkTMDbSyncWatchlist;
        private System.Windows.Forms.GroupBox grbCriticker;
        private System.Windows.Forms.Button btnCritickerMovieExportBrowse;
        private System.Windows.Forms.CheckBox chkCritickerEnabled;
        private System.Windows.Forms.TextBox txtCritickerMovieExportFile;
        private System.Windows.Forms.Label lblCritickerMovieExportFile;
        private System.Windows.Forms.LinkLabel lnkLogFolder;
        private System.Windows.Forms.Label lblBatchImportSize;
        private System.Windows.Forms.NumericUpDown nudBatchSize;
        private System.Windows.Forms.Button btnMaintenance;
        private System.Windows.Forms.RadioButton radTraktPinCode;
        private System.Windows.Forms.RadioButton radTraktUserPass;
        private System.Windows.Forms.Label lblTraktAuthMethod;
        private System.Windows.Forms.LinkLabel lnkTraktOAuth;
        private System.Windows.Forms.TextBox txtTraktPinCode;
        private System.Windows.Forms.Label lblWarnPeriod;
    }
}

