using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeBeanLibrary
{
    public class Utility
    {
        //===============================================================FAST UI==============================================================              
        public static string searchTCPlaceholder = "Search TC...";
        public static string schedulerPlaceholder = "[ 23 : 59 : 59 ]";
        //===============================================================FAST UI==============================================================              
        //================================================================COMMON==============================================================              
        public static string screenshotFolderName = "screenshots";
        public static string tcresults = "tcresults.txt";
        public static string tcstepresults = "stepresults.txt";
        public static string tsruntimeexec = "tsruntimeexec.txt";
        public static string tsruntimeexecmax = "tsruntimeexecmax.txt";
        public static string getModuleData = "getModuleData.txt";

        public static string chromeDownloadPath = System.IO.Directory.GetCurrentDirectory() + "\\downloads\\";
        //public static string chromeDriverPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "\\Resources\\chromedriver\\"));
        //public static string phantomDriverPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "\\Resources\\phantomjs-2.1.1-windows\\phantomjs-2.1.1-windows\\bin\\"));
        //public static string ieDriverPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "\\Resources\\iedriver\\"));

        public static string chromeDriverPath = System.IO.Directory.GetCurrentDirectory() + "\\Resources\\chromedriver\\";
        public static string phantomDriverPath = System.IO.Directory.GetCurrentDirectory() + "\\Resources\\phantomjs-2.1.1-windows\\phantomjs-2.1.1-windows\\bin\\";
        public static string ieDriverPath = System.IO.Directory.GetCurrentDirectory() + "\\Resources\\iedriver\\";

        public static string resultsDirectory = System.IO.Directory.GetCurrentDirectory() + "\\testresults\\";
        public static string backUpResultsDirectory = System.IO.Directory.GetCurrentDirectory() + "\\backup\\";
        public static string datedResultsDirectory = System.IO.Directory.GetCurrentDirectory() + "\\DatedResults\\";
        public static string dailyDatedDirectory = datedResultsDirectory + getEnvironment + "\\" + DateTime.Now.ToString("dd-MMM-yyyy") + "\\";
        public static string dailyDatedScenarioDetailedResults = dailyDatedDirectory + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
        public static string dailyDatedScenarioSummaryResults = dailyDatedDirectory + DateTime.Now.ToString("dd-MMM-yyyy") + "-Summary.txt";
        //================================================================COMMON==============================================================

        //These set of methods are for retrieval of data from the App.config file
        //The App.Config file contains all the options and default settings of this app
        //=============================================================EXCEL READER===========================================================
        //Driver type to use for running
        public static string getDriverType
        {
            get
            {
                return Config.GetKey("driverType");
            }
        }
        //=============================================================EXCEL READER===========================================================

        //=================================================================FAST===============================================================
        //Default Folder
        public static string defFolder
        {
            get
            {
                return Config.GetKey("defaultRep");
            }
        }

        //Maximum concurrent test case setting for Phantom JS
        public static int maxJS
        {
            get
            {
                return Int32.Parse(Config.GetKey("maxConPJS"));
            }
        }

        //Maximum concurrent test case setting for all other drivers
        public static int maxOthers
        {
            get
            {
                return Int32.Parse(Config.GetKey("maxConOther"));
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

        //Default environment to be used
        public static string defEnv
        {
            get
            {
                return Config.GetKey("defEnvironment");
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

        //Default setting for handling duplicates
        public static string adFlag
        {
            get
            {
                return Config.GetKey("allowDupe");
            }
        }

        //Assign environment value to getEnv
        public static string getEnvironment
        {
            get
            {
                return Config.GetKey("environment");
            }
        }

        //Default setting for scheduler checkbox
        public static string schedulerFlag
        {
            get
            {
                return Config.GetKey("schedulerFlag");
            }
        }

        //Assign default value to scheduler textbox
        public static string getSchedulerDefaultvalue
        {
            get
            {
                return Config.GetKey("schedulerDefaultTime");
            }
        }
        //=================================================================FAST===============================================================
    }
}
