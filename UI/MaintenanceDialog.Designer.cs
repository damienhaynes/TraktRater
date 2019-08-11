namespace TraktRater.UI
{
    partial class MaintenanceDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaintenanceDialog));
            this.grbMaintenance = new System.Windows.Forms.GroupBox();
            this.chkCustomLists = new System.Windows.Forms.CheckBox();
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grbMaintenance.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbMaintenance
            // 
            this.grbMaintenance.Controls.Add(this.chkCustomLists);
            this.grbMaintenance.Controls.Add(this.chkMoviePausedStates);
            this.grbMaintenance.Controls.Add(this.chkEpisodePausedStates);
            this.grbMaintenance.Controls.Add(this.chkMovieWatchlist);
            this.grbMaintenance.Controls.Add(this.chkSeasonWatchlist);
            this.grbMaintenance.Controls.Add(this.chkShowWatchlist);
            this.grbMaintenance.Controls.Add(this.chkEpisodeWatchlist);
            this.grbMaintenance.Controls.Add(this.chkMovieRatings);
            this.grbMaintenance.Controls.Add(this.chkSeasonRatings);
            this.grbMaintenance.Controls.Add(this.chkShowRatings);
            this.grbMaintenance.Controls.Add(this.chkEpisodeRatings);
            this.grbMaintenance.Controls.Add(this.chkMovieCollection);
            this.grbMaintenance.Controls.Add(this.chkEpisodeCollection);
            this.grbMaintenance.Controls.Add(this.chkMovieWatchedHistory);
            this.grbMaintenance.Controls.Add(this.chkEpisodeWatchedHistory);
            this.grbMaintenance.Controls.Add(this.lblSelect);
            this.grbMaintenance.Location = new System.Drawing.Point(8, 6);
            this.grbMaintenance.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grbMaintenance.Name = "grbMaintenance";
            this.grbMaintenance.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grbMaintenance.Size = new System.Drawing.Size(628, 378);
            this.grbMaintenance.TabIndex = 0;
            this.grbMaintenance.TabStop = false;
            // 
            // chkCustomLists
            // 
            this.chkCustomLists.AutoSize = true;
            this.chkCustomLists.Location = new System.Drawing.Point(340, 296);
            this.chkCustomLists.Name = "chkCustomLists";
            this.chkCustomLists.Size = new System.Drawing.Size(127, 24);
            this.chkCustomLists.TabIndex = 15;
            this.chkCustomLists.Text = "Custom Lists";
            this.chkCustomLists.UseVisualStyleBackColor = true;
            this.chkCustomLists.Click += new System.EventHandler(this.chkCustomLists_Click);
            // 
            // chkMoviePausedStates
            // 
            this.chkMoviePausedStates.AutoSize = true;
            this.chkMoviePausedStates.Location = new System.Drawing.Point(340, 258);
            this.chkMoviePausedStates.Name = "chkMoviePausedStates";
            this.chkMoviePausedStates.Size = new System.Drawing.Size(185, 24);
            this.chkMoviePausedStates.TabIndex = 14;
            this.chkMoviePausedStates.Text = "Movie Paused States";
            this.chkMoviePausedStates.UseVisualStyleBackColor = true;
            this.chkMoviePausedStates.Click += new System.EventHandler(this.chkMovieResumeTimes_Click);
            // 
            // chkEpisodePausedStates
            // 
            this.chkEpisodePausedStates.AutoSize = true;
            this.chkEpisodePausedStates.Location = new System.Drawing.Point(340, 219);
            this.chkEpisodePausedStates.Name = "chkEpisodePausedStates";
            this.chkEpisodePausedStates.Size = new System.Drawing.Size(202, 24);
            this.chkEpisodePausedStates.TabIndex = 13;
            this.chkEpisodePausedStates.Text = "Episode Paused States";
            this.chkEpisodePausedStates.UseVisualStyleBackColor = true;
            this.chkEpisodePausedStates.Click += new System.EventHandler(this.chkEpisodeResumeTimes_Click);
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
            this.chkMovieRatings.Location = new System.Drawing.Point(340, 180);
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
            this.chkSeasonRatings.Location = new System.Drawing.Point(340, 142);
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
            this.chkShowRatings.Location = new System.Drawing.Point(340, 103);
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
            this.chkEpisodeRatings.Location = new System.Drawing.Point(340, 65);
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
            this.chkEpisodeWatchedHistory.Click += new System.EventHandler(this.chkEpisodeWatchedHistory_Click);
            // 
            // lblSelect
            // 
            this.lblSelect.Location = new System.Drawing.Point(10, 25);
            this.lblSelect.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(598, 35);
            this.lblSelect.TabIndex = 0;
            this.lblSelect.Text = "Select what to remove from your user account at trakt.tv:";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(428, 398);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(207, 35);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start Maintenance...";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(306, 398);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 35);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // MaintenanceDialog
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(644, 448);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.grbMaintenance);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaintenanceDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Trakt Maintenance";
            this.grbMaintenance.ResumeLayout(false);
            this.grbMaintenance.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbMaintenance;
        private System.Windows.Forms.CheckBox chkMovieRatings;
        private System.Windows.Forms.CheckBox chkSeasonRatings;
        private System.Windows.Forms.CheckBox chkShowRatings;
        private System.Windows.Forms.CheckBox chkEpisodeRatings;
        private System.Windows.Forms.CheckBox chkMovieCollection;
        private System.Windows.Forms.CheckBox chkEpisodeCollection;
        private System.Windows.Forms.CheckBox chkMovieWatchedHistory;
        private System.Windows.Forms.CheckBox chkEpisodeWatchedHistory;
        private System.Windows.Forms.Label lblSelect;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkMovieWatchlist;
        private System.Windows.Forms.CheckBox chkSeasonWatchlist;
        private System.Windows.Forms.CheckBox chkShowWatchlist;
        private System.Windows.Forms.CheckBox chkEpisodeWatchlist;
        private System.Windows.Forms.CheckBox chkEpisodePausedStates;
        private System.Windows.Forms.CheckBox chkMoviePausedStates;
        private System.Windows.Forms.CheckBox chkCustomLists;
    }
}