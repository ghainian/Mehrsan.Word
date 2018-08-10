using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Mehrsan.Common
{
    public class Common
    {


        public static ConfigurationRoot Configuration { get; set; }


        public static void Initialise()
        {
            MaxReviewDate = 36000;
            DefaultReviewTimeSpan = 10000;
            MaxImagePerWord = int.Parse(Configuration["MaxImagePerWord"].ToString());
            AgentPath = Configuration["AgentPath"].ToString();
            BackupDir = Configuration["BackupDir"].ToString();
            VideoDirectory = Configuration["VideoDirectory"].ToString();
            DownloadGoogleImageWaitTime = int.Parse(Configuration["DownloadGoogleImageWaitTime"].ToString());
            NofRelatedSentences = int.Parse(Configuration["NofRelatedSentences"].ToString());
            LogDirectory = Configuration["LogDirectory"].ToString();
            HtmlTags = new string[] { "i", "b", "h1", "h2", "h3", "h4" };
            Separators = new char[] { '\'', '_', '>', '<', '\r', '\n', '\t', '?', '!', '#', '$', '€', '£', '=', ' ', '.', ')', '(', '-', '/', '\"', '@', ':', ';', ',', (char)(8203) };
            PersianAlphabet = new char[] { 'ا', 'ب', 'پ', 'ت', 'ث', 'ج', 'ح', 'خ', 'ه', 'ع', 'غ', 'ف', 'ق', 'ص', 'ض', 'ش', 'س', 'ی', 'ل', 'ن', 'م', 'ک', 'گ', 'ظ', 'ط', 'ز', 'ر', 'ذ', 'د', 'ئ', 'و' };

        }
        public static int MaxReviewDate = 36000;
        public static long DefaultReviewTimeSpan = 10000;
        public static int MaxImagePerWord { get; set; } = 0;
        public static string AgentPath { get; set; } = string.Empty;
        public static string BackupDir { get; set; } = string.Empty;
        public static string VideoDirectory { get; set; } = string.Empty;
        public static int DownloadGoogleImageWaitTime { get; set; } = 0;
        public static int NofRelatedSentences { get; set; } = 0;
        public static string LogDirectory { get; set; } = string.Empty;
        public static string[] HtmlTags { get; set; }
        public static char[] Separators { get; set; }
        public static char[] PersianAlphabet { get; set; }

        public static string ReadFile(string file)
        {
            using (StreamReader sr = new StreamReader(file))
                return sr.ReadToEnd();
        }

        public static bool IsFarsi(string word)
        {
            foreach (char c1 in word.ToCharArray())
            {
                foreach (char c2 in PersianAlphabet)
                {
                    if (c1 == c2)
                        return true;
                }
            }
            return false;
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
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
