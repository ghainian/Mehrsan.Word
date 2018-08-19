using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrsan.Common
{
    public class Logger
    {
        public static void Log(string message)
        {
            if (!Directory.Exists(Common.LogDirectory))
                Directory.CreateDirectory(Common.LogDirectory);
            var now = DateTime.Now;
            string todayLog = Common.LogDirectory + now.Year + "_" + now.Month + "_" + now.Day + ".txt";
            using (StreamWriter sw = new StreamWriter(todayLog, true))
            {
                sw.WriteLine(now);
                sw.WriteLine(message);
                sw.WriteLine("===============================================================================");
            }
        }
    }
}
