using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using TestCasesReader;


namespace MetropolisForm
{
    public partial class Settings : Form
    {
        /* ********************************************************
         * Created By by Karl Cantor
         * Frontier Automated System Testing Settings - WinForm
         * Date Created: 27/4/2016
         * Modification Log****************************************
         * 1. Initial Version - 27/4/2016 (Karl)
         * ********************************************************                               
        */

        // Code Start -----------------
        //-----------------------------

        //=============================================================INITIALIZATION===========================================================

        //Initialize all objects and create object handlers
        public Settings()
        {
            InitializeComponent();
        }

        //Declare Local Variables
        string defFolderPathSave;
        string maxConPJSSave;
        string maxConOthersSave;
        string driverUse;
        string defDriverSave;
        string aotFlagSave;
        delegate void SetTextCallback(string text);

        //Load Initial object values and states
        private void Settings_Load(object sender, EventArgs e)
        {
            //Set initial display
            defFolderPath.Text = defFolder;
            pjsMax.Text = maxJS;
            othersMax.Text = maxOthers;
            
            //Set initial saved values;
            defFolderPathSave = defFolderPath.Text;
            maxConPJSSave = pjsMax.Text;
            maxConOthersSave = othersMax.Text;
            defDriverSave = EvaluateTypeDriver();
            loadAOTFlag();
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
            Thread th = new Thread(new ThreadStart(this.ThreadCallProcess));
            //th.IsBackground = true;  
            th.SetApartmentState(ApartmentState.STA);
            th.Start();            
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
            if (this.checkBox1.Checked)
            {
                aotFlagSave = "True";
            }
            else { aotFlagSave = "False"; }
        }

        //This is to exit without saving the settings into app.config
        //Category: Action - Button
        //Object Text: Exit
        private void cancelSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Settings will not be saved. Continue?",
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
            if (MessageBox.Show("Save Settings?", "Save Settings", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                evaluateDriver();
                Config.UpdateConfig("defDriverType", defDriverSave);
                Config.UpdateConfig("defaultRep", defFolderPathSave);
                Config.UpdateConfig("maxConPJS", maxConPJSSave);
                Config.UpdateConfig("maxConOther", maxConOthersSave);
                Config.UpdateConfig("aotFlag", aotFlagSave);
                this.Close();
            }
            else { }
        }
        //================================================================ACTIONS===============================================================

        //================================================================UTILITY===============================================================
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

        //Actual call to folder browser dialog
        //Category: Utility        
        private void ThreadCallProcess()
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {                
                defFolderPathSave = folderBrowserDialog1.SelectedPath + "\\";
                //this.defFolderPath.Text = defFolderPathSave;
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

        //This is to load the Always on top flag from app.Config
        //Category: Utility    
        private void loadAOTFlag()
        {
            aotFlagSave = aotFlag;
            if (aotFlagSave.ToUpper() == "TRUE")
            {
                checkBox1.Checked = true;
            }
            else { checkBox1.Checked = false; }
        }
        //================================================================UTILITY===============================================================

        //================================================================CONFIG================================================================
        //These set of methos are for retrieval of data from the App.config file
        //The App.Config file contains all the options and default settings of this app

        //Default Folder
        public static string defFolder
        {
            get
            {
                return Config.GetKey("defaultRep");
            }
        }

        //Maximum concurrent test case setting for Phantom JS
        public static string maxJS
        {
            get
            {
                return Config.GetKey("maxConPJS");
            }
        }

        //Maximum concurrent test case setting for all other drivers
        public static string maxOthers
        {
            get
            {
                return Config.GetKey("maxConOther");
            }
        }

        //Default driver to be used
        public static string defDriver
        {
            get
            {
                return Config.GetKey("defDriverType");
            }
        }

        //Default setting for Always on top
        public static string aotFlag
        {
            get
            {
                return Config.GetKey("aotFlag");
            }
        }
        //================================================================CONFIG================================================================
        // Code End -------------------
        //-----------------------------
    }
}
