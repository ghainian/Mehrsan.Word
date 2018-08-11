using Mehrsan.Business;
using Mehrsan.Business.Interface;
using Mehrsan.Common;
using Mehrsan.Dal.DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mehrsan.Agent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        public IWordRepository WordRepositoryInstance { get; } = new WordRepository();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        
            //RemoveUnwantedFiles();

            GetBackup();
            //SyncMobile();

            //btnCreatGraph_Click(null, null);
            InsertSubtitles();

            UpdateNofSpaces();
            ////SetFileNames();

            CreateHistoryForWordsThatDoNotHave();
            //MergeRepetitiveWords();

            // RemoveSpecialCharsFromWords();
            //InsertSubtitles();
            while (true)
            {
                btnCreatGraph_Click(null, null);

                UpdateNofSpaces();
                btnLoadWordsFromOrdNet_Click(null, null);
                Thread.Sleep(10 * 60 * 1000);
            }

        }

        private void SyncMobile()
        {
            using (StreamReader sr = new StreamReader(@"D:\1.txt"))
            {
                var line = sr.ReadLine().Trim();
                while (!string.IsNullOrEmpty(line))
                {


                    string targetWord= line;
                    line = sr.ReadLine().Trim();

                    DateTime reviewTime = DateTime.Now;

                    try
                    {
                        reviewTime = DateTime.Parse(line.Trim());

                    }
                    catch (Exception ehhy)
                    {

                    }
                    Word word= WordRepositoryInstance.GetWordByTargetWord(targetWord);

                    if (word != null)
                    {
                        long wordId = word.Id;
                        var lastHistory = WordRepositoryInstance.GetLastHistory(word.Id);
                        List<History> histories = WordRepositoryInstance.GetHistories(wordId, reviewTime);

                        if ((histories == null || histories.Count == 0))
                        {

                            var reveiwPeriod = lastHistory == null ? 1 :  lastHistory.ReviewPeriod * 2;
                            if (reveiwPeriod < 0 || reveiwPeriod > 36000)
                                reveiwPeriod = 36000;
                            History history = new History()
                            {
                                WordId = wordId,
                                Result = true,
                                ReviewTime = reviewTime,
                                ReviewPeriod = reveiwPeriod,
                                ReviewTimeSpan = 10000,
                                UpdatedWord = word.TargetWord,
                                UpdatedMeaning = word.Meaning
                            };

                            DALGeneric<History>.Instance.Create(history);

                            var reviewPeriod = lastHistory.ReviewPeriod;
                            if (reviewPeriod > Common.Common.MaxReviewDate)
                                reviewPeriod = Common.Common.MaxReviewDate;

                            if (reviewPeriod < 0)
                                reviewPeriod = 1;

                            WordRepositoryInstance.DalInstance.UpdateWord(word.Id, string.Empty, string.Empty, null, null, reviewPeriod * 2, null, null, null);
                        }
                    }
                    try
                    {

                        line = sr.ReadLine().Trim();

                    }
                    catch (Exception ee)
                    {

                    }
                }
            }
        }

        private void RemoveUnwantedFiles()
        {
            List<string> dirs = new List<string>();
            string srcDir = @"D:\Code\mehran\Mehrsan_School\Mehrsan.Word\Mehrsan.Android.WV\Assets\Words\";
            dirs.Add(srcDir);

            do
            {
                string currentDir = dirs[0];
                dirs.RemoveAt(0);
                List<string> curDirFiles = new List<string>();
                try
                {

                    dirs.AddRange(Directory.GetDirectories(currentDir));
                    curDirFiles.AddRange(Directory.GetFiles(currentDir));
                }
                catch (Exception)
                {

                }

                foreach (string file in curDirFiles)
                {
                    string fName = System.IO.Path.GetFileName(file);
                    if (fName.EndsWith(".jpg"))
                    {
                        if (!fName.EndsWith("1.jpg") || fName.EndsWith("11.jpg"))
                        {
                            try
                            {
                                File.Delete(file);
                            }
                            catch (Exception)
                            {

                            }
                        }

                    }
                    else if (!file.EndsWith(".mp3"))
                        File.Delete(file);
                }


            } while (dirs.Count > 0);

        }

        private void CreateHistoryForWordsThatDoNotHave()
        {
            DateTime startTime = DateTime.Now;
            List<Word> words = WordRepositoryInstance.GetAllWords(string.Empty, string.Empty);
            foreach (Word word in words)
            {
                History history = WordRepositoryInstance.GetLastHistory(word.Id);
                word.TargetWord = Common.Common.HarrassWord(word.TargetWord);
                if (history == null)
                {
                    history = new History()
                    {
                        WordId = word.Id,
                        Result = true,
                        ReviewTime = DateTime.Now,
                        ReviewTimeSpan = Common.Common.DefaultReviewTimeSpan,
                        ReviewPeriod = 1,
                        UpdatedWord = word.TargetWord,
                        UpdatedMeaning = word.Meaning
                    };

                    WordRepositoryInstance.AddHistory(history);
                }
            }
            Logger.Log("CreateHistoryForWordsThatdoNotHave Finished at " + DateTime.Now.ToString("HH:mm:ss") + " after " + (DateTime.Now - startTime).ToString());

        }

        private void SetFileNames()
        {
            DateTime startTime = DateTime.Now;
            int targetIndex = 1;
            string mainFileName = "BenjaminButton";
            string changeTo = "1";
            string fileName2 = mainFileName + "(Split0)";
            string dir = @"D:\2\BenjaminButton\";
            foreach (string file in Directory.GetFiles(dir))
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Length < 20 * 1024 && file.EndsWith(".mp4"))
                    File.Delete(file);

            }

            // remove empty video files
            if (!string.IsNullOrEmpty(changeTo))
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    string srcFileName = file;
                    string dstFileName = file.Replace(mainFileName, changeTo);
                    File.Move(srcFileName, dstFileName);


                }
                mainFileName = changeTo;
            }

            for (int ind1 = 1; ;)
            {
                for (int ind2 = 0; ; ind2++)
                {

                    string srcFileName = dir + mainFileName + "(Split" + ind2 + ").ogv";
                    string dstFileName = dir + mainFileName + "_" + targetIndex + "_C.ogv";

                    if (File.Exists(srcFileName))
                    {
                        File.Move(srcFileName, dstFileName);
                        targetIndex++;
                    }
                    else
                    {
                        ind1++;
                        ind2 = 0;
                        srcFileName = dir + mainFileName + "_" + ind1 + "(Split" + ind2 + ").mp4";
                        if (!File.Exists(srcFileName))
                        {
                            Logger.Log("SetFileNames Finished at " + DateTime.Now.ToString("HH:mm:ss") + " after " + (DateTime.Now - startTime).ToString());
                            return;
                        }
                        ind2 = -1;
                    }
                }
            }
            throw new Exception("SetFileNames Encountered errors");
        }

        private void RemoveSpecialCharsFromWords()
        {
            DateTime startTime = DateTime.Now;
            WordRepositoryInstance.WordApisInstance.RemoveSpecialCharsFromWords();
            Logger.Log("RemoveSpecialCharsFromWords Finished at " + DateTime.Now.ToString("HH:mm:ss") + " after " + (DateTime.Now - startTime).ToString());

        }

        private void InsertSubtitles()
        {
            DateTime startTime = DateTime.Now;

            TimeSpan timeSpan = new TimeSpan(0, 0, 0, 1, 0);
            WordRepositoryInstance.WordApisInstance.UpdateSubtitlesInfo("Spartacus01_04");

            //WordManager.InsertSubtitlesByTimeConsideration("Zootopia", timeSpan);
            Logger.Log("MergeRepetitiveWords Finished at " + DateTime.Now.ToString("HH:mm:ss") + " after " + (DateTime.Now - startTime).ToString());
        }

        private void MergeRepetitiveWords()
        {
            DateTime startTime = DateTime.Now;
            WordRepositoryInstance.MergeRepetitiveWords();
            Logger.Log("MergeRepetitiveWords Finished at " + DateTime.Now.ToString("HH:mm:ss") + " after " + (DateTime.Now - startTime).ToString());

        }

        private void GetBackup()
        {
            DateTime startTime = DateTime.Now;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                DateTime t = DateTime.Now;
                string wordBackupFile = "Word_" + t.Year + "_" + t.Month.ToString("00") + "_" + t.Day.ToString("00") + "_" + t.Hour.ToString("00") + "_" + t.Minute.ToString("00") + "_" + t.Second.ToString("00") + ".bak";

                string backupDir = @"" + Common.Common.BackupDir;
                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);
                SqlCommand cmd = new SqlCommand("BACKUP DATABASE [Word] TO  DISK = '" + backupDir + wordBackupFile + "'", con);
                cmd.ExecuteNonQuery();
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LinkedInConnection"].ToString()))
            {
                con.Open();
                DateTime t = DateTime.Now;
                string linkedinBackupFile = "Linkedin_" + t.Year + "_" + t.Month.ToString("00") + "_" + t.Day.ToString("00") + "_" + t.Hour.ToString("00") + "_" + t.Minute.ToString("00") + "_" + t.Second.ToString("00") + ".bak";

                string backupDir = @"" + Common.Common.BackupDir;
                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);
                SqlCommand cmd = new SqlCommand("BACKUP DATABASE [Word] TO  DISK = '" + backupDir + linkedinBackupFile + "'", con);
                cmd.ExecuteNonQuery();
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["OCRConnection"].ToString()))
            {
                con.Open();
                DateTime t = DateTime.Now;
                string linkedinBackupFile = "OCR_" + t.Year + "_" + t.Month.ToString("00") + "_" + t.Day.ToString("00") + "_" + t.Hour.ToString("00") + "_" + t.Minute.ToString("00") + "_" + t.Second.ToString("00") + ".bak";

                string backupDir = @"" + Common.Common.BackupDir;
                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);
                SqlCommand cmd = new SqlCommand("BACKUP DATABASE [OCR] TO  DISK = '" + backupDir + linkedinBackupFile + "'", con);
                cmd.ExecuteNonQuery();
            }

            Logger.Log("Database backup Finished at " + DateTime.Now.ToString("HH:mm:ss") + " after " + (DateTime.Now - startTime).ToString());

        }

        private void UpdateNofSpaces()
        {
            WordRepositoryInstance.UpdateNofSpaces();
        }

        private void btnCreatGraph_Click(object sender, RoutedEventArgs e)
        {

            DateTime startTime = DateTime.Now;
            WordRepositoryInstance.CreateGraph();
            Logger.Log("Create Graph Finished at " + DateTime.Now.ToString("HH:mm:ss") + " after " + (DateTime.Now - startTime).ToString());

        }

        private async void btnLoadWordsFromOrdNet_Click(object sender, RoutedEventArgs e)
        {
            DateTime startTime = DateTime.Now;
            await WordRepositoryInstance.GetWordsRelatedInfo();
            Logger.Log("Loading words from ordnet Finished at " + DateTime.Now.ToString("HH:mm:ss") + " after " + (DateTime.Now - startTime).ToString());
        }
    }
}
