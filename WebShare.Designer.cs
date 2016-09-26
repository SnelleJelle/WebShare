namespace WebShare
{
    partial class WebShare
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebShare));
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lstSharedFolders = new System.Windows.Forms.ListBox();
            this.txtNewFolder = new System.Windows.Forms.TextBox();
            this.btnAddFolder = new System.Windows.Forms.Button();
            this.gbSharedFolders = new System.Windows.Forms.GroupBox();
            this.tcWebShare = new System.Windows.Forms.TabControl();
            this.tpSharedFolders = new System.Windows.Forms.TabPage();
            this.tcClients = new System.Windows.Forms.TabPage();
            this.lstWhitelistedClients = new System.Windows.Forms.ListBox();
            this.lstBlacklistedClients = new System.Windows.Forms.ListBox();
            this.lblWhitelistedClients = new System.Windows.Forms.Label();
            this.lblBlacklistedClients = new System.Windows.Forms.Label();
            this.btnBlacklistClient = new System.Windows.Forms.Button();
            this.btnWhitelistclient = new System.Windows.Forms.Button();
            this.gbClients = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.gbSharedFolders.SuspendLayout();
            this.tcWebShare.SuspendLayout();
            this.tpSharedFolders.SuspendLayout();
            this.tcClients.SuspendLayout();
            this.gbClients.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbLogo
            // 
            this.pbLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbLogo.Image")));
            this.pbLogo.Location = new System.Drawing.Point(17, 27);
            this.pbLogo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(309, 317);
            this.pbLogo.TabIndex = 0;
            this.pbLogo.TabStop = false;
            // 
            // lstSharedFolders
            // 
            this.lstSharedFolders.FormattingEnabled = true;
            this.lstSharedFolders.ItemHeight = 20;
            this.lstSharedFolders.Location = new System.Drawing.Point(350, 27);
            this.lstSharedFolders.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstSharedFolders.Name = "lstSharedFolders";
            this.lstSharedFolders.Size = new System.Drawing.Size(798, 304);
            this.lstSharedFolders.TabIndex = 1;
            // 
            // txtNewFolder
            // 
            this.txtNewFolder.Location = new System.Drawing.Point(17, 354);
            this.txtNewFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtNewFolder.Name = "txtNewFolder";
            this.txtNewFolder.Size = new System.Drawing.Size(751, 26);
            this.txtNewFolder.TabIndex = 2;
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.Location = new System.Drawing.Point(779, 351);
            this.btnAddFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(112, 35);
            this.btnAddFolder.TabIndex = 3;
            this.btnAddFolder.Text = "Add folder";
            this.btnAddFolder.UseVisualStyleBackColor = true;
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            // 
            // gbSharedFolders
            // 
            this.gbSharedFolders.Controls.Add(this.pbLogo);
            this.gbSharedFolders.Controls.Add(this.btnAddFolder);
            this.gbSharedFolders.Controls.Add(this.lstSharedFolders);
            this.gbSharedFolders.Controls.Add(this.txtNewFolder);
            this.gbSharedFolders.Location = new System.Drawing.Point(6, 6);
            this.gbSharedFolders.Name = "gbSharedFolders";
            this.gbSharedFolders.Size = new System.Drawing.Size(1210, 448);
            this.gbSharedFolders.TabIndex = 4;
            this.gbSharedFolders.TabStop = false;
            // 
            // tcWebShare
            // 
            this.tcWebShare.Controls.Add(this.tpSharedFolders);
            this.tcWebShare.Controls.Add(this.tcClients);
            this.tcWebShare.Location = new System.Drawing.Point(12, 12);
            this.tcWebShare.Name = "tcWebShare";
            this.tcWebShare.SelectedIndex = 0;
            this.tcWebShare.Size = new System.Drawing.Size(1352, 619);
            this.tcWebShare.TabIndex = 4;
            // 
            // tpSharedFolders
            // 
            this.tpSharedFolders.Controls.Add(this.gbSharedFolders);
            this.tpSharedFolders.Location = new System.Drawing.Point(4, 29);
            this.tpSharedFolders.Name = "tpSharedFolders";
            this.tpSharedFolders.Padding = new System.Windows.Forms.Padding(3);
            this.tpSharedFolders.Size = new System.Drawing.Size(1344, 586);
            this.tpSharedFolders.TabIndex = 0;
            this.tpSharedFolders.Text = "Shared folders";
            this.tpSharedFolders.UseVisualStyleBackColor = true;
            // 
            // tcClients
            // 
            this.tcClients.Controls.Add(this.gbClients);
            this.tcClients.Location = new System.Drawing.Point(4, 29);
            this.tcClients.Name = "tcClients";
            this.tcClients.Padding = new System.Windows.Forms.Padding(3);
            this.tcClients.Size = new System.Drawing.Size(1344, 586);
            this.tcClients.TabIndex = 1;
            this.tcClients.Text = "Clients";
            this.tcClients.UseVisualStyleBackColor = true;
            // 
            // lstWhitelistedClients
            // 
            this.lstWhitelistedClients.FormattingEnabled = true;
            this.lstWhitelistedClients.ItemHeight = 20;
            this.lstWhitelistedClients.Location = new System.Drawing.Point(21, 54);
            this.lstWhitelistedClients.Name = "lstWhitelistedClients";
            this.lstWhitelistedClients.Size = new System.Drawing.Size(191, 244);
            this.lstWhitelistedClients.TabIndex = 0;
            // 
            // lstBlacklistedClients
            // 
            this.lstBlacklistedClients.FormattingEnabled = true;
            this.lstBlacklistedClients.ItemHeight = 20;
            this.lstBlacklistedClients.Location = new System.Drawing.Point(267, 54);
            this.lstBlacklistedClients.Name = "lstBlacklistedClients";
            this.lstBlacklistedClients.Size = new System.Drawing.Size(191, 244);
            this.lstBlacklistedClients.TabIndex = 1;
            // 
            // lblWhitelistedClients
            // 
            this.lblWhitelistedClients.AutoSize = true;
            this.lblWhitelistedClients.Location = new System.Drawing.Point(17, 31);
            this.lblWhitelistedClients.Name = "lblWhitelistedClients";
            this.lblWhitelistedClients.Size = new System.Drawing.Size(136, 20);
            this.lblWhitelistedClients.TabIndex = 2;
            this.lblWhitelistedClients.Text = "Whitelisted clients";
            // 
            // lblBlacklistedClients
            // 
            this.lblBlacklistedClients.AutoSize = true;
            this.lblBlacklistedClients.Location = new System.Drawing.Point(263, 31);
            this.lblBlacklistedClients.Name = "lblBlacklistedClients";
            this.lblBlacklistedClients.Size = new System.Drawing.Size(134, 20);
            this.lblBlacklistedClients.TabIndex = 3;
            this.lblBlacklistedClients.Text = "Blacklisted clients";
            // 
            // btnBlacklistClient
            // 
            this.btnBlacklistClient.Location = new System.Drawing.Point(21, 316);
            this.btnBlacklistClient.Name = "btnBlacklistClient";
            this.btnBlacklistClient.Size = new System.Drawing.Size(75, 34);
            this.btnBlacklistClient.TabIndex = 4;
            this.btnBlacklistClient.Text = "Block";
            this.btnBlacklistClient.UseVisualStyleBackColor = true;
            // 
            // btnWhitelistclient
            // 
            this.btnWhitelistclient.Location = new System.Drawing.Point(267, 316);
            this.btnWhitelistclient.Name = "btnWhitelistclient";
            this.btnWhitelistclient.Size = new System.Drawing.Size(75, 34);
            this.btnWhitelistclient.TabIndex = 5;
            this.btnWhitelistclient.Text = "Allow";
            this.btnWhitelistclient.UseVisualStyleBackColor = true;
            // 
            // gbClients
            // 
            this.gbClients.Controls.Add(this.lblWhitelistedClients);
            this.gbClients.Controls.Add(this.btnWhitelistclient);
            this.gbClients.Controls.Add(this.lstWhitelistedClients);
            this.gbClients.Controls.Add(this.btnBlacklistClient);
            this.gbClients.Controls.Add(this.lstBlacklistedClients);
            this.gbClients.Controls.Add(this.lblBlacklistedClients);
            this.gbClients.Location = new System.Drawing.Point(6, 6);
            this.gbClients.Name = "gbClients";
            this.gbClients.Size = new System.Drawing.Size(1332, 574);
            this.gbClients.TabIndex = 6;
            this.gbClients.TabStop = false;
            // 
            // WebShare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1376, 643);
            this.Controls.Add(this.tcWebShare);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "WebShare";
            this.Text = "WebShare";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WebShare_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.gbSharedFolders.ResumeLayout(false);
            this.gbSharedFolders.PerformLayout();
            this.tcWebShare.ResumeLayout(false);
            this.tpSharedFolders.ResumeLayout(false);
            this.tcClients.ResumeLayout(false);
            this.gbClients.ResumeLayout(false);
            this.gbClients.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.ListBox lstSharedFolders;
        private System.Windows.Forms.TextBox txtNewFolder;
        private System.Windows.Forms.Button btnAddFolder;
        private System.Windows.Forms.GroupBox gbSharedFolders;
        private System.Windows.Forms.TabControl tcWebShare;
        private System.Windows.Forms.TabPage tpSharedFolders;
        private System.Windows.Forms.TabPage tcClients;
        private System.Windows.Forms.GroupBox gbClients;
        private System.Windows.Forms.Label lblWhitelistedClients;
        private System.Windows.Forms.Button btnWhitelistclient;
        private System.Windows.Forms.ListBox lstWhitelistedClients;
        private System.Windows.Forms.Button btnBlacklistClient;
        private System.Windows.Forms.ListBox lstBlacklistedClients;
        private System.Windows.Forms.Label lblBlacklistedClients;
    }
}

