using CoffeeBeanLibrary;

namespace CoffeeBeanForm
{
    partial class FAST_Settings
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
            this.filePathLabel = new System.Windows.Forms.Label();
            this.defaultPathGrp = new System.Windows.Forms.GroupBox();
            this.browseDefPath = new System.Windows.Forms.Button();
            this.saveSettings = new System.Windows.Forms.Button();
            this.cancelSave = new System.Windows.Forms.Button();
            this.maxThreadGrp = new System.Windows.Forms.GroupBox();
            this.othersMinLabel = new System.Windows.Forms.Label();
            this.pjsMinLabel = new System.Windows.Forms.Label();
            this.othersMax = new System.Windows.Forms.TextBox();
            this.pjsMax = new System.Windows.Forms.TextBox();
            this.maxThreadOthBrowserLabel = new System.Windows.Forms.Label();
            this.maxThreadPJSLabel = new System.Windows.Forms.Label();
            this.defaultDriverGrp = new System.Windows.Forms.GroupBox();
            this.defaultBrowserIEx = new System.Windows.Forms.RadioButton();
            this.defaultBrowserChrome = new System.Windows.Forms.RadioButton();
            this.defaultBrowserFFox = new System.Windows.Forms.RadioButton();
            this.defaultBrowserPJS = new System.Windows.Forms.RadioButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.aotGrp = new System.Windows.Forms.GroupBox();
            this.aotCheckbox = new System.Windows.Forms.CheckBox();
            this.allowDupesGrp = new System.Windows.Forms.GroupBox();
            this.allowDupes = new System.Windows.Forms.CheckBox();
            this.defaultEnvironmentGrp = new System.Windows.Forms.GroupBox();
            this.defaultEnvUAT2 = new System.Windows.Forms.RadioButton();
            this.defaultEnvUAT1 = new System.Windows.Forms.RadioButton();
            this.defaultEnvSIT2 = new System.Windows.Forms.RadioButton();
            this.defaultEnvSIT1 = new System.Windows.Forms.RadioButton();
            this.schedulerGrp = new System.Windows.Forms.GroupBox();
            this.defaultSchedTimeLabel = new System.Windows.Forms.Label();
            this.defaultSchedTimeTextbox = new System.Windows.Forms.TextBox();
            this.schedlrCheckbox = new System.Windows.Forms.CheckBox();
            this.defaultPathGrp.SuspendLayout();
            this.maxThreadGrp.SuspendLayout();
            this.defaultDriverGrp.SuspendLayout();
            this.aotGrp.SuspendLayout();
            this.allowDupesGrp.SuspendLayout();
            this.defaultEnvironmentGrp.SuspendLayout();
            this.schedulerGrp.SuspendLayout();
            this.SuspendLayout();
            // 
            // defFolderPath
            // 
            this.defFolderPath.Location = new System.Drawing.Point(6, 32);
            this.defFolderPath.Name = "defFolderPath";
            this.defFolderPath.Size = new System.Drawing.Size(346, 20);
            this.defFolderPath.TabIndex = 0;
            this.defFolderPath.TextChanged += new System.EventHandler(this.defFolderPath_TextChanged);
            // 
            // filePathLabel
            // 
            this.filePathLabel.AutoSize = true;
            this.filePathLabel.Location = new System.Drawing.Point(6, 16);
            this.filePathLabel.Name = "filePathLabel";
            this.filePathLabel.Size = new System.Drawing.Size(48, 13);
            this.filePathLabel.TabIndex = 1;
            this.filePathLabel.Text = "File Path";
            // 
            // defaultPathGrp
            // 
            this.defaultPathGrp.Controls.Add(this.browseDefPath);
            this.defaultPathGrp.Controls.Add(this.filePathLabel);
            this.defaultPathGrp.Controls.Add(this.defFolderPath);
            this.defaultPathGrp.Location = new System.Drawing.Point(12, 6);
            this.defaultPathGrp.Name = "defaultPathGrp";
            this.defaultPathGrp.Size = new System.Drawing.Size(358, 87);
            this.defaultPathGrp.TabIndex = 2;
            this.defaultPathGrp.TabStop = false;
            this.defaultPathGrp.Text = "Setup Default Path";
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
            this.saveSettings.Location = new System.Drawing.Point(214, 451);
            this.saveSettings.Name = "saveSettings";
            this.saveSettings.Size = new System.Drawing.Size(75, 23);
            this.saveSettings.TabIndex = 3;
            this.saveSettings.Text = "Save";
            this.saveSettings.UseVisualStyleBackColor = true;
            this.saveSettings.Click += new System.EventHandler(this.saveSettings_Click);
            // 
            // cancelSave
            // 
            this.cancelSave.Location = new System.Drawing.Point(295, 451);
            this.cancelSave.Name = "cancelSave";
            this.cancelSave.Size = new System.Drawing.Size(75, 23);
            this.cancelSave.TabIndex = 4;
            this.cancelSave.Text = "Exit";
            this.cancelSave.UseVisualStyleBackColor = true;
            this.cancelSave.Click += new System.EventHandler(this.cancelSave_Click);
            // 
            // maxThreadGrp
            // 
            this.maxThreadGrp.Controls.Add(this.othersMinLabel);
            this.maxThreadGrp.Controls.Add(this.pjsMinLabel);
            this.maxThreadGrp.Controls.Add(this.othersMax);
            this.maxThreadGrp.Controls.Add(this.pjsMax);
            this.maxThreadGrp.Controls.Add(this.maxThreadOthBrowserLabel);
            this.maxThreadGrp.Controls.Add(this.maxThreadPJSLabel);
            this.maxThreadGrp.Location = new System.Drawing.Point(12, 99);
            this.maxThreadGrp.Name = "maxThreadGrp";
            this.maxThreadGrp.Size = new System.Drawing.Size(358, 64);
            this.maxThreadGrp.TabIndex = 5;
            this.maxThreadGrp.TabStop = false;
            this.maxThreadGrp.Text = "Customize Maximum Threads";
            // 
            // othersMinLabel
            // 
            this.othersMinLabel.AutoSize = true;
            this.othersMinLabel.Location = new System.Drawing.Point(118, 42);
            this.othersMinLabel.Name = "othersMinLabel";
            this.othersMinLabel.Size = new System.Drawing.Size(66, 13);
            this.othersMinLabel.TabIndex = 5;
            this.othersMinLabel.Text = "(Minimum: 1)";
            // 
            // pjsMinLabel
            // 
            this.pjsMinLabel.AutoSize = true;
            this.pjsMinLabel.Location = new System.Drawing.Point(118, 20);
            this.pjsMinLabel.Name = "pjsMinLabel";
            this.pjsMinLabel.Size = new System.Drawing.Size(66, 13);
            this.pjsMinLabel.TabIndex = 4;
            this.pjsMinLabel.Text = "(Minimum: 1)";
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
            // maxThreadOthBrowserLabel
            // 
            this.maxThreadOthBrowserLabel.AutoSize = true;
            this.maxThreadOthBrowserLabel.Location = new System.Drawing.Point(9, 42);
            this.maxThreadOthBrowserLabel.Name = "maxThreadOthBrowserLabel";
            this.maxThreadOthBrowserLabel.Size = new System.Drawing.Size(69, 13);
            this.maxThreadOthBrowserLabel.TabIndex = 1;
            this.maxThreadOthBrowserLabel.Text = "Other Drivers";
            // 
            // maxThreadPJSLabel
            // 
            this.maxThreadPJSLabel.AutoSize = true;
            this.maxThreadPJSLabel.Location = new System.Drawing.Point(9, 20);
            this.maxThreadPJSLabel.Name = "maxThreadPJSLabel";
            this.maxThreadPJSLabel.Size = new System.Drawing.Size(61, 13);
            this.maxThreadPJSLabel.TabIndex = 0;
            this.maxThreadPJSLabel.Text = "PhantomJS";
            // 
            // defaultDriverGrp
            // 
            this.defaultDriverGrp.Controls.Add(this.defaultBrowserIEx);
            this.defaultDriverGrp.Controls.Add(this.defaultBrowserChrome);
            this.defaultDriverGrp.Controls.Add(this.defaultBrowserFFox);
            this.defaultDriverGrp.Controls.Add(this.defaultBrowserPJS);
            this.defaultDriverGrp.Location = new System.Drawing.Point(12, 168);
            this.defaultDriverGrp.Name = "defaultDriverGrp";
            this.defaultDriverGrp.Size = new System.Drawing.Size(358, 72);
            this.defaultDriverGrp.TabIndex = 6;
            this.defaultDriverGrp.TabStop = false;
            this.defaultDriverGrp.Text = "Default Driver/Browser";
            // 
            // defaultBrowserIEx
            // 
            this.defaultBrowserIEx.AutoSize = true;
            this.defaultBrowserIEx.Location = new System.Drawing.Point(97, 43);
            this.defaultBrowserIEx.Name = "defaultBrowserIEx";
            this.defaultBrowserIEx.Size = new System.Drawing.Size(102, 17);
            this.defaultBrowserIEx.TabIndex = 3;
            this.defaultBrowserIEx.TabStop = true;
            this.defaultBrowserIEx.Text = "Internet Explorer";
            this.defaultBrowserIEx.UseVisualStyleBackColor = true;
            // 
            // defaultBrowserChrome
            // 
            this.defaultBrowserChrome.AutoSize = true;
            this.defaultBrowserChrome.Location = new System.Drawing.Point(97, 20);
            this.defaultBrowserChrome.Name = "defaultBrowserChrome";
            this.defaultBrowserChrome.Size = new System.Drawing.Size(61, 17);
            this.defaultBrowserChrome.TabIndex = 2;
            this.defaultBrowserChrome.TabStop = true;
            this.defaultBrowserChrome.Text = "Chrome";
            this.defaultBrowserChrome.UseVisualStyleBackColor = true;
            // 
            // defaultBrowserFFox
            // 
            this.defaultBrowserFFox.AutoSize = true;
            this.defaultBrowserFFox.Location = new System.Drawing.Point(12, 43);
            this.defaultBrowserFFox.Name = "defaultBrowserFFox";
            this.defaultBrowserFFox.Size = new System.Drawing.Size(56, 17);
            this.defaultBrowserFFox.TabIndex = 1;
            this.defaultBrowserFFox.TabStop = true;
            this.defaultBrowserFFox.Text = "Firefox";
            this.defaultBrowserFFox.UseVisualStyleBackColor = true;
            // 
            // defaultBrowserPJS
            // 
            this.defaultBrowserPJS.AutoSize = true;
            this.defaultBrowserPJS.Location = new System.Drawing.Point(12, 20);
            this.defaultBrowserPJS.Name = "defaultBrowserPJS";
            this.defaultBrowserPJS.Size = new System.Drawing.Size(79, 17);
            this.defaultBrowserPJS.TabIndex = 0;
            this.defaultBrowserPJS.TabStop = true;
            this.defaultBrowserPJS.Text = "PhantomJS";
            this.defaultBrowserPJS.UseVisualStyleBackColor = true;
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Please select the folder where your test case files are located:";
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // aotGrp
            // 
            this.aotGrp.Controls.Add(this.aotCheckbox);
            this.aotGrp.Location = new System.Drawing.Point(12, 402);
            this.aotGrp.Name = "aotGrp";
            this.aotGrp.Size = new System.Drawing.Size(179, 40);
            this.aotGrp.TabIndex = 7;
            this.aotGrp.TabStop = false;
            this.aotGrp.Text = "Always On Top";
            // 
            // aotCheckbox
            // 
            this.aotCheckbox.AutoSize = true;
            this.aotCheckbox.Location = new System.Drawing.Point(9, 18);
            this.aotCheckbox.Name = "aotCheckbox";
            this.aotCheckbox.Size = new System.Drawing.Size(158, 17);
            this.aotCheckbox.TabIndex = 0;
            this.aotCheckbox.Text = "Select to put window on top";
            this.aotCheckbox.UseVisualStyleBackColor = true;
            this.aotCheckbox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // allowDupesGrp
            // 
            this.allowDupesGrp.Controls.Add(this.allowDupes);
            this.allowDupesGrp.Location = new System.Drawing.Point(197, 402);
            this.allowDupesGrp.Name = "allowDupesGrp";
            this.allowDupesGrp.Size = new System.Drawing.Size(173, 40);
            this.allowDupesGrp.TabIndex = 8;
            this.allowDupesGrp.TabStop = false;
            this.allowDupesGrp.Text = "Duplicates to be added";
            // 
            // allowDupes
            // 
            this.allowDupes.AutoSize = true;
            this.allowDupes.Checked = true;
            this.allowDupes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allowDupes.Enabled = false;
            this.allowDupes.Location = new System.Drawing.Point(9, 18);
            this.allowDupes.Name = "allowDupes";
            this.allowDupes.Size = new System.Drawing.Size(51, 17);
            this.allowDupes.TabIndex = 0;
            this.allowDupes.Text = "Allow";
            this.allowDupes.UseVisualStyleBackColor = true;
            this.allowDupes.CheckedChanged += new System.EventHandler(this.allowDupes_CheckedChanged);
            // 
            // defaultEnvironmentGrp
            // 
            this.defaultEnvironmentGrp.Controls.Add(this.defaultEnvUAT2);
            this.defaultEnvironmentGrp.Controls.Add(this.defaultEnvUAT1);
            this.defaultEnvironmentGrp.Controls.Add(this.defaultEnvSIT2);
            this.defaultEnvironmentGrp.Controls.Add(this.defaultEnvSIT1);
            this.defaultEnvironmentGrp.Location = new System.Drawing.Point(12, 246);
            this.defaultEnvironmentGrp.Name = "defaultEnvironmentGrp";
            this.defaultEnvironmentGrp.Size = new System.Drawing.Size(358, 72);
            this.defaultEnvironmentGrp.TabIndex = 7;
            this.defaultEnvironmentGrp.TabStop = false;
            this.defaultEnvironmentGrp.Text = "Default Environment";
            // 
            // defaultEnvUAT2
            // 
            this.defaultEnvUAT2.AutoSize = true;
            this.defaultEnvUAT2.Location = new System.Drawing.Point(97, 43);
            this.defaultEnvUAT2.Name = "defaultEnvUAT2";
            this.defaultEnvUAT2.Size = new System.Drawing.Size(53, 17);
            this.defaultEnvUAT2.TabIndex = 3;
            this.defaultEnvUAT2.TabStop = true;
            this.defaultEnvUAT2.Text = "UAT2";
            this.defaultEnvUAT2.UseVisualStyleBackColor = true;
            // 
            // defaultEnvUAT1
            // 
            this.defaultEnvUAT1.AutoSize = true;
            this.defaultEnvUAT1.Location = new System.Drawing.Point(97, 20);
            this.defaultEnvUAT1.Name = "defaultEnvUAT1";
            this.defaultEnvUAT1.Size = new System.Drawing.Size(53, 17);
            this.defaultEnvUAT1.TabIndex = 2;
            this.defaultEnvUAT1.TabStop = true;
            this.defaultEnvUAT1.Text = "UAT1";
            this.defaultEnvUAT1.UseVisualStyleBackColor = true;
            // 
            // defaultEnvSIT2
            // 
            this.defaultEnvSIT2.AutoSize = true;
            this.defaultEnvSIT2.Location = new System.Drawing.Point(12, 43);
            this.defaultEnvSIT2.Name = "defaultEnvSIT2";
            this.defaultEnvSIT2.Size = new System.Drawing.Size(48, 17);
            this.defaultEnvSIT2.TabIndex = 1;
            this.defaultEnvSIT2.TabStop = true;
            this.defaultEnvSIT2.Text = "SIT2";
            this.defaultEnvSIT2.UseVisualStyleBackColor = true;
            // 
            // defaultEnvSIT1
            // 
            this.defaultEnvSIT1.AutoSize = true;
            this.defaultEnvSIT1.Location = new System.Drawing.Point(12, 20);
            this.defaultEnvSIT1.Name = "defaultEnvSIT1";
            this.defaultEnvSIT1.Size = new System.Drawing.Size(48, 17);
            this.defaultEnvSIT1.TabIndex = 0;
            this.defaultEnvSIT1.TabStop = true;
            this.defaultEnvSIT1.Text = "SIT1";
            this.defaultEnvSIT1.UseVisualStyleBackColor = true;
            // 
            // schedulerGrp
            // 
            this.schedulerGrp.Controls.Add(this.defaultSchedTimeLabel);
            this.schedulerGrp.Controls.Add(this.defaultSchedTimeTextbox);
            this.schedulerGrp.Controls.Add(this.schedlrCheckbox);
            this.schedulerGrp.Location = new System.Drawing.Point(12, 324);
            this.schedulerGrp.Name = "schedulerGrp";
            this.schedulerGrp.Size = new System.Drawing.Size(358, 72);
            this.schedulerGrp.TabIndex = 8;
            this.schedulerGrp.TabStop = false;
            this.schedulerGrp.Text = "Scheduler";
            // 
            // defaultSchedTimeLabel
            // 
            this.defaultSchedTimeLabel.AutoSize = true;
            this.defaultSchedTimeLabel.Location = new System.Drawing.Point(9, 42);
            this.defaultSchedTimeLabel.Name = "defaultSchedTimeLabel";
            this.defaultSchedTimeLabel.Size = new System.Drawing.Size(109, 13);
            this.defaultSchedTimeLabel.TabIndex = 24;
            this.defaultSchedTimeLabel.Text = "Default schedule time";
            // 
            // defaultSchedTimeTextbox
            // 
            this.defaultSchedTimeTextbox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defaultSchedTimeTextbox.ForeColor = System.Drawing.SystemColors.GrayText;
            this.defaultSchedTimeTextbox.Location = new System.Drawing.Point(136, 39);
            this.defaultSchedTimeTextbox.MaxLength = 8;
            this.defaultSchedTimeTextbox.Name = "defaultSchedTimeTextbox";
            this.defaultSchedTimeTextbox.Size = new System.Drawing.Size(130, 20);
            this.defaultSchedTimeTextbox.TabIndex = 23;
            this.defaultSchedTimeTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.defaultSchedTimeTextbox.WordWrap = false;
            this.defaultSchedTimeTextbox.TextChanged += new System.EventHandler(this.defaultSchedTimeTextbox_TextChanged);
            // 
            // schedlrCheckbox
            // 
            this.schedlrCheckbox.AutoSize = true;
            this.schedlrCheckbox.Location = new System.Drawing.Point(12, 19);
            this.schedlrCheckbox.Name = "schedlrCheckbox";
            this.schedlrCheckbox.Size = new System.Drawing.Size(201, 17);
            this.schedlrCheckbox.TabIndex = 1;
            this.schedlrCheckbox.Text = "Select to enable scheduler by default";
            this.schedlrCheckbox.UseVisualStyleBackColor = true;
            this.schedlrCheckbox.CheckedChanged += new System.EventHandler(this.schedlrCheckbox_CheckedChanged);
            // 
            // FAST_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 486);
            this.Controls.Add(this.schedulerGrp);
            this.Controls.Add(this.defaultEnvironmentGrp);
            this.Controls.Add(this.allowDupesGrp);
            this.Controls.Add(this.aotGrp);
            this.Controls.Add(this.defaultDriverGrp);
            this.Controls.Add(this.maxThreadGrp);
            this.Controls.Add(this.cancelSave);
            this.Controls.Add(this.saveSettings);
            this.Controls.Add(this.defaultPathGrp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(500, 625);
            this.MaximizeBox = false;
            this.Name = "FAST_Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customize";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.defaultPathGrp.ResumeLayout(false);
            this.defaultPathGrp.PerformLayout();
            this.maxThreadGrp.ResumeLayout(false);
            this.maxThreadGrp.PerformLayout();
            this.defaultDriverGrp.ResumeLayout(false);
            this.defaultDriverGrp.PerformLayout();
            this.aotGrp.ResumeLayout(false);
            this.aotGrp.PerformLayout();
            this.allowDupesGrp.ResumeLayout(false);
            this.allowDupesGrp.PerformLayout();
            this.defaultEnvironmentGrp.ResumeLayout(false);
            this.defaultEnvironmentGrp.PerformLayout();
            this.schedulerGrp.ResumeLayout(false);
            this.schedulerGrp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox defFolderPath;
        private System.Windows.Forms.Label filePathLabel;
        private System.Windows.Forms.GroupBox defaultPathGrp;
        private System.Windows.Forms.Button browseDefPath;
        private System.Windows.Forms.Button saveSettings;
        private System.Windows.Forms.Button cancelSave;
        private System.Windows.Forms.GroupBox maxThreadGrp;
        private System.Windows.Forms.TextBox othersMax;
        private System.Windows.Forms.TextBox pjsMax;
        private System.Windows.Forms.Label maxThreadOthBrowserLabel;
        private System.Windows.Forms.Label maxThreadPJSLabel;
        private System.Windows.Forms.GroupBox defaultDriverGrp;
        private System.Windows.Forms.RadioButton defaultBrowserIEx;
        private System.Windows.Forms.RadioButton defaultBrowserChrome;
        private System.Windows.Forms.RadioButton defaultBrowserFFox;
        private System.Windows.Forms.RadioButton defaultBrowserPJS;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox aotGrp;
        private System.Windows.Forms.CheckBox aotCheckbox;
        private System.Windows.Forms.GroupBox allowDupesGrp;
        private System.Windows.Forms.CheckBox allowDupes;
        private System.Windows.Forms.Label othersMinLabel;
        private System.Windows.Forms.Label pjsMinLabel;
        private System.Windows.Forms.GroupBox defaultEnvironmentGrp;
        private System.Windows.Forms.RadioButton defaultEnvUAT2;
        private System.Windows.Forms.RadioButton defaultEnvUAT1;
        private System.Windows.Forms.RadioButton defaultEnvSIT2;
        private System.Windows.Forms.RadioButton defaultEnvSIT1;
        private System.Windows.Forms.GroupBox schedulerGrp;
        private System.Windows.Forms.CheckBox schedlrCheckbox;
        private System.Windows.Forms.Label defaultSchedTimeLabel;
        private System.Windows.Forms.TextBox defaultSchedTimeTextbox;
    }
}