using Mehrsan.Common.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrsan.Common
{
    public class Logger : Mehrsan.Common.Interface.ILogger
    {
        #region Fields
        private string _defaultCategory = "Default Category";

        #endregion

        #region Properties

        public Microsoft.Extensions.Logging.ILogger LoggerInstance { get; }
        
        #endregion
        

        public Logger()
        {
            //LoggerInstance = new Logger<Mehrsan.Common.Logger>(new LoggerFactory());
        }

        public void Log(string message)
        {
            //if (!Directory.Exists(Common.LogDirectory))
            //    Directory.CreateDirectory(Common.LogDirectory);
            //string todayLog = Common.LogDirectory + now.Year + "_" + now.Month + "_" + now.Day + ".txt";
            //using (StreamWriter sw = new StreamWriter(todayLog, true))
            //{
            //LoggerInstance.Log(LogLevel.Information,message);
            //}
        }

        public void Log(Exception ex, string layer)
        {
            //LoggerInstance.LogCritical(ex, ex.Message);
        }

        public void Log(Exception ex)
        {
            //LoggerInstance.LogCritical(ex, " Error occured "+ DateTime.Now);
        }
    }
}
