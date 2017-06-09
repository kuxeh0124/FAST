using CoffeeBeanLibrary;
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace CoffeeBeanForm
{
    public partial class FAST_Settings : Form
    {
        /* ********************************************************
         * Created By by Karl Cantor
         * Frontier Automated System Testing Settings - WinForm
         * Date Created: 27/4/2016
         * Modification Log****************************************
         * 1. Initial Version - 27/4/2016 (Karl)
         * 2. Added default environment - 9/5/2016 (Alex)
         * ********************************************************                               
        */

        // Code Start -----------------
        //-----------------------------
        //=============================================================INITIALIZATION===========================================================

        //Initialize all objects and create object handlers
        public FAST_Settings()
        {
            InitializeComponent();
        }

        //Declare Local Variables
        private FAST fast = new FAST();
        private int maxJS = Utility.maxJS;
        private int maxOthers = Utility.maxOthers;
        private string adFlag = Utility.adFlag;
        private string aotFlag = Utility.aotFlag;
        private string defFolder = Utility.defFolder;
        private string defEnv = Utility.defEnv;
        private string defDriver = Utility.defDriver;
        private string schedulerFlag = Utility.schedulerFlag;
        private string schedulerPlaceholder = Utility.schedulerPlaceholder;
        private string schedulerDefaultValue = Utility.getSchedulerDefaultvalue;
        private delegate void SetTextCallback(string text);
        private string allowDupesSave;
        private string aotFlagSave;
        private string driverUse;
        private string defDriverSave;
        private string defEnvSave;
        private string defFolderPathSave;
        private string maxConPJSSave;
        private string maxConOthersSave;
        private string schedulerSave;
        private string schedulerDefaultTimeSave;

        //Load Initial object values and states
        private void Settings_Load(object sender, EventArgs e)
        {
            placeLowerRight();

            //Set initial display
            defFolderPath.Text = defFolder;
            pjsMax.Text = maxJS.ToString();
            othersMax.Text = maxOthers.ToString();
            defaultSchedTimeTextbox.Text = schedulerDefaultValue;

            //Set initial saved values;
            maxConPJSSave = pjsMax.Text;
            maxConOthersSave = othersMax.Text;
            defFolderPathSave = defFolderPath.Text;
            schedulerDefaultTimeSave = defaultSchedTimeTextbox.Text;
            defDriverSave = EvaluateTypeDriver();
            defEnvSave = EvaluateEnvironment();
            loadAOTFlag();
            loadDupesFlag();
            loadSchedulerFlag();
        }
        //=============================================================INITIALIZATION===========================================================

        //================================================================ACTIONS===============================================================
        //this is for browsing for a test case file
        //Note: Browser dialogs run on a different thread
        //Note*: For this browse button, it calls:
        //          1. ThreadCalllProcess - This is the actual browse dialog
        //          2. ThreadSetText - This sets the text on the text box after getting the value from browse
        //       Rationale: Calling a browse dialge starts a new thread but the textbox of where we want to store the text
        //                  is on our main thread. To do this, we invoked a delegate to to execute the command on the main thread.
        //Category: Action - Button
        //Object Text: Browse   
        private void browseDefPath_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            Thread th = new Thread(new ThreadStart(this.ThreadCallProcess));
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        //This is to save the value of the default folder path upon change of text of the defFolderPath textbox
        //Category: Action - Textbox
        //Object Text: File Path
        private void defFolderPath_TextChanged(object sender, EventArgs e)
        {
            defFolderPathSave = this.defFolderPath.Text;
        }

        //This is to save the value of the maximum concurrent test cases upon change of text of the pjsMax textbox
        //Category: Action - Textbox
        //Object Text: PhantomJS
        private void pjsMax_TextChanged(object sender, EventArgs e)
        {
            maxConPJSSave = this.pjsMax.Text;
        }

        //This is to save the value of the maximum concurrent test cases upon change of text of the othersMax textbox
        //Category: Action - Textbox
        //Object Text: Other Drivers
        private void othersMax_TextChanged(object sender, EventArgs e)
        {
            maxConOthersSave = this.othersMax.Text;
        }

        //This is to set whether the application should be on topmost or not
        //Category: Action - Checkbox
        //Object Text: Select to put window on top
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.aotCheckbox.Checked)
            {
                aotFlagSave = "True";
            }
            else { aotFlagSave = "False"; }
        }

        //This is to set whether the application should accept duplicates for the datagridview
        //Category: Action - Checkbox
        //Object Text: Allow
        private void allowDupes_CheckedChanged(object sender, EventArgs e)
        {
            if (this.allowDupes.Checked)
            {
                allowDupesSave = "True";
            }
            else { allowDupesSave = "False"; }
        }

        //This is to set whether the scheduler checkbox will be selected by default on application load
        //Category: Action - Checkbox
        //Object Text: Select to enable scheduler by default
        private void schedlrCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            defaultSchedTimeTextbox.Enabled = (schedlrCheckbox.CheckState == CheckState.Checked);

            if (this.schedlrCheckbox.Checked)
            {
                schedulerSave = "True";
                defaultSchedTimeTextbox.Text = schedulerDefaultTimeSave;
                defaultSchedTimeLabel.ForeColor = System.Drawing.Color.Black;
                schedRunTextBox_GotFocus(this, e);
            }
            else
            {
                schedulerSave = "False";
                defaultSchedTimeTextbox.Text = schedulerPlaceholder;
                defaultSchedTimeLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
                schedRunTextBox_RemoveFocus(this, e);
            }
        }

        //This is to save the value of the default scheduled time upon change of text of the defaultSchedTimeTextbox textbox
        //Category: Action - Textbox
        //Object Text: Default Scheduled Time
        private void defaultSchedTimeTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!this.defaultSchedTimeTextbox.Text.Equals(schedulerPlaceholder))
            {
                schedulerDefaultTimeSave = this.defaultSchedTimeTextbox.Text;
            }
        }

        //This is to exit without saving the settings into app.config
        //Category: Action - Button
        //Object Text: Exit
        private void cancelSave_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Show(this, "Settings will not be saved. Continue?",
                    "Save Settings", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Close();
            }
            else { }
        }

        //This is to exit while saving the settings into app.config
        //Category: Action - Button
        //Object Text: Save
        private void saveSettings_Click(object sender, EventArgs e)
        {
            string str = defaultSchedTimeTextbox.Text;
            Boolean valid = defaultSchedTimeTextbox_ValidateFormat(str);

            if (!valid && this.schedlrCheckbox.Checked)
            {
                MessageBoxEx.Show(this, "Please enter a valid time in HH:mm:ss [23:59:59] format on the 'Default schedule time' field.", "Time Format Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else
            {
                if (MessageBoxEx.Show(this, "Save Settings?", "Save Settings", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    evaluateDriver();
                    evaluateEnvironment();
                    Config.UpdateConfig("defDriverType", defDriverSave);
                    Config.UpdateConfig("defEnvironment", defEnvSave);
                    Config.UpdateConfig("defaultRep", defFolderPathSave);
                    Config.UpdateConfig("maxConPJS", maxConPJSSave);
                    Config.UpdateConfig("maxConOther", maxConOthersSave);
                    Config.UpdateConfig("aotFlag", aotFlagSave);
                    Config.UpdateConfig("allowDupe", allowDupesSave);
                    Config.UpdateConfig("schedulerFlag", schedulerSave);
                    Config.UpdateConfig("schedulerDefaultTime", schedulerDefaultTimeSave);
                    this.Close();
                }
            }
        }

        private Boolean defaultSchedTimeTextbox_ValidateFormat(string time)
        {
            DateTime ignored;
            return DateTime.TryParseExact(time, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ignored);
        }
        //================================================================ACTIONS===============================================================

        //================================================================UTILITY===============================================================
        //Removing default text for Schedule Run textbox
        //Category: Utility - Remove placeholder
        private void schedRunTextBox_GotFocus(object sender, EventArgs e)
        {
            string str = defaultSchedTimeTextbox.Text;
            defaultSchedTimeTextbox.ForeColor = System.Drawing.Color.Black;

            if (str.Equals(schedulerPlaceholder))
            {
                defaultSchedTimeTextbox.Text = schedulerDefaultValue;
            }

            else
            {
                defaultSchedTimeTextbox.Text = str;
            }
        }

        //Setting default text for Schedule Run textbox
        //Category: Utility - Set placeholder
        private void schedRunTextBox_RemoveFocus(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(defaultSchedTimeTextbox.Text))
            {
                defaultSchedTimeTextbox.Text = schedulerPlaceholder;
                defaultSchedTimeTextbox.ForeColor = System.Drawing.SystemColors.GrayText;
            }
        }

        //This is to evaulate the drivertype COMING FROM the appConfig
        //Category: Utility        
        private string EvaluateTypeDriver()
        {
            switch (defDriver)
            {
                case "1":
                    driverUse = "PhantomJS";
                    break;
                case "2":
                    driverUse = "Chrome";
                    break;
                case "3":
                    driverUse = "Firefox";
                    break;
                case "4":
                    driverUse = "Internet Explorer";
                    break;
            }

            foreach (Control control in this.defaultDriverGrp.Controls)
            {
                if (control.Text == driverUse)
                {
                    RadioButton radio = control as RadioButton;
                    radio.Checked = true;
                }
            }

            return driverUse;
        }

        //This is to evaulate the environment COMING FROM the appConfig
        //Category: Utility        
        private string EvaluateEnvironment()
        {
            foreach (Control control in this.defaultEnvironmentGrp.Controls)
            {
                if (control.Text == defEnv)
                {
                    RadioButton radio = control as RadioButton;
                    radio.Checked = true;
                }
            }

            return defEnv;
        }

        //Actual call to folder browser dialog
        //Category: Utility        
        private void ThreadCallProcess()
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                defFolderPathSave = folderBrowserDialog1.SelectedPath + "\\";
                this.ThreadSetText(defFolderPathSave);
            }
        }

        //Sets the text that was obtained from ThreadCallProcess by using a delegate to invoke the control on the main thread
        //Category: Utility
        private void ThreadSetText(string text)
        {
            if (this.defFolderPath.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(ThreadSetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.defFolderPath.Text = text;
            }
        }

        //This is to evaulate the drivertype GOING TO the appConfig
        //Category: Utility        
        private void evaluateDriver()
        {
            string selectedDriver = "";
            foreach (Control control in this.defaultDriverGrp.Controls)
            {
                RadioButton radio = control as RadioButton;
                if (radio.Checked)
                {
                    selectedDriver = radio.Text;
                }
            }
            switch (selectedDriver)
            {
                case "PhantomJS":
                    defDriverSave = "1";
                    break;
                case "Chrome":
                    defDriverSave = "2";
                    break;
                case "Firefox":
                    defDriverSave = "3";
                    break;
                case "Internet Explorer":
                    defDriverSave = "4";
                    break;
            }
        }

        //This is to evaulate the environment GOING TO the appConfig
        //Category: Utility        
        private void evaluateEnvironment()
        {
            foreach (Control control in this.defaultEnvironmentGrp.Controls)
            {
                RadioButton radio = control as RadioButton;
                if (radio.Checked)
                {
                    defEnvSave = radio.Text;
                }
            }
        }

        //This is to load the Always on top flag from app.Config
        //Category: Utility    
        private void loadAOTFlag()
        {
            aotFlagSave = aotFlag;
            if (aotFlagSave.ToUpper() == "TRUE")
            {
                aotCheckbox.Checked = true;
            }
            else { aotCheckbox.Checked = false; }

            if (aotCheckbox.Checked)
            {
                this.TopMost = true;
            }
        }

        //This is to load the allow dupes flag from app.Config
        //Category: Utility    
        private void loadDupesFlag()
        {
            allowDupesSave = adFlag;
            if (allowDupesSave.ToUpper() == "TRUE")
            {
                allowDupes.Checked = true;
            }
            else { allowDupes.Checked = false; }
        }

        //This is to load the scheduler default state flag from app.Config
        //Category: Utility    
        private void loadSchedulerFlag()
        {
            schedulerSave = schedulerFlag;
            if (schedulerSave.ToUpper() == "TRUE")
            {
                schedlrCheckbox.Checked = true;
                defaultSchedTimeTextbox.Enabled = true;
                defaultSchedTimeTextbox.Text = schedulerDefaultTimeSave;
                defaultSchedTimeLabel.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                schedlrCheckbox.Checked = false;
                defaultSchedTimeTextbox.Enabled = false;
                defaultSchedTimeTextbox.Text = schedulerPlaceholder;
                defaultSchedTimeLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            }
        }

        private void placeLowerRight()
        {
            //Determine "rightmost" screen
            Screen rightmost = Screen.AllScreens[0];
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                    rightmost = screen;
            }

            int left = (fast.ClientSize.Width - this.Width) / 2;
            int top = (fast.ClientSize.Height - this.Height) / 2;
            int screenleft = rightmost.WorkingArea.Right - this.Width;
            int screentop = rightmost.WorkingArea.Bottom - this.Height;

            this.Left = screenleft - left;
            this.Top = screentop;
        }
        //================================================================UTILITY===============================================================

        //================================================================CONFIG================================================================

        //================================================================CONFIG================================================================
        // Code End -------------------
        //-----------------------------
    }
}
