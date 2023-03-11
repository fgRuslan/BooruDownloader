namespace BooruDownloader
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tagsBox = new System.Windows.Forms.TextBox();
            this.downloadButton = new System.Windows.Forms.Button();
            this.domainBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ratingCheckBox = new System.Windows.Forms.CheckBox();
            this.detectButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.limitBox = new System.Windows.Forms.TextBox();
            this.isDanbooruSite = new System.Windows.Forms.CheckBox();
            this.apiKeyTextBox = new System.Windows.Forms.TextBox();
            this.loginTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tagsBox
            // 
            this.tagsBox.Location = new System.Drawing.Point(74, 45);
            this.tagsBox.Name = "tagsBox";
            this.tagsBox.Size = new System.Drawing.Size(320, 20);
            this.tagsBox.TabIndex = 0;
            // 
            // downloadButton
            // 
            this.downloadButton.Location = new System.Drawing.Point(74, 133);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(75, 23);
            this.downloadButton.TabIndex = 1;
            this.downloadButton.Text = "Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // domainBox
            // 
            this.domainBox.Location = new System.Drawing.Point(74, 19);
            this.domainBox.Name = "domainBox";
            this.domainBox.Size = new System.Drawing.Size(202, 20);
            this.domainBox.TabIndex = 2;
            this.domainBox.Text = "http://gelbooru.com";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Website:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tags:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(409, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Status:";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statusLabel.ForeColor = System.Drawing.Color.Green;
            this.statusLabel.Location = new System.Drawing.Point(482, 25);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(65, 20);
            this.statusLabel.TabIndex = 6;
            this.statusLabel.Text = "Ready.";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(155, 137);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(121, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Keep original names";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(482, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "label4";
            // 
            // ratingCheckBox
            // 
            this.ratingCheckBox.AutoSize = true;
            this.ratingCheckBox.Location = new System.Drawing.Point(155, 160);
            this.ratingCheckBox.Name = "ratingCheckBox";
            this.ratingCheckBox.Size = new System.Drawing.Size(216, 17);
            this.ratingCheckBox.TabIndex = 11;
            this.ratingCheckBox.Text = "Include rating in beginning of a file name";
            this.ratingCheckBox.UseVisualStyleBackColor = true;
            // 
            // detectButton
            // 
            this.detectButton.Location = new System.Drawing.Point(74, 156);
            this.detectButton.Name = "detectButton";
            this.detectButton.Size = new System.Drawing.Size(75, 23);
            this.detectButton.TabIndex = 12;
            this.detectButton.Text = "Detect";
            this.detectButton.UseVisualStyleBackColor = true;
            this.detectButton.Click += new System.EventHandler(this.detectButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(413, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Download limit: ";
            // 
            // limitBox
            // 
            this.limitBox.Location = new System.Drawing.Point(500, 77);
            this.limitBox.Name = "limitBox";
            this.limitBox.Size = new System.Drawing.Size(47, 20);
            this.limitBox.TabIndex = 14;
            // 
            // isDanbooruSite
            // 
            this.isDanbooruSite.AutoSize = true;
            this.isDanbooruSite.Location = new System.Drawing.Point(283, 21);
            this.isDanbooruSite.Name = "isDanbooruSite";
            this.isDanbooruSite.Size = new System.Drawing.Size(73, 17);
            this.isDanbooruSite.TabIndex = 9;
            this.isDanbooruSite.Text = "Danbooru";
            this.isDanbooruSite.UseVisualStyleBackColor = true;
            // 
            // apiKeyTextBox
            // 
            this.apiKeyTextBox.Location = new System.Drawing.Point(74, 72);
            this.apiKeyTextBox.Name = "apiKeyTextBox";
            this.apiKeyTextBox.Size = new System.Drawing.Size(202, 20);
            this.apiKeyTextBox.TabIndex = 15;
            // 
            // loginTextBox
            // 
            this.loginTextBox.Location = new System.Drawing.Point(74, 99);
            this.loginTextBox.Name = "loginTextBox";
            this.loginTextBox.Size = new System.Drawing.Size(202, 20);
            this.loginTextBox.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "API Key:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Login:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 186);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.loginTextBox);
            this.Controls.Add(this.apiKeyTextBox);
            this.Controls.Add(this.isDanbooruSite);
            this.Controls.Add(this.limitBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.detectButton);
            this.Controls.Add(this.ratingCheckBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.domainBox);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.tagsBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "BooruDownloader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tagsBox;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.TextBox domainBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox ratingCheckBox;
        private System.Windows.Forms.Button detectButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox limitBox;
        private System.Windows.Forms.CheckBox isDanbooruSite;
        private System.Windows.Forms.TextBox apiKeyTextBox;
        private System.Windows.Forms.TextBox loginTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}

