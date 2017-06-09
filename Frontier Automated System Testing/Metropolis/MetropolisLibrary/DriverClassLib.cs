using log4net;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Automation;

namespace CoffeeBeanLibrary
{
    public class DriverClassLib
    {
        //Adding the logger
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ILog Logger
        {
            get { return _logger; }
        }

        IWebDriver Driver;
        Actions Builder;
        String parentHandle;
        int globalTimeout = 30;
        public int drivertype;
        private string screenshotFolderName = Utility.screenshotFolderName;
        private string resultsDirectory = Utility.resultsDirectory;
        private string chromeDownloadPath = Utility.chromeDownloadPath;
        private string chromeDriverPath = Utility.chromeDriverPath;
        private string phantomDriverPath = Utility.phantomDriverPath;
        private string ieDriverPath = Utility.ieDriverPath;

        public DriverClassLib(int drivertype)
        {
            //this.drivertype = drivertype;

            switch (drivertype)
            {
                case 1:
                    //Headless PhantomJS Driver
                    Driver = new PhantomJSDriver(phantomDriverPath);
                    Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, globalTimeout));
                    Logger.Info("PhantomJS Driver Path:" + phantomDriverPath);
                    break;

                case 2:

                    Logger.Info("Chrome Driver Path:" + chromeDriverPath);

                    //Chrome
                    OpenQA.Selenium.Remote.DesiredCapabilities capabilities = OpenQA.Selenium.Remote.DesiredCapabilities.Chrome();
                    // Add the WebDriver proxy capability.
                    Proxy proxy = new Proxy();
                    proxy.NoProxy = null;
                    capabilities.SetCapability("proxy", proxy);
                    //capabilities.IsJavaScriptEnabled = true;

                    var optionsChrome = new ChromeOptions();
                    optionsChrome.AddUserProfilePreference("download.default_directory", chromeDownloadPath);
                    optionsChrome.AddUserProfilePreference("download.prompt_for_download", false);
                    optionsChrome.AddLocalStatePreference("profile.default_content_settings.popups", 0);
                    optionsChrome.AddArguments("--disable-extensions");
                    capabilities.SetCapability(ChromeOptions.Capability, optionsChrome);

                    ChromeDriverService driverService = ChromeDriverService.CreateDefaultService(chromeDriverPath);
                    //driverService.HideCommandPromptWindow = true;
                    driverService.SuppressInitialDiagnosticInformation = true;


                    //Driver = new ChromeDriver(chromeDriverPath, optionsChrome, TimeSpan.FromSeconds(globalTimeout));
                    Driver = new ChromeDriver(driverService, optionsChrome);
                    Driver.Manage().Window.Maximize();

                    break;

                case 3:
                    //Firefox
                    Driver = new FirefoxDriver();
                    Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, globalTimeout));
                    Logger.Info("Setting the firefox driver type");
                    break;
                case 4:
                    //IE
                    var options = new InternetExplorerOptions();
                    Driver = new InternetExplorerDriver(ieDriverPath, options, TimeSpan.FromSeconds(globalTimeout));
                    Driver.Manage().Window.Maximize();
                    Logger.Info("Setting the iexplore driver type");
                    break;
                default:
                    break;
            }
            Builder = new Actions(Driver);
        }

        //ACTIONS==========================================================================================
        public String GetData(IWebElement element)
        {
            String foo = "";
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));

            try
            {
                foo = WaitUntil(element, e => element.Displayed).Text;
            }
            catch (Exception e)
            {
                Logger.Error("GetData: " + e.ToString());
            }
            return foo;
        }

        public String GetElementAttribute(IWebElement element, String attrib)
        {
            String foo = "";
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));

            try
            {
                foo = WaitUntil(element, e => element.Displayed).GetAttribute(attrib);
            }
            catch (Exception e)
            {
                Logger.Error("GetElementAttribute: " + e.ToString());
            }
            return foo;
        }

        public Boolean SwitchToNewFrame(IWebElement element)
        {

            Boolean foo = false;
            var iframe = element;
            parentHandle = Driver.CurrentWindowHandle;

            try
            {
                Driver.SwitchTo().Frame(iframe);
                foo = true;
            }
            catch (Exception e)
            {
                Logger.Error("SwitchToNewFrame: " + e.ToString());
            }
            return foo;
        }

        public Boolean SwitchToNewWindow(String keys)
        {
            Boolean foo = false;
            IWebDriver newWindow = null;
            parentHandle = Driver.CurrentWindowHandle;
            var windows = Driver.WindowHandles;

            while (windows.Count < 2)
            {
                windows = Driver.WindowHandles;
            }

            try
            {
                foreach (var window in windows)
                {
                    newWindow = Driver.SwitchTo().Window(window);
                    if (newWindow.PageSource.Contains(keys))
                    {
                        newWindow.Manage().Window.Maximize();
                        foo = true;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("SwitchToNewWindow: " + e.ToString());
            }
            return foo;
        }

        public Boolean SwitchToParentWindow()
        {
            Boolean foo = false;

            try
            {
                Driver.SwitchTo().Window(parentHandle);
                foo = true;
            }
            catch (Exception e)
            {
                Logger.Error("SwitchToParentWindow: " + e.ToString());
            }
            return foo;
        }

        public Boolean Skip()
        {
            Boolean foo = true;
            return foo;
        }

        public Boolean AcceptAlert()
        {
            Boolean foo = false;            

            try
            {
                //  IAlert alert = new WebDriverWait(Driver, TimeSpan.FromSeconds(30)).Until(ExpectedConditions.AlertIsPresent());
                Driver.SwitchTo().Alert().Accept();

                foo = true;
            }
            catch (Exception e)
            {

                Logger.Error("AcceptAlert: " + e.ToString());
                Driver.SwitchTo().Alert().Accept();
                //System.Windows.Forms.SendKeys.SendWait("{Enter}");
            }
            return foo;
        }

        public Boolean IsAlertPresent()
        {
            try
            {
                Driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException e)
            {
                return false;
            }
        }


        public Boolean ConfirmJavescriptWindow(String windowName)
        {
            Boolean foo = false;

            AutomationElement targetElement = null;
            AutomationElement targetWindow = null;

            try
            {
                DateTime timeStart = new DateTime();
                timeStart = System.DateTime.Now;
                do
                {
                    Logger.Info("Come into click the popup button");
                    Thread.Sleep(500);
                    targetWindow = AutomationElement.RootElement.FindFirst(TreeScope.Children,
                        new PropertyCondition(AutomationElement.NameProperty, windowName));

                    targetElement = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(
                        new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button),
                        new PropertyCondition(AutomationElement.NameProperty, "OK")));

                    Thread.Sleep(1000);
                } while ((targetElement == null) && timeStart.AddSeconds(30) > DateTime.Now);

                InvokePattern BClick = (InvokePattern)targetElement.GetCurrentPattern(InvokePattern.Pattern);
                BClick.Invoke();

                foo = true;
            }
            catch (Exception e)
            {
                Logger.Error("Click Javascript Window error: " + e.ToString());
                foo = false;
            }

            return foo;
        }

        public Boolean DismissAlert()
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));

            try
            {
                Driver.SwitchTo().Alert().Dismiss();
                foo = true;
            }
            catch (Exception e)
            {
                Logger.Error("DismissAlert: " + e.ToString());
            }
            return foo;
        }

        public Boolean Open(String url)
        {
            Boolean foo = false;
            try
            {
                Driver.Navigate().GoToUrl(url);
                foo = true;
            }
            catch (Exception e)
            {
                Logger.Error("Open: " + e.ToString());
            }
            return foo;
        }

        public Boolean ClickObject(IWebElement element)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));

            try
            {
                try
                {
                    WaitUntil(element, e => element.Displayed).Click();
                    //Wait.Until(d => ((IJavaScriptExecutor)Driver).ExecuteScript("return document.readyState").Equals("complete"));
                    foo = true;
                }
                catch (SystemException e)
                {
                    if (e.ToString().Contains("Element is not clickable at point"))
                    {
                        //This is to handle a common chromedriver issue that returns SystemException - Element not clickable at point(x,y)...
                        ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollTo(0," + element.Location.Y + ")");
                        element.Click();
                        //Wait.Until(d => ((IJavaScriptExecutor)Driver).ExecuteScript("return document.readyState").Equals("complete"));
                        foo = true;
                    }

                    if (e.ToString().Contains("unexpected alert open"))
                    {
                        foo = true;
                    }
                }
            }
            catch (Exception e)
            {
                if (e.ToString().Contains("unexpected alert open"))
                {
                    foo = true;
                }
                else { Logger.Error("ClickObject: " + e.ToString()); }
            }
            return foo;
        }

        public Boolean ClickByJS(IWebElement element)
        {
            Boolean foo = false;
            try
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
                foo = true;
            }
            catch (Exception e)
            {
                if (e.ToString().Contains("stale element"))
                {
                    foo = true;
                }
                else if (e.ToString().Contains("execute timed out after 60 seconds"))
                {
                    foo = true;
                }
                else { Logger.Error("ClickByJS: " + e.ToString()); }
            }
            return foo;
        }

        public Boolean SendKeys(IWebElement element, String keys)
        {
            int retry = 0;
            int retryLimit = 3;
            int timeout = 1000;
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));

            try
            {
                WaitUntil(element, e => element.Displayed);
            retry: //To handle InvalidElementStateException: invalid element state: Element is not currently interactable and may not be manipulated.
                try
                {
                    while (retry <= retryLimit)
                    {
                        retry++;
                        element.Clear();
                        retry = retryLimit + 1;
                    }
                }
                catch
                {
                    Thread.Sleep(timeout);
                    goto retry;
                }

                element.SendKeys(keys);
                Wait.Until(d => element.GetAttribute("value").Equals(keys));
                foo = true;

            }
            catch (Exception e)
            {
                Logger.Error("SendKeys: " + e.ToString());
            }
            return foo;
        }

        public Boolean SendKeysByJS(IWebElement element, String keys)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));

            try
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].value='" + keys + "';", element);
                Wait.Until(d => element.GetAttribute("value").Equals(keys));
                foo = true;
            }
            catch (Exception e)
            {
                Logger.Error("SendKeysByJS: " + e.ToString());
            }
            return foo;
        }

        public Boolean SelectOptions(IWebElement element, String keys)
        {
            Boolean foo = false;
            try
            {
                if (WaitUntil(element, e => element.Displayed).Displayed)
                {
                    SelectElement dropdown = new SelectElement(element);
                    dropdown.SelectByText(keys);
                    foo = true;
                    //TEMP FIX: Set result to true after SelectByText(keys) is executed. The below condition check is unstable and intermittently throwing a stale element exception
                    //foo = dropdown.SelectedOption.Text.Equals(keys);
                }
            }
            catch (Exception e)
            {
                Logger.Error("SelectOptions: " + e.ToString());
            }
            return foo;
        }

        public Boolean DelayExplicit(String keys)
        {
            Boolean foo = true;
            int bar = (Int32.Parse(keys)) * 1000;

            Thread.Sleep(bar);

            return foo;
        }

        public Boolean DelayUntilFieldIsPopulated(IWebElement element)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));

            try
            {
                Wait.Until(d => element.GetAttribute("value").Length > 0);
                foo = true;
            }
            catch (Exception e)
            {
                Logger.Error("DelayUntilFieldIsPopulated: " + e.ToString());
            }
            return foo;
        }

        public Boolean DelayUntilElemSelected(IWebElement element)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));

            try
            {
                Wait.Until(d => element.Selected);
                foo = true;
            }
            catch (Exception e)
            {
                Logger.Error("DelayUntilElemSelected: " + e.ToString());
            }
            return foo;
        }

        public Boolean DelayUntilElemNotSelected(IWebElement element)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));

            try
            {
                Wait.Until(d => !element.Selected);
                foo = true;
            }
            catch (Exception e)
            {
                Logger.Error("DelayUntilElemNotSelected: " + e.ToString());
            }
            return foo;
        }

        //CUSTOM ACTIONS==================================================================================
        public Boolean CustomInternetVerifyCorrespondenceAppDetails(IWebElement element)
        {
            //Not yet used on the excel. 
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));

            try
            {
                Wait.Until(d => element.Displayed);
                foo = element.Displayed;
            }
            catch (Exception e)
            {
                Logger.Error("CustomInternetVerifyCorrespondenceAppDetails: " + e.ToString());
            }

            #region Correpspondence Retry
            if (!foo)
            {
                int counter = 0;
                while (counter < 5 && !foo)
                {
                    Driver.Navigate().Refresh();
                    Driver.FindElement(By.XPath("//*[contains(@id,'tab')]//a[contains(.,'Correspondence')]")).Click();
                    try
                    {
                        foo = element.Displayed;
                    }
                    catch
                    {
                        foo = false;
                    }
                    counter++;
                }
            }
            #endregion

            return foo;
        }

        public Boolean CustomInternetVerifyCorrespondenceCorrDetails(IWebElement element)
        {
            Boolean foo = false;

            try
            {
                Builder.MoveToElement(WaitUntil(element, e => element.Displayed));
                element.Click();
                foo = true;
            }
            catch (Exception e)
            {
                Logger.Error("CustomInternetVerifyCorrespondenceCorrDetails: " + e.ToString());
            }

            #region Correpspondence Retry
            if (!foo)
            {
                int counter = 0;
                while (counter < 5 && !foo)
                {
                    Driver.Navigate().Refresh();
                    try
                    {
                        Builder.MoveToElement(WaitUntil(element, e => element.Displayed));
                        element.Click();
                        foo = true;
                    }
                    catch
                    {
                        foo = false;
                    }
                    counter++;
                }
            }
            #endregion

            return foo;
        }

        public Boolean CustomActionUploadInternetMandatoryDoc(String keys)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));
            string xpath = "//tr[contains(@id,'attchment')][contains(.,'*')]//input[@name='uploadFile']";

            try
            {
                //One time check to see if there are any mandatory documents; Only proceed if true.
                if (Driver.FindElements(By.XPath(xpath)).Count != 0)
                {
                    IList<IWebElement> documents = Driver.FindElements(By.XPath(xpath));

                    //Upload a file for all mandatory docs
                    foreach (IWebElement document in documents)
                    {
                        string id = document.GetAttribute("id");
                        document.SendKeys(keys);
                        Wait.Until(d => Driver.FindElement(By.XPath("//tr[contains(@id,'" + id + "')]//a[1]")).Displayed);
                        foo = Driver.FindElement(By.XPath("//tr[contains(@id,'" + id + "')]//a[1]")).Displayed;
                    }
                }
                else
                {
                    foo = true;
                }
            }
            catch (Exception e)
            {
                Logger.Error("CustomActionUploadInternetMandatoryDoc: " + e.ToString());
            }
            return foo;
        }

        public Boolean CustomActionUploadInternetNonMandatoryDoc(String keys)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));
            string xpath = "//tr[contains(@id,'attchment')][not(contains(.,'*'))]//input[@name='uploadFile']";

            try
            {
                //One time check to see if there are any mandatory documents; Only proceed if true.
                if (Driver.FindElements(By.XPath(xpath)).Count != 0)
                {
                    IList<IWebElement> documents = Driver.FindElements(By.XPath(xpath));

                    //Upload a file for all mandatory docs
                    foreach (IWebElement document in documents)
                    {
                        string id = document.GetAttribute("id");
                        document.SendKeys(keys);
                        Wait.Until(d => Driver.FindElement(By.XPath("//tr[contains(@id,'" + id + "')]//a[1]")).Displayed);
                        foo = Driver.FindElement(By.XPath("//tr[contains(@id,'" + id + "')]//a[1]")).Displayed;
                    }
                }
                else
                {
                    foo = true;
                }
            }
            catch (Exception e)
            {
                Logger.Error("CustomActionUploadInternetNonMandatoryDoc: " + e.ToString());
            }
            return foo;
        }

        public Boolean CustomActionAVAUploadInternetMandatoryDoc(String keys)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));
            string xpath = "//tr[contains(@id,'document')][contains(.,'*')]//input[@name='uploadFile']";

            try
            {
                //One time check to see if there are any mandatory documents; Only proceed if true.
                if (Driver.FindElements(By.XPath(xpath)).Count != 0)
                {
                    IList<IWebElement> documents = Driver.FindElements(By.XPath(xpath));

                    //Upload a file for all mandatory docs
                    foreach (IWebElement document in documents)
                    {
                        string id = document.GetAttribute("id");
                        document.SendKeys(keys);
                        Wait.Until(d => Driver.FindElement(By.XPath("//tr[contains(@id,'" + id + "')]//a[1]")).Displayed);
                        foo = Driver.FindElement(By.XPath("//tr[contains(@id,'" + id + "')]//a[1]")).Displayed;
                    }
                }
                else
                {
                    foo = true;
                }
            }
            catch (Exception e)
            {
                Logger.Error("CustomActionAVAUploadInternetMandatoryDoc: " + e.ToString());
            }
            return foo;
        }

        public Boolean CustomActionAVAUploadInternetNonMandatoryDoc(String keys)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));
            string xpath = "//tr[contains(@id,'document')][not(contains(.,'*'))]//input[@name='uploadFile']";

            try
            {
                //One time check to see if there are any mandatory documents; Only proceed if true.
                if (Driver.FindElements(By.XPath(xpath)).Count != 0)
                {
                    IList<IWebElement> documents = Driver.FindElements(By.XPath(xpath));

                    //Upload a file for all mandatory docs
                    foreach (IWebElement document in documents)
                    {
                        string id = document.GetAttribute("id");
                        document.SendKeys(keys);
                        Wait.Until(d => Driver.FindElement(By.XPath("//tr[contains(@id,'" + id + "')]//a[1]")).Displayed);
                        foo = Driver.FindElement(By.XPath("//tr[contains(@id,'" + id + "')]//a[1]")).Displayed;
                    }
                }
                else
                {
                    foo = true;
                }
            }
            catch (Exception e)
            {
                Logger.Error("CustomActionAVAUploadInternetNonMandatoryDoc: " + e.ToString());
            }
            return foo;
        }

        public Boolean CustomActionInternetCompareTableDataDates(String dateCol)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));
            IList<IWebElement> elements = Driver.FindElements(By.XPath("//tbody[contains(@class,'table-data')]//tr[.//a]//td[" + dateCol + "]"));
            String dateTime = null;
            String pdateTime = null;
            DateTime dt;
            DateTime pdt;

            try
            {
                foreach (IWebElement elem in elements)
                {
                    dateTime = elem.Text.Trim();
                    dt = Convert.ToDateTime(dateTime);

                    if (pdateTime == null)
                    {
                        pdateTime = elem.Text.Trim();
                    }

                    else
                    {
                        pdt = Convert.ToDateTime(pdateTime);
                        int res = DateTime.Compare(pdt, dt);

                        if (res >= 0)
                        {
                            foo = true;
                        }
                        else
                        {
                            foo = false;
                        }

                        pdateTime = dateTime;
                    }
                }
            }
            catch (Exception e)
            {
                foo = false;
                Logger.Error("CustomActionInternetCompareTableDataDates: " + e.ToString());
            }
            return foo;
        }

        public Boolean CustomActionIntranetSearchAppNum(String keys)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));
            int rcount = 0;

            try
            {
                ClickObject(Driver.FindElement(By.Id("btnSearch")));
                while (!IsElementPresent(By.LinkText(keys), 2) && rcount < 20)
                {
                    ClickObject(Driver.FindElement(By.Id("btnSearch")));
                    rcount++;
                }
                foo = true;
            }
            catch (Exception e)
            {
                Logger.Error("CustomActionIntranetSearchAppNum: " + e.ToString());
            }
            return foo;
        }

        public Boolean CustomActionIntranetBPMChangeValues(String keys)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));
            String[] keysAr = keys.Split('|');

            try
            {
                BPMChangeValue(keysAr[0], keysAr[1]);
                foo = true;
            }
            catch (Exception e)
            {
                Logger.Error("CustomActionIntranetBPMChangeValues: " + e.ToString());
            }
            return foo;
        }

        private Boolean IsElementPresent(By by, int timeout = 30)
        {
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));

            try
            {
                Wait.Until(ExpectedConditions.ElementExists(by));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private String BPMChangeValue(string text, string onevalue)
        {
            string changedvalue = "";

            try
            {
                changedvalue = BPMChangeValueOriginal(text, onevalue);
            }
            catch (Exception e)
            {
                Logger.Error("CustomActionIntranetBPMOfficerRecommendation: " + e.ToString());
            }

            return changedvalue;
        }

        private String BPMChangeValueOriginal(string text, string onevalue)
        {
            Builder.MoveToElement(Driver.FindElement(By.XPath("//label[contains(text(),'" + text + "')]"))).Perform();
            String id = Driver.FindElement(By.XPath("//label[contains(text(),'" + text + "')]/../..")).GetAttribute("id");
            if (IsElementPresent(By.XPath("//*[@id='" + id + "']/div[2]/div[2]/div[1]"), 2))
            {
                var dropdown = Driver.FindElement(By.XPath("//*[@id='" + id + "']/div[2]/div[2]/div[1]"));
                var input = Driver.FindElement(By.XPath("//*[@id='" + id + "']/div[2]/div[2]/div[3]/input"));
                string aria_haspopup = input.GetAttribute("aria-haspopup");
            }
            while (BPMGetDropDownValue(id).ToUpper().Trim() != onevalue.Replace("&", "&amp;").ToUpper().Trim())
            {
                BPMSetValue(id, onevalue);
                Thread.Sleep(300);
            }
            return onevalue;
        }

        private String BPMGetDropDownValue(string id)
        {
            var input = Driver.FindElement(By.XPath("//*[@id='" + id + "']/div[@class='content']/div[1]/span"));
            return input.GetAttribute("innerHTML");
        }

        private void BPMSetValue(string id, string value)
        {
            var input = Driver.FindElement(By.XPath("//*[@id='" + id + "']/div[2]/div[2]/div[3]/input"));
            string isInvalid = "true";
            while (isInvalid == "true")
            {
                input.Clear();
                input.SendKeys(value);
                input.Click();
                input.SendKeys("\t");
                isInvalid = input.GetAttribute("aria-invalid");
                if (isInvalid == "true")
                {
                    Thread.Sleep(300);
                    isInvalid = input.GetAttribute("aria-invalid");
                }
            }
        }

        //VERIFICATIONS===================================================================================
        public Boolean VerifyElementCount(String testdata, String locator, String locvalue, String attribute, String attrvalue)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));
            IList<IWebElement> elements = LocateElementsByType(locator, locvalue, attribute, attrvalue);

            try
            {
                if (elements.Count == Convert.ToInt32(testdata))
                {
                    foo = true;
                }
            }
            catch (Exception e)
            {
                Logger.Error("VerifyElementCount: " + e.ToString());
            }
            return foo;
        }

        public Boolean VerifyMinElementCount(String testdata, String locator, String locvalue, String attribute, String attrvalue)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));
            IList<IWebElement> elements = LocateElementsByType(locator, locvalue, attribute, attrvalue);

            try
            {
                if (elements.Count >= Convert.ToInt32(testdata))
                {
                    foo = true;
                }
            }
            catch (Exception e)
            {
                Logger.Error("VerifyMinElementCount: " + e.ToString());
            }
            return foo;
        }

        public Boolean VerifyMaxElementCount(String testdata, String locator, String locvalue, String attribute, String attrvalue)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));
            IList<IWebElement> elements = LocateElementsByType(locator, locvalue, attribute, attrvalue);

            try
            {
                if (elements.Count <= Convert.ToInt32(testdata))
                {
                    foo = true;
                }
            }
            catch (Exception e)
            {
                Logger.Error("VerifyMaxElementCount: " + e.ToString());
            }
            return foo;
        }

        public Boolean VerifyFieldValue(IWebElement element, String testdata)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));

            try
            {
                Wait.Until(d => element.Displayed);
                foo = element.GetAttribute("value").Equals(testdata);
            }
            catch (Exception e)
            {
                Logger.Error("VerifyFieldValue: " + e.ToString());
            }
            return foo;
        }

        public Boolean VerifyElementIsSelected(IWebElement element)
        {
            Boolean foo = false;

            try
            {
                foo = element.Selected;
            }
            catch (Exception e)
            {
                Logger.Error("VerifyElementIsSelected: " + e.ToString());
            }
            return foo;
        }

        public Boolean VerifyElementIsNotSelected(IWebElement element)
        {
            Boolean foo = false;

            try
            {
                foo = !element.Selected;
            }
            catch (Exception e)
            {
                Logger.Error("VerifyElementIsNotSelected: " + e.ToString());
            }
            return foo;
        }

        public Boolean VerifyElementPresent(IWebElement element)
        {
            int retry = 0;
            int retryLimit = 5;
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));

            try
            {
                while (!foo && retry < retryLimit)
                {
                    foo = WaitUntil(element, e => element.Displayed).Displayed;
                    Thread.Sleep(500);
                    retry++;
                }
            }
            catch (Exception e)
            {
                Logger.Error("VerifyElementPresent: " + e.ToString());
            }
            return foo;
        }

        public Boolean VerifyElementNotPresent(IWebElement element)
        {
            int retry = 0;
            int retryLimit = 5;
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));

            if (element == null)
            {
                foo = true;
            }

            else
            {
                try
                {
                    while (!foo && retry < retryLimit)
                    {
                        Wait.Until(d => !element.Displayed);
                        retry++;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("VerifyElementNotPresent: " + e.ToString());
                }
            }
            return foo;
        }

        public Boolean VerifyElementContainsText(IWebElement element, String keys)
        {
            Boolean foo = false;
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(globalTimeout));

            try
            {
                foo = WaitUntil(element, e => element.Displayed).Text.Contains(keys);
            }
            catch (Exception e)
            {
                Logger.Error("VerifyElementContainsText: " + e.ToString());
            }
            return foo;
        }

        //UTILS===========================================================================================
        public IWebElement WaitUntil(IWebElement element, Func<IWebElement, bool> condition)
        {
            IWait<IWebElement> wait = new DefaultWait<IWebElement>(element);
            wait.Timeout = TimeSpan.FromSeconds(globalTimeout);
            wait.PollingInterval = TimeSpan.FromSeconds(5);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            return element;
        }

        //Take snapshots or screenshots of the page
        public void TakeSnapshots(String environment, String fileName, String testCaseName, String imgName)
        {
            ImageFormat imgFormat = ImageFormat.Png;
            imgName = imgName + ".png";

            string str = fileName.Remove(fileName.LastIndexOf('.'));
            string newpath = resultsDirectory + environment + "\\" + str + "\\" + screenshotFolderName + "\\" + testCaseName + "\\";
            System.IO.Directory.CreateDirectory(newpath);
            try
            {
                ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(newpath + imgName, imgFormat);
            }
            catch (Exception e)
            {
                Logger.Error("TakeSnapshots: " + e.ToString());
            }
        }

        //Take page snapshot with element highlighted
        public void TakeElemSnapshot(IWebElement element, String environment, String fileName, String testCaseName, String imgName)
        {
            //ImageFormat imgFormat = ImageFormat.Png;
            imgName = imgName + ".png";

            string newImgName = "temp_" + imgName;
            string str = fileName.Remove(fileName.LastIndexOf('.'));

            string newpath = resultsDirectory + environment + "\\" + str + "\\" + screenshotFolderName + "\\" + testCaseName + "\\";

            if (!System.IO.Directory.Exists(newpath))
            {
                System.IO.Directory.CreateDirectory(newpath);
            }

            try
            {
                try
                {
                    //((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(newpath + imgName, imgFormat);

                    //Attempt to highligh the element, exits function if any exception is encountered - element will not be highlighted in this case.
                    //using (Image image = Image.FromFile(newpath + imgName))
                    //{
                    //    Point point = element.Location;
                    //    int eleWidth = element.Size.Width;
                    //    int eleHeight = element.Size.Height;
                    //    Size size = new Size(eleWidth, eleHeight);

                    //    using (Graphics graphics = Graphics.FromImage(image))
                    //    {
                    //        Pen pen;
                    //        Rectangle rect;
                    //        pen = new Pen(Color.Red, 2);
                    //        rect = new Rectangle(point, size);
                    //        graphics.DrawRectangle(pen, rect);

                    //        Color customColor = Color.FromArgb(50, Color.Yellow);
                    //        SolidBrush shadowBrush = new SolidBrush(customColor);
                    //        graphics.FillRectangle(shadowBrush, rect);
                    //    }
                    //    newpath = newpath.Replace("/", "\\");
                    //    image.Save(newpath + newImgName, imgFormat);
                    //}
                    //System.IO.File.Delete(newpath + imgName);
                    try
                    {
                        //((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].setAttribute('style', 'border: 2px solid red;');", element);
                        ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(newpath + imgName, ImageFormat.Png);
                        //((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].removeAttribute('style','border: 2px solid red;');", element);
                    }
                    catch (Exception e)
                    {

                    }

                    //System.IO.File.Move(newpath + newImgName, newpath + imgName);

                }
                catch
                {
                    ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(newpath + imgName, ImageFormat.Png);
                    Logger.Error("Highlighting step element " + imgName + "... failed!");
                }
            }
            catch (Exception e)
            {
                Logger.Error("Unable to take step screenshot " + e.ToString());
            }
        }

        //Get entire page snapshot. Invoke using GetEntireScreenshot().Save(newpath + imgName, imgFormat); Not being used currently.
        public Bitmap GetEntireScreenshot()
        {

            Bitmap stitchedImage = null;
            try
            {
                long totalwidth1 = (long)((IJavaScriptExecutor)Driver).ExecuteScript("return document.body.offsetWidth");//documentElement.scrollWidth");

                long totalHeight1 = (long)((IJavaScriptExecutor)Driver).ExecuteScript("return  document.body.parentNode.scrollHeight");

                int totalWidth = (int)totalwidth1;
                int totalHeight = (int)totalHeight1;

                // Get the Size of the Viewport
                long viewportWidth1 = (long)((IJavaScriptExecutor)Driver).ExecuteScript("return document.body.clientWidth");//documentElement.scrollWidth");
                long viewportHeight1 = (long)((IJavaScriptExecutor)Driver).ExecuteScript("return window.innerHeight");//documentElement.scrollWidth");

                int viewportWidth = (int)viewportWidth1;
                int viewportHeight = (int)viewportHeight1;


                // Split the Screen in multiple Rectangles
                List<Rectangle> rectangles = new List<Rectangle>();
                // Loop until the Total Height is reached
                for (int i = 0; i < totalHeight; i += viewportHeight)
                {
                    int newHeight = viewportHeight;
                    // Fix if the Height of the Element is too big
                    if (i + viewportHeight > totalHeight)
                    {
                        newHeight = totalHeight - i;
                    }
                    // Loop until the Total Width is reached
                    for (int ii = 0; ii < totalWidth; ii += viewportWidth)
                    {
                        int newWidth = viewportWidth;
                        // Fix if the Width of the Element is too big
                        if (ii + viewportWidth > totalWidth)
                        {
                            newWidth = totalWidth - ii;
                        }

                        // Create and add the Rectangle
                        Rectangle currRect = new Rectangle(ii, i, newWidth, newHeight);
                        rectangles.Add(currRect);
                    }
                }

                // Build the Image
                stitchedImage = new Bitmap(totalWidth, totalHeight);
                // Get all Screenshots and stitch them together
                Rectangle previous = Rectangle.Empty;
                foreach (var rectangle in rectangles)
                {
                    // Calculate the Scrolling (if needed)
                    if (previous != Rectangle.Empty)
                    {
                        int xDiff = rectangle.Right - previous.Right;
                        int yDiff = rectangle.Bottom - previous.Bottom;
                        // Scroll
                        //selenium.RunScript(String.Format("window.scrollBy({0}, {1})", xDiff, yDiff));
                        ((IJavaScriptExecutor)Driver).ExecuteScript(String.Format("window.scrollBy({0}, {1})", xDiff, yDiff));
                        System.Threading.Thread.Sleep(200);
                    }

                    // Take Screenshot
                    var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();

                    // Build an Image out of the Screenshot
                    Image screenshotImage;
                    using (MemoryStream memStream = new MemoryStream(screenshot.AsByteArray))
                    {
                        screenshotImage = Image.FromStream(memStream);
                    }

                    // Calculate the Source Rectangle
                    Rectangle sourceRectangle = new Rectangle(viewportWidth - rectangle.Width, viewportHeight - rectangle.Height, rectangle.Width, rectangle.Height);

                    // Copy the Image
                    using (Graphics g = Graphics.FromImage(stitchedImage))
                    {
                        g.DrawImage(screenshotImage, rectangle, sourceRectangle, GraphicsUnit.Pixel);
                    }

                    // Set the Previous Rectangle
                    previous = rectangle;
                }
            }
            catch
            {
            }
            return stitchedImage;
        }

        //Returns a webelement by providing a locator, locator value. Optional attribute and attribute value
        public IWebElement LocateByType(String locator, String locvalue, String attribute, String attrvalue)
        {
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement getElement = null;
            IWebElement elem = null;
            int rcount = 0;

        retry:
            try
            {
                if ("class".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.ClassName(locvalue)) != null);
                    elem = Driver.FindElement(By.ClassName(locvalue));

                }
                else if ("css".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.CssSelector(locvalue)) != null);
                    elem = Driver.FindElement(By.CssSelector(locvalue));

                }
                else if ("id".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.Id(locvalue)) != null);
                    elem = Driver.FindElement(By.Id(locvalue));

                }
                else if ("link".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.LinkText(locvalue)) != null);
                    elem = Driver.FindElement(By.LinkText(locvalue));

                }
                else if ("name".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.Name(locvalue)) != null);
                    elem = Driver.FindElement(By.Name(locvalue));

                }
                else if ("partiallink".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.PartialLinkText(locvalue)) != null);
                    elem = Driver.FindElement(By.PartialLinkText(locvalue));

                }
                else if ("tag".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.TagName(locvalue)) != null);
                    elem = Driver.FindElement(By.TagName(locvalue));

                }
                else if ("xpath".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.XPath(locvalue)) != null);
                    elem = Driver.FindElement(By.XPath(locvalue));
                }

                if (!attribute.Equals("") && !attribute.Equals(null))
                {
                    if (elem.GetAttribute(attribute).Contains(attrvalue))
                    {
                        Logger.Info(locator + " " + locvalue + " and attribute " + attrvalue + " found...");
                        getElement = elem;
                    }
                }
                else
                {
                    getElement = elem;
                }

                Logger.Info(locator + " " + locvalue + " and attribute " + attrvalue + " found... **********");

                //Builder.MoveToElement(getElement).Perform();
            }
            catch (Exception ex)
            {
                if (rcount <= 3)
                {
                    rcount++;
                    Thread.Sleep(1000);
                    Logger.Info(String.Format("Unable to locate element. Retrying... loc = {0}, locv = {1}, attr = {2}, attrv = {3}, error = {4}", locator, locvalue, attribute, attrvalue, ex.ToString()));
                    goto retry;  
                }
                Logger.Error(String.Format("Unable to locate element for this step... loc = {0}, locv = {1}, attr = {2}, attrv = {3}, error = {4}", locator, locvalue, attribute, attrvalue, ex.ToString()));
            }
            return getElement;
        }

        //Returns a list of webelements by providing a locator, locator value. Optional attribute and attribute value
        public IList<IWebElement> LocateElementsByType(String locator, String locvalue, String attribute, String attrvalue)
        {
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IList<IWebElement> getElements = null;
            IList<IWebElement> elems = null;
            IWebElement welem = null;

            try
            {
                if ("class".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.ClassName(locvalue)) != null);
                    welem = Driver.FindElement(By.ClassName(locvalue));
                    elems = Driver.FindElements(By.ClassName(locvalue));
                    if (attribute.Equals(""))
                    {
                        getElements = elems;
                    }

                    else
                    {
                        if (welem.GetAttribute(attribute).Contains(attrvalue))
                        {
                            Logger.Info("Class name " + locvalue + " and attribute " + attrvalue + " found...");
                            getElements = elems;
                        }
                    }
                }
                else if ("css".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.CssSelector(locvalue)) != null);
                    welem = Driver.FindElement(By.CssSelector(locvalue));
                    elems = Driver.FindElements(By.CssSelector(locvalue));
                    if (attribute.Equals(""))
                    {
                        getElements = elems;
                    }

                    else
                    {
                        if (welem.GetAttribute(attribute).Contains(attrvalue))
                        {
                            Logger.Info("Css name " + locvalue + " and attribute " + attrvalue + " found...");
                            getElements = elems;
                        }
                    }
                }
                else if ("id".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.Id(locvalue)) != null);
                    welem = Driver.FindElement(By.Id(locvalue));
                    elems = Driver.FindElements(By.Id(locvalue));
                    if (attribute.Equals(""))
                    {
                        getElements = elems;
                    }

                    else
                    {
                        if (welem.GetAttribute(attribute).Contains(attrvalue))
                        {
                            Logger.Info("Id " + locvalue + " and attribute " + attrvalue + " found...");
                            getElements = elems;
                        }
                    }
                }
                else if ("link".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.LinkText(locvalue)) != null);
                    welem = Driver.FindElement(By.LinkText(locvalue));
                    elems = Driver.FindElements(By.LinkText(locvalue));
                    if (attribute.Equals(""))
                    {
                        getElements = elems;
                    }

                    else
                    {
                        if (welem.GetAttribute(attribute).Contains(attrvalue))
                        {
                            Logger.Info("Link " + locvalue + " and attribute " + attrvalue + " found...");
                            getElements = elems;
                        }
                    }
                }
                else if ("name".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.Name(locvalue)) != null);
                    welem = Driver.FindElement(By.Name(locvalue));
                    elems = Driver.FindElements(By.Name(locvalue));
                    if (attribute.Equals(""))
                    {
                        getElements = elems;
                    }

                    else
                    {
                        if (welem.GetAttribute(attribute).Contains(attrvalue))
                        {
                            Logger.Info("Name " + locvalue + " and attribute " + attrvalue + " found...");
                            getElements = elems;
                        }
                    }
                }
                else if ("partiallink".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.PartialLinkText(locvalue)) != null);
                    welem = Driver.FindElement(By.PartialLinkText(locvalue));
                    elems = Driver.FindElements(By.PartialLinkText(locvalue));
                    if (attribute.Equals(""))
                    {
                        getElements = elems;
                    }

                    else
                    {
                        if (welem.GetAttribute(attribute).Contains(attrvalue))
                        {
                            Logger.Info("Partial Link " + locvalue + " and attribute " + attrvalue + " found...");
                            getElements = elems;
                        }
                    }
                }
                else if ("tag".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.TagName(locvalue)) != null);
                    welem = Driver.FindElement(By.TagName(locvalue));
                    elems = Driver.FindElements(By.TagName(locvalue));
                    if (attribute.Equals(""))
                    {
                        getElements = elems;
                    }

                    else
                    {
                        if (welem.GetAttribute(attribute).Contains(attrvalue))
                        {
                            Logger.Info("Tag " + locvalue + " and attribute " + attrvalue + " found...");
                            getElements = elems;
                        }
                    }
                }
                else if ("xpath".Equals(locator))
                {
                    Wait.Until(d => Driver.FindElement(By.XPath(locvalue)) != null);
                    welem = Driver.FindElement(By.XPath(locvalue));
                    elems = Driver.FindElements(By.XPath(locvalue));
                    if (attribute.Equals(""))
                    {
                        getElements = elems;
                    }

                    else
                    {
                        if (welem.GetAttribute(attribute).Contains(attrvalue))
                        {
                            Logger.Info("XPath " + locvalue + " and attribute " + attrvalue + " found...");
                            getElements = elems;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return getElements;
        }

        [TearDown]
        //TearDown attribute defines that the following method will run after each method runs.
        public void TearDown()
        {

        }

        [OneTimeTearDown]
        //OneTimeTearDown attribute defines that the following method will run once the methods in the class have run, i.e. once in the end of the run
        public void Shutdown()
        {
            //Close the IWebDriver instance
            Driver.Quit();
        }
    }
}
