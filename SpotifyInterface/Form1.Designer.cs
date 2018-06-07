namespace SpotifyInterface
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.authButton = new System.Windows.Forms.Button();
            this.playlistsListBox = new System.Windows.Forms.ListBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.countryLabel = new System.Windows.Forms.Label();
            this.emailLabel = new System.Windows.Forms.Label();
            this.accountLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.fromFile = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fromMonitor = new System.Windows.Forms.RadioButton();
            this.fromUrl = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.urlBox = new System.Windows.Forms.TextBox();
            this.openFileButton = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(382, 388);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Run";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // authButton
            // 
            this.authButton.Location = new System.Drawing.Point(46, 241);
            this.authButton.Name = "authButton";
            this.authButton.Size = new System.Drawing.Size(142, 28);
            this.authButton.TabIndex = 1;
            this.authButton.Text = "Authenticate Session";
            this.authButton.UseVisualStyleBackColor = true;
            this.authButton.Click += new System.EventHandler(this.authButton_Click);
            // 
            // playlistsListBox
            // 
            this.playlistsListBox.FormattingEnabled = true;
            this.playlistsListBox.Location = new System.Drawing.Point(244, 40);
            this.playlistsListBox.Name = "playlistsListBox";
            this.playlistsListBox.Size = new System.Drawing.Size(190, 342);
            this.playlistsListBox.TabIndex = 13;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(73, 25);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(10, 13);
            this.nameLabel.TabIndex = 14;
            this.nameLabel.Text = "-";
            // 
            // countryLabel
            // 
            this.countryLabel.AutoSize = true;
            this.countryLabel.Location = new System.Drawing.Point(73, 43);
            this.countryLabel.Name = "countryLabel";
            this.countryLabel.Size = new System.Drawing.Size(10, 13);
            this.countryLabel.TabIndex = 15;
            this.countryLabel.Text = "-";
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(73, 61);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(10, 13);
            this.emailLabel.TabIndex = 16;
            this.emailLabel.Text = "-";
            // 
            // accountLabel
            // 
            this.accountLabel.AutoSize = true;
            this.accountLabel.Location = new System.Drawing.Point(73, 79);
            this.accountLabel.Name = "accountLabel";
            this.accountLabel.Size = new System.Drawing.Size(10, 13);
            this.accountLabel.TabIndex = 17;
            this.accountLabel.Text = "-";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Country:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Email:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Account:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nameLabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.accountLabel);
            this.groupBox1.Controls.Add(this.emailLabel);
            this.groupBox1.Controls.Add(this.countryLabel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(21, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 113);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Details";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(463, 388);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 24;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Location = new System.Drawing.Point(481, 40);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.fileNameTextBox.TabIndex = 26;
            // 
            // fromFile
            // 
            this.fromFile.AutoSize = true;
            this.fromFile.Location = new System.Drawing.Point(9, 21);
            this.fromFile.Name = "fromFile";
            this.fromFile.Size = new System.Drawing.Size(87, 17);
            this.fromFile.TabIndex = 28;
            this.fromFile.TabStop = true;
            this.fromFile.Text = "Data from file";
            this.fromFile.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fromMonitor);
            this.groupBox2.Controls.Add(this.fromUrl);
            this.groupBox2.Controls.Add(this.fromFile);
            this.groupBox2.Location = new System.Drawing.Point(21, 282);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Input Controls";
            // 
            // fromMonitor
            // 
            this.fromMonitor.AutoSize = true;
            this.fromMonitor.Location = new System.Drawing.Point(10, 67);
            this.fromMonitor.Name = "fromMonitor";
            this.fromMonitor.Size = new System.Drawing.Size(86, 17);
            this.fromMonitor.TabIndex = 30;
            this.fromMonitor.TabStop = true;
            this.fromMonitor.Text = "Time Monitor";
            this.fromMonitor.UseVisualStyleBackColor = true;
            // 
            // fromUrl
            // 
            this.fromUrl.AutoSize = true;
            this.fromUrl.Location = new System.Drawing.Point(9, 44);
            this.fromUrl.Name = "fromUrl";
            this.fromUrl.Size = new System.Drawing.Size(85, 17);
            this.fromUrl.TabIndex = 29;
            this.fromUrl.TabStop = true;
            this.fromUrl.Text = "Data from url";
            this.fromUrl.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(241, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Tracks Being Added:";
            // 
            // urlBox
            // 
            this.urlBox.Location = new System.Drawing.Point(481, 66);
            this.urlBox.Name = "urlBox";
            this.urlBox.Size = new System.Drawing.Size(100, 20);
            this.urlBox.TabIndex = 32;
            // 
            // openFileButton
            // 
            this.openFileButton.Image = global::SpotifyInterface.Properties.Resources.if_Open_1493293;
            this.openFileButton.Location = new System.Drawing.Point(453, 38);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(27, 23);
            this.openFileButton.TabIndex = 25;
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(75, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(100, 100);
            this.pictureBox.TabIndex = 23;
            this.pictureBox.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::SpotifyInterface.Properties.Resources.if_Internet_Line_20_1587498;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Location = new System.Drawing.Point(458, 65);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 23);
            this.pictureBox1.TabIndex = 34;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 428);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.urlBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.fileNameTextBox);
            this.Controls.Add(this.openFileButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.playlistsListBox);
            this.Controls.Add(this.authButton);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New This Week";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button authButton;
        private System.Windows.Forms.ListBox playlistsListBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label countryLabel;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.Label accountLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.RadioButton fromFile;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox urlBox;
        private System.Windows.Forms.RadioButton fromMonitor;
        private System.Windows.Forms.RadioButton fromUrl;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

