using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Mehrsan.Dal.DB;
using System.Threading.Tasks;
using System.IO;
using Mehrsan.Business;
using Mehrsan.Common;

namespace Mehrsan.Test.Controllers
{
    [TestClass]
    public class EmployeeControllerTest
    {
        #region Fields
        IWebDriver driver = null;

        private const string IE_DRIVER_PATH = @"C:\SeleniumDriver";
        private const string CHROME_DRIVER_PATH = @"C:\SeleniumDriver";
        private const string ScreenShotLocation = @"C:\ScreenShot";

        #endregion

        #region Others
        [TestMethod]
        public async Task AutoClick()
        {
            driver = new ChromeDriver(CHROME_DRIVER_PATH);

            var baseUrl = "https://www.dating.dk/start?b=1&ref=topmenu";
            driver.Navigate().GoToUrl(baseUrl);

            while (true)
            {

                try
                {

                    long index = 0;
                    string messages = string.Empty;

                    Thread.Sleep(5000);

                    IWebElement btnEnter = driver.FindElement(By.LinkText("Jeg er interesseret!"));
                    btnEnter.Click();

                }
                catch (Exception e)
                {

                }
            }
        }

        [TestMethod]
        public async Task SendMessageToProfiles()
        {
            driver = new ChromeDriver(CHROME_DRIVER_PATH);

            var baseUrl = "https://www.dating.dk/start?b=1&ref=topmenu";
            driver.Navigate().GoToUrl(baseUrl);
            List<string> profiles = new List<string>();
            string profilesPath = @"D:\temp\profiles.txt";
            if (File.Exists(profilesPath))
            {
                using (StreamReader sr = new StreamReader(profilesPath))
                {
                    string[] lines = sr.ReadToEnd().Split('\n');
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrEmpty(line))
                            profiles.Add(line);
                    }
                }
            }

            foreach (string profile in profiles)
            {
                driver.Navigate().GoToUrl(profile);
                Thread.Sleep(2000);


                try
                {

                    try
                    {
                        IWebElement btnClose = driver.FindElement(By.ClassName("close"));
                        btnClose.Click();
                        Thread.Sleep(1000);

                    }
                    catch (Exception)
                    {

                    }
                    IWebElement btnEnter = driver.FindElement(By.LinkText("Jeg er interesseret"));
                    btnEnter.Click();
                    Thread.Sleep(1000);

                    IWebElement btnSendMessage = driver.FindElement(By.Id("ContentOverride_ContentHeaderFixed_btnSendMessage"));
                    btnSendMessage.Click();
                    Thread.Sleep(1000);

                    IWebElement btnSend = driver.FindElement(By.Id("btnSendMail"));

                    var iframe = driver.FindElement(By.TagName("iframe"));
                    var txtMessage = driver.FindElement(By.TagName("iframe"));
                    driver.SwitchTo().Frame(iframe);
                    //WebElement web1 = driver.findElement(By.xpath("//div[@class='p']"));
                    //web1.clear();
                    //Actions act1 = new Actions(driver);
                    //act1.sendKeys(web1, "Hello");
                    var body = driver.FindElement(By.TagName("body"));
                    IWebElement editable = driver.SwitchTo().ActiveElement();

                    string text2 = "Hi Dear";
                    editable.SendKeys(text2);
                    editable.SendKeys(Keys.Enter);
                    editable.SendKeys(Keys.Enter);

                    text2 = "i read what you wrote on your profile and know what you are looking for.  i am not a perfect match but who knows ? !? !?? ! maybe we can be among the best of we go further.";
                    editable.SendKeys(text2);
                    editable.SendKeys(Keys.Enter);
                    editable.SendKeys(Keys.Enter);

                    text2 = "you look really beautiful and kind and this is the only thing i am looking for it.";
                    editable.SendKeys(text2);
                    editable.SendKeys(Keys.Enter);

                    text2 = "spending happy moments with a lovely friend.";
                    editable.SendKeys(text2);
                    editable.SendKeys(Keys.Enter);
                    editable.SendKeys(Keys.Enter);


                    text2 = "About me:";
                    editable.SendKeys(text2);
                    editable.SendKeys(Keys.Enter);
                    text2 = "--------------------------------- ";

                    editable.SendKeys(text2);
                    editable.SendKeys(Keys.Enter);
                    text2 = "i am a nonsmoking Persian boy, 30 years old with a master in computer science and am eager to learn danish.although i have studied danish for almost two years.";
                    editable.SendKeys(text2);
                    editable.SendKeys(Keys.Enter);
                    editable.SendKeys(Keys.Enter);

                    text2 = "(i do not have any problem with anyone of your habits)";
                    editable.SendKeys(text2);
                    editable.SendKeys(Keys.Enter);
                    editable.SendKeys(Keys.Enter);


                    text2 = "i am looking forward to hear from you.";
                    editable.SendKeys(text2);
                    editable.SendKeys(Keys.Enter);
                    editable.SendKeys(Keys.Enter);

                    text2 = "Best Regards";
                    editable.SendKeys(text2);
                    editable.SendKeys(Keys.Enter);

                    text2 = "Nirvana";
                    editable.SendKeys(text2);
                    editable.SendKeys(Keys.Enter);

                    //string innerHtml = body.GetAttribute("innerHTML"); ;
                    //string text = File.ReadAllText(@"d:\temp\msg.txt");
                    //OpenQA.Selenium.Interactions.Actions act1 = new OpenQA.Selenium.Interactions.Actions(driver);


                    Thread.Sleep(1000);

                    driver.SwitchTo().DefaultContent();
                    btnSend.Click();
                    Thread.Sleep(1000);
                    IWebElement btnPinUser = driver.FindElement(By.Id("btnPinUser"));
                    btnPinUser.Click();
                    int waitTime = DateTime.Now.Second % 10;
                    Thread.Sleep(waitTime * 500);

                }
                catch (Exception ee)
                {

                }

            }

        }

        [TestMethod, Timeout(int.MaxValue)]
        public async Task SurfLinkedinProfiles()
        {
            driver = new ChromeDriver(CHROME_DRIVER_PATH);
            string content = Common.Common.ReadFile("Profiles.txt");
            List<string> profiles = new List<string>();
            profiles.AddRange(content.Split('\n'));

            int index = 0;
            foreach (string profile in profiles)
            {
                if (string.IsNullOrEmpty(profile.Trim()))
                    continue;

                driver.Navigate().GoToUrl(profile);
                try
                {
                    Thread.Sleep(2000);
                    IWebElement signInLink = driver.FindElement(By.ClassName("sign-in-link"));
                    signInLink.Click();
                    Thread.Sleep(1000);
                    IWebElement user = driver.FindElement(By.Id("session_key-login"));
                    user.SendKeys("ghainian@gmail.com");
                    IWebElement pass = driver.FindElement(By.Id("session_password-login"));
                    pass.SendKeys("hafz852");

                    IWebElement btnLogin = driver.FindElement(By.ClassName("btn-primary"));
                    btnLogin.Click();
                }
                catch (Exception e)
                {

                }
                Random r = new Random();
                int sleepTime = r.Next() % 180;
                Thread.Sleep(8500 + sleepTime * 100);

            }




        }
        


        #endregion
        [TestMethod]
        public async Task GetWordsInfoFromSoundOfTextFromFile()
        {
            driver = new ChromeDriver(CHROME_DRIVER_PATH);
            string words = string.Empty;
            using (StreamReader sr = new StreamReader(@"D:\words.txt"))
                words = sr.ReadToEnd();

            var wordsArr = words.Split('\n');

            foreach (var targetWord in wordsArr)
            {
                if (!targetWord.Contains("�"))
                    LoadSoundForWord(targetWord, null);

            }
        }

        [TestMethod]
        public async Task GetWordsInfoFromSoundOfTextFromDB()
        {
            driver = new ChromeDriver(CHROME_DRIVER_PATH);
            string words = string.Empty;
            foreach (Word dbWord in DAL.GetWords(0, string.Empty))
            {
                //if (dbWord.IsMovieSubtitle == null || !dbWord.IsMovieSubtitle.Value)
                //    continue;

                string targetWord = dbWord.TargetWord;
                if (!targetWord.Contains("�"))
                    LoadSoundForWord(targetWord, dbWord);
                else
                {
                    string mp3File = GetMp3FilePath(dbWord.TargetWord);
                    File.Delete(mp3File);
                }

            }
        }


        private void LoadSoundForWord(string targetWord, Word word)
        {

            var baseUrl = "http://soundoftext.com/";
            long index = 0;
            string messages = string.Empty;

            foreach (char ch in Common.Common.Separators)
                targetWord = targetWord.Replace(ch, ' ').Trim(Common.Common.Separators);
            List<string> simpleWords = new List<string>(targetWord.Split(' '));
            //var googleImageUrl = System.Configuration.ConfigurationSettings.AppSettings["GoogleImageUrl"].ToString();

            if (simpleWords.Count == 1 || word == null)
            {
                foreach (string simpleWord in simpleWords)
                {
                    var trimedWord = simpleWord.Trim(Common.Common.Separators);


                    if (string.IsNullOrEmpty(simpleWord.Trim()))
                        continue;
                    try
                    {
                        index++;


                        //var url = baseUrl + trimedWord;
                        string danishMp3File = GetMp3FilePath(trimedWord);
                        string englishMp3File = Path.GetDirectoryName(danishMp3File) + "\\en" + Path.GetFileName(danishMp3File);
                        if (!File.Exists(danishMp3File))
                        {

                            try
                            {
                                DownloadMp3FileFromSoundOfText(simpleWord, danishMp3File, "Danish");
                            }
                            catch (Exception ex)
                            {
                                Logger.Log(ex.Message);
                            }
                            finally
                            {
                                //driver.Quit();
                                //driver.Dispose();
                            }
                        }

                        //if (!File.Exists(englishMp3File))
                        //{

                        //    try
                        //    {
                        //        DownloadMp3FileFromSoundOfText(simpleWord, englishMp3File, "English");
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Logger.Log(ex.Message);
                        //    }
                        //    finally
                        //    {
                        //        //driver.Quit();
                        //        //driver.Dispose();
                        //    }
                        //}
                    }
                    catch (Exception eee)
                    {

                    }
                }
            }
            else
            {
                try
                {
                    index++;



                    string danishMp3File = GetMp3FilePath(word.Id.ToString());
                    string englishMp3File = Path.GetDirectoryName(danishMp3File) + "\\en" + Path.GetFileName(danishMp3File);

                    if (!File.Exists(danishMp3File))
                    {

                        try
                        {
                            DownloadMp3FileFromSoundOfText(word.TargetWord, danishMp3File, "Danish");
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex.Message);
                        }
                        finally
                        {
                            //driver.Quit();
                            //driver.Dispose();
                        }
                    }

                    if (!File.Exists(englishMp3File))
                    {

                        try
                        {
                            if (!Common.Common.IsFarsi(word.Meaning))
                                DownloadMp3FileFromSoundOfText(word.Meaning, englishMp3File, "English");
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex.Message);
                        }
                        finally
                        {
                            //driver.Quit();
                            //driver.Dispose();
                        }
                    }
                }
                catch (Exception eee)
                {

                }
            }
        }

        private void DownloadMp3FileFromSoundOfText(string simpleWord, string mp3File, string language)
        {
            simpleWord = Common.Common.HarrassWord(simpleWord);

            driver.Navigate().GoToUrl("http://soundoftext.com/");

            Thread.Sleep(2000);
            // Find the text input element by its name
            IWebElement txtBox = null;
            try
            {
                txtBox =
                                        driver.FindElement(By.Id("input-text"));

            }
            catch (Exception)
            {
                Thread.Sleep(7000);
                txtBox =
                                        driver.FindElement(By.Id("input-text"));

            }
            txtBox.SendKeys(simpleWord);
            //txtBox.Click();

            IWebElement ddlLang = driver.FindElement(By.Name("lang"));
            SelectElement ddlSelLang = new SelectElement(ddlLang);
            ddlSelLang.SelectByText(language);

            IWebElement btnEnter = driver.FindElement(By.Id("submit-text"));
            btnEnter.Click();

            Thread.Sleep(2000);
            IWebElement btnSave = driver.FindElement(By.ClassName("save"));
            btnSave.Click();
            Thread.Sleep(2000);
            //string downloadDirectory = @"C:\Users\Acer\Downloads\";
            string downloadDirectory = @"C:\Users\mghasarouye\Downloads\";

            var dir = new DirectoryInfo(downloadDirectory);
            List<FileInfo> fileInfos = dir.GetFiles().ToList();

            List<FileInfo> mp3Files = fileInfos.Where(f => f.Name.EndsWith("mp3")).ToList();
            FileInfo lastMp3File = mp3Files.OrderByDescending(f => f.CreationTime).First();
            string downloadedFile = downloadDirectory + lastMp3File.Name;
            File.Move(downloadedFile, mp3File);
            //WordFile wordFile = new WordFile();
            //wordFile.CorrespondingText = simpleWord;
            //Language lang = new Language();
            //lang.Id = (long)(language.ToLower() == "english" ? Languages.English : Languages.Danish);
            //wordFile.Language = lang;
            //wordFile.LanguageId = lang.Id;
            //WordManager.AddWordFile(wordFile);

            //WordManager.SendCrossDomainCallForBinaryFile("http://soundoftext.com/static/sounds/da/"+simpleWord+".mp3", mp3File);

            Logger.Log(" Mp3 sound for word " + simpleWord + " downloaded successfully");
        }

        private static string GetMp3FilePath(string trimedWord)
        {
            trimedWord = trimedWord.Trim(Common.Common.Separators);

            string targetDirectory = @"D:\Code\mehran\Mehrsan_School\Mehrsan.Word\Mehrsan.Word\Words\";
            //string targetDirectory = @"D:\VisualStudioOnline\Mehrsan_School\Mehrsan.Word\Mehrsan.Word\Words\";
            string wordDirectory = WordManager.GetWordDirectory(trimedWord);
            targetDirectory = targetDirectory + wordDirectory;

            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            string mp3File = targetDirectory + trimedWord + ".mp3";
            return mp3File;
        }
    }
}
