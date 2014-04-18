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
            this.txtTraktPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTraktUsername = new System.Windows.Forms.TextBox();
            this.grbTVDb = new System.Windows.Forms.GroupBox();
            this.txtTVDbAccountId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDetails = new System.Windows.Forms.Label();
            this.btnImportRatings = new System.Windows.Forms.Button();
            this.pbrImportProgress = new System.Windows.Forms.ProgressBar();
            this.grbReport = new System.Windows.Forms.GroupBox();
            this.lblStatusMessage = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.txtImdbRatingsFilename = new System.Windows.Forms.TextBox();
            this.txtImdbWebUsername = new System.Windows.Forms.TextBox();
            this.txtImdbWatchlistFile = new System.Windows.Forms.TextBox();
            this.grbImdb = new System.Windows.Forms.GroupBox();
            this.btnImdbWatchlistBrowse = new System.Windows.Forms.Button();
            this.lblWatchlistFile = new System.Windows.Forms.Label();
            this.lblRatingsFile = new System.Windows.Forms.Label();
            this.lblIMDbDescription = new System.Windows.Forms.Label();
            this.rdnImdbUsername = new System.Windows.Forms.RadioButton();
            this.rdnImdbCSV = new System.Windows.Forms.RadioButton();
            this.chkImdbWebWatchlist = new System.Windows.Forms.CheckBox();
            this.btnImdbRatingsBrowse = new System.Windows.Forms.Button();
            this.dlgFileOpen = new System.Windows.Forms.OpenFileDialog();
            this.grbTMDb = new System.Windows.Forms.GroupBox();
            this.lnkTMDbStart = new System.Windows.Forms.LinkLabel();
            this.lblTMDbMessage = new System.Windows.Forms.Label();
            this.grbOptions = new System.Windows.Forms.GroupBox();
            this.chkIgnoreWatchedForWatchlists = new System.Windows.Forms.CheckBox();
            this.chkMarkAsWatched = new System.Windows.Forms.CheckBox();
            this.grbListal = new System.Windows.Forms.GroupBox();
            this.lnkListalExport = new System.Windows.Forms.LinkLabel();
            this.chkListalWebWatchlist = new System.Windows.Forms.CheckBox();
            this.btnListalXMLExport = new System.Windows.Forms.Button();
            this.txtListalMovieXMLExport = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.grbTrakt.SuspendLayout();
            this.grbTVDb.SuspendLayout();
            this.grbReport.SuspendLayout();
            this.grbImdb.SuspendLayout();
            this.grbTMDb.SuspendLayout();
            this.grbOptions.SuspendLayout();
            this.grbListal.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbTrakt
            // 
            this.grbTrakt.Controls.Add(this.txtTraktPassword);
            this.grbTrakt.Controls.Add(this.label2);
            this.grbTrakt.Controls.Add(this.label1);
            this.grbTrakt.Controls.Add(this.txtTraktUsername);
            this.grbTrakt.Location = new System.Drawing.Point(12, 12);
            this.grbTrakt.Name = "grbTrakt";
            this.grbTrakt.Size = new System.Drawing.Size(443, 81);
            this.grbTrakt.TabIndex = 0;
            this.grbTrakt.TabStop = false;
            this.grbTrakt.Text = "Trakt";
            // 
            // txtTraktPassword
            // 
            this.txtTraktPassword.Location = new System.Drawing.Point(177, 46);
            this.txtTraktPassword.Name = "txtTraktPassword";
            this.txtTraktPassword.Size = new System.Drawing.Size(244, 20);
            this.txtTraktPassword.TabIndex = 3;
            this.txtTraktPassword.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // txtTraktUsername
            // 
            this.txtTraktUsername.Location = new System.Drawing.Point(177, 20);
            this.txtTraktUsername.Name = "txtTraktUsername";
            this.txtTraktUsername.Size = new System.Drawing.Size(244, 20);
            this.txtTraktUsername.TabIndex = 1;
            this.txtTraktUsername.TextChanged += new System.EventHandler(this.txtTraktUsername_TextChanged);
            // 
            // grbTVDb
            // 
            this.grbTVDb.Controls.Add(this.txtTVDbAccountId);
            this.grbTVDb.Controls.Add(this.label3);
            this.grbTVDb.Location = new System.Drawing.Point(12, 115);
            this.grbTVDb.Name = "grbTVDb";
            this.grbTVDb.Size = new System.Drawing.Size(443, 54);
            this.grbTVDb.TabIndex = 2;
            this.grbTVDb.TabStop = false;
            this.grbTVDb.Text = "TVDb";
            // 
            // txtTVDbAccountId
            // 
            this.txtTVDbAccountId.Location = new System.Drawing.Point(177, 19);
            this.txtTVDbAccountId.Name = "txtTVDbAccountId";
            this.txtTVDbAccountId.Size = new System.Drawing.Size(244, 20);
            this.txtTVDbAccountId.TabIndex = 1;
            this.tipHelp.SetToolTip(this.txtTVDbAccountId, "The Account Identifier can be found in the account tab of\r\nhttp://thetvdb.com web" +
        "site. It is 16 hexidecimal characters e.g.\r\n\r\nEB6D329D10E3835A");
            this.txtTVDbAccountId.TextChanged += new System.EventHandler(this.txtTVDbAccountId_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Account Identifier:";
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Location = new System.Drawing.Point(9, 99);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(352, 13);
            this.lblDetails.TabIndex = 1;
            this.lblDetails.Text = "Enter in account details for sources you wish to transfer ratings to trakt.tv:";
            // 
            // btnImportRatings
            // 
            this.btnImportRatings.Location = new System.Drawing.Point(12, 429);
            this.btnImportRatings.Name = "btnImportRatings";
            this.btnImportRatings.Size = new System.Drawing.Size(891, 26);
            this.btnImportRatings.TabIndex = 6;
            this.btnImportRatings.Text = "Start Ratings Import";
            this.btnImportRatings.UseVisualStyleBackColor = true;
            this.btnImportRatings.Click += new System.EventHandler(this.btnImportRatings_Click);
            // 
            // pbrImportProgress
            // 
            this.pbrImportProgress.Location = new System.Drawing.Point(13, 461);
            this.pbrImportProgress.Name = "pbrImportProgress";
            this.pbrImportProgress.Size = new System.Drawing.Size(886, 23);
            this.pbrImportProgress.TabIndex = 7;
            // 
            // grbReport
            // 
            this.grbReport.Controls.Add(this.lblStatusMessage);
            this.grbReport.Controls.Add(this.label5);
            this.grbReport.Location = new System.Drawing.Point(12, 499);
            this.grbReport.Name = "grbReport";
            this.grbReport.Size = new System.Drawing.Size(887, 49);
            this.grbReport.TabIndex = 8;
            this.grbReport.TabStop = false;
            this.grbReport.Text = "Report";
            // 
            // lblStatusMessage
            // 
            this.lblStatusMessage.Location = new System.Drawing.Point(77, 20);
            this.lblStatusMessage.Name = "lblStatusMessage";
            this.lblStatusMessage.Size = new System.Drawing.Size(794, 23);
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
            this.txtImdbRatingsFilename.Location = new System.Drawing.Point(177, 66);
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
            this.txtImdbWebUsername.Location = new System.Drawing.Point(177, 154);
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
            this.txtImdbWatchlistFile.Location = new System.Drawing.Point(177, 111);
            this.txtImdbWatchlistFile.Name = "txtImdbWatchlistFile";
            this.txtImdbWatchlistFile.Size = new System.Drawing.Size(208, 20);
            this.txtImdbWatchlistFile.TabIndex = 7;
            this.tipHelp.SetToolTip(this.txtImdbWatchlistFile, "Leave field blank if you\'re not interested in importing a watchlist from IMDb to " +
        "trakt.tv.");
            this.txtImdbWatchlistFile.TextChanged += new System.EventHandler(this.txtImdbWatchlistFile_TextChanged);
            // 
            // grbImdb
            // 
            this.grbImdb.Controls.Add(this.btnImdbWatchlistBrowse);
            this.grbImdb.Controls.Add(this.txtImdbWatchlistFile);
            this.grbImdb.Controls.Add(this.lblWatchlistFile);
            this.grbImdb.Controls.Add(this.lblRatingsFile);
            this.grbImdb.Controls.Add(this.lblIMDbDescription);
            this.grbImdb.Controls.Add(this.rdnImdbUsername);
            this.grbImdb.Controls.Add(this.rdnImdbCSV);
            this.grbImdb.Controls.Add(this.chkImdbWebWatchlist);
            this.grbImdb.Controls.Add(this.txtImdbWebUsername);
            this.grbImdb.Controls.Add(this.btnImdbRatingsBrowse);
            this.grbImdb.Controls.Add(this.txtImdbRatingsFilename);
            this.grbImdb.Location = new System.Drawing.Point(12, 175);
            this.grbImdb.Name = "grbImdb";
            this.grbImdb.Size = new System.Drawing.Size(443, 214);
            this.grbImdb.TabIndex = 3;
            this.grbImdb.TabStop = false;
            this.grbImdb.Text = "IMDb";
            // 
            // btnImdbWatchlistBrowse
            // 
            this.btnImdbWatchlistBrowse.Location = new System.Drawing.Point(392, 108);
            this.btnImdbWatchlistBrowse.Name = "btnImdbWatchlistBrowse";
            this.btnImdbWatchlistBrowse.Size = new System.Drawing.Size(28, 23);
            this.btnImdbWatchlistBrowse.TabIndex = 8;
            this.btnImdbWatchlistBrowse.Text = "...";
            this.btnImdbWatchlistBrowse.UseVisualStyleBackColor = true;
            this.btnImdbWatchlistBrowse.Click += new System.EventHandler(this.btnImdbWatchlistBrowse_Click);
            // 
            // lblWatchlistFile
            // 
            this.lblWatchlistFile.AutoSize = true;
            this.lblWatchlistFile.Location = new System.Drawing.Point(177, 93);
            this.lblWatchlistFile.Name = "lblWatchlistFile";
            this.lblWatchlistFile.Size = new System.Drawing.Size(73, 13);
            this.lblWatchlistFile.TabIndex = 6;
            this.lblWatchlistFile.Text = "Watchlist File:";
            // 
            // lblRatingsFile
            // 
            this.lblRatingsFile.AutoSize = true;
            this.lblRatingsFile.Location = new System.Drawing.Point(174, 50);
            this.lblRatingsFile.Name = "lblRatingsFile";
            this.lblRatingsFile.Size = new System.Drawing.Size(65, 13);
            this.lblRatingsFile.TabIndex = 3;
            this.lblRatingsFile.Text = "Ratings File:";
            // 
            // lblIMDbDescription
            // 
            this.lblIMDbDescription.AutoSize = true;
            this.lblIMDbDescription.Location = new System.Drawing.Point(20, 20);
            this.lblIMDbDescription.Name = "lblIMDbDescription";
            this.lblIMDbDescription.Size = new System.Drawing.Size(347, 13);
            this.lblIMDbDescription.TabIndex = 1;
            this.lblIMDbDescription.Text = "Select \'CSV Import\' for static file import or \'Web Scrape\' for web retrieval:";
            // 
            // rdnImdbUsername
            // 
            this.rdnImdbUsername.AutoSize = true;
            this.rdnImdbUsername.Checked = true;
            this.rdnImdbUsername.Location = new System.Drawing.Point(21, 157);
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
            this.rdnImdbCSV.Location = new System.Drawing.Point(21, 48);
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
            this.chkImdbWebWatchlist.Location = new System.Drawing.Point(177, 180);
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
            this.btnImdbRatingsBrowse.Location = new System.Drawing.Point(391, 64);
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
            this.grbTMDb.Controls.Add(this.lnkTMDbStart);
            this.grbTMDb.Controls.Add(this.lblTMDbMessage);
            this.grbTMDb.Location = new System.Drawing.Point(461, 115);
            this.grbTMDb.Name = "grbTMDb";
            this.grbTMDb.Size = new System.Drawing.Size(443, 81);
            this.grbTMDb.TabIndex = 4;
            this.grbTMDb.TabStop = false;
            this.grbTMDb.Text = "TMDb";
            // 
            // lnkTMDbStart
            // 
            this.lnkTMDbStart.AutoSize = true;
            this.lnkTMDbStart.Location = new System.Drawing.Point(18, 52);
            this.lnkTMDbStart.Name = "lnkTMDbStart";
            this.lnkTMDbStart.Size = new System.Drawing.Size(113, 13);
            this.lnkTMDbStart.TabIndex = 1;
            this.lnkTMDbStart.TabStop = true;
            this.lnkTMDbStart.Text = "Start Request Process";
            this.lnkTMDbStart.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTMDbStart_LinkClicked);
            // 
            // lblTMDbMessage
            // 
            this.lblTMDbMessage.Location = new System.Drawing.Point(17, 18);
            this.lblTMDbMessage.Name = "lblTMDbMessage";
            this.lblTMDbMessage.Size = new System.Drawing.Size(406, 44);
            this.lblTMDbMessage.TabIndex = 0;
            this.lblTMDbMessage.Text = "To get user ratings from TMDb you must first allow this application to access you" +
    "r account details. This needs to be done by you in a webbrowser.";
            // 
            // grbOptions
            // 
            this.grbOptions.Controls.Add(this.chkIgnoreWatchedForWatchlists);
            this.grbOptions.Controls.Add(this.chkMarkAsWatched);
            this.grbOptions.Location = new System.Drawing.Point(461, 12);
            this.grbOptions.Name = "grbOptions";
            this.grbOptions.Size = new System.Drawing.Size(443, 81);
            this.grbOptions.TabIndex = 5;
            this.grbOptions.TabStop = false;
            this.grbOptions.Text = "Options";
            // 
            // chkIgnoreWatchedForWatchlists
            // 
            this.chkIgnoreWatchedForWatchlists.AutoSize = true;
            this.chkIgnoreWatchedForWatchlists.Location = new System.Drawing.Point(21, 43);
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
            this.grbListal.Controls.Add(this.label6);
            this.grbListal.Controls.Add(this.lnkListalExport);
            this.grbListal.Controls.Add(this.chkListalWebWatchlist);
            this.grbListal.Controls.Add(this.btnListalXMLExport);
            this.grbListal.Controls.Add(this.txtListalMovieXMLExport);
            this.grbListal.Controls.Add(this.label4);
            this.grbListal.Location = new System.Drawing.Point(462, 203);
            this.grbListal.Name = "grbListal";
            this.grbListal.Size = new System.Drawing.Size(442, 126);
            this.grbListal.TabIndex = 9;
            this.grbListal.TabStop = false;
            this.grbListal.Text = "Listal";
            // 
            // lnkListalExport
            // 
            this.lnkListalExport.AutoSize = true;
            this.lnkListalExport.Location = new System.Drawing.Point(20, 100);
            this.lnkListalExport.Name = "lnkListalExport";
            this.lnkListalExport.Size = new System.Drawing.Size(157, 13);
            this.lnkListalExport.TabIndex = 14;
            this.lnkListalExport.TabStop = true;
            this.lnkListalExport.Text = "Get your Listal export file(s) here";
            this.lnkListalExport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkListalExport_LinkClicked);
            // 
            // chkListalWebWatchlist
            // 
            this.chkListalWebWatchlist.AutoSize = true;
            this.chkListalWebWatchlist.Location = new System.Drawing.Point(155, 46);
            this.chkListalWebWatchlist.Name = "chkListalWebWatchlist";
            this.chkListalWebWatchlist.Size = new System.Drawing.Size(150, 17);
            this.chkListalWebWatchlist.TabIndex = 13;
            this.chkListalWebWatchlist.Text = "Sync Wantlist to Watchlist";
            this.chkListalWebWatchlist.UseVisualStyleBackColor = true;
            this.chkListalWebWatchlist.CheckedChanged += new System.EventHandler(this.chkListalWebWatchlist_CheckedChanged);
            // 
            // btnListalXMLExport
            // 
            this.btnListalXMLExport.Location = new System.Drawing.Point(392, 18);
            this.btnListalXMLExport.Name = "btnListalXMLExport";
            this.btnListalXMLExport.Size = new System.Drawing.Size(29, 23);
            this.btnListalXMLExport.TabIndex = 12;
            this.btnListalXMLExport.Text = "...";
            this.btnListalXMLExport.UseVisualStyleBackColor = true;
            this.btnListalXMLExport.Click += new System.EventHandler(this.btnListalXMLExport_Click);
            // 
            // txtListalMovieXMLExport
            // 
            this.txtListalMovieXMLExport.Location = new System.Drawing.Point(155, 20);
            this.txtListalMovieXMLExport.Name = "txtListalMovieXMLExport";
            this.txtListalMovieXMLExport.Size = new System.Drawing.Size(231, 20);
            this.txtListalMovieXMLExport.TabIndex = 4;
            this.txtListalMovieXMLExport.TextChanged += new System.EventHandler(this.txtListalMovieXMLExport_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Movie Export File:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(346, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Logon into the Listal website, then download export files from link below:";
            // 
            // TraktRater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 555);
            this.Controls.Add(this.grbListal);
            this.Controls.Add(this.grbOptions);
            this.Controls.Add(this.grbTMDb);
            this.Controls.Add(this.grbImdb);
            this.Controls.Add(this.grbReport);
            this.Controls.Add(this.pbrImportProgress);
            this.Controls.Add(this.btnImportRatings);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.grbTVDb);
            this.Controls.Add(this.grbTrakt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TraktRater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trakt Rater";
            this.grbTrakt.ResumeLayout(false);
            this.grbTrakt.PerformLayout();
            this.grbTVDb.ResumeLayout(false);
            this.grbTVDb.PerformLayout();
            this.grbReport.ResumeLayout(false);
            this.grbImdb.ResumeLayout(false);
            this.grbImdb.PerformLayout();
            this.grbTMDb.ResumeLayout(false);
            this.grbTMDb.PerformLayout();
            this.grbOptions.ResumeLayout(false);
            this.grbOptions.PerformLayout();
            this.grbListal.ResumeLayout(false);
            this.grbListal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grbTrakt;
        private System.Windows.Forms.TextBox txtTraktPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTraktUsername;
        private System.Windows.Forms.GroupBox grbTVDb;
        private System.Windows.Forms.TextBox txtTVDbAccountId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Button btnImportRatings;
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
        private System.Windows.Forms.Label lblIMDbDescription;
        private System.Windows.Forms.Button btnImdbWatchlistBrowse;
        private System.Windows.Forms.TextBox txtImdbWatchlistFile;
        private System.Windows.Forms.Label lblWatchlistFile;
        private System.Windows.Forms.Label lblRatingsFile;
        private System.Windows.Forms.CheckBox chkIgnoreWatchedForWatchlists;
        private System.Windows.Forms.GroupBox grbListal;
        private System.Windows.Forms.TextBox txtListalMovieXMLExport;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnListalXMLExport;
        private System.Windows.Forms.CheckBox chkListalWebWatchlist;
        private System.Windows.Forms.LinkLabel lnkListalExport;
        private System.Windows.Forms.Label label6;
    }
}

