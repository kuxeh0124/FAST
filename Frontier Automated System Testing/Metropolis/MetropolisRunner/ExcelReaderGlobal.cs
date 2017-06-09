using CoffeeBeanLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoffeeBeanRunner
{
    public class ExcelReaderGlobal
    {
        public static Dictionary<String, int> getRowsCount = new Dictionary<String, int>();

        public static Dictionary<String, String> getModuleData = new Dictionary<String, String>();

        public static Dictionary<String, Boolean> getRunningFlag = new Dictionary<String, Boolean>();

        public static Dictionary<String, DriverClassLib> getRunningDriver = new Dictionary<String, DriverClassLib>();

        public static Dictionary<String, object> getMonitoringObj = new Dictionary<String, object>();


        public static void TaskPause(String filename)
        {
            if (ExcelReaderGlobal.getRunningFlag["pauseflag_" + filename] == false)
            {
                Monitor.Enter(ExcelReaderGlobal.getMonitoringObj["object_" + filename]);
                ExcelReaderGlobal.getRunningFlag["pauseflag_" + filename] = true;
            }
        }

        public static void TaskResume(String filename)
        {
            if (ExcelReaderGlobal.getRunningFlag["pauseflag_" + filename])
            {
                ExcelReaderGlobal.getRunningFlag["pauseflag_" + filename] = false;
                Monitor.Exit(ExcelReaderGlobal.getMonitoringObj["object_" + filename]);
            }
        }
    }
}
