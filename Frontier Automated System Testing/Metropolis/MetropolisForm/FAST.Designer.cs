using CoffeeBeanLibrary;

namespace CoffeeBeanForm
{
    partial class FAST
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FAST));
            this.folderPath = new System.Windows.Forms.TextBox();
            this.browseTestCase = new System.Windows.Forms.Button();
            this.testCaseList = new System.Windows.Forms.ListBox();
            this.TestExecutionDataGrid = new System.Windows.Forms.DataGridView();
            this.getSelection = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.testName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.testStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.testResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progressPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.executionDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reportColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.hiddenModule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.executionToken = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.runCounter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addTestCase = new System.Windows.Forms.Button();
            this.RemoveAll = new System.Windows.Forms.Button();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAST = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customizeSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schedulerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSchedulerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.RemoveSelected = new System.Windows.Forms.Button();
            this.conTestCase = new System.Windows.Forms.TextBox();
            this.concurrntTC = new System.Windows.Forms.Label();
            this.driverSelection = new System.Windows.Forms.ComboBox();
            this.browser = new System.Windows.Forms.Label();
            this.launchReport = new System.Windows.Forms.Button();
            this.envSelection = new System.Windows.Forms.ComboBox();
            this.env = new System.Windows.Forms.Label();
            this.schedRunCheckBox = new System.Windows.Forms.CheckBox();
            this.schedRunTextBox = new System.Windows.Forms.TextBox();
            this.tcSearchBox = new System.Windows.Forms.TextBox();
            this.TestExecutionDataGridPanel = new System.Windows.Forms.Panel();
            this.schedulerTimer = new System.Windows.Forms.Timer(this.components);
            this.testStart = new System.Windows.Forms.Button();
            this.schedRunBtn = new System.Windows.Forms.Button();
            this.searchIcon = new System.Windows.Forms.PictureBox();
            this.pauseButton = new System.Windows.Forms.Button();
            this.addAllTestCase = new System.Windows.Forms.Button();
            this.testResultBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.TestExecutionDataGrid)).BeginInit();
            this.mainMenu.SuspendLayout();
            this.TestExecutionDataGridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testResultBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // folderPath
            // 
            this.folderPath.Location = new System.Drawing.Point(12, 29);
            this.folderPath.Name = "folderPath";
            this.folderPath.Size = new System.Drawing.Size(410, 20);
            this.folderPath.TabIndex = 0;
            this.folderPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.folderPath_Refresh);
            // 
            // browseTestCase
            // 
            this.browseTestCase.Location = new System.Drawing.Point(428, 28);
            this.browseTestCase.Name = "browseTestCase";
            this.browseTestCase.Size = new System.Drawing.Size(75, 22);
            this.browseTestCase.TabIndex = 1;
            this.browseTestCase.Text = "Browse";
            this.browseTestCase.UseVisualStyleBackColor = true;
            this.browseTestCase.Click += new System.EventHandler(this.browseTestCase_Click);
            // 
            // testCaseList
            // 
            this.testCaseList.FormattingEnabled = true;
            this.testCaseList.Location = new System.Drawing.Point(12, 95);
            this.testCaseList.Name = "testCaseList";
            this.testCaseList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.testCaseList.Size = new System.Drawing.Size(241, 186);
            this.testCaseList.TabIndex = 2;
            this.testCaseList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.testCaseList_MouseDoubleClick);
            // 
            // TestExecutionDataGrid
            // 
            this.TestExecutionDataGrid.AllowUserToAddRows = false;
            this.TestExecutionDataGrid.AllowUserToDeleteRows = false;
            this.TestExecutionDataGrid.AllowUserToResizeColumns = false;
            this.TestExecutionDataGrid.AllowUserToResizeRows = false;
            this.TestExecutionDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TestExecutionDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TestExecutionDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.getSelection,
            this.testName,
            this.testStatus,
            this.testResult,
            this.progressPercent,
            this.executionDate,
            this.reportColumn,
            this.hiddenModule,
            this.executionToken,
            this.runCounter});
            this.TestExecutionDataGrid.Location = new System.Drawing.Point(1, 1);
            this.TestExecutionDataGrid.MultiSelect = false;
            this.TestExecutionDataGrid.Name = "TestExecutionDataGrid";
            this.TestExecutionDataGrid.RowHeadersVisible = false;
            this.TestExecutionDataGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.TestExecutionDataGrid.Size = new System.Drawing.Size(628, 184);
            this.TestExecutionDataGrid.TabIndex = 3;
            this.TestExecutionDataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TestExecutionDataGrid_CellClick);
            this.TestExecutionDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TestExecutionDataGrid_CellContentClick);
            // 
            // getSelection
            // 
            this.getSelection.HeaderText = "";
            this.getSelection.Name = "getSelection";
            this.getSelection.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.getSelection.Width = 31;
            // 
            // testName
            // 
            this.testName.HeaderText = "Test Name";
            this.testName.Name = "testName";
            this.testName.ReadOnly = true;
            this.testName.Width = 183;
            // 
            // testStatus
            // 
            this.testStatus.HeaderText = "Status";
            this.testStatus.Name = "testStatus";
            this.testStatus.ReadOnly = true;
            // 
            // testResult
            // 
            this.testResult.HeaderText = "Result";
            this.testResult.Name = "testResult";
            this.testResult.ReadOnly = true;
            this.testResult.Width = 60;
            // 
            // progressPercent
            // 
            this.progressPercent.HeaderText = "%";
            this.progressPercent.Name = "progressPercent";
            this.progressPercent.ReadOnly = true;
            this.progressPercent.Width = 40;
            // 
            // executionDate
            // 
            this.executionDate.HeaderText = "Execution Date";
            this.executionDate.Name = "executionDate";
            this.executionDate.ReadOnly = true;
            this.executionDate.Width = 120;
            // 
            // reportColumn
            // 
            this.reportColumn.HeaderText = "";
            this.reportColumn.Name = "reportColumn";
            this.reportColumn.ReadOnly = true;
            this.reportColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.reportColumn.Width = 93;
            // 
            // hiddenModule
            // 
            this.hiddenModule.HeaderText = "Module";
            this.hiddenModule.Name = "hiddenModule";
            this.hiddenModule.ReadOnly = true;
            this.hiddenModule.Visible = false;
            // 
            // executionToken
            // 
            this.executionToken.HeaderText = "";
            this.executionToken.Name = "executionToken";
            this.executionToken.Visible = false;
            // 
            // runCounter
            // 
            this.runCounter.HeaderText = "";
            this.runCounter.Name = "runCounter";
            this.runCounter.Visible = false;
            // 
            // addTestCase
            // 
            this.addTestCase.Location = new System.Drawing.Point(12, 284);
            this.addTestCase.Name = "addTestCase";
            this.addTestCase.Size = new System.Drawing.Size(64, 23);
            this.addTestCase.TabIndex = 6;
            this.addTestCase.Text = "Add";
            this.addTestCase.UseVisualStyleBackColor = true;
            this.addTestCase.Click += new System.EventHandler(this.addTestCase_Click);
            // 
            // RemoveAll
            // 
            this.RemoveAll.Location = new System.Drawing.Point(489, 284);
            this.RemoveAll.Name = "RemoveAll";
            this.RemoveAll.Size = new System.Drawing.Size(75, 23);
            this.RemoveAll.TabIndex = 9;
            this.RemoveAll.Text = "Remove All";
            this.RemoveAll.UseVisualStyleBackColor = true;
            this.RemoveAll.Click += new System.EventHandler(this.RemoveAll_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(904, 24);
            this.mainMenu.TabIndex = 10;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeAST});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // closeAST
            // 
            this.closeAST.Name = "closeAST";
            this.closeAST.Size = new System.Drawing.Size(103, 22);
            this.closeAST.Text = "Close";
            this.closeAST.Click += new System.EventHandler(this.closeAST_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeSettingsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // customizeSettingsToolStripMenuItem
            // 
            this.customizeSettingsToolStripMenuItem.Name = "customizeSettingsToolStripMenuItem";
            this.customizeSettingsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.customizeSettingsToolStripMenuItem.Text = "Customize";
            this.customizeSettingsToolStripMenuItem.Click += new System.EventHandler(this.customizeSettingsToolStripMenuItem_Click);
            // 
            // schedulerToolStripMenuItem
            // 
            this.schedulerToolStripMenuItem.Name = "schedulerToolStripMenuItem";
            this.schedulerToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // openSchedulerToolStripMenuItem
            // 
            this.openSchedulerToolStripMenuItem.Name = "openSchedulerToolStripMenuItem";
            this.openSchedulerToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Please select the folder where your test case files are located:";
            // 
            // RemoveSelected
            // 
            this.RemoveSelected.Location = new System.Drawing.Point(377, 284);
            this.RemoveSelected.Name = "RemoveSelected";
            this.RemoveSelected.Size = new System.Drawing.Size(106, 23);
            this.RemoveSelected.TabIndex = 12;
            this.RemoveSelected.Text = "Remove Selected";
            this.RemoveSelected.UseVisualStyleBackColor = true;
            this.RemoveSelected.Click += new System.EventHandler(this.RemoveSelected_Click);
            // 
            // conTestCase
            // 
            this.conTestCase.Location = new System.Drawing.Point(861, 69);
            this.conTestCase.Name = "conTestCase";
            this.conTestCase.Size = new System.Drawing.Size(28, 20);
            this.conTestCase.TabIndex = 13;
            this.conTestCase.Text = "1";
            this.conTestCase.TextChanged += new System.EventHandler(this.conTestCase_TextChanged);
            // 
            // concurrntTC
            // 
            this.concurrntTC.AutoSize = true;
            this.concurrntTC.Location = new System.Drawing.Point(746, 72);
            this.concurrntTC.Name = "concurrntTC";
            this.concurrntTC.Size = new System.Drawing.Size(115, 13);
            this.concurrntTC.TabIndex = 14;
            this.concurrntTC.Text = "Concurrent Test Cases";
            // 
            // driverSelection
            // 
            this.driverSelection.FormattingEnabled = true;
            this.driverSelection.Items.AddRange(new object[] {
            "PhantomJS",
            "Chrome",
            "Firefox",
            "Internet Explorer"});
            this.driverSelection.Location = new System.Drawing.Point(307, 69);
            this.driverSelection.Name = "driverSelection";
            this.driverSelection.Size = new System.Drawing.Size(115, 21);
            this.driverSelection.TabIndex = 15;
            this.driverSelection.Text = "Chrome";
            this.driverSelection.SelectedIndexChanged += new System.EventHandler(this.driverSelection_SelectedIndexChanged);
            // 
            // browser
            // 
            this.browser.AutoSize = true;
            this.browser.Location = new System.Drawing.Point(259, 72);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(45, 13);
            this.browser.TabIndex = 16;
            this.browser.Text = "Browser";
            // 
            // launchReport
            // 
            this.launchReport.Location = new System.Drawing.Point(755, 284);
            this.launchReport.Name = "launchReport";
            this.launchReport.Size = new System.Drawing.Size(134, 23);
            this.launchReport.TabIndex = 18;
            this.launchReport.Text = "Launch Overall Report";
            this.launchReport.UseVisualStyleBackColor = true;
            this.launchReport.Click += new System.EventHandler(this.launchReport_Click);
            // 
            // envSelection
            // 
            this.envSelection.FormattingEnabled = true;
            this.envSelection.Items.AddRange(new object[] {
            "SIT1",
            "UAT1",
            "SIT2",
            "UAT2"});
            this.envSelection.Location = new System.Drawing.Point(495, 69);
            this.envSelection.Name = "envSelection";
            this.envSelection.Size = new System.Drawing.Size(115, 21);
            this.envSelection.TabIndex = 19;
            this.envSelection.Text = "SIT1";
            this.envSelection.SelectedIndexChanged += new System.EventHandler(this.envSelection_SelectedIndexChanged);
            // 
            // env
            // 
            this.env.AutoSize = true;
            this.env.Location = new System.Drawing.Point(426, 72);
            this.env.Name = "env";
            this.env.Size = new System.Drawing.Size(66, 13);
            this.env.TabIndex = 20;
            this.env.Text = "Environment";
            // 
            // schedRunCheckBox
            // 
            this.schedRunCheckBox.AutoSize = true;
            this.schedRunCheckBox.Location = new System.Drawing.Point(634, 32);
            this.schedRunCheckBox.Name = "schedRunCheckBox";
            this.schedRunCheckBox.Size = new System.Drawing.Size(94, 17);
            this.schedRunCheckBox.TabIndex = 21;
            this.schedRunCheckBox.Text = "Schedule Run";
            this.schedRunCheckBox.UseVisualStyleBackColor = true;
            this.schedRunCheckBox.CheckedChanged += new System.EventHandler(this.schedRunCheckBox_CheckStateChanged);
            // 
            // schedRunTextBox
            // 
            this.schedRunTextBox.Enabled = false;
            this.schedRunTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.schedRunTextBox.ForeColor = System.Drawing.SystemColors.GrayText;
            this.schedRunTextBox.Location = new System.Drawing.Point(728, 30);
            this.schedRunTextBox.MaxLength = 8;
            this.schedRunTextBox.Name = "schedRunTextBox";
            this.schedRunTextBox.Size = new System.Drawing.Size(130, 20);
            this.schedRunTextBox.TabIndex = 22;
            this.schedRunTextBox.Text = "[ 23 : 59 : 59 ]";
            this.schedRunTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.schedRunTextBox.WordWrap = false;
            this.schedRunTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.schedRunTextBox_TextChanged);
            // 
            // tcSearchBox
            // 
            this.tcSearchBox.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tcSearchBox.Location = new System.Drawing.Point(12, 70);
            this.tcSearchBox.Name = "tcSearchBox";
            this.tcSearchBox.Size = new System.Drawing.Size(241, 20);
            this.tcSearchBox.TabIndex = 23;
            this.tcSearchBox.Text = "Search TC...";
            this.tcSearchBox.WordWrap = false;
            this.tcSearchBox.GotFocus += new System.EventHandler(this.tcSearchBox_GotFocus);
            this.tcSearchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tcSearchBox_Refresh);
            this.tcSearchBox.LostFocus += new System.EventHandler(this.tcSearchBox_RemoveFocus);
            // 
            // TestExecutionDataGridPanel
            // 
            this.TestExecutionDataGridPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TestExecutionDataGridPanel.Controls.Add(this.TestExecutionDataGrid);
            this.TestExecutionDataGridPanel.Location = new System.Drawing.Point(259, 95);
            this.TestExecutionDataGridPanel.Name = "TestExecutionDataGridPanel";
            this.TestExecutionDataGridPanel.Size = new System.Drawing.Size(630, 186);
            this.TestExecutionDataGridPanel.TabIndex = 26;
            // 
            // schedulerTimer
            // 
            this.schedulerTimer.Interval = 1000;
            this.schedulerTimer.Tick += new System.EventHandler(this.schedulerTimer_Tick);
            // 
            // testStart
            // 
            this.testStart.Location = new System.Drawing.Point(294, 284);
            this.testStart.Name = "testStart";
            this.testStart.Size = new System.Drawing.Size(77, 23);
            this.testStart.TabIndex = 8;
            this.testStart.Text = "Start Test";
            this.testStart.UseVisualStyleBackColor = true;
            this.testStart.Click += new System.EventHandler(this.testStart_Click);
            // 
            // schedRunBtn
            // 
            this.schedRunBtn.Enabled = false;
            this.schedRunBtn.Image = ((System.Drawing.Image)(resources.GetObject("schedRunBtn.Image")));
            this.schedRunBtn.Location = new System.Drawing.Point(861, 29);
            this.schedRunBtn.Name = "schedRunBtn";
            this.schedRunBtn.Size = new System.Drawing.Size(28, 22);
            this.schedRunBtn.TabIndex = 25;
            this.schedRunBtn.UseVisualStyleBackColor = true;
            this.schedRunBtn.Click += new System.EventHandler(this.schedRunBtn_Clicked);
            // 
            // searchIcon
            // 
            this.searchIcon.BackColor = System.Drawing.SystemColors.HighlightText;
            this.searchIcon.Image = ((System.Drawing.Image)(resources.GetObject("searchIcon.Image")));
            this.searchIcon.Location = new System.Drawing.Point(235, 72);
            this.searchIcon.Name = "searchIcon";
            this.searchIcon.Size = new System.Drawing.Size(15, 15);
            this.searchIcon.TabIndex = 24;
            this.searchIcon.TabStop = false;
            // 
            // pauseButton
            // 
            this.pauseButton.Enabled = false;
            this.pauseButton.Location = new System.Drawing.Point(259, 284);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(29, 23);
            this.pauseButton.TabIndex = 27;
            this.pauseButton.Text = "| |";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // addAllTestCase
            // 
            this.addAllTestCase.Location = new System.Drawing.Point(82, 284);
            this.addAllTestCase.Name = "addAllTestCase";
            this.addAllTestCase.Size = new System.Drawing.Size(64, 23);
            this.addAllTestCase.TabIndex = 28;
            this.addAllTestCase.Text = "Add All";
            this.addAllTestCase.UseVisualStyleBackColor = true;
            this.addAllTestCase.Click += new System.EventHandler(this.addAllTestCase_Click);
            // 
            // testResultBindingSource
            // 
            this.testResultBindingSource.DataSource = typeof(CoffeeBeanForm.TestResult);
            // 
            // FAST
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 312);
            this.Controls.Add(this.addAllTestCase);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.schedRunBtn);
            this.Controls.Add(this.testCaseList);
            this.Controls.Add(this.searchIcon);
            this.Controls.Add(this.tcSearchBox);
            this.Controls.Add(this.schedRunTextBox);
            this.Controls.Add(this.schedRunCheckBox);
            this.Controls.Add(this.env);
            this.Controls.Add(this.envSelection);
            this.Controls.Add(this.launchReport);
            this.Controls.Add(this.browser);
            this.Controls.Add(this.driverSelection);
            this.Controls.Add(this.concurrntTC);
            this.Controls.Add(this.conTestCase);
            this.Controls.Add(this.RemoveSelected);
            this.Controls.Add(this.RemoveAll);
            this.Controls.Add(this.testStart);
            this.Controls.Add(this.addTestCase);
            this.Controls.Add(this.browseTestCase);
            this.Controls.Add(this.folderPath);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.TestExecutionDataGridPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(470, 625);
            this.MainMenuStrip = this.mainMenu;
            this.MaximizeBox = false;
            this.Name = "FAST";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frontier Automated System Testing";
            this.Load += new System.EventHandler(this.FAST_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TestExecutionDataGrid)).EndInit();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.TestExecutionDataGridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.searchIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testResultBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox folderPath;
        private System.Windows.Forms.Button browseTestCase;
        private System.Windows.Forms.ListBox testCaseList;
        private System.Windows.Forms.DataGridView TestExecutionDataGrid;
        private System.Windows.Forms.Panel TestExecutionDataGridPanel;
        private System.Windows.Forms.Button addTestCase;
        private System.Windows.Forms.Button testStart;
        private System.Windows.Forms.Button RemoveAll;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolStripMenuItem closeAST;
        private System.Windows.Forms.ToolStripMenuItem customizeSettingsToolStripMenuItem;
        private System.Windows.Forms.Button RemoveSelected;
        private System.Windows.Forms.TextBox conTestCase;
        private System.Windows.Forms.Label concurrntTC;
        private System.Windows.Forms.ComboBox driverSelection;
        private System.Windows.Forms.Label browser;
        private System.Windows.Forms.ToolStripMenuItem schedulerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSchedulerToolStripMenuItem;
        private System.Windows.Forms.Button launchReport;
        private System.Windows.Forms.BindingSource testResultBindingSource;
        private System.Windows.Forms.ComboBox envSelection;
        private System.Windows.Forms.Label env;
        private System.Windows.Forms.CheckBox schedRunCheckBox;
        private System.Windows.Forms.TextBox schedRunTextBox;
        private System.Windows.Forms.TextBox tcSearchBox;
        private System.Windows.Forms.PictureBox searchIcon;
        private System.Windows.Forms.Button schedRunBtn;
        private System.Windows.Forms.Timer schedulerTimer;
        private System.Windows.Forms.DataGridViewCheckBoxColumn getSelection;
        private System.Windows.Forms.DataGridViewTextBoxColumn testName;
        private System.Windows.Forms.DataGridViewTextBoxColumn testStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn testResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn progressPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn executionDate;
        private System.Windows.Forms.DataGridViewButtonColumn reportColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hiddenModule;
        private System.Windows.Forms.DataGridViewTextBoxColumn executionToken;
        private System.Windows.Forms.DataGridViewTextBoxColumn runCounter;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button addAllTestCase;
    }
}