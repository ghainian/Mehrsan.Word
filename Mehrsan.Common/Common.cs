using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrsan.Common
{
    public class Common
    {
        public static int MaxReviewDate = 36000;
        public static long DefaultReviewTimeSpan = 10000;

        public static int MaxImagePerWord { get; set; } = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["MaxImagePerWord"].ToString());
        public static string AgentPath { get; set; } = System.Configuration.ConfigurationSettings.AppSettings["AgentPath"].ToString();
        public static string BackupDir { get; set; } = System.Configuration.ConfigurationSettings.AppSettings["BackupDir"].ToString();
        public static string VideoDirectory { get; set; } = System.Configuration.ConfigurationSettings.AppSettings["VideoDirectory"].ToString();


        public static int DownloadGoogleImageWaitTime { get; set; } = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["DownloadGoogleImageWaitTime"].ToString());
        public static int NofRelatedSentences { get; set; } = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["NofRelatedSentences"].ToString());

        public static string LogDirectory { get; set; } = System.Configuration.ConfigurationSettings.AppSettings["LogDirectory"].ToString();

        public static string[] HtmlTags { get; set; } = new string[] { "i", "b", "h1", "h2", "h3", "h4"};

        public static char[] Separators { get; set; } = new char[] { '\'', '_', '>', '<', '\r', '\n', '\t', '?', '!', '#', '$', '€', '£', '=', ' ', '.', ')', '(', '-', '/', '\"', '@', ':', ';', ',', (char)(8203) };
        public static char[] PersianAlphabet { get; set; } = new char[] { 'ا', 'ب', 'پ', 'ت', 'ث', 'ج', 'ح', 'خ', 'ه', 'ع', 'غ', 'ف', 'ق', 'ص', 'ض', 'ش', 'س', 'ی', 'ل', 'ن', 'م', 'ک', 'گ', 'ظ', 'ط', 'ز', 'ر', 'ذ', 'د', 'ئ', 'و' };
        public static string ReadFile(string file)
        {
            using (StreamReader sr = new StreamReader(file))
                return sr.ReadToEnd();
        }

        public static bool IsFarsi(string word)
        {
            foreach(char c1 in word.ToCharArray())
            {
                foreach (char c2 in PersianAlphabet)
                {
                    if (c1 == c2)
                        return true;
                }
            }
            return false;
        }

        public static string HarrassWord(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            foreach (char ch in Common.Separators)
            {
                if (ch != ' ')
                    text = text.Replace(ch.ToString(), " ");
            }

            foreach (string tag in Common.HtmlTags)
            {
                text = text.Replace("<" + tag + ">", " ");
                text = text.Replace("</" + tag + ">", " ");

            }
            text = text.Replace("  ", " ");
            return text;
        }
    }

    public class Logger
    {
        public static void Log(string message)
        {
            if (!Directory.Exists(Common.LogDirectory))
                Directory.CreateDirectory(Common.LogDirectory);
            var now = DateTime.Now;
            string todayLog = Common.LogDirectory+now.Year + "_" + now.Month + "_" + now.Day + ".txt";
            using (StreamWriter sw = new StreamWriter(todayLog, true))
            {
                sw.WriteLine(now);
                sw.WriteLine(message);
                sw.WriteLine("===============================================================================");
            }
        }
    }

}
