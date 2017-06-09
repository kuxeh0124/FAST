namespace MetropolisForm
{
    partial class Settings
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
            this.defFolderPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.browseDefPath = new System.Windows.Forms.Button();
            this.saveSettings = new System.Windows.Forms.Button();
            this.cancelSave = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.othersMax = new System.Windows.Forms.TextBox();
            this.pjsMax = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.defaultDriverGrp = new System.Windows.Forms.GroupBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.defaultDriverGrp.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // defFolderPath
            // 
            this.defFolderPath.Location = new System.Drawing.Point(6, 32);
            this.defFolderPath.Name = "defFolderPath";
            this.defFolderPath.Size = new System.Drawing.Size(248, 20);
            this.defFolderPath.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "File Path";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.browseDefPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.defFolderPath);
            this.groupBox1.Location = new System.Drawing.Point(12, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 87);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Setup Default Path";
            // 
            // browseDefPath
            // 
            this.browseDefPath.Location = new System.Drawing.Point(6, 58);
            this.browseDefPath.Name = "browseDefPath";
            this.browseDefPath.Size = new System.Drawing.Size(75, 23);
            this.browseDefPath.TabIndex = 2;
            this.browseDefPath.Text = "Browse";
            this.browseDefPath.UseVisualStyleBackColor = true;
            this.browseDefPath.Click += new System.EventHandler(this.browseDefPath_Click);
            // 
            // saveSettings
            // 
            this.saveSettings.Location = new System.Drawing.Point(116, 287);
            this.saveSettings.Name = "saveSettings";
            this.saveSettings.Size = new System.Drawing.Size(75, 23);
            this.saveSettings.TabIndex = 3;
            this.saveSettings.Text = "Save";
            this.saveSettings.UseVisualStyleBackColor = true;
            this.saveSettings.Click += new System.EventHandler(this.saveSettings_Click);
            // 
            // cancelSave
            // 
            this.cancelSave.Location = new System.Drawing.Point(197, 287);
            this.cancelSave.Name = "cancelSave";
            this.cancelSave.Size = new System.Drawing.Size(75, 23);
            this.cancelSave.TabIndex = 4;
            this.cancelSave.Text = "Exit";
            this.cancelSave.UseVisualStyleBackColor = true;
            this.cancelSave.Click += new System.EventHandler(this.cancelSave_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.othersMax);
            this.groupBox2.Controls.Add(this.pjsMax);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 99);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 64);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Customize Maximum Threads";
            // 
            // othersMax
            // 
            this.othersMax.Location = new System.Drawing.Point(84, 39);
            this.othersMax.Name = "othersMax";
            this.othersMax.Size = new System.Drawing.Size(27, 20);
            this.othersMax.TabIndex = 3;
            this.othersMax.TextChanged += new System.EventHandler(this.othersMax_TextChanged);
            // 
            // pjsMax
            // 
            this.pjsMax.Location = new System.Drawing.Point(84, 17);
            this.pjsMax.Name = "pjsMax";
            this.pjsMax.Size = new System.Drawing.Size(27, 20);
            this.pjsMax.TabIndex = 2;
            this.pjsMax.TextChanged += new System.EventHandler(this.pjsMax_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Other Drivers";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "PhantomJS";
            // 
            // defaultDriverGrp
            // 
            this.defaultDriverGrp.Controls.Add(this.radioButton4);
            this.defaultDriverGrp.Controls.Add(this.radioButton3);
            this.defaultDriverGrp.Controls.Add(this.radioButton2);
            this.defaultDriverGrp.Controls.Add(this.radioButton1);
            this.defaultDriverGrp.Location = new System.Drawing.Point(12, 168);
            this.defaultDriverGrp.Name = "defaultDriverGrp";
            this.defaultDriverGrp.Size = new System.Drawing.Size(260, 72);
            this.defaultDriverGrp.TabIndex = 6;
            this.defaultDriverGrp.TabStop = false;
            this.defaultDriverGrp.Text = "Default Driver/Browser";
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(97, 43);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(102, 17);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Internet Explorer";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(97, 20);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(61, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Chrome";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(12, 43);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(56, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Firefox";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(12, 20);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(79, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "PhantomJS";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Please select the folder where your test case files are located:";
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Location = new System.Drawing.Point(12, 240);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(260, 40);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Always On Top";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 18);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(158, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Select to put window on top";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 312);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.defaultDriverGrp);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cancelSave);
            this.Controls.Add(this.saveSettings);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(500, 625);
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Customize";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.defaultDriverGrp.ResumeLayout(false);
            this.defaultDriverGrp.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox defFolderPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button browseDefPath;
        private System.Windows.Forms.Button saveSettings;
        private System.Windows.Forms.Button cancelSave;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox othersMax;
        private System.Windows.Forms.TextBox pjsMax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox defaultDriverGrp;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}