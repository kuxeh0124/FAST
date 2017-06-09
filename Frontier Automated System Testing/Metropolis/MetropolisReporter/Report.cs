using APIReport;
using CoffeeBeanLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using log4net;
using log4net.Core;
using log4net.Appender;
using log4net.Config;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeBeanReporter
{
    public class Report
    {
        private static string tcresults = Utility.tcresults;
        private static string tcstepresults = Utility.tcstepresults;
        private static string tsruntimeexec = Utility.tsruntimeexec;
        private static string tsruntimeexecmax = Utility.tsruntimeexecmax;
        private static string getModuleData = Utility.getModuleData;
        private static string resultsDirectory = Utility.resultsDirectory;
        private static string datedResultsDirectory = Utility.datedResultsDirectory;
        private static string backUpResultsDirectory = Utility.backUpResultsDirectory;
        private static string screenshotFolderName = Utility.screenshotFolderName;

        //Adding the logger
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ILog Logger
        {
            get { return _logger; }
        }

        public static void GenerateReport(String environment, String fileName)
        {
            Logger.Info("Generating report");
            try
            {
                int passed = GetScenarioSummary(environment, fileName)["Passed"];
                int failed = GetScenarioSummary(environment, fileName)["Failed"];
                string str = fileName.Remove(fileName.LastIndexOf('.'));
                string htmlFormatter = @"<style>body {font: 11px Verdana,sans-serif;margin-left: 70px;margin-right: 70px;}table {margin-top: 5px;} p {margin-bottom: 0px;} .summary {width: 200px;border-style: none!important;font-size: 12px!important;} td,tr[style] {font-size: 11px!important;border-width: 1px!important;border-style: solid!important;border-color: rgb(0, 0, 0) !important;}</style>";
                string resultsSummary = @"<table class=""summary""><thead></thead><tr><td class=""summary""><b>Total Executed:</b></td><td class=""summary"">" + (passed + failed) + @"</td></tr><tr><td class=""summary""><b>Passed:</b></td><td class=""summary"">" + passed + @"</td></tr><tr><td class=""summary""><b>Failed:</b></td><td class=""summary"">" + failed + "</td></tr></table>";

                DocumentHTML documentHTML = new DocumentHTML();

                DataTable tcDataTable = GetTestResultData(environment, str);
                DataTable dataTable = GetStepResultData(environment, str);
                DataTable dtCarrier = dataTable.DefaultView.ToTable(true, "Test Case");

                ReportSectionText blankline = new ReportSectionText("<br/>");
                ReportSectionData reportSectionDataTestResult = new ReportSectionData(tcDataTable, false);
                reportSectionDataTestResult.Alignment = ContentAlignment.MiddleCenter;
                reportSectionDataTestResult.TotalWidth = 510;
                reportSectionDataTestResult.Position = Position.left;
                reportSectionDataTestResult.HighLightColor = Color.FromArgb(204, 230, 255);
                reportSectionDataTestResult.ColWidth = new int[3] { 100, 310, 100 };
                reportSectionDataTestResult.setAlternatingRowBgColorAlgo1(Color.White, Color.WhiteSmoke);

                //Build HTML
                documentHTML.addSectionText(SectionText(htmlFormatter, Color.Black));
                documentHTML.addSectionText(SectionText("Test Scenario: " + str + "<br/>", Color.DarkRed, FontStyle.Bold, ContentAlignment.MiddleCenter, 25.0f));
                //Results Summary
                documentHTML.addSectionText(SectionText("<p>" + environment + " Results Summary:</p>", Color.Black, FontStyle.Bold));
                documentHTML.addSectionText(SectionText(resultsSummary, Color.Black, FontStyle.Regular));
                documentHTML.addSectionText(blankline);
                documentHTML.addSectionData(reportSectionDataTestResult);
                documentHTML.addSectionText(blankline);
                documentHTML.addSectionText(blankline);
                //Detailed Results
                documentHTML.addSectionText(SectionText("<p>Detailed Results:</p>", Color.Black, FontStyle.Bold));
                documentHTML.addSectionText(blankline);

                //Loop for each test case
                for (int i = 0; i < dtCarrier.Rows.Count; i++)
                {
                    DataView dataView = new DataView(dataTable);
                    dataView.RowFilter = "[Test Case]='" + dtCarrier.Rows[i]["Test Case"].ToString() + "'";
                    ReportSectionData reportSectionDataTestStepsResult = new ReportSectionData(dataView.ToTable(), false);
                    reportSectionDataTestStepsResult.Alignment = ContentAlignment.MiddleCenter;
                    reportSectionDataTestStepsResult.Position = Position.center;
                    reportSectionDataTestStepsResult.TotalWidth = 1150;
                    reportSectionDataTestStepsResult.HighLightColor = Color.FromArgb(204, 230, 255);
                    reportSectionDataTestStepsResult.ColWidth = new int[6] { 100, 325, 100, 400, 125, 200 };
                    reportSectionDataTestStepsResult.setAlternatingRowBgColorAlgo1(Color.White, Color.WhiteSmoke);
                    documentHTML.addSectionText(SectionText(@"<a name=""" + dtCarrier.Rows[i]["Test Case"].ToString() + @""">" + dtCarrier.Rows[i]["Test Case"].ToString() + "</a>", Color.DarkRed, FontStyle.Bold, ContentAlignment.BottomLeft, 14.0f));
                    documentHTML.addSectionData(reportSectionDataTestStepsResult);
                    documentHTML.addSectionText(blankline);
                }

                System.IO.StreamWriter systemWriter = new System.IO.StreamWriter(resultsDirectory + environment + "\\" + str + "\\" + str + ".html");
                systemWriter.WriteLine(documentHTML.getResult(true, true));
                systemWriter.Close();
            }
            catch (Exception ex)
            {
                Logger.Error(String.Format("GenerateReport: {0}, {1}, {2}", environment, fileName, ex.ToString()));
            }
            
        }

        public static void GenerateReportOverall(String environment, String fileName)
        {
            try
            {
                DocumentHTML documentHTML = new DocumentHTML();
                ReportSectionText blankline = new ReportSectionText("<br/>");
                string htmlFormatter = @"<style>body {font: 11px Verdana,sans-serif;margin-left: 70px;margin-right: 70px;}table {margin-top: 5px;} p {margin-bottom: 0px;} .summary {width: 200px;border-style: none!important;font-size: 12px!important;} td,tr[style] {font-size: 11px!important;border-width: 1px!important;border-style: solid!important;border-color: rgb(0, 0, 0) !important;}</style>";

                //Title
                documentHTML.addSectionText(SectionText(htmlFormatter, Color.Black));
                documentHTML.addSectionText(SectionText(environment + " Overall Test Report (" + System.DateTime.Now.ToShortDateString() + ")<br/>", Color.DarkRed, FontStyle.Bold, ContentAlignment.MiddleCenter, 25.0f));

                //Results Summary
                documentHTML.addSectionText(SectionText("<p>Summary:</p>", Color.Black, FontStyle.Bold));

                DataTable moduleDataTable = GetModResultsSummary(environment, fileName);
                ReportSectionData reportSectionDataModResult = new ReportSectionData(moduleDataTable, false);
                reportSectionDataModResult.Alignment = ContentAlignment.MiddleCenter;
                reportSectionDataModResult.TotalWidth = 1150;
                reportSectionDataModResult.Position = Position.left;
                reportSectionDataModResult.HighLightColor = Color.FromArgb(204, 230, 255);
                reportSectionDataModResult.ColWidth = new int[5] { 100, 260, 260, 260, 260 };
                reportSectionDataModResult.setAlternatingRowBgColorAlgo1(Color.White, Color.WhiteSmoke);

                documentHTML.addSectionData(reportSectionDataModResult);
                documentHTML.addSectionText(blankline);

                //Latest Runs
                documentHTML.addSectionText(SectionText("<p>Latest Runs:</p>", Color.Black, FontStyle.Bold));

                DataTable latestRunsDataTable = GetLatestRuns(environment, fileName);
                DataView sortByMod = new DataView(latestRunsDataTable);
                sortByMod.Sort = "Module";
                ReportSectionData reportSectionDataLatestResult = new ReportSectionData(sortByMod.ToTable(), false);
                reportSectionDataLatestResult.Alignment = ContentAlignment.MiddleCenter;
                reportSectionDataLatestResult.Position = Position.center;
                reportSectionDataLatestResult.TotalWidth = 1150;
                reportSectionDataLatestResult.HighLightColor = Color.FromArgb(204, 230, 255);
                reportSectionDataLatestResult.ColWidth = new int[9] { 100, 290, 100, 100, 100, 100, 100, 100, 160 };
                reportSectionDataLatestResult.setAlternatingRowBgColorAlgo1(Color.White, Color.WhiteSmoke);

                documentHTML.addSectionData(reportSectionDataLatestResult);
                documentHTML.addSectionText(blankline);

                //Detailed Runs
                documentHTML.addSectionText(SectionText("<p>All Runs:</p>", Color.Black, FontStyle.Bold));

                DataTable dataTable = GetTSOverallResult(environment, fileName);
                DataTable dtCarrier = dataTable.DefaultView.ToTable(true, "Module");
                for (int i = 0; i < dtCarrier.Rows.Count; i++)
                {
                    DataView dataView = new DataView(dataTable);
                    dataView.RowFilter = "[Module]='" + dtCarrier.Rows[i]["Module"].ToString() + "'";
                    ReportSectionData reportSectionDataTestStepsResult = new ReportSectionData(dataView.ToTable(), false);
                    reportSectionDataTestStepsResult.Alignment = ContentAlignment.MiddleCenter;
                    reportSectionDataTestStepsResult.Position = Position.center;
                    reportSectionDataTestStepsResult.TotalWidth = 1150;
                    reportSectionDataTestStepsResult.HighLightColor = Color.FromArgb(204, 230, 255);
                    reportSectionDataTestStepsResult.ColWidth = new int[9] { 100, 290, 100, 100, 100, 100, 100, 100, 160 };
                    reportSectionDataTestStepsResult.setAlternatingRowBgColorAlgo1(Color.White, Color.WhiteSmoke);
                    documentHTML.addSectionText(SectionText(dtCarrier.Rows[i]["Module"].ToString(), Color.DarkRed, FontStyle.Bold, ContentAlignment.BottomLeft, 14.0f));
                    documentHTML.addSectionData(reportSectionDataTestStepsResult);
                    documentHTML.addSectionText(blankline);
                }

                System.IO.StreamWriter sw = new System.IO.StreamWriter(datedResultsDirectory + environment + "\\" + fileName + "\\" + fileName + ".html");
                sw.WriteLine(documentHTML.getResult(true, true));
                sw.Close();
            }
            catch (Exception ex)
            {
                Logger.Error("Error generating overall report! " + ex.ToString());
            }
        }

        public static void BackUp(String environment, String fileName)
        {
            string str = fileName.Remove(fileName.LastIndexOf('.'));
            DirectoryInfo dir = new DirectoryInfo(resultsDirectory + environment + "\\" + str);

            Logger.Info(String.Format("BackUp data from {0},{1},{2}",resultsDirectory,environment,str));

            if (Directory.Exists(backUpResultsDirectory + environment + "\\" + str))
            {
                try
                {
                    Directory.Delete(backUpResultsDirectory + environment + "\\" + str, true);
                    //Directory.Move(resultsDirectory + environment + "\\" + str, backUpResultsDirectory + environment + "\\" + str);
                    dir.MoveTo(backUpResultsDirectory + environment + "\\" + str);
                }
                catch 
                {
                    Logger.Error("BackUp: Unable to find the path");
                }

            }
            else
            {

                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(backUpResultsDirectory + environment + "\\" + str));
                    //Directory.Move(resultsDirectory + environment + "\\" + str, backUpResultsDirectory + environment + "\\" + str);
                    dir.MoveTo(backUpResultsDirectory + environment + "\\" + str);
                }
                catch
                {
                    Logger.Error("BackUp: Unable to find the path");
                }

            }
        }

        public static void WriteTestStepResult(String environment, String fileName, String testModule, String testcase, String stepnumber, String stepdescription, Boolean result)
        {
            string str = fileName.Remove(fileName.LastIndexOf('.'));
            string screenshotLink = screenshotFolderName + "\\" + testcase + "\\" + stepnumber + ".png";
            Config.WriteFileAppend(resultsDirectory + environment + "\\" + str + "\\" + tcstepresults, testModule + "`" + testcase + "`" + stepnumber + "`" + stepdescription + "`" + result.ToString().Replace("True", @"<a target='_blank' href=""" + screenshotLink + @""">" + "Passed</a>").Replace("False", @"<a target='_blank' href=""" + screenshotLink + @""">" + "Failed</a>") + "`" + DateTime.Now + Environment.NewLine);
        }

        public static void WriteTestCaseResult(String environment, String fileName, String testModule, String testcase, Boolean result)
        {
            string str = fileName.Remove(fileName.LastIndexOf('.'));
            Config.WriteFileAppend(resultsDirectory + environment + "\\" + str + "\\" + tcresults, testModule + "," + testcase + "," + result.ToString().Replace("True", @"<a href=""#" + testcase + @""">" + "Passed</a>").Replace("False", @"<a href=""#" + testcase + @""">" + "Failed</a>") + Environment.NewLine);
        }

        public static void WriteRunTimeSteps(String environment, String fileName, float currentStep)
        {
            string str = fileName.Remove(fileName.LastIndexOf('.'));
            Config.WriteFileReplace(resultsDirectory + environment + "\\" + str + "\\" + tsruntimeexec, currentStep.ToString());
        }

        public static void WriteRunTimeStepsMax(String environment, String fileName, float maxSteps)
        {
            string str = fileName.Remove(fileName.LastIndexOf('.'));
            Config.WriteFileReplace(resultsDirectory + environment + "\\" + str + "\\" + tsruntimeexecmax, maxSteps.ToString());
        }

        public static void WriteModuleValue(String environment, String fileName, string moduleVal)
        {
            string str = fileName.Remove(fileName.LastIndexOf('.'));
            Config.WriteFileReplace(resultsDirectory + environment + "\\" + str + "\\" + getModuleData, moduleVal);
        }

        private static ReportSectionText SectionText(String text, Color fontColor, FontStyle fontStyle = FontStyle.Regular, ContentAlignment alignment = ContentAlignment.BottomLeft, float fSize = 16.0f)
        {
            ReportSectionText sectionText = new ReportSectionText(text);
            sectionText.FontText = new Font("Verdana", fSize, fontStyle);
            sectionText.ForeColor = fontColor;
            sectionText.Alignment = alignment;
            return sectionText;
        }

        private static DataTable GetTestResultData(String environment, String fileName)
        {
            DataTable dataTable = new DataTable();

            try
            {
                System.IO.StreamReader streamReader = new System.IO.StreamReader(resultsDirectory + environment + "\\" + fileName + "\\" + tcresults);

                //Add data table columns
                dataTable.Columns.Add("No");
                dataTable.Columns.Add("Module");
                dataTable.Columns.Add("Test Case");
                dataTable.Columns.Add("Status");

                string line = "";
                string[] lineAr = line.Split(',');
                int j = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    j++;
                    lineAr = line.Split(',');
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = j;
                    for (int i = 0; i <= lineAr.Length - 1; i++)
                    {
                        dataRow[i + 1] = lineAr[i].Replace("Passed", @"<font color=""#006600"">Passed</font>").Replace("Failed", @"<font color=""#CC0000"">Failed</font>");
                    }
                    dataTable.Rows.Add(dataRow);
                }
                dataTable.Columns.RemoveAt(0);
                streamReader.Close();
            }
            catch (Exception ex)
            {
                Logger.Error("Error on GetTestResultData! " + ex.ToString());
            }
            return dataTable;
        }

        private static DataTable GetStepResultData(String environment, String fileName)
        {
            DataTable dataTable = new DataTable();

            try
            {
                System.IO.StreamReader streamReader = new System.IO.StreamReader(resultsDirectory + environment + "\\" + fileName + "\\" + tcstepresults);

                //Add data table columns
                dataTable.Columns.Add("No");
                dataTable.Columns.Add("Module");
                dataTable.Columns.Add("Test Case");
                dataTable.Columns.Add("Test Step");
                dataTable.Columns.Add("Step Description");
                dataTable.Columns.Add("Status");
                dataTable.Columns.Add("Time Stamp");

                string line = "";
                string[] lineAr = line.Split('`');
                int j = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    j++;
                    lineAr = line.Split('`');
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = j;
                    for (int i = 0; i <= lineAr.Length - 1; i++)
                    {
                        dataRow[i + 1] = lineAr[i].Replace("Passed", @"<font color=""#006600"">Passed</font>").Replace("Failed", @"<font color=""#CC0000"">Failed</font>");
                    }
                    dataTable.Rows.Add(dataRow);
                }
                dataTable.Columns.RemoveAt(0);
                streamReader.Close();
            }
            catch (Exception ex)
            {
                Logger.Error("Error on GetStepResultData! " + ex.ToString());
            }
            return dataTable;
        }

        private static DataTable GetTSOverallResult(String environment, String fileName)
        {
            DataTable dataTable = new DataTable();
            try
            {
                System.IO.StreamReader streamReader = new System.IO.StreamReader(datedResultsDirectory + environment + "\\" + fileName + "\\" + fileName + ".txt");

                //Add data table columns
                dataTable.Columns.Add("No");
                dataTable.Columns.Add("Module");
                dataTable.Columns.Add("Test Scenario");
                dataTable.Columns.Add("Run Session");
                dataTable.Columns.Add("Test Result");
                dataTable.Columns.Add("Number of Test Cases");
                dataTable.Columns.Add("Number of Passed Test Cases");
                dataTable.Columns.Add("Number of Failed Test Cases");
                dataTable.Columns.Add("Pass Rate");
                dataTable.Columns.Add("Date of Execution");

                string line = "";
                string[] lineAr = line.Split(',');
                int j = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    j++;
                    lineAr = line.Split(',');
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = j;
                    for (int i = 0; i <= lineAr.Length - 1; i++)
                    {
                        dataRow[i + 1] = lineAr[i].Replace("Passed", @"<font color=""#006600"">Passed</font>").Replace("Failed", @"<font color=""#CC0000"">Failed</font>");
                    }
                    dataTable.Rows.Add(dataRow);
                }
                dataTable.Columns.RemoveAt(0);
                streamReader.Close();
            }
            catch (Exception ex)
            {
                Logger.Error("Error on GetTSOverallResult! " + ex.ToString());
            }
            return dataTable;
        }

        private static DataTable GetModResultsSummary(String environment, String fileName)
        {
            DataTable dataTable = new DataTable();
            try
            {
                string modsummary = datedResultsDirectory + environment + "\\" + fileName + "\\" + fileName + ".txt";

                //Add data table columns
                dataTable.Columns.Add("Module");
                dataTable.Columns.Add("Total TCs");
                dataTable.Columns.Add("Passed");
                dataTable.Columns.Add("Failed");
                dataTable.Columns.Add("Pass Rate");

                //Loop to get all modules
                List<String> modlist = new List<String>();
                foreach (string line in File.ReadLines(modsummary))
                {
                    modlist.Add(line.Remove(line.IndexOf(',')));
                    modlist = modlist.Distinct().ToList();
                }

                //Loop to get all test scenarios
                List<String> tslist = new List<String>();
                foreach (string line in File.ReadLines(modsummary))
                {
                    string[] lineAr = line.Split(',');
                    tslist.Add(lineAr[1]);
                    tslist = tslist.Distinct().ToList();
                }

                //Populate dataTable with latest run results
                float passedCount = 0;
                float failedCount = 0;
                float totaltc = 0;
                float passedRate = 0;
                foreach (string mod in modlist)
                {
                    //Reset count
                    passedCount = 0;
                    failedCount = 0;

                    DataRow dataRow = dataTable.NewRow();
                    foreach (string ts in tslist)
                    {
                        foreach (string line in File.ReadLines(modsummary).Reverse())
                        {
                            string[] lineAr = line.Split(',');
                            if (line.Contains(mod + ",") && line.Contains(ts))
                            {
                                passedCount = passedCount + float.Parse(lineAr[5]);
                                failedCount = failedCount + float.Parse(lineAr[6]);
                                break;
                            }
                        }
                    }

                    totaltc = passedCount + failedCount;
                    passedRate = (passedCount / totaltc) * 100;
                    dataRow[0] = mod;
                    dataRow[1] = passedCount + failedCount;
                    dataRow[2] = passedCount;
                    dataRow[3] = failedCount;
                    dataRow[4] = (int)passedRate + "%";
                    dataTable.Rows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error on GetModResultsSummary! " + ex.ToString());
            }

            return dataTable;
        }

        private static DataTable GetLatestRuns(String environment, String fileName)
        {
            DataTable dataTable = new DataTable();
            string modsummary = datedResultsDirectory + environment + "\\" + fileName + "\\" + fileName + ".txt";

            try
            {
                //Add data table columns
                dataTable.Columns.Add("Module");
                dataTable.Columns.Add("Test Scenario");
                dataTable.Columns.Add("Run Session");
                dataTable.Columns.Add("Test Result");
                dataTable.Columns.Add("Number of Test Cases");
                dataTable.Columns.Add("Number of Passed Test Cases");
                dataTable.Columns.Add("Number of Failed Test Cases");
                dataTable.Columns.Add("Pass Rate");
                dataTable.Columns.Add("Date of Execution");

                //Loop to get all modules
                List<String> modlist = new List<String>();
                foreach (string line in File.ReadLines(modsummary))
                {
                    modlist.Add(line.Remove(line.IndexOf(',')));
                    modlist = modlist.Distinct().ToList();
                }

                //Loop to get all test scenarios
                List<String> tslist = new List<String>();
                foreach (string line in File.ReadLines(modsummary))
                {
                    string[] lineAr = line.Split(',');
                    tslist.Add(lineAr[1]);
                    tslist = tslist.Distinct().ToList();
                }

                //Populate dataTable with latest run results
                foreach (string mod in modlist)
                {
                    foreach (string ts in tslist)
                    {
                        foreach (string line in File.ReadLines(modsummary).Reverse())
                        {
                            DataRow dataRow = dataTable.NewRow();
                            string[] lineAr = line.Split(',');
                            lineAr = line.Split(',');

                            //Build row to add
                            for (int i = 0; i <= lineAr.Length - 1; i++)
                            {
                                dataRow[i] = lineAr[i].Replace("Passed", @"<font color=""#006600"">Passed</font>").Replace("Failed", @"<font color=""#CC0000"">Failed</font>");
                            }

                            if (line.Contains(mod + ",") && line.Contains(ts))
                            {
                                dataTable.Rows.Add(dataRow);
                                break;
                            }

                            else if (line.Contains(mod + ",") && line.Contains(ts) && line.Contains("Failed"))
                            {
                                dataTable.Rows.Add(dataRow);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error on GetLatestRuns! " + ex.ToString());
            }
            return dataTable;
        }

        public static Dictionary<String, int> GetScenarioSummary(String environment, String fileName)
        {
            int passedCount = 0;
            int failedCount = 0;
            string str = fileName.Remove(fileName.LastIndexOf('.'));
            Dictionary<String, int> summary = new Dictionary<String, int>();

            try
            {
                //Added safety measures to check if the tsruntimeexec data is available
                if (File.Exists(resultsDirectory + environment + "\\" + str + "\\" + tcresults))
                {
                    foreach (string line in File.ReadLines(resultsDirectory + environment + "\\" + str + "\\" + tcresults))
                    {
                        if (line.Contains("Passed"))
                        {
                            passedCount++;
                        }

                        else if (line.Contains("Failed"))
                        {
                            failedCount++;
                        }
                    }
                    summary.Add("Passed", passedCount);
                    summary.Add("Failed", failedCount);
                }

            }
            catch (Exception ex)
            {
                Logger.Error("Error on GetScenarioSummary! " + ex.ToString());
            }
            return summary;
        }

        public static Dictionary<String, float> GetRunTimeStepValue(String environment, String fileName)
        {
            string str = fileName.Remove(fileName.LastIndexOf('.'));
            float getStepCtr = 0;
            Dictionary<String, float> runProgress = new Dictionary<String, float>();
            try
            {
                //Added safety measures to check if the tsruntimeexec data is available
                if (File.Exists(resultsDirectory + environment + "\\" + str + "\\" + tsruntimeexec))
                {
                    foreach (string line in File.ReadLines(resultsDirectory + environment + "\\" + str + "\\" + tsruntimeexec))
                    {
                        if (line != null)
                        {
                            getStepCtr = Int32.Parse(line);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(String.Format("Error on GetRunTimeStepValue! param = {0}, {1}, {3}, {4}, {5} - with error trace {2}", environment, fileName, ex.ToString(), resultsDirectory, str, tsruntimeexec));
            }
            runProgress.Add("CurrentStep", getStepCtr);
            return runProgress;
        }

        public static Dictionary<String, float> GetRunTimeMaxStepValue(String environment, String fileName)
        {
            string str = fileName.Remove(fileName.LastIndexOf('.'));
            float getStepCtr = 0;
            Dictionary<String, float> runProgress = new Dictionary<String, float>();
            try
            {
                //Added safety measures to check if the tsruntimeexecmax data is available
                if(File.Exists(resultsDirectory + environment + "\\" + str + "\\" + tsruntimeexecmax)){
                    foreach (string line in File.ReadLines(resultsDirectory + environment + "\\" + str + "\\" + tsruntimeexecmax))
                    {
                        getStepCtr = Int32.Parse(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error on GetRunTimeMaxStepValue! " + ex.ToString());
            }
            runProgress.Add("MaxStep", getStepCtr);
            return runProgress;
        }

        public static Dictionary<String, string> GetModuleValueFromExcel(String environment, String fileName)
        {
            string str = fileName.Remove(fileName.LastIndexOf('.'));
            string getModValue = "";
            Dictionary<String, string> modValue = new Dictionary<String, string>();
            try
            {
                //Added safety measures to check if the getModuleData data is available
                if(File.Exists(resultsDirectory + environment + "\\" + str + "\\" + getModuleData)){
                    foreach (string line in File.ReadLines(resultsDirectory + environment + "\\" + str + "\\" + getModuleData))
                    {
                        getModValue = line;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(String.Format("Error on GetModuleValueFromExcel! param = {0}, {1}, {3}, {4}, {5} - with error trace {2}", environment, fileName, ex.ToString(), resultsDirectory, str, getModuleData));
            }
            modValue.Add("ModuleValue", getModValue);
            return modValue;
        }
    }
}
