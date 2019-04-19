namespace TraktRater.UI
{
    partial class ExportDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportDialog));
            this.btnExport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grbExport = new System.Windows.Forms.GroupBox();
            this.chkMoviePausedStates = new System.Windows.Forms.CheckBox();
            this.chkEpisodePausedStates = new System.Windows.Forms.CheckBox();
            this.chkMovieWatchlist = new System.Windows.Forms.CheckBox();
            this.chkSeasonWatchlist = new System.Windows.Forms.CheckBox();
            this.chkShowWatchlist = new System.Windows.Forms.CheckBox();
            this.chkEpisodeWatchlist = new System.Windows.Forms.CheckBox();
            this.chkMovieRatings = new System.Windows.Forms.CheckBox();
            this.chkSeasonRatings = new System.Windows.Forms.CheckBox();
            this.chkShowRatings = new System.Windows.Forms.CheckBox();
            this.chkEpisodeRatings = new System.Windows.Forms.CheckBox();
            this.chkMovieCollection = new System.Windows.Forms.CheckBox();
            this.chkEpisodeCollection = new System.Windows.Forms.CheckBox();
            this.chkMovieWatchedHistory = new System.Windows.Forms.CheckBox();
            this.chkEpisodeWatchedHistory = new System.Windows.Forms.CheckBox();
            this.lblSelect = new System.Windows.Forms.Label();
            this.grbPath = new System.Windows.Forms.GroupBox();
            this.btnBrowsePath = new System.Windows.Forms.Button();
            this.txtExportPath = new System.Windows.Forms.TextBox();
            this.fbdExportPath = new System.Windows.Forms.FolderBrowserDialog();
            this.chkCustomLists = new System.Windows.Forms.CheckBox();
            this.chkEpisodeComments = new System.Windows.Forms.CheckBox();
            this.chkSeasonComments = new System.Windows.Forms.CheckBox();
            this.chkShowComments = new System.Windows.Forms.CheckBox();
            this.chkMovieComments = new System.Windows.Forms.CheckBox();
            this.chkListComments = new System.Windows.Forms.CheckBox();
            this.chkLikedComments = new System.Windows.Forms.CheckBox();
            this.chkLikedLists = new System.Windows.Forms.CheckBox();
            this.grbExport.SuspendLayout();
            this.grbPath.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(461, 643);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(176, 36);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Start Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(279, 643);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(176, 36);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grbExport
            // 
            this.grbExport.Controls.Add(this.chkLikedLists);
            this.grbExport.Controls.Add(this.chkLikedComments);
            this.grbExport.Controls.Add(this.chkListComments);
            this.grbExport.Controls.Add(this.chkMovieComments);
            this.grbExport.Controls.Add(this.chkShowComments);
            this.grbExport.Controls.Add(this.chkSeasonComments);
            this.grbExport.Controls.Add(this.chkEpisodeComments);
            this.grbExport.Controls.Add(this.chkCustomLists);
            this.grbExport.Controls.Add(this.chkMoviePausedStates);
            this.grbExport.Controls.Add(this.chkEpisodePausedStates);
            this.grbExport.Controls.Add(this.chkMovieWatchlist);
            this.grbExport.Controls.Add(this.chkSeasonWatchlist);
            this.grbExport.Controls.Add(this.chkShowWatchlist);
            this.grbExport.Controls.Add(this.chkEpisodeWatchlist);
            this.grbExport.Controls.Add(this.chkMovieRatings);
            this.grbExport.Controls.Add(this.chkSeasonRatings);
            this.grbExport.Controls.Add(this.chkShowRatings);
            this.grbExport.Controls.Add(this.chkEpisodeRatings);
            this.grbExport.Controls.Add(this.chkMovieCollection);
            this.grbExport.Controls.Add(this.chkEpisodeCollection);
            this.grbExport.Controls.Add(this.chkMovieWatchedHistory);
            this.grbExport.Controls.Add(this.chkEpisodeWatchedHistory);
            this.grbExport.Controls.Add(this.lblSelect);
            this.grbExport.Location = new System.Drawing.Point(13, 14);
            this.grbExport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grbExport.Name = "grbExport";
            this.grbExport.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grbExport.Size = new System.Drawing.Size(628, 507);
            this.grbExport.TabIndex = 3;
            this.grbExport.TabStop = false;
            // 
            // chkMoviePausedStates
            // 
            this.chkMoviePausedStates.AutoSize = true;
            this.chkMoviePausedStates.Location = new System.Drawing.Point(342, 143);
            this.chkMoviePausedStates.Name = "chkMoviePausedStates";
            this.chkMoviePausedStates.Size = new System.Drawing.Size(185, 24);
            this.chkMoviePausedStates.TabIndex = 14;
            this.chkMoviePausedStates.Text = "Movie Paused States";
            this.chkMoviePausedStates.UseVisualStyleBackColor = true;
            this.chkMoviePausedStates.Click += new System.EventHandler(this.chkMoviePausedStates_Click);
            // 
            // chkEpisodePausedStates
            // 
            this.chkEpisodePausedStates.AutoSize = true;
            this.chkEpisodePausedStates.Location = new System.Drawing.Point(342, 104);
            this.chkEpisodePausedStates.Name = "chkEpisodePausedStates";
            this.chkEpisodePausedStates.Size = new System.Drawing.Size(202, 24);
            this.chkEpisodePausedStates.TabIndex = 13;
            this.chkEpisodePausedStates.Text = "Episode Paused States";
            this.chkEpisodePausedStates.UseVisualStyleBackColor = true;
            this.chkEpisodePausedStates.Click += new System.EventHandler(this.chkEpisodePausedStates_Click);
            // 
            // chkMovieWatchlist
            // 
            this.chkMovieWatchlist.Location = new System.Drawing.Point(15, 334);
            this.chkMovieWatchlist.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkMovieWatchlist.Name = "chkMovieWatchlist";
            this.chkMovieWatchlist.Size = new System.Drawing.Size(219, 26);
            this.chkMovieWatchlist.TabIndex = 8;
            this.chkMovieWatchlist.Text = "Movie Watchlist";
            this.chkMovieWatchlist.UseVisualStyleBackColor = true;
            this.chkMovieWatchlist.Click += new System.EventHandler(this.chkMovieWatchlist_Click);
            // 
            // chkSeasonWatchlist
            // 
            this.chkSeasonWatchlist.Location = new System.Drawing.Point(15, 295);
            this.chkSeasonWatchlist.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSeasonWatchlist.Name = "chkSeasonWatchlist";
            this.chkSeasonWatchlist.Size = new System.Drawing.Size(219, 26);
            this.chkSeasonWatchlist.TabIndex = 7;
            this.chkSeasonWatchlist.Text = "Season Watchlist";
            this.chkSeasonWatchlist.UseVisualStyleBackColor = true;
            this.chkSeasonWatchlist.Click += new System.EventHandler(this.chkSeasonWatchlist_Click);
            // 
            // chkShowWatchlist
            // 
            this.chkShowWatchlist.Location = new System.Drawing.Point(15, 257);
            this.chkShowWatchlist.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkShowWatchlist.Name = "chkShowWatchlist";
            this.chkShowWatchlist.Size = new System.Drawing.Size(219, 26);
            this.chkShowWatchlist.TabIndex = 6;
            this.chkShowWatchlist.Text = "Show Watchlist";
            this.chkShowWatchlist.UseVisualStyleBackColor = true;
            this.chkShowWatchlist.Click += new System.EventHandler(this.chkShowWatchlist_Click);
            // 
            // chkEpisodeWatchlist
            // 
            this.chkEpisodeWatchlist.Location = new System.Drawing.Point(15, 218);
            this.chkEpisodeWatchlist.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkEpisodeWatchlist.Name = "chkEpisodeWatchlist";
            this.chkEpisodeWatchlist.Size = new System.Drawing.Size(219, 26);
            this.chkEpisodeWatchlist.TabIndex = 5;
            this.chkEpisodeWatchlist.Text = "Episode Watchlist";
            this.chkEpisodeWatchlist.UseVisualStyleBackColor = true;
            this.chkEpisodeWatchlist.Click += new System.EventHandler(this.chkEpisodeWatchlist_Click);
            // 
            // chkMovieRatings
            // 
            this.chkMovieRatings.Location = new System.Drawing.Point(342, 65);
            this.chkMovieRatings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkMovieRatings.Name = "chkMovieRatings";
            this.chkMovieRatings.Size = new System.Drawing.Size(219, 26);
            this.chkMovieRatings.TabIndex = 12;
            this.chkMovieRatings.Text = "Movie Ratings";
            this.chkMovieRatings.UseVisualStyleBackColor = true;
            this.chkMovieRatings.Click += new System.EventHandler(this.chkMovieRatings_Click);
            // 
            // chkSeasonRatings
            // 
            this.chkSeasonRatings.Location = new System.Drawing.Point(13, 447);
            this.chkSeasonRatings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSeasonRatings.Name = "chkSeasonRatings";
            this.chkSeasonRatings.Size = new System.Drawing.Size(219, 26);
            this.chkSeasonRatings.TabIndex = 11;
            this.chkSeasonRatings.Text = "Season Ratings";
            this.chkSeasonRatings.UseVisualStyleBackColor = true;
            this.chkSeasonRatings.Click += new System.EventHandler(this.chkSeasonRatings_Click);
            // 
            // chkShowRatings
            // 
            this.chkShowRatings.Location = new System.Drawing.Point(13, 408);
            this.chkShowRatings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkShowRatings.Name = "chkShowRatings";
            this.chkShowRatings.Size = new System.Drawing.Size(219, 26);
            this.chkShowRatings.TabIndex = 10;
            this.chkShowRatings.Text = "Show Ratings";
            this.chkShowRatings.UseVisualStyleBackColor = true;
            this.chkShowRatings.Click += new System.EventHandler(this.chkShowRatings_Click);
            // 
            // chkEpisodeRatings
            // 
            this.chkEpisodeRatings.Location = new System.Drawing.Point(13, 370);
            this.chkEpisodeRatings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkEpisodeRatings.Name = "chkEpisodeRatings";
            this.chkEpisodeRatings.Size = new System.Drawing.Size(219, 26);
            this.chkEpisodeRatings.TabIndex = 9;
            this.chkEpisodeRatings.Text = "Episode Ratings";
            this.chkEpisodeRatings.UseVisualStyleBackColor = true;
            this.chkEpisodeRatings.Click += new System.EventHandler(this.chkEpisodeRatings_Click);
            // 
            // chkMovieCollection
            // 
            this.chkMovieCollection.Location = new System.Drawing.Point(15, 180);
            this.chkMovieCollection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkMovieCollection.Name = "chkMovieCollection";
            this.chkMovieCollection.Size = new System.Drawing.Size(219, 26);
            this.chkMovieCollection.TabIndex = 4;
            this.chkMovieCollection.Text = "Movie Collection";
            this.chkMovieCollection.UseVisualStyleBackColor = true;
            this.chkMovieCollection.Click += new System.EventHandler(this.chkMovieCollection_Click);
            // 
            // chkEpisodeCollection
            // 
            this.chkEpisodeCollection.Location = new System.Drawing.Point(15, 142);
            this.chkEpisodeCollection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkEpisodeCollection.Name = "chkEpisodeCollection";
            this.chkEpisodeCollection.Size = new System.Drawing.Size(219, 26);
            this.chkEpisodeCollection.TabIndex = 3;
            this.chkEpisodeCollection.Text = "Episode Collection";
            this.chkEpisodeCollection.UseVisualStyleBackColor = true;
            this.chkEpisodeCollection.Click += new System.EventHandler(this.chkEpisodeCollection_Click);
            // 
            // chkMovieWatchedHistory
            // 
            this.chkMovieWatchedHistory.Location = new System.Drawing.Point(15, 103);
            this.chkMovieWatchedHistory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkMovieWatchedHistory.Name = "chkMovieWatchedHistory";
            this.chkMovieWatchedHistory.Size = new System.Drawing.Size(219, 26);
            this.chkMovieWatchedHistory.TabIndex = 2;
            this.chkMovieWatchedHistory.Text = "Movie Watched History";
            this.chkMovieWatchedHistory.UseVisualStyleBackColor = true;
            this.chkMovieWatchedHistory.Click += new System.EventHandler(this.chkMovieWatchedHistory_Click);
            // 
            // chkEpisodeWatchedHistory
            // 
            this.chkEpisodeWatchedHistory.Location = new System.Drawing.Point(15, 65);
            this.chkEpisodeWatchedHistory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkEpisodeWatchedHistory.Name = "chkEpisodeWatchedHistory";
            this.chkEpisodeWatchedHistory.Size = new System.Drawing.Size(219, 26);
            this.chkEpisodeWatchedHistory.TabIndex = 1;
            this.chkEpisodeWatchedHistory.Text = "Episode Watched History";
            this.chkEpisodeWatchedHistory.UseVisualStyleBackColor = true;
            this.chkEpisodeWatchedHistory.CheckedChanged += new System.EventHandler(this.chkEpisodeWatchedHistory_CheckedChanged);
            // 
            // lblSelect
            // 
            this.lblSelect.Location = new System.Drawing.Point(10, 25);
            this.lblSelect.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(598, 35);
            this.lblSelect.TabIndex = 0;
            this.lblSelect.Text = "Select what to export from your user account at trakt.tv:";
            // 
            // grbPath
            // 
            this.grbPath.Controls.Add(this.btnBrowsePath);
            this.grbPath.Controls.Add(this.txtExportPath);
            this.grbPath.Location = new System.Drawing.Point(12, 529);
            this.grbPath.Name = "grbPath";
            this.grbPath.Size = new System.Drawing.Size(625, 92);
            this.grbPath.TabIndex = 4;
            this.grbPath.TabStop = false;
            this.grbPath.Text = "Export Path";
            // 
            // btnBrowsePath
            // 
            this.btnBrowsePath.Location = new System.Drawing.Point(531, 33);
            this.btnBrowsePath.Name = "btnBrowsePath";
            this.btnBrowsePath.Size = new System.Drawing.Size(51, 35);
            this.btnBrowsePath.TabIndex = 1;
            this.btnBrowsePath.Text = "...";
            this.btnBrowsePath.UseVisualStyleBackColor = true;
            this.btnBrowsePath.Click += new System.EventHandler(this.btnBrowsePath_Click);
            // 
            // txtExportPath
            // 
            this.txtExportPath.Location = new System.Drawing.Point(14, 38);
            this.txtExportPath.Name = "txtExportPath";
            this.txtExportPath.ReadOnly = true;
            this.txtExportPath.Size = new System.Drawing.Size(510, 26);
            this.txtExportPath.TabIndex = 0;
            // 
            // fbdExportPath
            // 
            this.fbdExportPath.Description = "Select folder to create CSV files in:";
            // 
            // chkCustomLists
            // 
            this.chkCustomLists.AutoSize = true;
            this.chkCustomLists.Location = new System.Drawing.Point(342, 181);
            this.chkCustomLists.Name = "chkCustomLists";
            this.chkCustomLists.Size = new System.Drawing.Size(127, 24);
            this.chkCustomLists.TabIndex = 15;
            this.chkCustomLists.Text = "Custom Lists";
            this.chkCustomLists.UseVisualStyleBackColor = true;
            this.chkCustomLists.Click += new System.EventHandler(this.chkCustomLists_Click);
            // 
            // chkEpisodeComments
            // 
            this.chkEpisodeComments.AutoSize = true;
            this.chkEpisodeComments.Location = new System.Drawing.Point(342, 219);
            this.chkEpisodeComments.Name = "chkEpisodeComments";
            this.chkEpisodeComments.Size = new System.Drawing.Size(174, 24);
            this.chkEpisodeComments.TabIndex = 16;
            this.chkEpisodeComments.Text = "Episode Comments";
            this.chkEpisodeComments.UseVisualStyleBackColor = true;
            this.chkEpisodeComments.Click += new System.EventHandler(this.chkEpisodeComments_Click);
            // 
            // chkSeasonComments
            // 
            this.chkSeasonComments.AutoSize = true;
            this.chkSeasonComments.Location = new System.Drawing.Point(342, 258);
            this.chkSeasonComments.Name = "chkSeasonComments";
            this.chkSeasonComments.Size = new System.Drawing.Size(171, 24);
            this.chkSeasonComments.TabIndex = 17;
            this.chkSeasonComments.Text = "Season Comments";
            this.chkSeasonComments.UseVisualStyleBackColor = true;
            this.chkSeasonComments.Click += new System.EventHandler(this.chkSeasonComments_Click);
            // 
            // chkShowComments
            // 
            this.chkShowComments.AutoSize = true;
            this.chkShowComments.Location = new System.Drawing.Point(342, 296);
            this.chkShowComments.Name = "chkShowComments";
            this.chkShowComments.Size = new System.Drawing.Size(156, 24);
            this.chkShowComments.TabIndex = 18;
            this.chkShowComments.Text = "Show Comments";
            this.chkShowComments.UseVisualStyleBackColor = true;
            this.chkShowComments.Click += new System.EventHandler(this.chkShowComments_Click);
            // 
            // chkMovieComments
            // 
            this.chkMovieComments.AutoSize = true;
            this.chkMovieComments.Location = new System.Drawing.Point(342, 335);
            this.chkMovieComments.Name = "chkMovieComments";
            this.chkMovieComments.Size = new System.Drawing.Size(157, 24);
            this.chkMovieComments.TabIndex = 19;
            this.chkMovieComments.Text = "Movie Comments";
            this.chkMovieComments.UseVisualStyleBackColor = true;
            this.chkMovieComments.Click += new System.EventHandler(this.chkMovieComments_Click);
            // 
            // chkListComments
            // 
            this.chkListComments.AutoSize = true;
            this.chkListComments.Location = new System.Drawing.Point(342, 372);
            this.chkListComments.Name = "chkListComments";
            this.chkListComments.Size = new System.Drawing.Size(141, 24);
            this.chkListComments.TabIndex = 20;
            this.chkListComments.Text = "List Comments";
            this.chkListComments.UseVisualStyleBackColor = true;
            this.chkListComments.Click += new System.EventHandler(this.chkListComments_Click);
            // 
            // chkLikedComments
            // 
            this.chkLikedComments.AutoSize = true;
            this.chkLikedComments.Location = new System.Drawing.Point(342, 409);
            this.chkLikedComments.Name = "chkLikedComments";
            this.chkLikedComments.Size = new System.Drawing.Size(145, 24);
            this.chkLikedComments.TabIndex = 21;
            this.chkLikedComments.Text = "Comment Likes";
            this.chkLikedComments.UseVisualStyleBackColor = true;
            this.chkLikedComments.Click += new System.EventHandler(this.chkLikedComments_Click);
            // 
            // chkLikedLists
            // 
            this.chkLikedLists.AutoSize = true;
            this.chkLikedLists.Location = new System.Drawing.Point(342, 449);
            this.chkLikedLists.Name = "chkLikedLists";
            this.chkLikedLists.Size = new System.Drawing.Size(101, 24);
            this.chkLikedLists.TabIndex = 22;
            this.chkLikedLists.Text = "List Likes";
            this.chkLikedLists.UseVisualStyleBackColor = true;
            this.chkLikedLists.Click += new System.EventHandler(this.chkLikedLists_Click);
            // 
            // ExportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 693);
            this.Controls.Add(this.grbPath);
            this.Controls.Add(this.grbExport);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnExport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportDialog";
            this.Text = "Trakt Export";
            this.grbExport.ResumeLayout(false);
            this.grbExport.PerformLayout();
            this.grbPath.ResumeLayout(false);
            this.grbPath.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grbExport;
        private System.Windows.Forms.CheckBox chkMoviePausedStates;
        private System.Windows.Forms.CheckBox chkEpisodePausedStates;
        private System.Windows.Forms.CheckBox chkMovieWatchlist;
        private System.Windows.Forms.CheckBox chkSeasonWatchlist;
        private System.Windows.Forms.CheckBox chkShowWatchlist;
        private System.Windows.Forms.CheckBox chkEpisodeWatchlist;
        private System.Windows.Forms.CheckBox chkMovieRatings;
        private System.Windows.Forms.CheckBox chkSeasonRatings;
        private System.Windows.Forms.CheckBox chkShowRatings;
        private System.Windows.Forms.CheckBox chkEpisodeRatings;
        private System.Windows.Forms.CheckBox chkMovieCollection;
        private System.Windows.Forms.CheckBox chkEpisodeCollection;
        private System.Windows.Forms.CheckBox chkMovieWatchedHistory;
        private System.Windows.Forms.CheckBox chkEpisodeWatchedHistory;
        private System.Windows.Forms.Label lblSelect;
        private System.Windows.Forms.GroupBox grbPath;
        private System.Windows.Forms.Button btnBrowsePath;
        private System.Windows.Forms.TextBox txtExportPath;
        private System.Windows.Forms.FolderBrowserDialog fbdExportPath;
        private System.Windows.Forms.CheckBox chkCustomLists;
        private System.Windows.Forms.CheckBox chkListComments;
        private System.Windows.Forms.CheckBox chkMovieComments;
        private System.Windows.Forms.CheckBox chkShowComments;
        private System.Windows.Forms.CheckBox chkSeasonComments;
        private System.Windows.Forms.CheckBox chkEpisodeComments;
        private System.Windows.Forms.CheckBox chkLikedLists;
        private System.Windows.Forms.CheckBox chkLikedComments;
    }
}