﻿namespace TraktRater
{
    partial class TraktRaterApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TraktRaterApp));
            this.grbTrakt = new System.Windows.Forms.GroupBox();
            this.lblWarnPeriod = new System.Windows.Forms.Label();
            this.txtTraktPinCode = new System.Windows.Forms.TextBox();
            this.lnkTraktOAuth = new System.Windows.Forms.LinkLabel();
            this.btnMaintenance = new System.Windows.Forms.Button();
            this.grbTVDb = new System.Windows.Forms.GroupBox();
            this.chkTVDbEnabled = new System.Windows.Forms.CheckBox();
            this.txtTVDbAccountId = new System.Windows.Forms.TextBox();
            this.lblTVDbAccountId = new System.Windows.Forms.Label();
            this.btnStartSync = new System.Windows.Forms.Button();
            this.pbrImportProgress = new System.Windows.Forms.ProgressBar();
            this.grbReport = new System.Windows.Forms.GroupBox();
            this.lnkLogFolder = new System.Windows.Forms.LinkLabel();
            this.lblStatusMessage = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.nudBatchSize = new System.Windows.Forms.NumericUpDown();
            this.txtLetterboxdDiaryFile = new System.Windows.Forms.TextBox();
            this.chkCheckMoviesAddWatchedToWatchlist = new System.Windows.Forms.CheckBox();
            this.txtMovieLensActivity = new System.Windows.Forms.TextBox();
            this.txtMovieLensTags = new System.Windows.Forms.TextBox();
            this.txtCheckMoviesCsvFile = new System.Windows.Forms.TextBox();
            this.txtLetterboxdRatingsFile = new System.Windows.Forms.TextBox();
            this.txtLetterboxdWatchedFile = new System.Windows.Forms.TextBox();
            this.dlgFileOpen = new System.Windows.Forms.OpenFileDialog();
            this.grbTMDb = new System.Windows.Forms.GroupBox();
            this.chkTMDbSyncWatchlist = new System.Windows.Forms.CheckBox();
            this.chkTMDbEnabled = new System.Windows.Forms.CheckBox();
            this.lnkTMDbStart = new System.Windows.Forms.LinkLabel();
            this.lblTMDbMessage = new System.Windows.Forms.Label();
            this.grbOptions = new System.Windows.Forms.GroupBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.chkSetWatchedOnReleaseDay = new System.Windows.Forms.CheckBox();
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
            this.btnCritickerCSVExportBrowse = new System.Windows.Forms.Button();
            this.chkCritickerEnabled = new System.Windows.Forms.CheckBox();
            this.txtCritickerCSVExportFile = new System.Windows.Forms.TextBox();
            this.lblCritickerCSVExportFile = new System.Windows.Forms.Label();
            this.tabTraktRater = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grbPaypal = new System.Windows.Forms.GroupBox();
            this.lnkPaypal = new System.Windows.Forms.LinkLabel();
            this.pbPaypal = new System.Windows.Forms.PictureBox();
            this.lblPaypal = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.grbLetterboxd = new System.Windows.Forms.GroupBox();
            this.lblLetterboxdCustomList = new System.Windows.Forms.Label();
            this.btnLetterBoxRemoveList = new System.Windows.Forms.Button();
            this.btnLetterBoxAddList = new System.Windows.Forms.Button();
            this.listLetterboxdCustomLists = new System.Windows.Forms.ListBox();
            this.lblLetterboxdWatched = new System.Windows.Forms.Label();
            this.btnLetterboxdWatchedBrowse = new System.Windows.Forms.Button();
            this.lblLetterboxdDiary = new System.Windows.Forms.Label();
            this.btnLetterboxdDiaryBrowse = new System.Windows.Forms.Button();
            this.lblLetterboxdRatingsFile = new System.Windows.Forms.Label();
            this.btnLetterboxdRatingsBrowse = new System.Windows.Forms.Button();
            this.chkLetterboxdEnabled = new System.Windows.Forms.CheckBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.grbFlixster = new System.Windows.Forms.GroupBox();
            this.chkFlixsterSyncWantToSee = new System.Windows.Forms.CheckBox();
            this.lblFlixsterUserIdDesc = new System.Windows.Forms.Label();
            this.lblFlisterUserId = new System.Windows.Forms.Label();
            this.txtFlixsterUserId = new System.Windows.Forms.TextBox();
            this.chkFlixsterEnabled = new System.Windows.Forms.CheckBox();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.grbCheckMovies = new System.Windows.Forms.GroupBox();
            this.chkCheckMoviesAddMoviesToCollection = new System.Windows.Forms.CheckBox();
            this.lblCheckMoviesDelimiter = new System.Windows.Forms.Label();
            this.cboCheckMoviesDelimiter = new System.Windows.Forms.ComboBox();
            this.chkCheckMoviesUpdateWatchedStatus = new System.Windows.Forms.CheckBox();
            this.chkCheckMoviesEnabled = new System.Windows.Forms.CheckBox();
            this.btnCheckMoviesExportBrowse = new System.Windows.Forms.Button();
            this.lblCheckMoviesFile = new System.Windows.Forms.Label();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.grbToDoMovies = new System.Windows.Forms.GroupBox();
            this.btnToDoMoviesExportBrowse = new System.Windows.Forms.Button();
            this.chkToDoMoviesEnabled = new System.Windows.Forms.CheckBox();
            this.txtToDoMovieExportFile = new System.Windows.Forms.TextBox();
            this.lblToDoMovieExportFile = new System.Windows.Forms.Label();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.grbMovieLens = new System.Windows.Forms.GroupBox();
            this.lblMovieLensNote = new System.Windows.Forms.Label();
            this.lblMovieLensTags = new System.Windows.Forms.Label();
            this.btnMovieLensTags = new System.Windows.Forms.Button();
            this.lblMovieLensWishlist = new System.Windows.Forms.Label();
            this.btnMovieLensWishlist = new System.Windows.Forms.Button();
            this.txtMovieLensWishlist = new System.Windows.Forms.TextBox();
            this.lblMovieLensActivity = new System.Windows.Forms.Label();
            this.btnMovieLensActivity = new System.Windows.Forms.Button();
            this.lblMovieLensRatings = new System.Windows.Forms.Label();
            this.btnMovieLensRatings = new System.Windows.Forms.Button();
            this.txtMovieLensRatings = new System.Windows.Forms.TextBox();
            this.chkMovieLensEnabled = new System.Windows.Forms.CheckBox();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.ctrlImdb = new TraktRater.UI.Controls.IMDbUserControl();
            this.grbTrakt.SuspendLayout();
            this.grbTVDb.SuspendLayout();
            this.grbReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBatchSize)).BeginInit();
            this.grbTMDb.SuspendLayout();
            this.grbOptions.SuspendLayout();
            this.grbListal.SuspendLayout();
            this.grbCriticker.SuspendLayout();
            this.tabTraktRater.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.grbPaypal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPaypal)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.grbLetterboxd.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.grbFlixster.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.grbCheckMovies.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.grbToDoMovies.SuspendLayout();
            this.tabPage12.SuspendLayout();
            this.grbMovieLens.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbTrakt
            // 
            this.grbTrakt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbTrakt.Controls.Add(this.lblWarnPeriod);
            this.grbTrakt.Controls.Add(this.txtTraktPinCode);
            this.grbTrakt.Controls.Add(this.lnkTraktOAuth);
            this.grbTrakt.Location = new System.Drawing.Point(6, 6);
            this.grbTrakt.Name = "grbTrakt";
            this.grbTrakt.Size = new System.Drawing.Size(627, 113);
            this.grbTrakt.TabIndex = 0;
            this.grbTrakt.TabStop = false;
            this.grbTrakt.Text = "Trakt";
            // 
            // lblWarnPeriod
            // 
            this.lblWarnPeriod.AutoSize = true;
            this.lblWarnPeriod.Location = new System.Drawing.Point(19, 72);
            this.lblWarnPeriod.Name = "lblWarnPeriod";
            this.lblWarnPeriod.Size = new System.Drawing.Size(250, 13);
            this.lblWarnPeriod.TabIndex = 17;
            this.lblWarnPeriod.Text = "You have 15 mins to enter pin code and start import";
            // 
            // txtTraktPinCode
            // 
            this.txtTraktPinCode.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtTraktPinCode.Location = new System.Drawing.Point(19, 46);
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
            this.lnkTraktOAuth.Location = new System.Drawing.Point(19, 23);
            this.lnkTraktOAuth.Name = "lnkTraktOAuth";
            this.lnkTraktOAuth.Size = new System.Drawing.Size(203, 13);
            this.lnkTraktOAuth.TabIndex = 15;
            this.lnkTraktOAuth.TabStop = true;
            this.lnkTraktOAuth.Text = "Click to Authorise access to your account";
            this.lnkTraktOAuth.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTraktOAuth_LinkClicked);
            // 
            // btnMaintenance
            // 
            this.btnMaintenance.Location = new System.Drawing.Point(21, 179);
            this.btnMaintenance.Name = "btnMaintenance";
            this.btnMaintenance.Size = new System.Drawing.Size(276, 27);
            this.btnMaintenance.TabIndex = 5;
            this.btnMaintenance.Text = "Cleanup / Maintenance...";
            this.btnMaintenance.UseVisualStyleBackColor = true;
            this.btnMaintenance.Click += new System.EventHandler(this.btnMaintenance_Click);
            // 
            // grbTVDb
            // 
            this.grbTVDb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbTVDb.Controls.Add(this.chkTVDbEnabled);
            this.grbTVDb.Controls.Add(this.txtTVDbAccountId);
            this.grbTVDb.Controls.Add(this.lblTVDbAccountId);
            this.grbTVDb.Location = new System.Drawing.Point(6, 6);
            this.grbTVDb.Name = "grbTVDb";
            this.grbTVDb.Size = new System.Drawing.Size(627, 363);
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
            this.txtTVDbAccountId.Location = new System.Drawing.Point(175, 47);
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
            this.lblTVDbAccountId.Location = new System.Drawing.Point(16, 50);
            this.lblTVDbAccountId.Name = "lblTVDbAccountId";
            this.lblTVDbAccountId.Size = new System.Drawing.Size(93, 13);
            this.lblTVDbAccountId.TabIndex = 1;
            this.lblTVDbAccountId.Text = "Account Identifier:";
            // 
            // btnStartSync
            // 
            this.btnStartSync.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartSync.Location = new System.Drawing.Point(4, 417);
            this.btnStartSync.Name = "btnStartSync";
            this.btnStartSync.Size = new System.Drawing.Size(648, 27);
            this.btnStartSync.TabIndex = 8;
            this.btnStartSync.Text = "Start Import";
            this.btnStartSync.UseVisualStyleBackColor = true;
            this.btnStartSync.Click += new System.EventHandler(this.btnStartSync_Click);
            // 
            // pbrImportProgress
            // 
            this.pbrImportProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbrImportProgress.Location = new System.Drawing.Point(5, 448);
            this.pbrImportProgress.Name = "pbrImportProgress";
            this.pbrImportProgress.Size = new System.Drawing.Size(647, 23);
            this.pbrImportProgress.TabIndex = 9;
            // 
            // grbReport
            // 
            this.grbReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbReport.Controls.Add(this.lnkLogFolder);
            this.grbReport.Controls.Add(this.lblStatusMessage);
            this.grbReport.Controls.Add(this.label5);
            this.grbReport.Location = new System.Drawing.Point(4, 473);
            this.grbReport.Name = "grbReport";
            this.grbReport.Size = new System.Drawing.Size(648, 50);
            this.grbReport.TabIndex = 10;
            this.grbReport.TabStop = false;
            this.grbReport.Text = "Report";
            // 
            // lnkLogFolder
            // 
            this.lnkLogFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkLogFolder.AutoSize = true;
            this.lnkLogFolder.Location = new System.Drawing.Point(551, 20);
            this.lnkLogFolder.Name = "lnkLogFolder";
            this.lnkLogFolder.Size = new System.Drawing.Size(86, 13);
            this.lnkLogFolder.TabIndex = 2;
            this.lnkLogFolder.TabStop = true;
            this.lnkLogFolder.Text = "Open Log Folder";
            this.lnkLogFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLogFolder_LinkClicked);
            // 
            // lblStatusMessage
            // 
            this.lblStatusMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatusMessage.AutoEllipsis = true;
            this.lblStatusMessage.Location = new System.Drawing.Point(77, 18);
            this.lblStatusMessage.Name = "lblStatusMessage";
            this.lblStatusMessage.Size = new System.Drawing.Size(460, 23);
            this.lblStatusMessage.TabIndex = 1;
            this.lblStatusMessage.Text = "Ready for anything!";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.Location = new System.Drawing.Point(7, 18);
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
            // nudBatchSize
            // 
            this.nudBatchSize.Location = new System.Drawing.Point(134, 101);
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
            this.nudBatchSize.TabIndex = 4;
            this.tipHelp.SetToolTip(this.nudBatchSize, "Set the size of the batch when importing items to trakt.tv. Set lower if having i" +
        "ssues with the server.");
            this.nudBatchSize.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudBatchSize.ValueChanged += new System.EventHandler(this.nudBatchSize_ValueChanged);
            // 
            // txtLetterboxdDiaryFile
            // 
            this.txtLetterboxdDiaryFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtLetterboxdDiaryFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtLetterboxdDiaryFile.Enabled = false;
            this.txtLetterboxdDiaryFile.Location = new System.Drawing.Point(20, 173);
            this.txtLetterboxdDiaryFile.Name = "txtLetterboxdDiaryFile";
            this.txtLetterboxdDiaryFile.Size = new System.Drawing.Size(288, 20);
            this.txtLetterboxdDiaryFile.TabIndex = 0;
            this.tipHelp.SetToolTip(this.txtLetterboxdDiaryFile, "The Diary file includes everything marked as watched at a specified date, this da" +
        "te will override any movies found in the Watched file");
            this.txtLetterboxdDiaryFile.TextChanged += new System.EventHandler(this.txtLetterboxdDiaryFile_TextChanged);
            // 
            // chkCheckMoviesAddWatchedToWatchlist
            // 
            this.chkCheckMoviesAddWatchedToWatchlist.AutoSize = true;
            this.chkCheckMoviesAddWatchedToWatchlist.Location = new System.Drawing.Point(25, 159);
            this.chkCheckMoviesAddWatchedToWatchlist.Name = "chkCheckMoviesAddWatchedToWatchlist";
            this.chkCheckMoviesAddWatchedToWatchlist.Size = new System.Drawing.Size(438, 17);
            this.chkCheckMoviesAddWatchedToWatchlist.TabIndex = 6;
            this.chkCheckMoviesAddWatchedToWatchlist.Text = "Add watched movies to watchlist (If unchecked, only movies in \'watchlist\' will be" +
    " added)";
            this.tipHelp.SetToolTip(this.chkCheckMoviesAddWatchedToWatchlist, resources.GetString("chkCheckMoviesAddWatchedToWatchlist.ToolTip"));
            this.chkCheckMoviesAddWatchedToWatchlist.UseVisualStyleBackColor = true;
            this.chkCheckMoviesAddWatchedToWatchlist.CheckedChanged += new System.EventHandler(this.chkCheckMoviesAddWatchedToWatchlist_CheckedChanged);
            // 
            // txtMovieLensActivity
            // 
            this.txtMovieLensActivity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtMovieLensActivity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtMovieLensActivity.Enabled = false;
            this.txtMovieLensActivity.Location = new System.Drawing.Point(17, 174);
            this.txtMovieLensActivity.Name = "txtMovieLensActivity";
            this.txtMovieLensActivity.Size = new System.Drawing.Size(288, 20);
            this.txtMovieLensActivity.TabIndex = 1;
            this.tipHelp.SetToolTip(this.txtMovieLensActivity, "The Diary file includes everything marked as watched at a specified date, this da" +
        "te will override any movies found in the Watched file");
            this.txtMovieLensActivity.TextChanged += new System.EventHandler(this.txtMovieLensActivity_TextChanged);
            // 
            // txtMovieLensTags
            // 
            this.txtMovieLensTags.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtMovieLensTags.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtMovieLensTags.Enabled = false;
            this.txtMovieLensTags.Location = new System.Drawing.Point(19, 310);
            this.txtMovieLensTags.Name = "txtMovieLensTags";
            this.txtMovieLensTags.Size = new System.Drawing.Size(288, 20);
            this.txtMovieLensTags.TabIndex = 11;
            this.tipHelp.SetToolTip(this.txtMovieLensTags, "The Diary file includes everything marked as watched at a specified date, this da" +
        "te will override any movies found in the Watched file");
            this.txtMovieLensTags.Visible = false;
            this.txtMovieLensTags.TextChanged += new System.EventHandler(this.txtMovieLensTags_TextChanged);
            // 
            // txtCheckMoviesCsvFile
            // 
            this.txtCheckMoviesCsvFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtCheckMoviesCsvFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtCheckMoviesCsvFile.Enabled = false;
            this.txtCheckMoviesCsvFile.Location = new System.Drawing.Point(25, 77);
            this.txtCheckMoviesCsvFile.Name = "txtCheckMoviesCsvFile";
            this.txtCheckMoviesCsvFile.Size = new System.Drawing.Size(208, 20);
            this.txtCheckMoviesCsvFile.TabIndex = 2;
            this.txtCheckMoviesCsvFile.TextChanged += new System.EventHandler(this.txtCheckMoviesCsvFile_TextChanged);
            // 
            // txtLetterboxdRatingsFile
            // 
            this.txtLetterboxdRatingsFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtLetterboxdRatingsFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtLetterboxdRatingsFile.Enabled = false;
            this.txtLetterboxdRatingsFile.Location = new System.Drawing.Point(20, 77);
            this.txtLetterboxdRatingsFile.Name = "txtLetterboxdRatingsFile";
            this.txtLetterboxdRatingsFile.Size = new System.Drawing.Size(288, 20);
            this.txtLetterboxdRatingsFile.TabIndex = 2;
            this.txtLetterboxdRatingsFile.TextChanged += new System.EventHandler(this.txtLetterboxdRatingsFile_TextChanged);
            // 
            // txtLetterboxdWatchedFile
            // 
            this.txtLetterboxdWatchedFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtLetterboxdWatchedFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtLetterboxdWatchedFile.Enabled = false;
            this.txtLetterboxdWatchedFile.Location = new System.Drawing.Point(20, 122);
            this.txtLetterboxdWatchedFile.Name = "txtLetterboxdWatchedFile";
            this.txtLetterboxdWatchedFile.Size = new System.Drawing.Size(288, 20);
            this.txtLetterboxdWatchedFile.TabIndex = 6;
            this.txtLetterboxdWatchedFile.TextChanged += new System.EventHandler(this.txtLetterboxdWatchedFile_TextChanged);
            // 
            // dlgFileOpen
            // 
            this.dlgFileOpen.DefaultExt = "csv";
            // 
            // grbTMDb
            // 
            this.grbTMDb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbTMDb.Controls.Add(this.chkTMDbSyncWatchlist);
            this.grbTMDb.Controls.Add(this.chkTMDbEnabled);
            this.grbTMDb.Controls.Add(this.lnkTMDbStart);
            this.grbTMDb.Controls.Add(this.lblTMDbMessage);
            this.grbTMDb.Location = new System.Drawing.Point(6, 6);
            this.grbTMDb.Name = "grbTMDb";
            this.grbTMDb.Size = new System.Drawing.Size(627, 359);
            this.grbTMDb.TabIndex = 4;
            this.grbTMDb.TabStop = false;
            this.grbTMDb.Text = "TMDb";
            // 
            // chkTMDbSyncWatchlist
            // 
            this.chkTMDbSyncWatchlist.AutoSize = true;
            this.chkTMDbSyncWatchlist.Location = new System.Drawing.Point(20, 43);
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
            this.lnkTMDbStart.Location = new System.Drawing.Point(18, 101);
            this.lnkTMDbStart.Name = "lnkTMDbStart";
            this.lnkTMDbStart.Size = new System.Drawing.Size(113, 13);
            this.lnkTMDbStart.TabIndex = 3;
            this.lnkTMDbStart.TabStop = true;
            this.lnkTMDbStart.Text = "Start Request Process";
            this.lnkTMDbStart.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTMDbStart_LinkClicked);
            // 
            // lblTMDbMessage
            // 
            this.lblTMDbMessage.Location = new System.Drawing.Point(17, 65);
            this.lblTMDbMessage.Name = "lblTMDbMessage";
            this.lblTMDbMessage.Size = new System.Drawing.Size(406, 45);
            this.lblTMDbMessage.TabIndex = 2;
            this.lblTMDbMessage.Text = "To get user ratings from TMDb you must first allow this application to access you" +
    "r account details. This needs to be done by you in a webbrowser.";
            // 
            // grbOptions
            // 
            this.grbOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbOptions.Controls.Add(this.btnExport);
            this.grbOptions.Controls.Add(this.chkSetWatchedOnReleaseDay);
            this.grbOptions.Controls.Add(this.nudBatchSize);
            this.grbOptions.Controls.Add(this.lblBatchImportSize);
            this.grbOptions.Controls.Add(this.chkIgnoreWatchedForWatchlists);
            this.grbOptions.Controls.Add(this.chkMarkAsWatched);
            this.grbOptions.Controls.Add(this.btnMaintenance);
            this.grbOptions.Location = new System.Drawing.Point(6, 6);
            this.grbOptions.Name = "grbOptions";
            this.grbOptions.Size = new System.Drawing.Size(627, 357);
            this.grbOptions.TabIndex = 0;
            this.grbOptions.TabStop = false;
            this.grbOptions.Text = "Options";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(21, 212);
            this.btnExport.Margin = new System.Windows.Forms.Padding(2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(276, 27);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "Export from trakt to CSV...";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // chkSetWatchedOnReleaseDay
            // 
            this.chkSetWatchedOnReleaseDay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSetWatchedOnReleaseDay.AutoSize = true;
            this.chkSetWatchedOnReleaseDay.Location = new System.Drawing.Point(21, 68);
            this.chkSetWatchedOnReleaseDay.Name = "chkSetWatchedOnReleaseDay";
            this.chkSetWatchedOnReleaseDay.Size = new System.Drawing.Size(501, 17);
            this.chkSetWatchedOnReleaseDay.TabIndex = 2;
            this.chkSetWatchedOnReleaseDay.Text = "Set the watched date to the release date of the movie or episode when data is not" +
    " available in export";
            this.chkSetWatchedOnReleaseDay.UseVisualStyleBackColor = true;
            this.chkSetWatchedOnReleaseDay.CheckedChanged += new System.EventHandler(this.chkSetWatchedOnReleaseDay_CheckedChanged);
            // 
            // lblBatchImportSize
            // 
            this.lblBatchImportSize.AutoSize = true;
            this.lblBatchImportSize.Location = new System.Drawing.Point(18, 107);
            this.lblBatchImportSize.Name = "lblBatchImportSize";
            this.lblBatchImportSize.Size = new System.Drawing.Size(93, 13);
            this.lblBatchImportSize.TabIndex = 3;
            this.lblBatchImportSize.Text = "Batch Import Size:";
            // 
            // chkIgnoreWatchedForWatchlists
            // 
            this.chkIgnoreWatchedForWatchlists.AutoSize = true;
            this.chkIgnoreWatchedForWatchlists.Location = new System.Drawing.Point(21, 43);
            this.chkIgnoreWatchedForWatchlists.Name = "chkIgnoreWatchedForWatchlists";
            this.chkIgnoreWatchedForWatchlists.Size = new System.Drawing.Size(277, 17);
            this.chkIgnoreWatchedForWatchlists.TabIndex = 1;
            this.chkIgnoreWatchedForWatchlists.Text = "Ignore watched items when syncing to your Watchlist";
            this.chkIgnoreWatchedForWatchlists.UseVisualStyleBackColor = true;
            this.chkIgnoreWatchedForWatchlists.Click += new System.EventHandler(this.chkIgnoreWatchedForWatchlists_Click);
            // 
            // chkMarkAsWatched
            // 
            this.chkMarkAsWatched.AutoSize = true;
            this.chkMarkAsWatched.Location = new System.Drawing.Point(21, 19);
            this.chkMarkAsWatched.Name = "chkMarkAsWatched";
            this.chkMarkAsWatched.Size = new System.Drawing.Size(322, 17);
            this.chkMarkAsWatched.TabIndex = 0;
            this.chkMarkAsWatched.Text = "Mark episodes and movies as watched if they have been rated";
            this.chkMarkAsWatched.UseVisualStyleBackColor = true;
            this.chkMarkAsWatched.Click += new System.EventHandler(this.chkMarkAsWatched_Click);
            // 
            // grbListal
            // 
            this.grbListal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.grbListal.Location = new System.Drawing.Point(6, 6);
            this.grbListal.Name = "grbListal";
            this.grbListal.Size = new System.Drawing.Size(627, 367);
            this.grbListal.TabIndex = 6;
            this.grbListal.TabStop = false;
            this.grbListal.Text = "Listal";
            // 
            // chkListalEnabled
            // 
            this.chkListalEnabled.AutoSize = true;
            this.chkListalEnabled.Location = new System.Drawing.Point(20, 27);
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
            this.lblListalShowExportFile.Location = new System.Drawing.Point(16, 92);
            this.lblListalShowExportFile.Name = "lblListalShowExportFile";
            this.lblListalShowExportFile.Size = new System.Drawing.Size(106, 13);
            this.lblListalShowExportFile.TabIndex = 4;
            this.lblListalShowExportFile.Text = "TV Show Export File:";
            // 
            // btnListalShowXMLExport
            // 
            this.btnListalShowXMLExport.Location = new System.Drawing.Point(392, 81);
            this.btnListalShowXMLExport.Name = "btnListalShowXMLExport";
            this.btnListalShowXMLExport.Size = new System.Drawing.Size(29, 23);
            this.btnListalShowXMLExport.TabIndex = 6;
            this.btnListalShowXMLExport.Text = "...";
            this.btnListalShowXMLExport.UseVisualStyleBackColor = true;
            this.btnListalShowXMLExport.Click += new System.EventHandler(this.btnListalShowXMLExport_Click);
            // 
            // txtListalShowXMLExport
            // 
            this.txtListalShowXMLExport.Location = new System.Drawing.Point(155, 84);
            this.txtListalShowXMLExport.Name = "txtListalShowXMLExport";
            this.txtListalShowXMLExport.Size = new System.Drawing.Size(231, 20);
            this.txtListalShowXMLExport.TabIndex = 5;
            this.txtListalShowXMLExport.TextChanged += new System.EventHandler(this.txtListalShowXMLExport_TextChanged);
            // 
            // lblListalLinkInfo
            // 
            this.lblListalLinkInfo.AutoSize = true;
            this.lblListalLinkInfo.Location = new System.Drawing.Point(17, 153);
            this.lblListalLinkInfo.Name = "lblListalLinkInfo";
            this.lblListalLinkInfo.Size = new System.Drawing.Size(346, 13);
            this.lblListalLinkInfo.TabIndex = 8;
            this.lblListalLinkInfo.Text = "Logon into the Listal website, then download export files from link below:";
            // 
            // lnkListalExport
            // 
            this.lnkListalExport.AutoSize = true;
            this.lnkListalExport.Location = new System.Drawing.Point(17, 173);
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
            this.chkListalWebWatchlist.Location = new System.Drawing.Point(155, 119);
            this.chkListalWebWatchlist.Name = "chkListalWebWatchlist";
            this.chkListalWebWatchlist.Size = new System.Drawing.Size(150, 17);
            this.chkListalWebWatchlist.TabIndex = 7;
            this.chkListalWebWatchlist.Text = "Sync Wantlist to Watchlist";
            this.chkListalWebWatchlist.UseVisualStyleBackColor = true;
            this.chkListalWebWatchlist.CheckedChanged += new System.EventHandler(this.chkListalWebWatchlist_CheckedChanged);
            // 
            // btnListalMovieXMLExport
            // 
            this.btnListalMovieXMLExport.Location = new System.Drawing.Point(392, 55);
            this.btnListalMovieXMLExport.Name = "btnListalMovieXMLExport";
            this.btnListalMovieXMLExport.Size = new System.Drawing.Size(29, 23);
            this.btnListalMovieXMLExport.TabIndex = 3;
            this.btnListalMovieXMLExport.Text = "...";
            this.btnListalMovieXMLExport.UseVisualStyleBackColor = true;
            this.btnListalMovieXMLExport.Click += new System.EventHandler(this.btnListalMovieXMLExport_Click);
            // 
            // txtListalMovieXMLExport
            // 
            this.txtListalMovieXMLExport.Location = new System.Drawing.Point(155, 57);
            this.txtListalMovieXMLExport.Name = "txtListalMovieXMLExport";
            this.txtListalMovieXMLExport.Size = new System.Drawing.Size(231, 20);
            this.txtListalMovieXMLExport.TabIndex = 2;
            this.txtListalMovieXMLExport.TextChanged += new System.EventHandler(this.txtListalMovieXMLExport_TextChanged);
            // 
            // lblListalMovieExportFile
            // 
            this.lblListalMovieExportFile.AutoSize = true;
            this.lblListalMovieExportFile.Location = new System.Drawing.Point(16, 61);
            this.lblListalMovieExportFile.Name = "lblListalMovieExportFile";
            this.lblListalMovieExportFile.Size = new System.Drawing.Size(91, 13);
            this.lblListalMovieExportFile.TabIndex = 1;
            this.lblListalMovieExportFile.Text = "Movie Export File:";
            // 
            // grbCriticker
            // 
            this.grbCriticker.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbCriticker.Controls.Add(this.btnCritickerCSVExportBrowse);
            this.grbCriticker.Controls.Add(this.chkCritickerEnabled);
            this.grbCriticker.Controls.Add(this.txtCritickerCSVExportFile);
            this.grbCriticker.Controls.Add(this.lblCritickerCSVExportFile);
            this.grbCriticker.Location = new System.Drawing.Point(6, 6);
            this.grbCriticker.Name = "grbCriticker";
            this.grbCriticker.Size = new System.Drawing.Size(627, 363);
            this.grbCriticker.TabIndex = 7;
            this.grbCriticker.TabStop = false;
            this.grbCriticker.Text = "Criticker";
            // 
            // btnCritickerCSVExportBrowse
            // 
            this.btnCritickerCSVExportBrowse.Location = new System.Drawing.Point(388, 55);
            this.btnCritickerCSVExportBrowse.Name = "btnCritickerCSVExportBrowse";
            this.btnCritickerCSVExportBrowse.Size = new System.Drawing.Size(29, 23);
            this.btnCritickerCSVExportBrowse.TabIndex = 3;
            this.btnCritickerCSVExportBrowse.Text = "...";
            this.btnCritickerCSVExportBrowse.UseVisualStyleBackColor = true;
            this.btnCritickerCSVExportBrowse.Click += new System.EventHandler(this.btnCritickerCSVExportBrowse_Click);
            // 
            // chkCritickerEnabled
            // 
            this.chkCritickerEnabled.AutoSize = true;
            this.chkCritickerEnabled.Location = new System.Drawing.Point(20, 30);
            this.chkCritickerEnabled.Name = "chkCritickerEnabled";
            this.chkCritickerEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkCritickerEnabled.TabIndex = 0;
            this.chkCritickerEnabled.Text = "Enabled";
            this.chkCritickerEnabled.UseVisualStyleBackColor = true;
            this.chkCritickerEnabled.CheckedChanged += new System.EventHandler(this.chkCritickerEnabled_CheckedChanged);
            // 
            // txtCritickerCSVExportFile
            // 
            this.txtCritickerCSVExportFile.Location = new System.Drawing.Point(151, 57);
            this.txtCritickerCSVExportFile.Name = "txtCritickerCSVExportFile";
            this.txtCritickerCSVExportFile.Size = new System.Drawing.Size(231, 20);
            this.txtCritickerCSVExportFile.TabIndex = 2;
            this.txtCritickerCSVExportFile.TextChanged += new System.EventHandler(this.txtCritickerMovieExportFile_TextChanged);
            // 
            // lblCritickerCSVExportFile
            // 
            this.lblCritickerCSVExportFile.AutoSize = true;
            this.lblCritickerCSVExportFile.Location = new System.Drawing.Point(17, 59);
            this.lblCritickerCSVExportFile.Name = "lblCritickerCSVExportFile";
            this.lblCritickerCSVExportFile.Size = new System.Drawing.Size(80, 13);
            this.lblCritickerCSVExportFile.TabIndex = 1;
            this.lblCritickerCSVExportFile.Text = "CSV Export File";
            // 
            // tabTraktRater
            // 
            this.tabTraktRater.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabTraktRater.Controls.Add(this.tabPage1);
            this.tabTraktRater.Controls.Add(this.tabPage2);
            this.tabTraktRater.Controls.Add(this.tabPage3);
            this.tabTraktRater.Controls.Add(this.tabPage4);
            this.tabTraktRater.Controls.Add(this.tabPage5);
            this.tabTraktRater.Controls.Add(this.tabPage7);
            this.tabTraktRater.Controls.Add(this.tabPage8);
            this.tabTraktRater.Controls.Add(this.tabPage6);
            this.tabTraktRater.Controls.Add(this.tabPage9);
            this.tabTraktRater.Controls.Add(this.tabPage11);
            this.tabTraktRater.Controls.Add(this.tabPage12);
            this.tabTraktRater.Controls.Add(this.tabPage10);
            this.tabTraktRater.Location = new System.Drawing.Point(4, 13);
            this.tabTraktRater.Name = "tabTraktRater";
            this.tabTraktRater.SelectedIndex = 0;
            this.tabTraktRater.Size = new System.Drawing.Size(652, 398);
            this.tabTraktRater.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grbPaypal);
            this.tabPage1.Controls.Add(this.grbTrakt);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(644, 372);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Login";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grbPaypal
            // 
            this.grbPaypal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbPaypal.Controls.Add(this.lnkPaypal);
            this.grbPaypal.Controls.Add(this.pbPaypal);
            this.grbPaypal.Controls.Add(this.lblPaypal);
            this.grbPaypal.Location = new System.Drawing.Point(7, 124);
            this.grbPaypal.Name = "grbPaypal";
            this.grbPaypal.Size = new System.Drawing.Size(626, 240);
            this.grbPaypal.TabIndex = 1;
            this.grbPaypal.TabStop = false;
            this.grbPaypal.Text = "Paypal";
            // 
            // lnkPaypal
            // 
            this.lnkPaypal.AutoSize = true;
            this.lnkPaypal.Location = new System.Drawing.Point(71, 43);
            this.lnkPaypal.Name = "lnkPaypal";
            this.lnkPaypal.Size = new System.Drawing.Size(193, 13);
            this.lnkPaypal.TabIndex = 3;
            this.lnkPaypal.TabStop = true;
            this.lnkPaypal.Text = "https://www.paypal.me/damienlhaynes";
            this.lnkPaypal.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPaypal_LinkClicked);
            // 
            // pbPaypal
            // 
            this.pbPaypal.Image = ((System.Drawing.Image)(resources.GetObject("pbPaypal.Image")));
            this.pbPaypal.Location = new System.Drawing.Point(17, 19);
            this.pbPaypal.Name = "pbPaypal";
            this.pbPaypal.Size = new System.Drawing.Size(48, 48);
            this.pbPaypal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPaypal.TabIndex = 1;
            this.pbPaypal.TabStop = false;
            this.pbPaypal.Click += new System.EventHandler(this.pbPaypal_Click);
            // 
            // lblPaypal
            // 
            this.lblPaypal.AutoSize = true;
            this.lblPaypal.Location = new System.Drawing.Point(71, 25);
            this.lblPaypal.Name = "lblPaypal";
            this.lblPaypal.Size = new System.Drawing.Size(469, 13);
            this.lblPaypal.TabIndex = 0;
            this.lblPaypal.Text = "If you enjoy using the Trakt Rater tool please consider donating to ensure contin" +
    "ued development.";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.grbTVDb);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(644, 372);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "TVDb";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.grbTMDb);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(644, 372);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "TMDb";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.Transparent;
            this.tabPage4.Controls.Add(this.ctrlImdb);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(644, 372);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "IMDb";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.grbListal);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(644, 372);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Listal";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.grbCriticker);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(644, 372);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "Criticker";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.grbLetterboxd);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(644, 372);
            this.tabPage8.TabIndex = 7;
            this.tabPage8.Text = "Letterboxd";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // grbLetterboxd
            // 
            this.grbLetterboxd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbLetterboxd.Controls.Add(this.lblLetterboxdCustomList);
            this.grbLetterboxd.Controls.Add(this.btnLetterBoxRemoveList);
            this.grbLetterboxd.Controls.Add(this.btnLetterBoxAddList);
            this.grbLetterboxd.Controls.Add(this.listLetterboxdCustomLists);
            this.grbLetterboxd.Controls.Add(this.lblLetterboxdWatched);
            this.grbLetterboxd.Controls.Add(this.btnLetterboxdWatchedBrowse);
            this.grbLetterboxd.Controls.Add(this.txtLetterboxdWatchedFile);
            this.grbLetterboxd.Controls.Add(this.lblLetterboxdDiary);
            this.grbLetterboxd.Controls.Add(this.btnLetterboxdDiaryBrowse);
            this.grbLetterboxd.Controls.Add(this.txtLetterboxdDiaryFile);
            this.grbLetterboxd.Controls.Add(this.lblLetterboxdRatingsFile);
            this.grbLetterboxd.Controls.Add(this.btnLetterboxdRatingsBrowse);
            this.grbLetterboxd.Controls.Add(this.txtLetterboxdRatingsFile);
            this.grbLetterboxd.Controls.Add(this.chkLetterboxdEnabled);
            this.grbLetterboxd.Location = new System.Drawing.Point(6, 6);
            this.grbLetterboxd.Name = "grbLetterboxd";
            this.grbLetterboxd.Size = new System.Drawing.Size(627, 363);
            this.grbLetterboxd.TabIndex = 0;
            this.grbLetterboxd.TabStop = false;
            this.grbLetterboxd.Text = "Letterboxd";
            // 
            // lblLetterboxdCustomList
            // 
            this.lblLetterboxdCustomList.AutoSize = true;
            this.lblLetterboxdCustomList.Location = new System.Drawing.Point(17, 210);
            this.lblLetterboxdCustomList.Name = "lblLetterboxdCustomList";
            this.lblLetterboxdCustomList.Size = new System.Drawing.Size(69, 13);
            this.lblLetterboxdCustomList.TabIndex = 13;
            this.lblLetterboxdCustomList.Text = "Custom Lists:";
            // 
            // btnLetterBoxRemoveList
            // 
            this.btnLetterBoxRemoveList.Location = new System.Drawing.Point(314, 258);
            this.btnLetterBoxRemoveList.Name = "btnLetterBoxRemoveList";
            this.btnLetterBoxRemoveList.Size = new System.Drawing.Size(28, 23);
            this.btnLetterBoxRemoveList.TabIndex = 16;
            this.btnLetterBoxRemoveList.Text = "-";
            this.btnLetterBoxRemoveList.UseVisualStyleBackColor = true;
            this.btnLetterBoxRemoveList.Click += new System.EventHandler(this.btnLetterBoxRemoveList_Click);
            // 
            // btnLetterBoxAddList
            // 
            this.btnLetterBoxAddList.Location = new System.Drawing.Point(314, 226);
            this.btnLetterBoxAddList.Name = "btnLetterBoxAddList";
            this.btnLetterBoxAddList.Size = new System.Drawing.Size(28, 23);
            this.btnLetterBoxAddList.TabIndex = 15;
            this.btnLetterBoxAddList.Text = "+";
            this.btnLetterBoxAddList.UseVisualStyleBackColor = true;
            this.btnLetterBoxAddList.Click += new System.EventHandler(this.btnLetterBoxAddList_Click);
            // 
            // listLetterboxdCustomLists
            // 
            this.listLetterboxdCustomLists.FormattingEnabled = true;
            this.listLetterboxdCustomLists.Location = new System.Drawing.Point(20, 226);
            this.listLetterboxdCustomLists.Name = "listLetterboxdCustomLists";
            this.listLetterboxdCustomLists.Size = new System.Drawing.Size(290, 121);
            this.listLetterboxdCustomLists.TabIndex = 14;
            // 
            // lblLetterboxdWatched
            // 
            this.lblLetterboxdWatched.AutoSize = true;
            this.lblLetterboxdWatched.Location = new System.Drawing.Point(17, 106);
            this.lblLetterboxdWatched.Name = "lblLetterboxdWatched";
            this.lblLetterboxdWatched.Size = new System.Drawing.Size(73, 13);
            this.lblLetterboxdWatched.TabIndex = 4;
            this.lblLetterboxdWatched.Text = "Watched File:";
            // 
            // btnLetterboxdWatchedBrowse
            // 
            this.btnLetterboxdWatchedBrowse.Enabled = false;
            this.btnLetterboxdWatchedBrowse.Location = new System.Drawing.Point(313, 121);
            this.btnLetterboxdWatchedBrowse.Name = "btnLetterboxdWatchedBrowse";
            this.btnLetterboxdWatchedBrowse.Size = new System.Drawing.Size(29, 23);
            this.btnLetterboxdWatchedBrowse.TabIndex = 7;
            this.btnLetterboxdWatchedBrowse.Text = "...";
            this.btnLetterboxdWatchedBrowse.UseVisualStyleBackColor = true;
            this.btnLetterboxdWatchedBrowse.Click += new System.EventHandler(this.btnLetterboxdWatchedBrowse_Click);
            // 
            // lblLetterboxdDiary
            // 
            this.lblLetterboxdDiary.AutoSize = true;
            this.lblLetterboxdDiary.Location = new System.Drawing.Point(17, 157);
            this.lblLetterboxdDiary.Name = "lblLetterboxdDiary";
            this.lblLetterboxdDiary.Size = new System.Drawing.Size(53, 13);
            this.lblLetterboxdDiary.TabIndex = 8;
            this.lblLetterboxdDiary.Text = "Diary File:";
            // 
            // btnLetterboxdDiaryBrowse
            // 
            this.btnLetterboxdDiaryBrowse.Enabled = false;
            this.btnLetterboxdDiaryBrowse.Location = new System.Drawing.Point(313, 170);
            this.btnLetterboxdDiaryBrowse.Name = "btnLetterboxdDiaryBrowse";
            this.btnLetterboxdDiaryBrowse.Size = new System.Drawing.Size(29, 23);
            this.btnLetterboxdDiaryBrowse.TabIndex = 1;
            this.btnLetterboxdDiaryBrowse.Text = "...";
            this.btnLetterboxdDiaryBrowse.UseVisualStyleBackColor = true;
            this.btnLetterboxdDiaryBrowse.Click += new System.EventHandler(this.btnLetterboxdDiaryBrowse_Click);
            // 
            // lblLetterboxdRatingsFile
            // 
            this.lblLetterboxdRatingsFile.AutoSize = true;
            this.lblLetterboxdRatingsFile.Location = new System.Drawing.Point(17, 59);
            this.lblLetterboxdRatingsFile.Name = "lblLetterboxdRatingsFile";
            this.lblLetterboxdRatingsFile.Size = new System.Drawing.Size(65, 13);
            this.lblLetterboxdRatingsFile.TabIndex = 1;
            this.lblLetterboxdRatingsFile.Text = "Ratings File:";
            // 
            // btnLetterboxdRatingsBrowse
            // 
            this.btnLetterboxdRatingsBrowse.Enabled = false;
            this.btnLetterboxdRatingsBrowse.Location = new System.Drawing.Point(313, 74);
            this.btnLetterboxdRatingsBrowse.Name = "btnLetterboxdRatingsBrowse";
            this.btnLetterboxdRatingsBrowse.Size = new System.Drawing.Size(29, 23);
            this.btnLetterboxdRatingsBrowse.TabIndex = 3;
            this.btnLetterboxdRatingsBrowse.Text = "...";
            this.btnLetterboxdRatingsBrowse.UseVisualStyleBackColor = true;
            this.btnLetterboxdRatingsBrowse.Click += new System.EventHandler(this.btnLetterboxdRatingsBrowse_Click);
            // 
            // chkLetterboxdEnabled
            // 
            this.chkLetterboxdEnabled.AutoSize = true;
            this.chkLetterboxdEnabled.Location = new System.Drawing.Point(20, 28);
            this.chkLetterboxdEnabled.Name = "chkLetterboxdEnabled";
            this.chkLetterboxdEnabled.Size = new System.Drawing.Size(59, 17);
            this.chkLetterboxdEnabled.TabIndex = 0;
            this.chkLetterboxdEnabled.Text = "Enable";
            this.chkLetterboxdEnabled.UseVisualStyleBackColor = true;
            this.chkLetterboxdEnabled.CheckedChanged += new System.EventHandler(this.chkLetterboxdEnabled_CheckedChanged);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.grbFlixster);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(644, 372);
            this.tabPage6.TabIndex = 9;
            this.tabPage6.Text = "Flixster";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // grbFlixster
            // 
            this.grbFlixster.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbFlixster.Controls.Add(this.chkFlixsterSyncWantToSee);
            this.grbFlixster.Controls.Add(this.lblFlixsterUserIdDesc);
            this.grbFlixster.Controls.Add(this.lblFlisterUserId);
            this.grbFlixster.Controls.Add(this.txtFlixsterUserId);
            this.grbFlixster.Controls.Add(this.chkFlixsterEnabled);
            this.grbFlixster.Location = new System.Drawing.Point(6, 4);
            this.grbFlixster.Name = "grbFlixster";
            this.grbFlixster.Size = new System.Drawing.Size(627, 365);
            this.grbFlixster.TabIndex = 0;
            this.grbFlixster.TabStop = false;
            this.grbFlixster.Text = "Flixster";
            // 
            // chkFlixsterSyncWantToSee
            // 
            this.chkFlixsterSyncWantToSee.AutoSize = true;
            this.chkFlixsterSyncWantToSee.Location = new System.Drawing.Point(20, 115);
            this.chkFlixsterSyncWantToSee.Name = "chkFlixsterSyncWantToSee";
            this.chkFlixsterSyncWantToSee.Size = new System.Drawing.Size(180, 17);
            this.chkFlixsterSyncWantToSee.TabIndex = 3;
            this.chkFlixsterSyncWantToSee.Text = "Sync \'Want To See\' to Watchlist";
            this.chkFlixsterSyncWantToSee.UseVisualStyleBackColor = true;
            this.chkFlixsterSyncWantToSee.CheckedChanged += new System.EventHandler(this.chkFlixsterSyncWantToSee_CheckedChanged);
            // 
            // lblFlixsterUserIdDesc
            // 
            this.lblFlixsterUserIdDesc.AutoEllipsis = true;
            this.lblFlixsterUserIdDesc.AutoSize = true;
            this.lblFlixsterUserIdDesc.Location = new System.Drawing.Point(17, 156);
            this.lblFlixsterUserIdDesc.Name = "lblFlixsterUserIdDesc";
            this.lblFlixsterUserIdDesc.Size = new System.Drawing.Size(468, 13);
            this.lblFlixsterUserIdDesc.TabIndex = 4;
            this.lblFlixsterUserIdDesc.Text = "Get your User ID from your profile page URL e.g. http://www.flixster.com/user/YOU" +
    "R_USER_ID/";
            // 
            // lblFlisterUserId
            // 
            this.lblFlisterUserId.AutoSize = true;
            this.lblFlisterUserId.Location = new System.Drawing.Point(17, 61);
            this.lblFlisterUserId.Name = "lblFlisterUserId";
            this.lblFlisterUserId.Size = new System.Drawing.Size(46, 13);
            this.lblFlisterUserId.TabIndex = 1;
            this.lblFlisterUserId.Text = "User ID:";
            // 
            // txtFlixsterUserId
            // 
            this.txtFlixsterUserId.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtFlixsterUserId.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtFlixsterUserId.Enabled = false;
            this.txtFlixsterUserId.Location = new System.Drawing.Point(20, 79);
            this.txtFlixsterUserId.Name = "txtFlixsterUserId";
            this.txtFlixsterUserId.Size = new System.Drawing.Size(208, 20);
            this.txtFlixsterUserId.TabIndex = 2;
            this.txtFlixsterUserId.TextChanged += new System.EventHandler(this.txtFlixsterUserId_TextChanged);
            // 
            // chkFlixsterEnabled
            // 
            this.chkFlixsterEnabled.AutoSize = true;
            this.chkFlixsterEnabled.Location = new System.Drawing.Point(20, 28);
            this.chkFlixsterEnabled.Name = "chkFlixsterEnabled";
            this.chkFlixsterEnabled.Size = new System.Drawing.Size(59, 17);
            this.chkFlixsterEnabled.TabIndex = 0;
            this.chkFlixsterEnabled.Text = "Enable";
            this.chkFlixsterEnabled.UseVisualStyleBackColor = true;
            this.chkFlixsterEnabled.CheckedChanged += new System.EventHandler(this.chkFlixsterEnabled_CheckedChanged);
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.grbCheckMovies);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPage9.Size = new System.Drawing.Size(644, 372);
            this.tabPage9.TabIndex = 9;
            this.tabPage9.Text = "iCheckMovies";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // grbCheckMovies
            // 
            this.grbCheckMovies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbCheckMovies.Controls.Add(this.chkCheckMoviesAddMoviesToCollection);
            this.grbCheckMovies.Controls.Add(this.lblCheckMoviesDelimiter);
            this.grbCheckMovies.Controls.Add(this.cboCheckMoviesDelimiter);
            this.grbCheckMovies.Controls.Add(this.chkCheckMoviesUpdateWatchedStatus);
            this.grbCheckMovies.Controls.Add(this.chkCheckMoviesAddWatchedToWatchlist);
            this.grbCheckMovies.Controls.Add(this.chkCheckMoviesEnabled);
            this.grbCheckMovies.Controls.Add(this.btnCheckMoviesExportBrowse);
            this.grbCheckMovies.Controls.Add(this.txtCheckMoviesCsvFile);
            this.grbCheckMovies.Controls.Add(this.lblCheckMoviesFile);
            this.grbCheckMovies.Location = new System.Drawing.Point(7, 6);
            this.grbCheckMovies.Name = "grbCheckMovies";
            this.grbCheckMovies.Size = new System.Drawing.Size(626, 367);
            this.grbCheckMovies.TabIndex = 0;
            this.grbCheckMovies.TabStop = false;
            this.grbCheckMovies.Text = "iCheckMovies";
            // 
            // chkCheckMoviesAddMoviesToCollection
            // 
            this.chkCheckMoviesAddMoviesToCollection.AutoSize = true;
            this.chkCheckMoviesAddMoviesToCollection.Location = new System.Drawing.Point(25, 205);
            this.chkCheckMoviesAddMoviesToCollection.Margin = new System.Windows.Forms.Padding(2);
            this.chkCheckMoviesAddMoviesToCollection.Name = "chkCheckMoviesAddMoviesToCollection";
            this.chkCheckMoviesAddMoviesToCollection.Size = new System.Drawing.Size(187, 17);
            this.chkCheckMoviesAddMoviesToCollection.TabIndex = 8;
            this.chkCheckMoviesAddMoviesToCollection.Text = "Add collected movies to collection";
            this.chkCheckMoviesAddMoviesToCollection.UseVisualStyleBackColor = true;
            this.chkCheckMoviesAddMoviesToCollection.CheckedChanged += new System.EventHandler(this.chkCheckMoviesAddMoviesToCollection_CheckedChanged);
            // 
            // lblCheckMoviesDelimiter
            // 
            this.lblCheckMoviesDelimiter.AutoSize = true;
            this.lblCheckMoviesDelimiter.Location = new System.Drawing.Point(24, 105);
            this.lblCheckMoviesDelimiter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCheckMoviesDelimiter.Name = "lblCheckMoviesDelimiter";
            this.lblCheckMoviesDelimiter.Size = new System.Drawing.Size(47, 13);
            this.lblCheckMoviesDelimiter.TabIndex = 4;
            this.lblCheckMoviesDelimiter.Text = "Delimiter";
            // 
            // cboCheckMoviesDelimiter
            // 
            this.cboCheckMoviesDelimiter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCheckMoviesDelimiter.FormattingEnabled = true;
            this.cboCheckMoviesDelimiter.Items.AddRange(new object[] {
            "Comma",
            "Semicolon"});
            this.cboCheckMoviesDelimiter.Location = new System.Drawing.Point(25, 125);
            this.cboCheckMoviesDelimiter.Margin = new System.Windows.Forms.Padding(2);
            this.cboCheckMoviesDelimiter.Name = "cboCheckMoviesDelimiter";
            this.cboCheckMoviesDelimiter.Size = new System.Drawing.Size(208, 21);
            this.cboCheckMoviesDelimiter.TabIndex = 5;
            this.cboCheckMoviesDelimiter.SelectedIndexChanged += new System.EventHandler(this.cboCheckMoviesDelimiter_SelectedIndexChanged);
            // 
            // chkCheckMoviesUpdateWatchedStatus
            // 
            this.chkCheckMoviesUpdateWatchedStatus.AutoSize = true;
            this.chkCheckMoviesUpdateWatchedStatus.Location = new System.Drawing.Point(25, 181);
            this.chkCheckMoviesUpdateWatchedStatus.Name = "chkCheckMoviesUpdateWatchedStatus";
            this.chkCheckMoviesUpdateWatchedStatus.Size = new System.Drawing.Size(214, 17);
            this.chkCheckMoviesUpdateWatchedStatus.TabIndex = 7;
            this.chkCheckMoviesUpdateWatchedStatus.Text = "Add watched movies to watched history";
            this.chkCheckMoviesUpdateWatchedStatus.UseVisualStyleBackColor = true;
            this.chkCheckMoviesUpdateWatchedStatus.CheckedChanged += new System.EventHandler(this.chkCheckMoviesUpdateWatchedStatus_CheckedChanged);
            // 
            // chkCheckMoviesEnabled
            // 
            this.chkCheckMoviesEnabled.AutoSize = true;
            this.chkCheckMoviesEnabled.Location = new System.Drawing.Point(19, 25);
            this.chkCheckMoviesEnabled.Name = "chkCheckMoviesEnabled";
            this.chkCheckMoviesEnabled.Size = new System.Drawing.Size(59, 17);
            this.chkCheckMoviesEnabled.TabIndex = 0;
            this.chkCheckMoviesEnabled.Text = "Enable";
            this.chkCheckMoviesEnabled.UseVisualStyleBackColor = true;
            this.chkCheckMoviesEnabled.CheckedChanged += new System.EventHandler(this.chkCheckMoviesEnabled_CheckedChanged);
            // 
            // btnCheckMoviesExportBrowse
            // 
            this.btnCheckMoviesExportBrowse.Enabled = false;
            this.btnCheckMoviesExportBrowse.Location = new System.Drawing.Point(241, 75);
            this.btnCheckMoviesExportBrowse.Name = "btnCheckMoviesExportBrowse";
            this.btnCheckMoviesExportBrowse.Size = new System.Drawing.Size(28, 23);
            this.btnCheckMoviesExportBrowse.TabIndex = 3;
            this.btnCheckMoviesExportBrowse.Text = "...";
            this.btnCheckMoviesExportBrowse.UseVisualStyleBackColor = true;
            this.btnCheckMoviesExportBrowse.Click += new System.EventHandler(this.btnCheckMoviesBrowse_Click);
            // 
            // lblCheckMoviesFile
            // 
            this.lblCheckMoviesFile.AutoSize = true;
            this.lblCheckMoviesFile.Enabled = false;
            this.lblCheckMoviesFile.Location = new System.Drawing.Point(24, 59);
            this.lblCheckMoviesFile.Name = "lblCheckMoviesFile";
            this.lblCheckMoviesFile.Size = new System.Drawing.Size(97, 13);
            this.lblCheckMoviesFile.TabIndex = 1;
            this.lblCheckMoviesFile.Text = "Movie list to import:";
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.grbToDoMovies);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage11.Size = new System.Drawing.Size(644, 372);
            this.tabPage11.TabIndex = 11;
            this.tabPage11.Text = "ToDoMovies";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // grbToDoMovies
            // 
            this.grbToDoMovies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbToDoMovies.Controls.Add(this.btnToDoMoviesExportBrowse);
            this.grbToDoMovies.Controls.Add(this.chkToDoMoviesEnabled);
            this.grbToDoMovies.Controls.Add(this.txtToDoMovieExportFile);
            this.grbToDoMovies.Controls.Add(this.lblToDoMovieExportFile);
            this.grbToDoMovies.Location = new System.Drawing.Point(6, 6);
            this.grbToDoMovies.Name = "grbToDoMovies";
            this.grbToDoMovies.Size = new System.Drawing.Size(627, 365);
            this.grbToDoMovies.TabIndex = 8;
            this.grbToDoMovies.TabStop = false;
            this.grbToDoMovies.Text = "ToDoMovies";
            // 
            // btnToDoMoviesExportBrowse
            // 
            this.btnToDoMoviesExportBrowse.Location = new System.Drawing.Point(388, 55);
            this.btnToDoMoviesExportBrowse.Name = "btnToDoMoviesExportBrowse";
            this.btnToDoMoviesExportBrowse.Size = new System.Drawing.Size(29, 23);
            this.btnToDoMoviesExportBrowse.TabIndex = 3;
            this.btnToDoMoviesExportBrowse.Text = "...";
            this.btnToDoMoviesExportBrowse.UseVisualStyleBackColor = true;
            this.btnToDoMoviesExportBrowse.Click += new System.EventHandler(this.btnToDoMoviesExportBrowse_Click);
            // 
            // chkToDoMoviesEnabled
            // 
            this.chkToDoMoviesEnabled.AutoSize = true;
            this.chkToDoMoviesEnabled.Location = new System.Drawing.Point(20, 28);
            this.chkToDoMoviesEnabled.Name = "chkToDoMoviesEnabled";
            this.chkToDoMoviesEnabled.Size = new System.Drawing.Size(59, 17);
            this.chkToDoMoviesEnabled.TabIndex = 0;
            this.chkToDoMoviesEnabled.Text = "Enable";
            this.chkToDoMoviesEnabled.UseVisualStyleBackColor = true;
            this.chkToDoMoviesEnabled.CheckedChanged += new System.EventHandler(this.chkToDoMoviesEnabled_CheckedChanged);
            // 
            // txtToDoMovieExportFile
            // 
            this.txtToDoMovieExportFile.Location = new System.Drawing.Point(151, 57);
            this.txtToDoMovieExportFile.Name = "txtToDoMovieExportFile";
            this.txtToDoMovieExportFile.Size = new System.Drawing.Size(231, 20);
            this.txtToDoMovieExportFile.TabIndex = 2;
            this.txtToDoMovieExportFile.TextChanged += new System.EventHandler(this.txtToDoMovieExportFile_TextChanged);
            // 
            // lblToDoMovieExportFile
            // 
            this.lblToDoMovieExportFile.AutoSize = true;
            this.lblToDoMovieExportFile.Location = new System.Drawing.Point(17, 59);
            this.lblToDoMovieExportFile.Name = "lblToDoMovieExportFile";
            this.lblToDoMovieExportFile.Size = new System.Drawing.Size(91, 13);
            this.lblToDoMovieExportFile.TabIndex = 1;
            this.lblToDoMovieExportFile.Text = "Movie Export File:";
            // 
            // tabPage12
            // 
            this.tabPage12.Controls.Add(this.grbMovieLens);
            this.tabPage12.Location = new System.Drawing.Point(4, 22);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage12.Size = new System.Drawing.Size(644, 372);
            this.tabPage12.TabIndex = 12;
            this.tabPage12.Text = "MovieLens";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // grbMovieLens
            // 
            this.grbMovieLens.Controls.Add(this.lblMovieLensNote);
            this.grbMovieLens.Controls.Add(this.lblMovieLensTags);
            this.grbMovieLens.Controls.Add(this.btnMovieLensTags);
            this.grbMovieLens.Controls.Add(this.txtMovieLensTags);
            this.grbMovieLens.Controls.Add(this.lblMovieLensWishlist);
            this.grbMovieLens.Controls.Add(this.btnMovieLensWishlist);
            this.grbMovieLens.Controls.Add(this.txtMovieLensWishlist);
            this.grbMovieLens.Controls.Add(this.lblMovieLensActivity);
            this.grbMovieLens.Controls.Add(this.btnMovieLensActivity);
            this.grbMovieLens.Controls.Add(this.txtMovieLensActivity);
            this.grbMovieLens.Controls.Add(this.lblMovieLensRatings);
            this.grbMovieLens.Controls.Add(this.btnMovieLensRatings);
            this.grbMovieLens.Controls.Add(this.txtMovieLensRatings);
            this.grbMovieLens.Controls.Add(this.chkMovieLensEnabled);
            this.grbMovieLens.Location = new System.Drawing.Point(7, 7);
            this.grbMovieLens.Name = "grbMovieLens";
            this.grbMovieLens.Size = new System.Drawing.Size(626, 359);
            this.grbMovieLens.TabIndex = 0;
            this.grbMovieLens.TabStop = false;
            this.grbMovieLens.Text = "MovieLens";
            // 
            // lblMovieLensNote
            // 
            this.lblMovieLensNote.AutoSize = true;
            this.lblMovieLensNote.Location = new System.Drawing.Point(17, 209);
            this.lblMovieLensNote.Name = "lblMovieLensNote";
            this.lblMovieLensNote.Size = new System.Drawing.Size(567, 13);
            this.lblMovieLensNote.TabIndex = 13;
            this.lblMovieLensNote.Text = "Note that when providing an activity log, more details can be used for imports su" +
    "ch as the date/time a movie was rated.";
            // 
            // lblMovieLensTags
            // 
            this.lblMovieLensTags.AutoSize = true;
            this.lblMovieLensTags.Location = new System.Drawing.Point(17, 294);
            this.lblMovieLensTags.Name = "lblMovieLensTags";
            this.lblMovieLensTags.Size = new System.Drawing.Size(53, 13);
            this.lblMovieLensTags.TabIndex = 9;
            this.lblMovieLensTags.Text = "Tags File:";
            this.lblMovieLensTags.Visible = false;
            // 
            // btnMovieLensTags
            // 
            this.btnMovieLensTags.Enabled = false;
            this.btnMovieLensTags.Location = new System.Drawing.Point(313, 307);
            this.btnMovieLensTags.Name = "btnMovieLensTags";
            this.btnMovieLensTags.Size = new System.Drawing.Size(29, 23);
            this.btnMovieLensTags.TabIndex = 12;
            this.btnMovieLensTags.Text = "...";
            this.btnMovieLensTags.UseVisualStyleBackColor = true;
            this.btnMovieLensTags.Visible = false;
            this.btnMovieLensTags.Click += new System.EventHandler(this.btnMovieLensTags_Click);
            // 
            // lblMovieLensWishlist
            // 
            this.lblMovieLensWishlist.AutoSize = true;
            this.lblMovieLensWishlist.Location = new System.Drawing.Point(16, 108);
            this.lblMovieLensWishlist.Name = "lblMovieLensWishlist";
            this.lblMovieLensWishlist.Size = new System.Drawing.Size(118, 13);
            this.lblMovieLensWishlist.TabIndex = 5;
            this.lblMovieLensWishlist.Text = "Wishlist (Watchlist) File:";
            // 
            // btnMovieLensWishlist
            // 
            this.btnMovieLensWishlist.Enabled = false;
            this.btnMovieLensWishlist.Location = new System.Drawing.Point(312, 121);
            this.btnMovieLensWishlist.Name = "btnMovieLensWishlist";
            this.btnMovieLensWishlist.Size = new System.Drawing.Size(29, 23);
            this.btnMovieLensWishlist.TabIndex = 8;
            this.btnMovieLensWishlist.Text = "...";
            this.btnMovieLensWishlist.UseVisualStyleBackColor = true;
            this.btnMovieLensWishlist.Click += new System.EventHandler(this.btnMovieLensWishlist_Click);
            // 
            // txtMovieLensWishlist
            // 
            this.txtMovieLensWishlist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtMovieLensWishlist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtMovieLensWishlist.Enabled = false;
            this.txtMovieLensWishlist.Location = new System.Drawing.Point(18, 124);
            this.txtMovieLensWishlist.Name = "txtMovieLensWishlist";
            this.txtMovieLensWishlist.Size = new System.Drawing.Size(288, 20);
            this.txtMovieLensWishlist.TabIndex = 7;
            this.txtMovieLensWishlist.TextChanged += new System.EventHandler(this.txtMovieLensWishlist_TextChanged);
            // 
            // lblMovieLensActivity
            // 
            this.lblMovieLensActivity.AutoSize = true;
            this.lblMovieLensActivity.Location = new System.Drawing.Point(15, 158);
            this.lblMovieLensActivity.Name = "lblMovieLensActivity";
            this.lblMovieLensActivity.Size = new System.Drawing.Size(84, 13);
            this.lblMovieLensActivity.TabIndex = 0;
            this.lblMovieLensActivity.Text = "Activity Log File:";
            // 
            // btnMovieLensActivity
            // 
            this.btnMovieLensActivity.Enabled = false;
            this.btnMovieLensActivity.Location = new System.Drawing.Point(311, 171);
            this.btnMovieLensActivity.Name = "btnMovieLensActivity";
            this.btnMovieLensActivity.Size = new System.Drawing.Size(29, 23);
            this.btnMovieLensActivity.TabIndex = 2;
            this.btnMovieLensActivity.Text = "...";
            this.btnMovieLensActivity.UseVisualStyleBackColor = true;
            this.btnMovieLensActivity.Click += new System.EventHandler(this.btnMovieLensActivity_Click);
            // 
            // lblMovieLensRatings
            // 
            this.lblMovieLensRatings.AutoSize = true;
            this.lblMovieLensRatings.Location = new System.Drawing.Point(16, 60);
            this.lblMovieLensRatings.Name = "lblMovieLensRatings";
            this.lblMovieLensRatings.Size = new System.Drawing.Size(65, 13);
            this.lblMovieLensRatings.TabIndex = 1;
            this.lblMovieLensRatings.Text = "Ratings File:";
            // 
            // btnMovieLensRatings
            // 
            this.btnMovieLensRatings.Enabled = false;
            this.btnMovieLensRatings.Location = new System.Drawing.Point(312, 73);
            this.btnMovieLensRatings.Name = "btnMovieLensRatings";
            this.btnMovieLensRatings.Size = new System.Drawing.Size(29, 23);
            this.btnMovieLensRatings.TabIndex = 4;
            this.btnMovieLensRatings.Text = "...";
            this.btnMovieLensRatings.UseVisualStyleBackColor = true;
            this.btnMovieLensRatings.Click += new System.EventHandler(this.btnMovieLensRatings_Click);
            // 
            // txtMovieLensRatings
            // 
            this.txtMovieLensRatings.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtMovieLensRatings.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtMovieLensRatings.Enabled = false;
            this.txtMovieLensRatings.Location = new System.Drawing.Point(18, 76);
            this.txtMovieLensRatings.Name = "txtMovieLensRatings";
            this.txtMovieLensRatings.Size = new System.Drawing.Size(288, 20);
            this.txtMovieLensRatings.TabIndex = 3;
            this.txtMovieLensRatings.TextChanged += new System.EventHandler(this.txtMovieLensRatings_TextChanged);
            // 
            // chkMovieLensEnabled
            // 
            this.chkMovieLensEnabled.AutoSize = true;
            this.chkMovieLensEnabled.Location = new System.Drawing.Point(19, 27);
            this.chkMovieLensEnabled.Name = "chkMovieLensEnabled";
            this.chkMovieLensEnabled.Size = new System.Drawing.Size(59, 17);
            this.chkMovieLensEnabled.TabIndex = 0;
            this.chkMovieLensEnabled.Text = "Enable";
            this.chkMovieLensEnabled.UseVisualStyleBackColor = true;
            this.chkMovieLensEnabled.CheckedChanged += new System.EventHandler(this.chkMovieLensEnabled_CheckedChanged);
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.grbOptions);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(644, 372);
            this.tabPage10.TabIndex = 10;
            this.tabPage10.Text = "Options";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // ctrlImdb
            // 
            this.ctrlImdb.Location = new System.Drawing.Point(6, 2);
            this.ctrlImdb.Name = "ctrlImdb";
            this.ctrlImdb.Size = new System.Drawing.Size(634, 370);
            this.ctrlImdb.TabIndex = 0;
            // 
            // TraktRater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(660, 537);
            this.Controls.Add(this.tabTraktRater);
            this.Controls.Add(this.grbReport);
            this.Controls.Add(this.pbrImportProgress);
            this.Controls.Add(this.btnStartSync);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(506, 466);
            this.Name = "TraktRater";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Trakt Rater";
            this.grbTrakt.ResumeLayout(false);
            this.grbTrakt.PerformLayout();
            this.grbTVDb.ResumeLayout(false);
            this.grbTVDb.PerformLayout();
            this.grbReport.ResumeLayout(false);
            this.grbReport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBatchSize)).EndInit();
            this.grbTMDb.ResumeLayout(false);
            this.grbTMDb.PerformLayout();
            this.grbOptions.ResumeLayout(false);
            this.grbOptions.PerformLayout();
            this.grbListal.ResumeLayout(false);
            this.grbListal.PerformLayout();
            this.grbCriticker.ResumeLayout(false);
            this.grbCriticker.PerformLayout();
            this.tabTraktRater.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.grbPaypal.ResumeLayout(false);
            this.grbPaypal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPaypal)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.grbLetterboxd.ResumeLayout(false);
            this.grbLetterboxd.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.grbFlixster.ResumeLayout(false);
            this.grbFlixster.PerformLayout();
            this.tabPage9.ResumeLayout(false);
            this.grbCheckMovies.ResumeLayout(false);
            this.grbCheckMovies.PerformLayout();
            this.tabPage11.ResumeLayout(false);
            this.grbToDoMovies.ResumeLayout(false);
            this.grbToDoMovies.PerformLayout();
            this.tabPage12.ResumeLayout(false);
            this.grbMovieLens.ResumeLayout(false);
            this.grbMovieLens.PerformLayout();
            this.tabPage10.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbTrakt;
        private System.Windows.Forms.GroupBox grbTVDb;
        private System.Windows.Forms.TextBox txtTVDbAccountId;
        private System.Windows.Forms.Label lblTVDbAccountId;
        private System.Windows.Forms.Button btnStartSync;
        private System.Windows.Forms.ProgressBar pbrImportProgress;
        private System.Windows.Forms.GroupBox grbReport;
        public System.Windows.Forms.Label lblStatusMessage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolTip tipHelp;
        private System.Windows.Forms.OpenFileDialog dlgFileOpen;
        private System.Windows.Forms.GroupBox grbTMDb;
        private System.Windows.Forms.LinkLabel lnkTMDbStart;
        private System.Windows.Forms.Label lblTMDbMessage;
        private System.Windows.Forms.GroupBox grbOptions;
        private System.Windows.Forms.CheckBox chkMarkAsWatched;
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
        private System.Windows.Forms.CheckBox chkTMDbEnabled;
        private System.Windows.Forms.CheckBox chkListalEnabled;
        private System.Windows.Forms.CheckBox chkTMDbSyncWatchlist;
        private System.Windows.Forms.GroupBox grbCriticker;
        private System.Windows.Forms.Button btnCritickerCSVExportBrowse;
        private System.Windows.Forms.CheckBox chkCritickerEnabled;
        private System.Windows.Forms.TextBox txtCritickerCSVExportFile;
        private System.Windows.Forms.Label lblCritickerCSVExportFile;
        private System.Windows.Forms.LinkLabel lnkLogFolder;
        private System.Windows.Forms.Label lblBatchImportSize;
        private System.Windows.Forms.NumericUpDown nudBatchSize;
        private System.Windows.Forms.Button btnMaintenance;
        private System.Windows.Forms.LinkLabel lnkTraktOAuth;
        private System.Windows.Forms.Label lblWarnPeriod;
        private System.Windows.Forms.TabControl tabTraktRater;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.TabPage tabPage10;
        private System.Windows.Forms.GroupBox grbLetterboxd;
        private System.Windows.Forms.Label lblLetterboxdWatched;
        private System.Windows.Forms.Button btnLetterboxdWatchedBrowse;
        private System.Windows.Forms.TextBox txtLetterboxdWatchedFile;
        private System.Windows.Forms.Label lblLetterboxdDiary;
        private System.Windows.Forms.Button btnLetterboxdDiaryBrowse;
        private System.Windows.Forms.TextBox txtLetterboxdDiaryFile;
        private System.Windows.Forms.Label lblLetterboxdRatingsFile;
        private System.Windows.Forms.Button btnLetterboxdRatingsBrowse;
        private System.Windows.Forms.TextBox txtLetterboxdRatingsFile;
        private System.Windows.Forms.CheckBox chkLetterboxdEnabled;
        private System.Windows.Forms.CheckBox chkSetWatchedOnReleaseDay;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.GroupBox grbFlixster;
        private System.Windows.Forms.Label lblFlixsterUserIdDesc;
        private System.Windows.Forms.Label lblFlisterUserId;
        private System.Windows.Forms.TextBox txtFlixsterUserId;
        private System.Windows.Forms.CheckBox chkFlixsterEnabled;
        private System.Windows.Forms.CheckBox chkFlixsterSyncWantToSee;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.GroupBox grbCheckMovies;
        private System.Windows.Forms.CheckBox chkCheckMoviesEnabled;
        private System.Windows.Forms.Button btnCheckMoviesExportBrowse;
        private System.Windows.Forms.TextBox txtCheckMoviesCsvFile;
        private System.Windows.Forms.Label lblCheckMoviesFile;
        private System.Windows.Forms.CheckBox chkCheckMoviesAddWatchedToWatchlist;
        private System.Windows.Forms.CheckBox chkCheckMoviesUpdateWatchedStatus;
        private System.Windows.Forms.Label lblCheckMoviesDelimiter;
        private System.Windows.Forms.ComboBox cboCheckMoviesDelimiter;
        private System.Windows.Forms.CheckBox chkCheckMoviesAddMoviesToCollection;
        private System.Windows.Forms.TextBox txtTraktPinCode;
        private System.Windows.Forms.TabPage tabPage11;
        private System.Windows.Forms.GroupBox grbToDoMovies;
        private System.Windows.Forms.Button btnToDoMoviesExportBrowse;
        private System.Windows.Forms.CheckBox chkToDoMoviesEnabled;
        private System.Windows.Forms.TextBox txtToDoMovieExportFile;
        private System.Windows.Forms.Label lblToDoMovieExportFile;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.GroupBox grbPaypal;
        private System.Windows.Forms.PictureBox pbPaypal;
        private System.Windows.Forms.Label lblPaypal;
        private System.Windows.Forms.LinkLabel lnkPaypal;
        private System.Windows.Forms.Label lblLetterboxdCustomList;
        private System.Windows.Forms.Button btnLetterBoxRemoveList;
        private System.Windows.Forms.Button btnLetterBoxAddList;
        private System.Windows.Forms.ListBox listLetterboxdCustomLists;
        private System.Windows.Forms.TabPage tabPage12;
        private System.Windows.Forms.GroupBox grbMovieLens;
        private System.Windows.Forms.Label lblMovieLensWishlist;
        private System.Windows.Forms.Button btnMovieLensWishlist;
        private System.Windows.Forms.TextBox txtMovieLensWishlist;
        private System.Windows.Forms.Label lblMovieLensActivity;
        private System.Windows.Forms.Button btnMovieLensActivity;
        private System.Windows.Forms.TextBox txtMovieLensActivity;
        private System.Windows.Forms.Label lblMovieLensRatings;
        private System.Windows.Forms.Button btnMovieLensRatings;
        private System.Windows.Forms.TextBox txtMovieLensRatings;
        private System.Windows.Forms.CheckBox chkMovieLensEnabled;
        private System.Windows.Forms.Label lblMovieLensTags;
        private System.Windows.Forms.Button btnMovieLensTags;
        private System.Windows.Forms.TextBox txtMovieLensTags;
        private System.Windows.Forms.Label lblMovieLensNote;
        private TraktRater.UI.Controls.IMDbUserControl ctrlImdb;
    }
}

