namespace TraktRater.UI.Controls
{
    partial class IMDbUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grbImdb = new System.Windows.Forms.GroupBox();
            this.lblImdbNote = new System.Windows.Forms.Label();
            this.lblImdbCustomLists = new System.Windows.Forms.Label();
            this.btnImdbDeleteList = new System.Windows.Forms.Button();
            this.btnImdbAddList = new System.Windows.Forms.Button();
            this.lsImdbCustomLists = new System.Windows.Forms.ListBox();
            this.chkIMDbEnabled = new System.Windows.Forms.CheckBox();
            this.btnImdbWatchlistBrowse = new System.Windows.Forms.Button();
            this.txtImdbWatchlistFile = new System.Windows.Forms.TextBox();
            this.lblImdbWatchlistFile = new System.Windows.Forms.Label();
            this.lblImdbRatingsFile = new System.Windows.Forms.Label();
            this.lblImdbDescription = new System.Windows.Forms.Label();
            this.rdnImdbUsername = new System.Windows.Forms.RadioButton();
            this.rdnImdbCSV = new System.Windows.Forms.RadioButton();
            this.chkImdbWebWatchlist = new System.Windows.Forms.CheckBox();
            this.txtImdbWebUsername = new System.Windows.Forms.TextBox();
            this.btnImdbRatingsBrowse = new System.Windows.Forms.Button();
            this.txtImdbRatingsFilename = new System.Windows.Forms.TextBox();
            this.dlgFileOpen = new System.Windows.Forms.OpenFileDialog();
            this.grbImdb.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbImdb
            // 
            this.grbImdb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbImdb.Controls.Add(this.lblImdbNote);
            this.grbImdb.Controls.Add(this.lblImdbCustomLists);
            this.grbImdb.Controls.Add(this.btnImdbDeleteList);
            this.grbImdb.Controls.Add(this.btnImdbAddList);
            this.grbImdb.Controls.Add(this.lsImdbCustomLists);
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
            this.grbImdb.Location = new System.Drawing.Point(3, 3);
            this.grbImdb.Name = "grbImdb";
            this.grbImdb.Size = new System.Drawing.Size(537, 364);
            this.grbImdb.TabIndex = 1;
            this.grbImdb.TabStop = false;
            this.grbImdb.Text = "IMDb";
            // 
            // lblImdbNote
            // 
            this.lblImdbNote.AutoEllipsis = true;
            this.lblImdbNote.Location = new System.Drawing.Point(18, 316);
            this.lblImdbNote.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblImdbNote.Name = "lblImdbNote";
            this.lblImdbNote.Size = new System.Drawing.Size(428, 38);
            this.lblImdbNote.TabIndex = 16;
            this.lblImdbNote.Text = "Note: If web-scrape option is selected and fails, this may indicate a change to t" +
    "he IMDb website. Please fallback to CSV import instead.";
            // 
            // lblImdbCustomLists
            // 
            this.lblImdbCustomLists.AutoSize = true;
            this.lblImdbCustomLists.Location = new System.Drawing.Point(175, 164);
            this.lblImdbCustomLists.Name = "lblImdbCustomLists";
            this.lblImdbCustomLists.Size = new System.Drawing.Size(69, 13);
            this.lblImdbCustomLists.TabIndex = 9;
            this.lblImdbCustomLists.Text = "Custom Lists:";
            // 
            // btnImdbDeleteList
            // 
            this.btnImdbDeleteList.Location = new System.Drawing.Point(390, 215);
            this.btnImdbDeleteList.Name = "btnImdbDeleteList";
            this.btnImdbDeleteList.Size = new System.Drawing.Size(28, 23);
            this.btnImdbDeleteList.TabIndex = 12;
            this.btnImdbDeleteList.Text = "-";
            this.btnImdbDeleteList.UseVisualStyleBackColor = true;
            this.btnImdbDeleteList.Click += new System.EventHandler(this.btnImdbDeleteList_Click);
            // 
            // btnImdbAddList
            // 
            this.btnImdbAddList.Location = new System.Drawing.Point(390, 183);
            this.btnImdbAddList.Name = "btnImdbAddList";
            this.btnImdbAddList.Size = new System.Drawing.Size(28, 23);
            this.btnImdbAddList.TabIndex = 11;
            this.btnImdbAddList.Text = "+";
            this.btnImdbAddList.UseVisualStyleBackColor = true;
            this.btnImdbAddList.Click += new System.EventHandler(this.btnImdbAddList_Click);
            // 
            // lsImdbCustomLists
            // 
            this.lsImdbCustomLists.FormattingEnabled = true;
            this.lsImdbCustomLists.Location = new System.Drawing.Point(175, 183);
            this.lsImdbCustomLists.Name = "lsImdbCustomLists";
            this.lsImdbCustomLists.Size = new System.Drawing.Size(208, 56);
            this.lsImdbCustomLists.TabIndex = 10;
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
            this.btnImdbWatchlistBrowse.Location = new System.Drawing.Point(390, 135);
            this.btnImdbWatchlistBrowse.Name = "btnImdbWatchlistBrowse";
            this.btnImdbWatchlistBrowse.Size = new System.Drawing.Size(28, 23);
            this.btnImdbWatchlistBrowse.TabIndex = 8;
            this.btnImdbWatchlistBrowse.Text = "...";
            this.btnImdbWatchlistBrowse.UseVisualStyleBackColor = true;
            this.btnImdbWatchlistBrowse.Click += new System.EventHandler(this.btnImdbWatchlistBrowse_Click);
            // 
            // txtImdbWatchlistFile
            // 
            this.txtImdbWatchlistFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtImdbWatchlistFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtImdbWatchlistFile.Location = new System.Drawing.Point(175, 137);
            this.txtImdbWatchlistFile.Name = "txtImdbWatchlistFile";
            this.txtImdbWatchlistFile.Size = new System.Drawing.Size(208, 20);
            this.txtImdbWatchlistFile.TabIndex = 7;
            this.txtImdbWatchlistFile.TextChanged += new System.EventHandler(this.txtImdbWatchlistFile_TextChanged);
            // 
            // lblImdbWatchlistFile
            // 
            this.lblImdbWatchlistFile.AutoSize = true;
            this.lblImdbWatchlistFile.Location = new System.Drawing.Point(175, 119);
            this.lblImdbWatchlistFile.Name = "lblImdbWatchlistFile";
            this.lblImdbWatchlistFile.Size = new System.Drawing.Size(73, 13);
            this.lblImdbWatchlistFile.TabIndex = 6;
            this.lblImdbWatchlistFile.Text = "Watchlist File:";
            // 
            // lblImdbRatingsFile
            // 
            this.lblImdbRatingsFile.AutoSize = true;
            this.lblImdbRatingsFile.Location = new System.Drawing.Point(172, 77);
            this.lblImdbRatingsFile.Name = "lblImdbRatingsFile";
            this.lblImdbRatingsFile.Size = new System.Drawing.Size(65, 13);
            this.lblImdbRatingsFile.TabIndex = 3;
            this.lblImdbRatingsFile.Text = "Ratings File:";
            // 
            // lblImdbDescription
            // 
            this.lblImdbDescription.AutoSize = true;
            this.lblImdbDescription.Location = new System.Drawing.Point(18, 46);
            this.lblImdbDescription.Name = "lblImdbDescription";
            this.lblImdbDescription.Size = new System.Drawing.Size(423, 13);
            this.lblImdbDescription.TabIndex = 1;
            this.lblImdbDescription.Text = "Select \'CSV Import\' (recommended) for static file import or \'Web Scrape\' for web " +
    "retrieval:";
            // 
            // rdnImdbUsername
            // 
            this.rdnImdbUsername.AutoSize = true;
            this.rdnImdbUsername.Location = new System.Drawing.Point(19, 263);
            this.rdnImdbUsername.Name = "rdnImdbUsername";
            this.rdnImdbUsername.Size = new System.Drawing.Size(88, 17);
            this.rdnImdbUsername.TabIndex = 13;
            this.rdnImdbUsername.Text = "Web Scrape:";
            this.rdnImdbUsername.UseVisualStyleBackColor = true;
            // 
            // rdnImdbCSV
            // 
            this.rdnImdbCSV.AutoSize = true;
            this.rdnImdbCSV.Checked = true;
            this.rdnImdbCSV.Location = new System.Drawing.Point(19, 75);
            this.rdnImdbCSV.Name = "rdnImdbCSV";
            this.rdnImdbCSV.Size = new System.Drawing.Size(81, 17);
            this.rdnImdbCSV.TabIndex = 2;
            this.rdnImdbCSV.TabStop = true;
            this.rdnImdbCSV.Text = "CSV Import:";
            this.rdnImdbCSV.UseVisualStyleBackColor = true;
            this.rdnImdbCSV.CheckedChanged += new System.EventHandler(this.rdnImdbCSV_CheckedChanged);
            // 
            // chkImdbWebWatchlist
            // 
            this.chkImdbWebWatchlist.AutoSize = true;
            this.chkImdbWebWatchlist.Location = new System.Drawing.Point(175, 288);
            this.chkImdbWebWatchlist.Name = "chkImdbWebWatchlist";
            this.chkImdbWebWatchlist.Size = new System.Drawing.Size(97, 17);
            this.chkImdbWebWatchlist.TabIndex = 15;
            this.chkImdbWebWatchlist.Text = "Sync Watchlist";
            this.chkImdbWebWatchlist.UseVisualStyleBackColor = true;
            this.chkImdbWebWatchlist.CheckedChanged += new System.EventHandler(this.chkImdbWebWatchlist_CheckedChanged);
            // 
            // txtImdbWebUsername
            // 
            this.txtImdbWebUsername.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtImdbWebUsername.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtImdbWebUsername.Location = new System.Drawing.Point(175, 261);
            this.txtImdbWebUsername.Name = "txtImdbWebUsername";
            this.txtImdbWebUsername.Size = new System.Drawing.Size(206, 20);
            this.txtImdbWebUsername.TabIndex = 14;
            this.txtImdbWebUsername.TextChanged += new System.EventHandler(this.txtImdbWebUsername_TextChanged);
            // 
            // btnImdbRatingsBrowse
            // 
            this.btnImdbRatingsBrowse.Enabled = false;
            this.btnImdbRatingsBrowse.Location = new System.Drawing.Point(389, 92);
            this.btnImdbRatingsBrowse.Name = "btnImdbRatingsBrowse";
            this.btnImdbRatingsBrowse.Size = new System.Drawing.Size(29, 23);
            this.btnImdbRatingsBrowse.TabIndex = 5;
            this.btnImdbRatingsBrowse.Text = "...";
            this.btnImdbRatingsBrowse.UseVisualStyleBackColor = true;
            this.btnImdbRatingsBrowse.Click += new System.EventHandler(this.btnImdbRatingsBrowse_Click);
            // 
            // txtImdbRatingsFilename
            // 
            this.txtImdbRatingsFilename.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtImdbRatingsFilename.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtImdbRatingsFilename.Enabled = false;
            this.txtImdbRatingsFilename.Location = new System.Drawing.Point(175, 93);
            this.txtImdbRatingsFilename.Name = "txtImdbRatingsFilename";
            this.txtImdbRatingsFilename.Size = new System.Drawing.Size(208, 20);
            this.txtImdbRatingsFilename.TabIndex = 4;
            this.txtImdbRatingsFilename.TextChanged += new System.EventHandler(this.txtImdbRatingsFilename_TextChanged);
            // 
            // dlgFileOpen
            // 
            this.dlgFileOpen.DefaultExt = "csv";
            // 
            // IMDbControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grbImdb);
            this.Name = "IMDbControl";
            this.Size = new System.Drawing.Size(543, 370);
            this.grbImdb.ResumeLayout(false);
            this.grbImdb.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbImdb;
        private System.Windows.Forms.Label lblImdbNote;
        private System.Windows.Forms.Label lblImdbCustomLists;
        private System.Windows.Forms.Button btnImdbDeleteList;
        private System.Windows.Forms.Button btnImdbAddList;
        private System.Windows.Forms.ListBox lsImdbCustomLists;
        private System.Windows.Forms.CheckBox chkIMDbEnabled;
        private System.Windows.Forms.Button btnImdbWatchlistBrowse;
        private System.Windows.Forms.TextBox txtImdbWatchlistFile;
        private System.Windows.Forms.Label lblImdbWatchlistFile;
        private System.Windows.Forms.Label lblImdbRatingsFile;
        private System.Windows.Forms.Label lblImdbDescription;
        private System.Windows.Forms.RadioButton rdnImdbUsername;
        private System.Windows.Forms.RadioButton rdnImdbCSV;
        private System.Windows.Forms.CheckBox chkImdbWebWatchlist;
        private System.Windows.Forms.TextBox txtImdbWebUsername;
        private System.Windows.Forms.Button btnImdbRatingsBrowse;
        private System.Windows.Forms.TextBox txtImdbRatingsFilename;
        private System.Windows.Forms.OpenFileDialog dlgFileOpen;
    }
}
