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
            this.grbTrakt.SuspendLayout();
            this.grbTVDb.SuspendLayout();
            this.grbReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbTrakt
            // 
            this.grbTrakt.Controls.Add(this.txtTraktPassword);
            this.grbTrakt.Controls.Add(this.label2);
            this.grbTrakt.Controls.Add(this.label1);
            this.grbTrakt.Controls.Add(this.txtTraktUsername);
            this.grbTrakt.Location = new System.Drawing.Point(13, 13);
            this.grbTrakt.Name = "grbTrakt";
            this.grbTrakt.Size = new System.Drawing.Size(442, 81);
            this.grbTrakt.TabIndex = 0;
            this.grbTrakt.TabStop = false;
            this.grbTrakt.Text = "trakt.tv";
            // 
            // txtTraktPassword
            // 
            this.txtTraktPassword.Location = new System.Drawing.Point(177, 46);
            this.txtTraktPassword.Name = "txtTraktPassword";
            this.txtTraktPassword.Size = new System.Drawing.Size(244, 20);
            this.txtTraktPassword.TabIndex = 3;
            this.txtTraktPassword.UseSystemPasswordChar = true;
            this.txtTraktPassword.TextChanged += new System.EventHandler(this.txtTraktPassword_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Username:";
            // 
            // txtTraktUsername
            // 
            this.txtTraktUsername.Location = new System.Drawing.Point(177, 20);
            this.txtTraktUsername.Name = "txtTraktUsername";
            this.txtTraktUsername.Size = new System.Drawing.Size(244, 20);
            this.txtTraktUsername.TabIndex = 0;
            this.txtTraktUsername.TextChanged += new System.EventHandler(this.txtTraktUsername_TextChanged);
            // 
            // grbTVDb
            // 
            this.grbTVDb.Controls.Add(this.txtTVDbAccountId);
            this.grbTVDb.Controls.Add(this.label3);
            this.grbTVDb.Location = new System.Drawing.Point(12, 122);
            this.grbTVDb.Name = "grbTVDb";
            this.grbTVDb.Size = new System.Drawing.Size(442, 54);
            this.grbTVDb.TabIndex = 1;
            this.grbTVDb.TabStop = false;
            this.grbTVDb.Text = "thetvdb.com";
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
            this.label3.Location = new System.Drawing.Point(18, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Account Identifier:";
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Location = new System.Drawing.Point(13, 101);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(352, 13);
            this.lblDetails.TabIndex = 3;
            this.lblDetails.Text = "Enter in account details for sources you wish to transfer ratings to trakt.tv:";
            // 
            // btnImportRatings
            // 
            this.btnImportRatings.Location = new System.Drawing.Point(12, 182);
            this.btnImportRatings.Name = "btnImportRatings";
            this.btnImportRatings.Size = new System.Drawing.Size(442, 26);
            this.btnImportRatings.TabIndex = 4;
            this.btnImportRatings.Text = "Start Ratings Import";
            this.btnImportRatings.UseVisualStyleBackColor = true;
            this.btnImportRatings.Click += new System.EventHandler(this.btnImportRatings_Click);
            // 
            // pbrImportProgress
            // 
            this.pbrImportProgress.Location = new System.Drawing.Point(13, 214);
            this.pbrImportProgress.Name = "pbrImportProgress";
            this.pbrImportProgress.Size = new System.Drawing.Size(441, 23);
            this.pbrImportProgress.TabIndex = 5;
            // 
            // grbReport
            // 
            this.grbReport.Controls.Add(this.lblStatusMessage);
            this.grbReport.Controls.Add(this.label5);
            this.grbReport.Location = new System.Drawing.Point(14, 243);
            this.grbReport.Name = "grbReport";
            this.grbReport.Size = new System.Drawing.Size(441, 49);
            this.grbReport.TabIndex = 6;
            this.grbReport.TabStop = false;
            this.grbReport.Text = "Report";
            // 
            // lblStatusMessage
            // 
            this.lblStatusMessage.Location = new System.Drawing.Point(77, 20);
            this.lblStatusMessage.Name = "lblStatusMessage";
            this.lblStatusMessage.Size = new System.Drawing.Size(346, 23);
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
            // TraktRater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 299);
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
    }
}

