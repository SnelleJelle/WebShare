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
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLogo
            // 
            this.pbLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbLogo.Image")));
            this.pbLogo.Location = new System.Drawing.Point(12, 12);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(206, 206);
            this.pbLogo.TabIndex = 0;
            this.pbLogo.TabStop = false;
            // 
            // lstSharedFolders
            // 
            this.lstSharedFolders.FormattingEnabled = true;
            this.lstSharedFolders.Location = new System.Drawing.Point(224, 12);
            this.lstSharedFolders.Name = "lstSharedFolders";
            this.lstSharedFolders.Size = new System.Drawing.Size(541, 199);
            this.lstSharedFolders.TabIndex = 1;
            // 
            // txtNewFolder
            // 
            this.txtNewFolder.Location = new System.Drawing.Point(12, 244);
            this.txtNewFolder.Name = "txtNewFolder";
            this.txtNewFolder.Size = new System.Drawing.Size(502, 20);
            this.txtNewFolder.TabIndex = 2;
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.Location = new System.Drawing.Point(520, 242);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(75, 23);
            this.btnAddFolder.TabIndex = 3;
            this.btnAddFolder.Text = "Add folder";
            this.btnAddFolder.UseVisualStyleBackColor = true;
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            // 
            // WebShare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 288);
            this.Controls.Add(this.btnAddFolder);
            this.Controls.Add(this.txtNewFolder);
            this.Controls.Add(this.lstSharedFolders);
            this.Controls.Add(this.pbLogo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WebShare";
            this.Text = "WebShare";
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.ListBox lstSharedFolders;
        private System.Windows.Forms.TextBox txtNewFolder;
        private System.Windows.Forms.Button btnAddFolder;
    }
}

