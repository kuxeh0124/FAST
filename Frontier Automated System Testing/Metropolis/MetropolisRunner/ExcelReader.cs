using log4net;
using CoffeeBeanLibrary;
using CoffeeBeanReporter;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace CoffeeBeanRunner
{
    public class ExcelReader
    {
        string filePath;
        string sheetName;
        string fileName;
        string testscenario;
        string testcase;
        string stepnumber;
        string stepdescription;
        string action;
        string testdata;
        string locator;
        string locvalue;
        string attribute;
        string attrvalue;
        string capturedtext;
        string testModule;
        XSSFWorkbook xlsxworkbook;
        HSSFWorkbook xlsworkbook;
        public float maxRange;
        public float curRange;
        public string nullElemTS = "";
        public Boolean nullElemFlag = false;
        public Boolean exitIfNull = true;
        public Boolean stopRequestFlag;
        
        //Adding logger
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ILog Logger
        {
            get { return _logger; }
        }

        DriverClassLib driver = null;

        Boolean overallResult;
        List<String> testCasesList = new List<String>();
        List<Boolean> testCaseResultsList = new List<Boolean>();
        Dictionary<String, Boolean> tcDictionary = new Dictionary<String, Boolean>();
        //DatabaseConnector dbConn = new DatabaseConnector();

        public ExcelReader(string getFileName, string getFilePath, string getEnv)
        {
            fileName = getFileName;
            filePath = getFilePath;
            sheetName = getEnv;
        }

        public Boolean TestRunner()
        {
            try
            {
                driver = new DriverClassLib(Int32.Parse(Utility.getDriverType));

                InsDictRec("CurRow_" + fileName, 0);
                InsDictRec("MaxRow_" + fileName, 0);
                InsDictRec("ModuleVal_" + fileName, null);
                InsDictRec("nullElemTS_" + fileName, null);
                InsDictRec("nullElemFlag_" + fileName, false);
                InsDictRec("stopRequestFlag" + fileName, false);
                InsDictRec("object_" + fileName, new Object());
                InsDictRecDriver("driver" + fileName, driver);
                InsDictRec("pauseflag_" + fileName, false);

                int startingRow = 1;
                int worksheetRange = GetTestCases(fileName);

                InsDictRec("MaxRow_" + fileName, worksheetRange);

                maxRange = (float)worksheetRange;
                Logger.Info(maxRange);

                //Backup previous report
                Report.BackUp(sheetName, fileName);

                //For showing execution progress in FAST application
                //Report.WriteRunTimeStepsMax(sheetName, fileName, worksheetRange); //Removed as obsolete

                #region Execute test cases
                //Execute each test case
                foreach (string tc in testCasesList)
                {
                    List<Boolean> tempResults = new List<Boolean>();

                    for (int row = startingRow; row < worksheetRange; row++)
                    {

                        #region for loop

                        //Locking mechanism
                        lock (ExcelReaderGlobal.getMonitoringObj["object_" + fileName]) { }

                        InsDictRec("CurRow_" + fileName, row);

                        //Get row data 
                        GetRowData(fileName, row);

                        //Check if the step is intended to be skipped
                        if(action.Equals("skip"))
                        {
                            continue;
                        }

                        //Set element to be interacted upon
                        IWebElement elem = driver.LocateByType(locator, locvalue, attribute, attrvalue);

                        #region Stop flag check
                        //Stop test if flag is true
                        if (ExcelReaderGlobal.getRunningFlag["stopRequestFlag"])
                        {
                            Boolean result = GetTestCaseResult(tempResults);
                            DataFlush();
                            return result;
                        }
                        #endregion

                        #region Execute test steps
                        //Execute test case steps
                        if (tc.Equals(testcase))
                        {
                            Boolean result = RunSteps(elem, row);
                            tempResults.Add(result);

                            //Log test steps
                            if (!result && elem == null && exitIfNull)
                            {
                                //Do the following if elem is null
                                string err = "<br>Unable to locate element for this step [" + locator + "=" + locvalue + "]";
                                Report.WriteTestStepResult(sheetName, fileName, testModule, tc, stepnumber, stepdescription + err, result);

                                //try
                                //{
                                //    dbConn.testResultsInsert(fileName, testModule, tc, stepnumber, stepdescription + err, result == true ? "Pass" : "Fail");
                                //}
                                //catch (Exception e)
                                //{
                                //    Console.WriteLine(e.ToString());
                                //}

                                //nullElemFlag = true; //removed
                                InsDictRec("nullElemFlag_" + fileName, true);
                                InsDictRec("nullElemTS_" + fileName, fileName);
                                //nullElemTS = fileName; // removed as when calling a thread, should not access via static
                                result = GetTestCaseResult(tempResults);
                                testCaseResultsList.Add(result);
                                tcDictionary.Add(tc, result);
                                Report.WriteTestCaseResult(sheetName, fileName, testModule, tc, result);
                                //Report.WriteModuleValue(sheetName, fileName, testModule);
                                InsDictRec("ModuleVal_" + fileName, testModule);

                                try
                                {
                                    Report.GenerateReport(sheetName, fileName);
                                }
                                catch(Exception e)
                                {
                                    Logger.Error("Unable to generate report!" + e.ToString());
                                }
                                

                                DataFlush();
                                driver.Shutdown();
                                return result;
                            }
                            else
                            {
                                exitIfNull = true;
                                Report.WriteTestStepResult(sheetName, fileName, testModule, tc, stepnumber, stepdescription, result);
                                //try
                                //{
                                //    dbConn.testResultsInsert(fileName, testModule, tc, stepnumber, stepdescription, result == true ? "Pass" : "Fail");
                                //}
                                //catch (Exception e)
                                //{
                                //    Console.WriteLine(e.ToString());
                                //}
                            }

                            //For showing execution progress in FAST application
                            //Report.WriteRunTimeSteps(sheetName, fileName, row); //Obsolete
                        }
                        #endregion

                        #region Move to next TC within the file
                        //Log test case result and move to next test case
                        if (!tc.Equals(testcase) || row.Equals(worksheetRange - 1))
                        {
                            Boolean result = GetTestCaseResult(tempResults);
                            testCaseResultsList.Add(result);
                            tcDictionary.Add(tc, result);

                            //Log test case result
                            Report.WriteTestCaseResult(sheetName, fileName, testModule, tc, result);

                            //Set starting row for next test case
                            startingRow = row;
                            break;
                        }
                        #endregion

                        #endregion for loop
                    }
                    //Clear data
                    //Report.WriteModuleValue(sheetName, fileName, testModule);
                    InsDictRec("ModuleVal_" + fileName, testModule);

                    DataFlush();
                }
                #endregion

                //Generate Report
                try 
                {
                    Report.GenerateReport(sheetName, fileName);
                }
                catch (Exception ex) 
                {
                    Logger.Error(String.Format("TestRunner Error {0},{1},{2}", sheetName, fileName, ex.ToString()));
                }
                

                //Return overall test scenario result
                overallResult = GetTestCaseResult(testCaseResultsList);

                //Close WebDriver instance
                driver.Shutdown();
                return overallResult;
            }
            catch (Exception e)
            {
                driver.Shutdown();
                Logger.Error("TestRunner Error: " + e.ToString());
                return false;
            }
        }

        #region define the dictionary
        //Inserting dictionary for int
        private void InsDictRec(String key, int value)
        {
            try
            {
                ExcelReaderGlobal.getRowsCount.Add(key, value);
            }
            catch
            {
                ExcelReaderGlobal.getRowsCount[key] = value;
            }
        }

        //Inserting dictionary for string
        private void InsDictRec(String key, String value)
        {
            try
            {
                ExcelReaderGlobal.getModuleData.Add(key, value);
            }
            catch
            {
                ExcelReaderGlobal.getModuleData[key] = value;
            }
        }

        //Inserting dictionary for boolean
        private void InsDictRec(String key, Boolean value)
        {
            try
            {
                ExcelReaderGlobal.getRunningFlag.Add(key, value);
            }
            catch
            {
                ExcelReaderGlobal.getRunningFlag[key] = value;
            }
        }

        //Inserting dictionary for object
        private void InsDictRec(String key, object value)
        {
            try
            {
                ExcelReaderGlobal.getMonitoringObj.Add(key, value);
            }
            catch
            {
                ExcelReaderGlobal.getMonitoringObj[key] = value;
            }
        }

        //Inserting dictionary for driver class
        private void InsDictRecDriver(String key, DriverClassLib value)
        {
            try
            {
                ExcelReaderGlobal.getRunningDriver.Add(key, value);
            }
            catch
            {
                ExcelReaderGlobal.getRunningDriver[key] = value;
            }
        }
        #endregion


        //UTILS================================================================================================

        private XSSFWorkbook OpenXLSXFile(string fileName)
        {
        retry:
            try
            {
                string newFileName = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), filePath + fileName));
                FileStream file = new FileStream(newFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XSSFWorkbook workbook = new XSSFWorkbook(file);
                file.Close();

                //WORKAROUND: Replace values for unsupported formulas @"Utils" worksheet with the exact formula stored as string
                TempUtils(workbook.GetSheet("Utils"));

                workbook.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll();
                return workbook;
            }
            catch (IOException)
            {
                MessageBox.Show(new Form() { TopMost = true }, "Please close the test case file: " + fileName + " and click on OK.", "File Access Conflict", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                goto retry;
            }
        }

        private HSSFWorkbook OpenXLSFile(string fileName)
        {
        retry:
            try
            {
                string newFileName = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), filePath + fileName));
                FileStream file = new FileStream(newFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                HSSFWorkbook workbook = new HSSFWorkbook(file);
                file.Close();

                //WORKAROUND: Replace values for unsupported formulas @"Utils" worksheet with the exact formula stored as string
                TempUtils(workbook.GetSheet("Utils"));

                workbook.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll();
                return workbook;
            }
            catch (IOException)
            {
                MessageBox.Show(new Form() { TopMost = true }, "Please close the test case file: " + fileName + " and click on OK.", "File Access Conflict", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                goto retry;
            }
        }

        private string GetStringValue(ICell cell)
        {
            string str = null;

            try
            {
                switch (cell.CellType)
                {
                    case CellType.Formula:
                        CellType typ = cell.CachedFormulaResultType;
                        switch (typ)
                        {
                            case CellType.Numeric:
                                if (DateUtil.IsCellDateFormatted(cell))
                                {
                                    str = cell.DateCellValue.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    str = cell.NumericCellValue.ToString();
                                }
                                break;
                            default:
                                str = cell.RichStringCellValue.String;
                                break;
                        }
                        break;
                    case CellType.Numeric:
                        str = cell.NumericCellValue.ToString();
                        break;
                    case CellType.Blank:
                        str = "";
                        break;
                    default:
                        str = cell.RichStringCellValue.String;
                        break;
                }
            }
            catch (InvalidOperationException)
            {
                str = cell.ToString();
            }
            catch (NullReferenceException)
            {
                str = "";
            }

            return str;
        }

        private Boolean WriteCellData(string fileName, string value, int row)
        {
            int colRange = 0;
            int dataColumn = 0;
            string headers = null;
            ISheet sheet = null;
            IRow hrow = null;
            Boolean foo = false;

            string newFileName = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), filePath + fileName));

            if (fileName.EndsWith(".xlsx"))
            {
                sheet = xlsxworkbook.GetSheet(sheetName);
                xlsxworkbook.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll();
            }
            else if (fileName.EndsWith(".xls"))
            {
                sheet = xlsworkbook.GetSheet(sheetName);
                xlsworkbook.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll();
            }

            //Get the number of headers to read range
            colRange = sheet.GetRow(0).PhysicalNumberOfCells;

            //Header row
            hrow = sheet.GetRow(0);

            //Write cell data
            for (int col = 0; col < colRange; col++)
            {
                headers = GetStringValue(hrow.GetCell(col));

                if (headers.Contains("CapturedText"))
                {
                    dataColumn = hrow.GetCell(col).ColumnIndex;
                    sheet.GetRow(row).CreateCell(dataColumn);
                    sheet.GetRow(row).GetCell(dataColumn).SetCellValue(value);
                    sheet.ForceFormulaRecalculation = true;

                retry:
                    try
                    {
                        using (FileStream file = new FileStream(newFileName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                        {
                            if (fileName.EndsWith(".xlsx"))
                            {
                                xlsxworkbook.Write(file);
                                file.Close();
                            }
                            else if (fileName.EndsWith(".xls"))
                            {
                                xlsworkbook.Write(file);
                                file.Close();
                            }
                        }

                    }
                    catch (IOException)
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "Please close the test case file: " + fileName + " and click on OK.", "File Access Conflict", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        goto retry;
                    }
                    foo = true;
                }
            }
            return foo;
        }

        private Boolean IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        private Boolean GetTestCaseResult(List<Boolean> stepResultsList)
        {
            Boolean testResult = true;
            foreach (Boolean stepResult in stepResultsList)
            {
                testResult = testResult && stepResult;
            }
            return testResult;
        }

        private Boolean RunSteps(IWebElement elem, int row = 0)
        {
            Boolean stepResult = true;

            if (action.Equals("click") || action.Equals("clickByJS"))
            {
                driver.TakeElemSnapshot(elem, sheetName, fileName, testcase, stepnumber);
            }

            switch (action)
            {
                case "open":
                    stepResult = driver.Open(testdata);
                    break;
                case "click":
                    stepResult = driver.ClickObject(elem);
                    break;
                case "clickByJS":
                    stepResult = driver.ClickByJS(elem);
                    break;
                case "select":
                    stepResult = driver.SelectOptions(elem, testdata);
                    break;
                case "type":
                    stepResult = driver.SendKeys(elem, testdata);
                    break;
                case "typeByJS":
                    stepResult = driver.SendKeysByJS(elem, testdata);
                    break;
                case "delay":
                    stepResult = driver.DelayExplicit(testdata);
                    break;
                case "delayUntilFieldHasValue":
                    stepResult = driver.DelayUntilFieldIsPopulated(elem);
                    break;
                case "acceptAlert":
                    stepResult = driver.AcceptAlert();
                    exitIfNull = false;
                    break;
                case "dismissAlert":
                    stepResult = driver.DismissAlert();
                    exitIfNull = false;
                    break;
                case "getdata":
                    stepResult = WriteCellData(fileName, driver.GetData(elem), row);
                    break;
                case "getElementAttribute":
                    stepResult = WriteCellData(fileName, driver.GetElementAttribute(elem, testdata), row);
                    break;
                case "customUploadInternetMandatoryDocs":
                    stepResult = driver.CustomActionUploadInternetMandatoryDoc(testdata);
                    break;
                case "customUploadInternetNonMandatoryDocs":
                    stepResult = driver.CustomActionUploadInternetNonMandatoryDoc(testdata);
                    break;
                case "customUploadAVAInternetMandatoryDocs":
                    stepResult = driver.CustomActionAVAUploadInternetMandatoryDoc(testdata);
                    break;
                case "customUploadAVAInternetNonMandatoryDocs":
                    stepResult = driver.CustomActionAVAUploadInternetNonMandatoryDoc(testdata);
                    break;
                case "customInternetCompareTableDataDates":
                    stepResult = driver.CustomActionInternetCompareTableDataDates(testdata);
                    break;
                case "customInternetVerifyCorrespondenceAppDetails":
                    stepResult = driver.CustomInternetVerifyCorrespondenceAppDetails(elem);
                    exitIfNull = false;
                    break;
                case "customInternetVerifyCorrespondenceCorrDetails":
                    stepResult = driver.CustomInternetVerifyCorrespondenceCorrDetails(elem);
                    exitIfNull = false;
                    break;
                case "customIntranetSearchAppNum":
                    stepResult = driver.CustomActionIntranetSearchAppNum(testdata);
                    exitIfNull = false;
                    break;
                case "customIntranetBPMChangeValues":
                    stepResult = driver.CustomActionIntranetBPMChangeValues(testdata);
                    exitIfNull = false;
                    break;
                case "skip":
                    stepResult = driver.Skip();
                    exitIfNull = false;
                    break;
                case "switchToNewFrame":
                    stepResult = driver.SwitchToNewFrame(elem);
                    exitIfNull = false;
                    break;
                case "switchToNewWindow":
                    stepResult = driver.SwitchToNewWindow(testdata);
                    exitIfNull = false;
                    break;
                case "switchToParentWindow":
                    stepResult = driver.SwitchToParentWindow();
                    exitIfNull = false;
                    break;
                case "verifyElementPresent":
                    stepResult = driver.VerifyElementPresent(elem);
                    exitIfNull = false;
                    break;
                case "verifyElementIsSelected":
                    stepResult = driver.VerifyElementIsSelected(elem);
                    exitIfNull = false;
                    break;
                case "verifyElementIsNotSelected":
                    stepResult = driver.VerifyElementIsNotSelected(elem);
                    exitIfNull = false;
                    break;
                case "verifyElementNotPresent":
                    stepResult = driver.VerifyElementNotPresent(elem);
                    exitIfNull = false;
                    break;
                case "verifyFieldValue":
                    stepResult = driver.VerifyFieldValue(elem, testdata);
                    exitIfNull = false;
                    break;
                case "verifyElementContainsText":
                    stepResult = driver.VerifyElementContainsText(elem, testdata);
                    exitIfNull = false;
                    break;
                case "verifyElementCount":
                    stepResult = driver.VerifyElementCount(testdata, locator, locvalue, attribute, attrvalue);
                    exitIfNull = false;
                    break;
                case "verifyMinElementCount":
                    stepResult = driver.VerifyMinElementCount(testdata, locator, locvalue, attribute, attrvalue);
                    exitIfNull = false;
                    break;
                case "verifyMaxElementCount":
                    stepResult = driver.VerifyMaxElementCount(testdata, locator, locvalue, attribute, attrvalue);
                    exitIfNull = false;
                    break;
                case "confirmJavescriptWindow":
                    stepResult = driver.ConfirmJavescriptWindow(testdata);
                    exitIfNull = false;
                    break;
                default:
                    stepResult = false;
                    break;
            }

            if (!action.Equals("click") && !action.Equals("clickByJS"))
            {
                driver.TakeElemSnapshot(elem, sheetName, fileName, testcase, stepnumber);
            }

            return stepResult;
        }

        private int GetTestCases(string fileName)
        {
            int colRange = 0;
            int rowRange = 0;
            int dataColumn = 0;
            string headers = null;
            ISheet sheet = null;
            IRow hrow = null;

            if (fileName.EndsWith(".xlsx"))
            {
                xlsxworkbook = OpenXLSXFile(fileName);
                sheet = xlsxworkbook.GetSheet(sheetName);
            }
            else if (fileName.EndsWith(".xls"))
            {
                xlsworkbook = OpenXLSFile(fileName);
                sheet = xlsworkbook.GetSheet(sheetName);
            }

            //Get the number of rows to read range from
            rowRange = sheet.LastRowNum + 1;

            //Get the number of headers to read range from
            colRange = sheet.GetRow(0).PhysicalNumberOfCells;

            //Header row
            hrow = sheet.GetRow(0);

            //Look for the column having the label "Test Case"
            for (int col = 0; col < colRange; col++)
            {
                headers = GetStringValue(hrow.GetCell(col));

                if (headers.Contains("Test Case"))
                {
                    dataColumn = hrow.GetCell(col).ColumnIndex;
                    col = colRange;

                    //Read test cases column
                    for (int row = 1; row < rowRange; row++)
                    {
                        try
                        {
                            testcase = GetStringValue(sheet.GetRow(row).GetCell(dataColumn));

                            if (!testcase.Equals(""))
                            {
                                testCasesList.Add(testcase);
                                testCasesList = testCasesList.Distinct().ToList();
                            }
                            else
                            {
                                rowRange = row;
                                Logger.Info(fileName + " last valid row changed to row: " + row);
                            }
                        }
                        catch (NullReferenceException)
                        {
                            rowRange = row;
                            Logger.Error(fileName + " last valid row changed to row: " + row);
                        }
                    }
                }
            }
            DataFlush();
            Logger.Debug("Sheet " + sheet.SheetName + ", Row " + rowRange);
            return rowRange;
        }

        private void GetRowData(string fileName, int row) 
        {
            int colRange = 0;
            int dataColumn = 0;
            string headers = null;
            ISheet sheet = null;
            IRow hrow = null;

            if (fileName.EndsWith(".xlsx"))
            {
                sheet = xlsxworkbook.GetSheet(sheetName);
                xlsxworkbook.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll();
            }
            else if (fileName.EndsWith(".xls"))
            {
                sheet = xlsworkbook.GetSheet(sheetName);
                xlsworkbook.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll();
            }

            //Get the number of headers to read range
            colRange = sheet.GetRow(0).PhysicalNumberOfCells;

            //Header row
            hrow = sheet.GetRow(0);

            //Read row data
            for (int col = 0; col < colRange; col++)
            {

                headers = GetStringValue(hrow.GetCell(col));
                dataColumn = hrow.GetCell(col).ColumnIndex;

                if (headers.Contains("Test Scenario"))
                {
                    try
                    {
                        testscenario = GetStringValue(sheet.GetRow(row).GetCell(dataColumn));
                    }
                    catch(Exception ex)
                    {
                        Logger.Error(String.Format("Null for the {0} -> {1}", headers,ex.ToString()));
                        testscenario = "";
                    }
                    
                }

                else if (headers.Contains("Test Case"))
                {
                    try
                    {
                        testcase = GetStringValue(sheet.GetRow(row).GetCell(dataColumn));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(String.Format("Null for the {0} -> {1}", headers, ex.ToString()));
                        testcase = "";
                    }
                    
                }

                else if (headers.Contains("Module"))
                {
                    try
                    {
                        testModule = GetStringValue(sheet.GetRow(row).GetCell(dataColumn));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(String.Format("Null for the {0} -> {1}", headers, ex.ToString()));
                        testModule = "";
                    }
                    
                }

                else if (headers.Contains("Step Number"))
                {
                    try
                    {
                        stepnumber = GetStringValue(sheet.GetRow(row).GetCell(dataColumn));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(String.Format("Null for the {0} -> {1}", headers, ex.ToString()));
                        stepnumber = "";
                    }
                    
                }

                else if (headers.Contains("Step Description"))
                {
                    try
                    {
                        stepdescription = GetStringValue(sheet.GetRow(row).GetCell(dataColumn));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(String.Format("Null for the {0} -> {1}", headers, ex.ToString()));
                        stepdescription = "";
                    }
                    
                }

                else if (headers.Contains("Action"))
                {
                    try
                    {
                        action = GetStringValue(sheet.GetRow(row).GetCell(dataColumn));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(String.Format("Null for the {0} -> {1}", headers, ex.ToString()));
                        action = "";
                    }
                    
                }

                else if (headers.Contains("Test Data"))
                {
                    try
                    {
                        testdata = GetStringValue(sheet.GetRow(row).GetCell(dataColumn));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(String.Format("Null for the {0} -> {1}", headers, ex.ToString()));
                        testdata = "";
                    }
                    
                }

                else if (headers.Contains("Locator"))
                {
                    try
                    {
                        locator = GetStringValue(sheet.GetRow(row).GetCell(dataColumn));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(String.Format("Null for the {0} -> {1}", headers, ex.ToString()));
                        locator = "";
                    }
                    
                }

                else if (headers.Contains("LocValue"))
                {
                    try
                    {
                        locvalue = GetStringValue(sheet.GetRow(row).GetCell(dataColumn));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(String.Format("Null for the {0} -> {1}", headers, ex.ToString()));
                        locvalue = "";
                    }
                    
                }

                else if (headers.Contains("Attribute"))
                {
                    try
                    {
                        attribute = GetStringValue(sheet.GetRow(row).GetCell(dataColumn));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(String.Format("Null for the {0} -> {1}", headers, ex.ToString()));
                        attribute = "";
                    }
                    
                }

                else if (headers.Contains("AttribValue"))
                {
                    try
                    {
                        attrvalue = GetStringValue(sheet.GetRow(row).GetCell(dataColumn));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(String.Format("Null for the {0} -> {1}", headers, ex.ToString()));
                        attrvalue = "";
                    }
                    
                }

                else if (headers.Contains("CapturedText"))
                {
                    try
                    {
                        capturedtext = GetStringValue(sheet.GetRow(row).GetCell(dataColumn));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(String.Format("Null for the {0} -> {1}", headers, ex.ToString()));
                        capturedtext = "";
                    }
                    
                }
            }
        }

        private void TempUtils(ISheet sheet)
        {
            //Workaround for SystemNotImplemented Exception when invoking workbook.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll();
            foreach (IRow row in sheet)
            {
                foreach (ICell cell in row)
                {
                    string temp = cell.ToString();
                    if (cell.CellType == CellType.Formula && temp.Contains("CONCAT") && temp.Contains("FIND("))
                    {
                        cell.SetCellType(CellType.String);
                        cell.SetCellValue(temp);
                    }
                }
            }
        }

        private void DataFlush()
        {
            testscenario = null;
            testcase = null;
            stepnumber = null;
            stepdescription = null;
            action = null;
            testdata = null;
            locator = null;
            locvalue = null;
            attribute = null;
            attrvalue = null;
            capturedtext = null;
            testModule = null;
        }
    }
}
