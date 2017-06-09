using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Core;
using log4net.Appender;
using log4net.Config;
using System.Linq;
using System.Text;
using CoffeeBeanLibrary;
using CoffeeBeanRunner;
using CoffeeBeanReporter;

namespace CoffeeBeanForm
{

    public partial class FAST : Form
    {
        /* ********************************************************
         * Created By by Karl Cantor
         * Frontier Automated System Testing - WinForm
         * Date Created: 21/4/2016
         * Modification Log****************************************
         * 1. Initial Version - 21/4/2016 (Karl)
         * 2. Added environment dropdown - 6/5/2016 (Alex)
         * 3. Added logic on updating the list when textbox value is changed - 9/5/2016 (Alex)
         * 4. Added test case search box - 31/5/2016 (Alex)
         * 5. Added scheduler and custom class [CoffeeBeanLibrary.MessageBoxEx] - 6/6/2016 (Alex)
         * 6. Added stop test function. Added logic for handling null element - stop the test and mark as Blocked - 8/6/2016 (Alex)
         * ********************************************************                               
        */

        // Code Start -----------------
        //-----------------------------
        //=============================================================INITIALIZATION===========================================================
        //Initialize all objects and create object handlers
        public FAST()
        {
            InitializeComponent();
        }

        private static readonly ILog _logger = LogManager.GetLogger(typeof(FAST));

        public static ILog Logger
        {
            get { return _logger; }
        }

        //Form default values
        private string searchTCPlaceholder = Utility.searchTCPlaceholder;
        private string schedulerPlaceholder = Utility.schedulerPlaceholder;
        private string schedulerDefaultValue = Utility.getSchedulerDefaultvalue;

        //Declare Local Variables
        private int timeLeft;
        private int maxJS = Utility.maxJS;
        private int maxOthers = Utility.maxOthers;
        private string aotFlag = Utility.aotFlag;
        private string adFlag = Utility.adFlag;
        private string defFolder = Utility.defFolder;
        private string defEnv = Utility.defEnv;
        private string defDriver = Utility.defDriver;
        private string resultsDirectory = Utility.resultsDirectory;
        private string datedResultsDirectory = Utility.datedResultsDirectory;
        private string dailyDatedDirectory = Utility.dailyDatedDirectory;
        private string dailyDatedScenarioDetailedResults = Utility.dailyDatedScenarioDetailedResults;
        private string dailyDatedScenarioSummaryResults = Utility.dailyDatedScenarioSummaryResults;
        private string schedulerFlag = Utility.schedulerFlag;
        private string tsruntimeexec = Utility.tsruntimeexec;
        private string tsruntimeexecmax = Utility.tsruntimeexecmax;

        List<string> pathList = new List<string>();
        List<string> executionTimeList = new List<string>();
        List<string> moduleTSCodeList = new List<string>();
        List<float> maxRangeList = new List<float>();

        //Thread controls
        delegate void SetTextCallback(string text);
        private bool isRunning = false;
        private bool isAbortRequested = false;
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FAST));

        //Load Initial object values and states
        private void FAST_Load(object sender, EventArgs e)
        {
            Logger.Info("\n");
            Logger.Info("========================================================================");
            Logger.Info("===================== RUNNING CoffeeBean V1 APPL =======================");
            Logger.Info("========================================================================\n");

            Logger.Info(defFolder);

            placeLowerRight();
            loadDefaultFolderPath(defFolder);
            loadDefaultDriverType();
            loadDefaultEnv();
            checkAOTFlag();
            checkSchedulerFlag();
        }
        //=============================================================INITIALIZATION===========================================================

        //================================================================ACTIONS===============================================================
        //this is for browsing for a test case file
        //Note: Browser dialogs run on a different thread
        //Category: Action - Button
        //Object Text: Browse      
        private void browseTestCase_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.folderPath.Text = folderBrowserDialog.SelectedPath;
                loadDefaultFolderPath(this.folderPath.Text);
            }

        }

        //This is for refreshing the test case list after changing the path and hitting enter key
        //Category: Action - Enter key pressed on text box
        private void folderPath_Refresh(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    loadDefaultFolderPath(this.folderPath.Text);
                }
                catch (DirectoryNotFoundException)
                {
                    //Clear list if user supplies invalid directory
                    this.testCaseList.Items.Clear();
                }
            }
        }

        //This is to add a row into the datagridview using the Add button
        //Category: Action - Button
        //Object Text: Add              
        private void addTestCase_Click(object sender, EventArgs e)
        {
            if (checkIfTestCasesSelected())
            {
                foreach (string testCaseName in testCaseList.SelectedItems)
                {
                    addToDataGrid(testCaseName);
                }
                //Calls checkRows() utility to make sure the app follows the concurrent test case execution setting
                checkRows();
            }
        }

        //Adds the test case into the datagridview by double clicking on a selection
        //Category: Action - Double Click
        //Object Text: Test Case List
        private void testCaseList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.testCaseList.IndexFromPoint(e.Location);
            try
            {
                if (index != System.Windows.Forms.ListBox.NoMatches)
                {
                    addToDataGrid(testCaseList.SelectedItem.ToString());
                }
                checkRows();
            }
            catch { }
        }

        //Action is initiated when the Start Test button is clicked
        /*Following Logic:
         * 1. Take all rows in data grid and create an array for execution
         * 2. Check if there are added rows and if there are selected items in each row
         * 3. Start a new thread to execute the tasks(test cases)
         * 4. Evaluate the arrays and setup execution concurrency
         * 5. Define the tasks boolean arrays wherein the TaskCount is the number of selected rows in the datagrid
         * 6. Start the task loop. Each task will be executed
         * 7. Looping to check whether the tasks state are RanToCompletion. When all tasks are complete, the loop is stopped
        */
        //Category: Action - Button
        //Object Text: Start Test
        private void testStart_Click(object sender, EventArgs e)
        {
            runProgram();
        }

        private async void runProgram()
        {
            if (isRunning)
            {
                ExcelReaderGlobal.getRunningFlag["stopRequestFlag"] = true;
                isAbortRequested = true;
                isRunning = false;
                SetStartButtonText("Start Test");

            }

            else
            {
                #region Start process upon button click if there are no tests running.

                ExcelReaderGlobal.getRunningFlag["stopRequestFlag"] = false;
                isAbortRequested = false;
                isRunning = true;

                Boolean allTasksComplete = new Boolean();
                Boolean runNext = false;
                int TaskCount = 0;
                int currentTasks;
                int remTasks = 0;
                int getTotalSelected = 0;
                int r = 0;
                int[] rowSelected;
                string[] allValues;
                string[] allPaths;
                string[] allModCodes;
                string runVal;
                List<string> getValues = new List<string>();
                List<int> getRows = new List<int>();

                //1. Take all rows in data grid and create an array for execution--------
                foreach (DataGridViewRow row in TestExecutionDataGrid.Rows)
                {
                    if (row.Cells[0].Value.ToString() == "True")
                    {
                        string value = row.Cells[1].Value.ToString();
                        row.Cells[2].Value = "";
                        row.Cells[3].Value = "";
                        row.Cells[4].Value = "";
                        row.Cells[5].Value = "";
                        getValues.Add(value);
                        getRows.Add(row.Index);
                        getTotalSelected = getTotalSelected + 1;
                        r = r + 1;
                    }
                }
                rowSelected = getRows.ToArray();
                allValues = getValues.ToArray();
                allPaths = pathList.ToArray();

                //2. Check if there are added rows and if there are selected items in each row
                if (checkIfItemsAdded() && checkIfSelectedTestCaseIsMore())
                {
                    //3. Start a new thread to execute the tasks(test cases)
                    Thread t = new Thread(new ThreadStart(() =>
                    {

                        //This is to setup variables for the constant checking of task execution status
                        int currentRunning = 0;
                        
                        List<Boolean> lstTasksComplete = new List<Boolean>();
                        float[] allRange = maxRangeList.ToArray();
                        float curStep = 0;
                        float maxStep = 1;
                        string moduleValue = ""; //This is hidden  

                        //3.1 Change button text to "Stop Test"
                        SetStartButtonText("    Stop Test");

                        //4. Evaluate the arrays and setup execution concurrency
                        string[] testArray = allValues;
                        TaskCount = testArray.Length;
                        Boolean[] allTasks = new Boolean[TaskCount];
                        if (testArray.Length < Int32.Parse(conTestCase.Text))
                        {
                            currentTasks = testArray.Length;
                        }
                        else
                        {
                            currentTasks = Int32.Parse(conTestCase.Text);
                            remTasks = TaskCount - currentTasks;
                        }

                        //5. Define the tasks boolean arrays wherein the TaskCount is the number of selected rows in the datagrid
                        Task<Boolean>[] tasks = new Task<Boolean>[TaskCount];

                        //6. Start the task loop. Each task will be executed at this point
                        //Note: The number of tasks executed is evaluated over at the maximum concurrent tasks (settings) and concurrent task options
                        for (int x = 0; x <= (currentTasks - 1); x++)
                        {
                            ExcelReader foo = new ExcelReader(testArray[x], allPaths[x], Utility.getEnvironment);
                            tasks[x] = Task.Run(() => foo.TestRunner());
                            TestExecutionDataGrid.Rows[rowSelected[x]].Cells[2].Value = "Starting Execution";
                            TestExecutionDataGrid.Rows[rowSelected[x]].Cells[5].Value = DateTime.Now.ToString();
                            maxRangeList.Add(foo.maxRange);
                        }


                        //7. Looping to check whether the tasks state are RanToCompletion. When all tasks are complete, the loop is stopped
                        while (!allTasksComplete)
                        {
                            for (int y = 0; y <= currentTasks - 1; y++)
                            {

                                currentRunning = 0;
                                try
                                {
                                    curStep = Convert.ToSingle(ExcelReaderGlobal.getRowsCount["CurRow_" + TestExecutionDataGrid.Rows[rowSelected[y]].Cells[1].Value.ToString()]);
                                    maxStep = Convert.ToSingle(ExcelReaderGlobal.getRowsCount["MaxRow_" + TestExecutionDataGrid.Rows[rowSelected[y]].Cells[1].Value.ToString()]);

                                    moduleValue = ExcelReaderGlobal.getModuleData["ModuleVal_" + TestExecutionDataGrid.Rows[rowSelected[y]].Cells[1].Value.ToString()];
                                }
                                catch { };

                                if (!tasks[y].IsCompleted)
                                {
                                    try
                                    {
                                        TestExecutionDataGrid.Rows[rowSelected[y]].Cells[2].Value = "Running";
                                        TestExecutionDataGrid.Rows[rowSelected[y]].Cells[4].Value =
                                        (100 * (curStep / maxStep)).ToString("#") + "%";
                                        TestExecutionDataGrid.Rows[rowSelected[y]].Cells[7].Value = moduleValue;
                                    }
                                    catch { }
                                    currentRunning = currentRunning + 1;
                                    allTasks[y] = false;
                                }
                                else
                                {
                                    if (TestExecutionDataGrid.Rows[rowSelected[y]].Cells[2].Value.ToString() != "Completed" && TestExecutionDataGrid.Rows[rowSelected[y]].Cells[2].Value.ToString() != "Blocked")
                                    {
                                        if (isAbortRequested)
                                        {
                                            currentRunning = currentRunning - 1;
                                            TestExecutionDataGrid.Rows[rowSelected[y]].Cells[2].Value = "Stopped";
                                            TestExecutionDataGrid.Rows[rowSelected[y]].Cells[7].Value = moduleValue;
                                            
                                        }
                                        //                                        else if (ExcelReader.nullElemFlag && ExcelReader.nullElemTS.Equals(TestExecutionDataGrid.Rows[rowSelected[y]].Cells[1].Value))
                                        else if (ExcelReaderGlobal.getRunningFlag["nullElemFlag_" + TestExecutionDataGrid.Rows[rowSelected[y]].Cells[1].Value.ToString()] &&
                                            ExcelReaderGlobal.getModuleData["nullElemTS_" + TestExecutionDataGrid.Rows[rowSelected[y]].Cells[1].Value.ToString()].Equals(TestExecutionDataGrid.Rows[rowSelected[y]].Cells[1].Value.ToString()))
                                        {
                                            currentRunning = currentRunning - 1;
                                            TestExecutionDataGrid.Rows[rowSelected[y]].Cells[2].Value = "Blocked";
                                            TestExecutionDataGrid.Rows[rowSelected[y]].Cells[7].Value = moduleValue;
                                            ExcelReaderGlobal.getRunningFlag["nullElemFlag_" + TestExecutionDataGrid.Rows[rowSelected[y]].Cells[1].Value.ToString()] = false;
                                            ExcelReaderGlobal.getModuleData["nullElemTS_" + TestExecutionDataGrid.Rows[rowSelected[y]].Cells[1].Value] = "";
                                            ExcelReaderGlobal.getRunningDriver["driver" + TestExecutionDataGrid.Rows[rowSelected[y]].Cells[1].Value].Shutdown();
                                        }
                                        else
                                        {
                                            currentRunning = currentRunning - 1;
                                            TestExecutionDataGrid.Rows[rowSelected[y]].Cells[2].Value = "Completed";
                                            TestExecutionDataGrid.Rows[rowSelected[y]].Cells[4].Value = "100%";
                                            TestExecutionDataGrid.Rows[rowSelected[y]].Cells[7].Value = moduleValue;
                                        }

                                        try
                                        {
                                            //Added to classify if the running mode is aborted or not.
                                            if (isAbortRequested)
                                            {
                                                TestExecutionDataGrid.Rows[rowSelected[y]].Cells[3].Value = "Aborted";
                                                ExcelReaderGlobal.getRunningDriver["driver" + TestExecutionDataGrid.Rows[rowSelected[y]].Cells[1].Value].Shutdown();
                                            }
                                            else
                                            {
                                                if (tasks[y].Result)
                                                {
                                                    TestExecutionDataGrid.Rows[rowSelected[y]].Cells[3].Value = "Passed";
                                                }
                                                else 
                                                { 
                                                    TestExecutionDataGrid.Rows[rowSelected[y]].Cells[3].Value = "Failed";
                                                    ExcelReaderGlobal.getRunningDriver["driver" + TestExecutionDataGrid.Rows[rowSelected[y]].Cells[1].Value].Shutdown();
                                                }
                                            }

                                        }
                                        catch
                                        {
                                            TestExecutionDataGrid.Rows[rowSelected[y]].Cells[3].Value = "Failed";
                                            ExcelReaderGlobal.getRunningDriver["driver" + TestExecutionDataGrid.Rows[rowSelected[y]].Cells[1].Value].Shutdown();
                                        }
                                        allTasks[y] = true;
                                        runNext = true;
                                    }
                                }
                            }

                            //This checks if there are still remaining tasks left un-executed. 
                            //Conditions to go into this case:
                            /*
                             * 1. Remaining tasks is not equal to 0
                             * 2. runNext flag is set to true;
                             * 3. concurrentRunning (check of if max test cases are executed) is not equal to max setting
                             */
                            if (remTasks != 0 && runNext && currentRunning != Int32.Parse(conTestCase.Text))
                            {
                                ExcelReader foo = new ExcelReader(testArray[currentTasks], allPaths[currentTasks], Utility.getEnvironment);
                                tasks[currentTasks] = Task.Run(() => foo.TestRunner());
                                TestExecutionDataGrid.Rows[rowSelected[currentTasks]].Cells[2].Value = "Starting Execution";
                                TestExecutionDataGrid.Rows[rowSelected[currentTasks]].Cells[5].Value = DateTime.Now.ToString();
                                currentTasks = currentTasks + 1;
                                remTasks = TaskCount - currentTasks;
                                runNext = false;
                                allTasks[currentTasks - 1] = false;
                            }

                            for (int z = 0; z <= allTasks.Length - 1; z++)
                            {
                                lstTasksComplete.Add(allTasks[z]);
                            }

                            //Check if all tasks are completed based on flags set inside allTasks[]
                            allTasksComplete = EvaluateTasksStatus(lstTasksComplete.ToArray());
                            if (!allTasksComplete)
                            {
                                lstTasksComplete.Clear();
                            }
                        }

                        isRunning = false;
                        SetStartButtonText("Start Test");

                        CreateFolderForCurrentExec();
                        runVal = CopyCurrentExecution();
                        allModCodes = moduleTSCodeList.ToArray();
                        WriteTestDataGridResults(runVal);
                        Report.GenerateReportOverall(Utility.getEnvironment, DateTime.Now.ToString("dd-MMM-yyyy"));
                    }));

                    t.IsBackground = true;
                    t.Start();
                }
                #endregion
            }
        }

        //Removes all the rows inside the datagrid row
        //Category: Action - Button
        //Object Text: Remove All
        private void RemoveAll_Click(object sender, EventArgs e)
        {
            TestExecutionDataGrid.Rows.Clear();
            pathList.Clear();
            conTestCase.Text = "1";
        }

        //Removes selected rows from the datagrid
        //Category: Action - Button
        //Object Text: Remove Selected
        private void RemoveSelected_Click(object sender, EventArgs e)
        {
            try
            {
                TestExecutionDataGrid.Rows.RemoveAt(TestExecutionDataGrid.CurrentCell.RowIndex);
                if (TestExecutionDataGrid.Rows.Count == 0)
                {
                    conTestCase.Text = "1";
                }
                else
                {
                    conTestCase.Text = String.Format("{0}", TestExecutionDataGrid.Rows.Count);
                }
                
            }
            catch 
            { 
                if(TestExecutionDataGrid.Rows.Count > 0)
                {
                    MessageBox.Show(this,"Please select item to remove!","Warning",MessageBoxButtons.OK,MessageBoxIcon.Asterisk); 
                }

            }
        }

        //Updates the app.config upon changing of browser to be used for execution
        //Category: Action - Dropdown Change
        //Object Text: Browser
        private void driverSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkRows();
            Config.UpdateConfig("driverType", (driverSelection.SelectedIndex + 1).ToString());
        }

        //Updates the app.config upon changing of environment to be used for execution
        //Category: Action - Dropdown Change
        //Object Text: Environment
        private void envSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = null;
            switch (envSelection.SelectedIndex)
            {
                case 0:
                    str = "SIT1";
                    break;
                case 1:
                    str = "UAT1";
                    break;
                case 2:
                    str = "SIT2";
                    break;
                case 3:
                    str = "UAT2";
                    break;
                default:
                    break;
            }
            Config.UpdateConfig("environment", str);
        }

        //Launches the overall report for the current execution
        //Category: Action - Button
        //Object Text: Launch Overall Report
        private void launchReport_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            Thread t = new Thread(new ThreadStart(() =>
            {
                Application.Run(new FAST_ReportDatePicker());
            }));
            t.Start();
            t.Join();
            if (aotFlag.ToUpper() == "TRUE")
            {
                this.TopMost = true;
            }
            else { this.TopMost = false; }
        }

        //Adds the test case into the datagridview by double clicking on a selection
        //Category: Action - Button (Datagridview)
        //Object Text: Launch Report (Datagridview)
        private void TestExecutionDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string resFileName;

            if (e.ColumnIndex == 6)
            {
                resFileName = resultsDirectory + Utility.getEnvironment + "\\" +
                TestExecutionDataGrid.Rows[e.RowIndex]
                .Cells[1].Value.ToString().Replace(".xlsx", "").Replace(".xls", "") +
                "\\" + TestExecutionDataGrid.Rows[e.RowIndex]
                .Cells[1].Value.ToString().Replace(".xlsx", "").Replace(".xls", "") +
                ".html";
                try
                {
                    System.Diagnostics.Process.Start(resFileName);
                }
                catch (Exception)
                {
                    MessageBoxEx.Show(this, "Result File Missing", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        //Clicking on close in the menustrip
        //Category: Action - MenuStrip Item
        //Object Text: Close
        private void closeAST_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Show(this, "Are you sure you want to exit?", "Exit", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        //Clicking on settings to launch the settings form
        //Category: Action - MenuStrip Item
        //Object Text: Settings
        private void customizeSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            Thread t = new Thread(new ThreadStart(() =>
            {
                Application.Run(new FAST_Settings());
            }));
            t.Start();
            t.Join();
            if (aotFlag.ToUpper() == "TRUE")
            {
                this.TopMost = true;
            }
            else { this.TopMost = false; }

        }

        //Selecting Schedule Run checkbox
        //Category: Action - Checkbox selected
        //Object Text: Schedule Run
        private void schedRunCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            schedRunTextBox.Enabled = (schedRunCheckBox.CheckState == CheckState.Checked);
            schedRunBtn.Enabled = (schedRunCheckBox.CheckState == CheckState.Checked);

            if (schedRunTextBox.Enabled)
            {
                schedRunTextBox.Text = schedulerDefaultValue;
                schedRunTextBox_GotFocus(this, e);
            }
            else
            {
                schedRunTextBox.Text = schedulerPlaceholder;
                schedRunTextBox_RemoveFocus(this, e);
                schedulerTimer.Stop();
            }
        }

        //This is for refreshing the test case list based on the search param provided on tcSearchBox
        //Category: Action - Enter key pressed on text box
        private void tcSearchBox_Refresh(object sender, KeyEventArgs e)
        {
            List<Object> toRemove = new List<Object>();

            if (e.KeyCode == Keys.Enter)
            {
                string str = this.tcSearchBox.Text;
                string[] strAr = str.Split(',');
                Boolean removeItem;
                Boolean contains;

                //Display the original contents of the browsed folder
                folderPath_Refresh(this, e);

                //Add non-match items to toRemove list
                foreach (var item in this.testCaseList.Items)
                {
                    removeItem = true;
                    foreach (string search in strAr)
                    {
                        contains = item.ToString().IndexOf(search.Trim(), StringComparison.OrdinalIgnoreCase) >= 0;
                        if (!contains)
                        {
                            removeItem = removeItem && true;
                        }

                        else
                        {
                            removeItem = removeItem && false;
                        }
                    }

                    if (removeItem)
                    {
                        toRemove.Add(item);
                    }
                }

                //Remove all non-match items
                foreach (var item in toRemove)
                {
                    this.testCaseList.Items.Remove(item);
                }

                //Refresh the listbox
                this.testCaseList.Refresh();
            }
        }

        //Setting a value on the Schedule Run text box
        //Category: Action - Set textbox value, reject non numerical values.
        private void schedRunTextBox_TextChanged(object sender, KeyPressEventArgs e)
        {
            string str = schedRunTextBox.Text;
            schedRunTextBox.ForeColor = System.Drawing.Color.Maroon;

            //Only allow numbers and keyboard controls. Restrict the use of Crtl button.
            if (e.KeyChar == 22 || !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && (e.KeyChar != ':'))
            {
                e.Handled = true;
            }

            schedulerTimer.Stop();
        }

        //Set test execution trigger time upon clicking the scheduler icon.
        //Category: Action - Set test trigger time
        private void schedRunBtn_Clicked(object sender, EventArgs e)
        {
            string str = schedRunTextBox.Text;
            Boolean valid = schedRunTextBox_ValidateFormat(str);
            DateTime triggerTime;

            if (!valid)
            {
                MessageBoxEx.Show(this, "Please enter a valid time in HH:mm:ss [23:59:59] format.", "Time Format Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                schedRunTextBox.Text = schedulerDefaultValue;

                schedulerTimer.Stop();
            }
            else
            {
                triggerTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + schedRunTextBox.Text);

                //Add 1 day if the value for scheduler textbox is less than the current time
                //The test will be executed on the defined the the following day.
                if (DateTime.Compare(triggerTime, DateTime.Now) < 0)
                {
                    triggerTime = triggerTime.AddDays(1);
                }

                setupTimer(triggerTime);

                MessageBoxEx.Show(this, "Tests are scheduled to start automatically on " + triggerTime + ".", "Scheduler Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                schedRunTextBox.ForeColor = System.Drawing.Color.Green;
            }
        }

        //Determines the time left from the configured trigger time on the scheduler textbox and starts the application timer
        //Category: Action - Start timer
        private void setupTimer(DateTime triggerTime)
        {
            DateTime current = DateTime.Now;
            timeLeft = (int)((triggerTime - current).TotalSeconds);
            schedulerTimer.Start();
        }

        //Starts timer countdown starting from the determined time left. Runs the test when countdown timer reaches 0
        //Category: Action - Scheduler countdown
        private void schedulerTimer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft = timeLeft - 1;
            }
            else
            {
                schedulerTimer.Stop();
                timeLeft = 0;
                testStart_Click(this, e);
            }
        }

        //Validate Schedule Run input upon clicking the scheduler icon.
        //Category: Action - Validate input
        private Boolean schedRunTextBox_ValidateFormat(string time)
        {
            DateTime ignored;
            return DateTime.TryParseExact(time, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ignored);
        }
        //================================================================ACTIONS===============================================================

        //================================================================UTILITY===============================================================
        //Sets the text that was obtained from ThreadCallProcess by using a delegate to invoke the control on the main thread
        //Category: Utility
        private void SetStartButtonText(string text)
        {
            if (this.testStart.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetStartButtonText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.testStart.Text = text;

                if (text.Equals("    Stop Test"))
                {
                    this.testStart.Image = global::CoffeeBeanForm.Properties.Resources.icon_stop;
                    this.testStart.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
                    this.testStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    this.testStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                }
                else
                {
                    this.testStart.Image = null;
                    this.testStart.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
                    this.testStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                }
            }
        }

        //Removing default text for TC search box
        //Category: Utility - Remove placeholder
        private void tcSearchBox_GotFocus(object sender, EventArgs e)
        {
            string str = tcSearchBox.Text;
            tcSearchBox.ForeColor = System.Drawing.SystemColors.WindowText;

            if (str.Equals(searchTCPlaceholder))
            {
                tcSearchBox.Text = "";
            }

            else
            {
                tcSearchBox.Text = str;
            }
        }

        //Setting default text for TC search box
        //Category: Utility - Set placeholder
        private void tcSearchBox_RemoveFocus(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tcSearchBox.Text))
            {
                tcSearchBox.Text = searchTCPlaceholder;
                tcSearchBox.ForeColor = System.Drawing.SystemColors.GrayText;
            }
        }

        //Removing default text for Schedule Run textbox
        //Category: Utility - Remove placeholder
        private void schedRunTextBox_GotFocus(object sender, EventArgs e)
        {
            string str = schedRunTextBox.Text;
            schedRunTextBox.ForeColor = System.Drawing.Color.Maroon;

            if (str.Equals(schedulerPlaceholder))
            {
                schedRunTextBox.Text = "";
            }

            else
            {
                schedRunTextBox.Text = str;
            }
        }

        //Setting default text for Schedule Run textbox
        //Category: Utility - Set placeholder
        private void schedRunTextBox_RemoveFocus(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(schedRunTextBox.Text))
            {
                schedRunTextBox.Text = schedulerPlaceholder;
                schedRunTextBox.ForeColor = System.Drawing.SystemColors.GrayText;
            }
        }

        //Load the folder path after browsing
        //Category: Utility
        private void loadDefaultFolderPath(string getFolderPath)
        {
            try
            {
                this.folderPath.Text = getFolderPath;
                string getFilePath = getFolderPath + "\\";

                DirectoryInfo dinfo = new DirectoryInfo(getFilePath);
                FileInfo[] Files = dinfo.GetFiles("*.xls*");
                this.testCaseList.Items.Clear();
                foreach (FileInfo file in Files)
                {
                    this.testCaseList.Items.Add(file.Name);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("loadDefaultFolderPath Error: " + ex.ToString());
            }
            
        }

        //Put the app to be on the topmost when exeucted (Always on top)
        //Category: Utility
        private void checkAOTFlag()
        {
            if (aotFlag.ToUpper() == "TRUE")
            {
                this.TopMost = true;
            }
        }

        //Put the app to be on the topmost when exeucted (Always on top)
        //Category: Utility
        private void checkSchedulerFlag()
        {
            if (schedulerFlag.ToUpper() == "TRUE")
            {
                this.schedRunCheckBox.Checked = true;
            }
        }

        //This is to check the task status bool whether all tasks have been completed or not
        //Category: Utility
        private Boolean EvaluateTasksStatus(Boolean[] verifications)
        {
            Boolean testResult = true;
            foreach (Boolean verification in verifications)
            {
                testResult = testResult && verification;
            }
            return testResult;
        }

        //This is to update the number of concurrent test cases setting depending on driver type or number of rows
        //Category: Utility
        private void checkRows()
        {
            int maxValue = getMaxValue();
            if (TestExecutionDataGrid.Rows.Count <= maxValue)
            {
                if (TestExecutionDataGrid.Rows.Count == 0)
                {
                    conTestCase.Text = (TestExecutionDataGrid.Rows.Count + 1).ToString();
                }
                else { conTestCase.Text = TestExecutionDataGrid.Rows.Count.ToString(); }
            }
            else { conTestCase.Text = maxValue.ToString(); }
        }

        //This is to determine the number of maximum concurrent test cases executed
        //Values are derived from app.config
        //Category: Utility
        private int getMaxValue()
        {
            int maxValue;
            if (driverSelection.SelectedIndex == 0)
            {
                maxValue = maxJS;
            }
            else { maxValue = maxOthers; }
            return maxValue;
        }

        //This is to load the default driver type to be used by the tool based on settings on app.config
        //Category: Utility
        private void loadDefaultDriverType()
        {
            int driverIndex = 0;
            switch (defDriver)
            {
                case "1":
                    driverIndex = 0;
                    break;
                case "2":
                    driverIndex = 1;
                    break;
                case "3":
                    driverIndex = 2;
                    break;
                case "4":
                    driverIndex = 3;
                    break;
            }

            driverSelection.SelectedIndex = driverIndex;
            Config.UpdateConfig("driverType", defDriver);
        }

        //This is to load the default environment to be used by the tool based on settings on app.config
        //Category: Utility
        private void loadDefaultEnv()
        {
            int envIndex = 0;
            switch (defEnv)
            {
                case "SIT1":
                    envIndex = 0;
                    break;
                case "UAT1":
                    envIndex = 1;
                    break;
                case "SIT2":
                    envIndex = 2;
                    break;
                case "UAT2":
                    envIndex = 3;
                    break;
            }

            envSelection.SelectedIndex = envIndex;
            Config.UpdateConfig("environment", defEnv);
        }

        //This is to create a directory to store dated results so that results of a previous execution can be viewed
        //Category: Utility
        private void CreateFolderForCurrentExec()
        {
            if (!System.IO.Directory.Exists(datedResultsDirectory))
            {
                System.IO.Directory.CreateDirectory(datedResultsDirectory);
            }
            else
            {
                if (!System.IO.Directory.Exists(dailyDatedDirectory))
                {
                    System.IO.Directory.CreateDirectory(dailyDatedDirectory);
                }
            }
        }

        //This is to copy the results into the dated results folders
        //Category: Utility
        private string CopyCurrentExecution()
        {
            string getReturnValue = "";
            try
            {
                foreach (DataGridViewRow row in TestExecutionDataGrid.Rows)
                {
                    int directoryCount = 1;
                    string shortTestCaseName = row.Cells[1].Value.ToString().Remove(row.Cells[1].Value.ToString().LastIndexOf('.'));
                    string testScenDirectory = dailyDatedDirectory + shortTestCaseName + "\\";
                    string sourcePath = resultsDirectory + Utility.getEnvironment + "\\" + shortTestCaseName + "\\";
                    File.Delete(resultsDirectory + Utility.getEnvironment + "\\" + shortTestCaseName + "\\" + tsruntimeexec);
                    File.Delete(resultsDirectory + Utility.getEnvironment + "\\" + shortTestCaseName + "\\" + tsruntimeexecmax);

                    if (!Directory.Exists(testScenDirectory))
                    {
                        CreateTestCaseDirectory(testScenDirectory, sourcePath);
                        row.Cells[9].Value = directoryCount;
                    }
                    else
                    {
                        {
                            foreach (string s in Directory.GetDirectories(dailyDatedDirectory))
                            {
                                if (s.Contains(shortTestCaseName))
                                {
                                    directoryCount += 1;
                                }
                            }
                            CreateTestCaseDirectory(dailyDatedDirectory + shortTestCaseName + "_RUN_" + directoryCount + "\\", sourcePath);
                            getReturnValue = "_RUN_" + directoryCount;
                            row.Cells[9].Value = directoryCount;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("Test Exception: " + e.ToString());
            }
            return getReturnValue;
        }

        //This creates a folder for each test case
        //Category: Utility
        private void CreateTestCaseDirectory(string tsDirectory, string sourcePath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, tsDirectory));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, tsDirectory), true);
        }

        //This writes the overall results into a text file
        //Category: Utility
        public void WriteTestDataGridResults(string runValue)
        {
            int passed;
            int failed;
            float passPer;
            float getTotalResult;
            string tsName;
            string tsResult;
            string tsNameRaw;
            string tsExecTime;
            string tsActualRunFolder;
            string tsModule;
            string tsDirectoryCount = "";

            try
            {
                foreach (DataGridViewRow row in TestExecutionDataGrid.Rows)
                {
                    tsNameRaw = row.Cells[1].Value.ToString();
                    tsName = tsNameRaw.Remove(tsNameRaw.LastIndexOf('.'));
                    tsResult = row.Cells[3].Value.ToString();
                    tsExecTime = row.Cells[5].Value.ToString();
                    tsModule = row.Cells[7].Value.ToString();
                    tsDirectoryCount = row.Cells[9].Value.ToString();
                    if (Int32.Parse(tsDirectoryCount) == 1)
                    {
                        tsActualRunFolder = "";
                        runValue = "Initial Run";
                    }
                    else
                    {
                        tsActualRunFolder = "_RUN_" + tsDirectoryCount;
                        runValue = "_RUN_" + tsDirectoryCount;
                    }

                    passed = Report.GetScenarioSummary(Utility.getEnvironment, tsNameRaw)["Passed"];
                    failed = Report.GetScenarioSummary(Utility.getEnvironment, tsNameRaw)["Failed"];
                    getTotalResult = passed + failed;
                    passPer = 100 * (passed / getTotalResult);
                    Config.WriteFileAppend(dailyDatedScenarioDetailedResults,
                        tsModule + "," +
                        tsName + "," +
                        runValue.Replace("_RUN_", "Run ") + "," +
                        "<a target='_blank' href=" + "\"" + tsName + tsActualRunFolder + "/" + tsName + ".html\"</a>" + tsResult + "," +
                        getTotalResult.ToString() + "," +
                        passed + "," +
                        failed + "," +
                        (int)passPer + "%" + "," +
                        tsExecTime +
                        Environment.NewLine);

                    //Write 2nd File
                    Config.WriteFileAppend(dailyDatedScenarioSummaryResults,
                        tsModule + "^" + tsName + "Last" + "," +
                        getTotalResult.ToString() + "," +
                        passed + "," +
                        failed + "," +
                        Environment.NewLine);
                }
            }
            catch
            {
            }
        }

        //This is to add a data into the datagrid with logic handling for duplicates
        //Category: Utility
        private void addToDataGrid(string testCaseName)
        {
            Boolean toAdd = true;
            foreach (DataGridViewRow row in TestExecutionDataGrid.Rows)
            {
                if (testCaseName == row.Cells[1].Value.ToString())
                {
                    toAdd = allowDuplicates();
                    break;
                }
                else
                {
                    toAdd = true;
                }
            }
            if (toAdd)
            {
                TestExecutionDataGrid.Rows.Add(true, testCaseName, "", "", "", "", "Launch Report", "");
                pathList.Add(folderPath.Text + "\\");
            }
        }

        //Place the form on the lower right corner of the screen
        //Category: Utility
        private void placeLowerRight()
        {
            //Determine "rightmost" screen
            Screen rightmost = Screen.AllScreens[0];
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                    rightmost = screen;
            }

            this.Left = rightmost.WorkingArea.Right - this.Width;
            this.Top = rightmost.WorkingArea.Bottom - this.Height;
        }
        //================================================================UTILITY===============================================================

        //==============================================================VALIDATIONS=============================================================
        //Validation on changing of value concurrent test cases        
        /*Validation Rules:
         * 1. Concurrent test cases should not be greater than the set maximum value
         * 2. Concurrent test cases should not be equal to 0
         */
        private void conTestCase_TextChanged(object sender, EventArgs e)
        {
            int maxValue = getMaxValue();

            if (conTestCase.Text == "0" || conTestCase.Text == "")
            {
                string message = "Concurrent test cases executed should at least be 1";
                string caption = "Concurrent Test Case Validation";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBoxEx.Show(this, message, caption, buttons, icon);
                conTestCase.Text = "1";
            }

            if (Int32.Parse(conTestCase.Text) > maxValue)
            {
                string message = "Cannot exceed maximum value for concurrent runs \n\n" +
                                 "Phantom JS: " + maxJS.ToString() + ", Others: " + maxOthers.ToString();
                string caption = "Concurrent Test Case Validation";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBoxEx.Show(this, message, caption, buttons, icon);
                conTestCase.Text = "1";
            }
        }

        //Validation for clicking on start before adding test cases      
        /*Validation Rule:
         * 1. There should be at least 1 test case  added to the datagridview before clicking on start         
         */
        private Boolean checkIfItemsAdded()
        {
            if (TestExecutionDataGrid.Rows.Count == 0)
            {
                string message = "Please add test cases to execute before clicking on start";
                string caption = "No test case detected";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBoxEx.Show(this, message, caption, buttons, icon);
                return false;
            }
            else { return true; }
        }

        //Validation for clicking on start without selecting test cases     
        /*Validation Rule:
         * 1. There should be at least 1 test case selected on the datagridview before clicking on start         
         */
        private Boolean checkIfTestCasesSelected()
        {
            if (testCaseList.SelectedItems.Count == 0)
            {
                string message = "Please select at least 1 test case before clicking on add";
                string caption = "No test case selected";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBoxEx.Show(this, message, caption, buttons, icon);
                return false;
            }
            else { return true; }
        }

        //Validation to check if concurrent test case setting is greater than number of selected test cases   
        /*Validation Rule:
         * 1. Number of selected test cases should be greater than the concurrent test case setting
         */
        private Boolean checkIfSelectedTestCaseIsMore()
        {
            int getTotalSelections = 0;
            foreach (DataGridViewRow row in TestExecutionDataGrid.Rows)
            {
                if (row.Cells[getSelection.Name].Value.ToString() == "True")
                {
                    getTotalSelections = getTotalSelections + 1;
                }
            }
            if (Int32.Parse(conTestCase.Text) > getTotalSelections)
            {
                string message = "Concurrent test cases should not be greater than selected test cases";
                string caption = "Concurrent Test Case Validation";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBoxEx.Show(this, message, caption, buttons, icon);
                checkRows();
                return false;
            }
            else { return true; }
        }

        //Validation based on allowing duplicates is called
        /* Valudation Rule:
         * 1. If allowDupes in app.config is set to True then continue to add; else, error message will pop out indicating that this has to be set up
         * 2. allowing of dupes will be available in a future release
         */
        private Boolean allowDuplicates()
        {
            if (adFlag.ToUpper() == "TRUE")
            {
                return true;
            }
            else
            {
                MessageBoxEx.Show(this, "Duplicates are not allowed", "Duplicate Test Case Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Config.UpdateConfig("allowDupe", "False"); // This is to force to turn the option off.
                return false;
            }
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {

            if (this.pauseButton.Text == "| |")
            {
                try
                {
                    if (ExcelReaderGlobal.getMonitoringObj.ContainsKey("object_" + TestExecutionDataGrid.Rows[TestExecutionDataGrid.CurrentCell.RowIndex].Cells[1].Value.ToString()))
                    {
                        ExcelReaderGlobal.TaskPause(TestExecutionDataGrid.Rows[TestExecutionDataGrid.CurrentCell.RowIndex].Cells[1].Value.ToString());
                        this.pauseButton.Text = " > ";
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                }
            }
            else if (this.pauseButton.Text == " > ")
            {
                try
                {
                    if (ExcelReaderGlobal.getMonitoringObj.ContainsKey("object_" + TestExecutionDataGrid.Rows[TestExecutionDataGrid.CurrentCell.RowIndex].Cells[1].Value.ToString()))
                    {
                        ExcelReaderGlobal.TaskResume(TestExecutionDataGrid.Rows[TestExecutionDataGrid.CurrentCell.RowIndex].Cells[1].Value.ToString());
                        this.pauseButton.Text = "| |";
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                }
            }

            
        }

        private void addAllTestCase_Click(object sender, EventArgs e)
        {
            foreach (string testCaseName in testCaseList.Items)
            {
                addToDataGrid(testCaseName);
            }
            //Calls checkRows() utility to make sure the app follows the concurrent test case execution setting
            checkRows();
        }

        private void TestExecutionDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (TestExecutionDataGrid.CurrentCell.Selected)
            {
                if (ExcelReaderGlobal.getMonitoringObj.ContainsKey("object_" + TestExecutionDataGrid.Rows[TestExecutionDataGrid.CurrentCell.RowIndex].Cells[1].Value.ToString()))
                {
                    this.pauseButton.Enabled = true;

                    if (ExcelReaderGlobal.getRunningFlag["pauseflag_" + TestExecutionDataGrid.Rows[TestExecutionDataGrid.CurrentCell.RowIndex].Cells[1].Value.ToString()])
                    {
                        this.pauseButton.Text = " > "; 
                    }
                    else
                    {
                        this.pauseButton.Text = "| |"; 
                    }

                }
                else
                {
                    this.pauseButton.Enabled = false;
                }
            }
            else
            {
                this.pauseButton.Enabled = false;
            }
        }

        //==============================================================VALIDATIONS=============================================================

        //================================================================CONFIG================================================================

        //================================================================CONFIG================================================================
        // Code End -------------------
        //-----------------------------
    }
}
