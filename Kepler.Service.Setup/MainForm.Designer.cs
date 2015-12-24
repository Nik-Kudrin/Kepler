namespace Kepler.Service.Setup
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.InstallButton = new System.Windows.Forms.Button();
            this.browseButton = new System.Windows.Forms.Button();
            this.installationPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.MS_SQL_infoLabel = new System.Windows.Forms.Label();
            this.userMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.passwordMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Specify directory for installation Kepler Service";
            // 
            // InstallButton
            // 
            this.InstallButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InstallButton.Location = new System.Drawing.Point(367, 292);
            this.InstallButton.Name = "InstallButton";
            this.InstallButton.Size = new System.Drawing.Size(75, 23);
            this.InstallButton.TabIndex = 3;
            this.InstallButton.Text = "Install";
            this.InstallButton.UseVisualStyleBackColor = true;
            this.InstallButton.Click += new System.EventHandler(this.InstallButton_Click);
            // 
            // browseButton
            // 
            this.browseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseButton.Location = new System.Drawing.Point(367, 23);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 0;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // installationPath
            // 
            this.installationPath.Location = new System.Drawing.Point(12, 26);
            this.installationPath.Name = "installationPath";
            this.installationPath.Size = new System.Drawing.Size(349, 20);
            this.installationPath.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 210);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 4;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(12, 302);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(75, 13);
            this.versionLabel.TabIndex = 5;
            this.versionLabel.Text = "Version - 1.0.0";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(9, 163);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(224, 13);
            this.portLabel.TabIndex = 6;
            this.portLabel.Text = "Specify Kepler Service Port (or left the default)";
            // 
            // MS_SQL_infoLabel
            // 
            this.MS_SQL_infoLabel.AutoSize = true;
            this.MS_SQL_infoLabel.Location = new System.Drawing.Point(9, 77);
            this.MS_SQL_infoLabel.Name = "MS_SQL_infoLabel";
            this.MS_SQL_infoLabel.Size = new System.Drawing.Size(123, 13);
            this.MS_SQL_infoLabel.TabIndex = 7;
            this.MS_SQL_infoLabel.Text = "MS SQL connection info";
            // 
            // userMaskedTextBox
            // 
            this.userMaskedTextBox.Location = new System.Drawing.Point(44, 96);
            this.userMaskedTextBox.Name = "userMaskedTextBox";
            this.userMaskedTextBox.Size = new System.Drawing.Size(130, 20);
            this.userMaskedTextBox.TabIndex = 8;
            // 
            // passwordMaskedTextBox
            // 
            this.passwordMaskedTextBox.Location = new System.Drawing.Point(239, 96);
            this.passwordMaskedTextBox.Name = "passwordMaskedTextBox";
            this.passwordMaskedTextBox.PasswordChar = '*';
            this.passwordMaskedTextBox.Size = new System.Drawing.Size(122, 20);
            this.passwordMaskedTextBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "User";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(180, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Password";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 324);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.passwordMaskedTextBox);
            this.Controls.Add(this.userMaskedTextBox);
            this.Controls.Add(this.MS_SQL_infoLabel);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.InstallButton);
            this.Controls.Add(this.installationPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.browseButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kepler Service Installer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button InstallButton;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox installationPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Label MS_SQL_infoLabel;
        private System.Windows.Forms.MaskedTextBox userMaskedTextBox;
        private System.Windows.Forms.MaskedTextBox passwordMaskedTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

