using Mehrsan.Business.Interface;
using Mehrsan.Common;
using Mehrsan.Dal.DB;
using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mehrsan.Dal.DB.Interface;
using Mehrsan.Common.Interface;

namespace Mehrsan.Business
{
    public class WordApis : IWordApis
    {
        public ILogger Logger { get; }
        #region Fields

        private IDAL _dalInstance;

        #endregion
        #region Properties
        
        public IDAL DalInstance { get { return _dalInstance; } }

        #endregion


        #region Methods

        public WordApis(ILogger logger, IDAL dalInstance)
        {
            this.Logger = logger;
            _dalInstance = dalInstance;
        }

        public bool WholeWordIsUsed(Word wordUsedByMainWord, Word mainWord)
        {
            string dstWordNew = " " + wordUsedByMainWord.TargetWord.ToLower().Trim(Common.Common.Separators) + " ";

            string mainWordNew = mainWord.TargetWord.ToLower().Trim(Common.Common.Separators);
            foreach (char sep1 in Common.Common.Separators)
            {
                foreach (char sep2 in Common.Common.Separators)
                {
                    if (dstWordNew.Contains(sep1 + mainWordNew + sep2))
                    {
                        // if (DalInstance.AddToGraph(mainWord, wordUsedByMainWord))
                        return true;

                    }

                }
            }
            return false;

        }

        public Word GetSerializableWord(Word word)
        {
            return new Word()
            {
                Id = word.Id,
                TargetWord = word.TargetWord,
                IsAmbiguous = word.IsAmbiguous,
                MovieName = word.MovieName,
                IsMovieSubtitle = word.IsMovieSubtitle,
                Meaning = word.Meaning,
                StartTime = word.StartTime,
                EndTime = word.EndTime,
                WrittenByMe = word.WrittenByMe,
                NofSpace = word.NofSpace,
                TargetLanguageId = word.TargetLanguageId,
                MeaningLanguageId = word.MeaningLanguageId
                //Graphs = null,
                //Graphs1 = null,


            };
        }

        public void UpdateSubtitlesInfo(string movieName)
        {
            foreach (string file in Directory.GetFiles(Common.Common.VideoDirectory))
            {
                if (Path.GetExtension(file).ToLower().EndsWith("srt"))
                {
                    if (Path.GetFileNameWithoutExtension(file).EndsWith("da"))
                    {
                        string englishSubtitle = file;
                        string danishContent = Common.Common.ReadFile(file);


                        List<string> danishLines = new List<string>();
                        danishLines.AddRange(danishContent.Split('\n'));

                        int index = 0;
                        TimeSpan startTime = TimeSpan.MinValue;
                        TimeSpan endTime = TimeSpan.MinValue;
                        string danishSentence = string.Empty;

                        bool newSentenceDetected = false;
                        foreach (string line in danishLines)
                        {
                            string currentLine = line.Trim(Common.Common.Separators);
                            if (line.Contains("-->"))
                            {
                                newSentenceDetected = true;
                                danishSentence = string.Empty;

                                startTime = TimeSpan.Parse(currentLine.Substring(0, 12).Replace(",", "."));
                                endTime = TimeSpan.Parse(currentLine.Substring(16, 12).Replace(",", "."));

                            }
                            else if (newSentenceDetected)
                            {
                                if (string.IsNullOrEmpty(currentLine))
                                {
                                    newSentenceDetected = false;

                                    Word newWord = new Word();

                                    newWord.TargetWord = danishSentence.Trim(Common.Common.Separators);
                                    var foundWord = GetWordByTargetWord(newWord.TargetWord);

                                    if (foundWord != null && foundWord.TargetWord.ToLower().Trim() ==
                                        newWord.TargetWord.ToLower().Trim())
                                    {
                                        //newWord.Meaning = engLishSentence.Trim(Common.Common.Separators);
                                        newWord.Meaning = foundWord.Meaning;
                                        newWord.Id = foundWord.Id;
                                        newWord.NextReviewDate = foundWord.NextReviewDate;
                                        foreach (char ch in Common.Common.Separators)
                                        {
                                            newWord.TargetWord = newWord.TargetWord.Replace(ch, ' ');
                                            newWord.Meaning = newWord.Meaning.Replace(ch, ' ');
                                        }

                                        newWord.IsMovieSubtitle = true;
                                        newWord.StartTime = startTime;
                                        newWord.EndTime = endTime;
                                        newWord.MovieName = movieName;
                                        newWord.NextReviewDate = DateTime.Now.AddDays(1);
                                        newWord.WrittenByMe = false;

                                        if (newWord.TargetWord.ToLower().Contains("uanmeldt"))
                                        {

                                        }

                                        try
                                        {
                                            UpdateWord(foundWord.Id, newWord);
                                        }
                                        catch (Exception e)
                                        {
                                        }
                                    }
                                    startTime = TimeSpan.MinValue;
                                    endTime = TimeSpan.MinValue;
                                    danishSentence = string.Empty;

                                    Logger.Log("Word " + newWord.TargetWord + " from movie " + newWord.MovieName + " inserted successfully");
                                }
                                else
                                {
                                    danishSentence += " " + danishLines[index].Trim(Common.Common.Separators);
                                }
                            }
                            index++;
                        }

                    }
                }

            }

        }

        public void InsertSubtitlesWidoutMeaning(string movieName)
        {
            foreach (string file in Directory.GetFiles(Common.Common.VideoDirectory))
            {
                if (Path.GetExtension(file).ToLower().EndsWith("srt"))
                {
                    if (Path.GetFileNameWithoutExtension(file).EndsWith("da"))
                    {
                        string englishSubtitle = file;
                        string danishContent = Common.Common.ReadFile(file);


                        List<string> danishLines = new List<string>();
                        danishLines.AddRange(danishContent.Split('\n'));

                        int index = 0;
                        TimeSpan startTime = TimeSpan.MinValue;
                        TimeSpan endTime = TimeSpan.MinValue;
                        string danishSentence = string.Empty;

                        bool newSentenceDetected = false;
                        foreach (string line in danishLines)
                        {
                            string currentLine = line.Trim(Common.Common.Separators);
                            if (line.Contains("-->"))
                            {
                                newSentenceDetected = true;
                                danishSentence = string.Empty;

                                startTime = TimeSpan.Parse(currentLine.Substring(0, 12).Replace(",", "."));
                                endTime = TimeSpan.Parse(currentLine.Substring(16, 12).Replace(",", "."));

                            }
                            else if (newSentenceDetected)
                            {
                                if (string.IsNullOrEmpty(currentLine))
                                {
                                    newSentenceDetected = false;

                                    Word newWord = new Word();

                                    newWord.TargetWord = danishSentence.Trim(Common.Common.Separators);
                                    //newWord.Meaning = engLishSentence.Trim(Common.Common.Separators);
                                    newWord.Meaning = "undefined";

                                    foreach (char ch in Common.Common.Separators)
                                    {
                                        newWord.TargetWord = newWord.TargetWord.Replace(ch, ' ');
                                        newWord.Meaning = newWord.Meaning.Replace(ch, ' ');
                                    }

                                    newWord.IsMovieSubtitle = true;
                                    newWord.StartTime = startTime;
                                    newWord.EndTime = endTime;
                                    newWord.MovieName = movieName;
                                    newWord.NextReviewDate = DateTime.Now.AddDays(1);

                                    if (newWord.TargetWord.ToLower().Contains("uanmeldt"))
                                    {

                                    }

                                    try
                                    {
                                        CreateDefaultWord(newWord);
                                    }
                                    catch (Exception e)
                                    {
                                    }

                                    startTime = TimeSpan.MinValue;
                                    endTime = TimeSpan.MinValue;
                                    danishSentence = string.Empty;

                                    Logger.Log("Word " + newWord.TargetWord + " from movie " + newWord.MovieName + " inserted successfully");
                                }
                                else
                                {
                                    danishSentence += " " + danishLines[index].Trim(Common.Common.Separators);
                                }
                            }
                            index++;
                        }

                    }
                }

            }

        }

        public void InsertSubtitles(string movieName)
        {
            foreach (string file in Directory.GetFiles(Common.Common.VideoDirectory))
            {
                if (Path.GetExtension(file).ToLower().EndsWith("srt"))
                {
                    if (Path.GetFileNameWithoutExtension(file).EndsWith("en"))
                    {
                        string englishSubtitle = file;
                        string danishSubtitle = englishSubtitle.Replace("_en.srt", "_da.srt");
                        string englishContent = Common.Common.ReadFile(englishSubtitle);
                        string danishContent = Common.Common.ReadFile(danishSubtitle);

                        List<string> englishLines = new List<string>();
                        englishLines.AddRange(englishContent.Split('\n'));

                        List<string> danishLines = new List<string>();
                        danishLines.AddRange(danishContent.Split('\n'));

                        int index = 0;
                        TimeSpan startTime = TimeSpan.MinValue;
                        TimeSpan endTime = TimeSpan.MinValue;
                        string engLishSentence = string.Empty;
                        string danishSentence = string.Empty;

                        bool newSentenceDetected = false;
                        foreach (string line in englishLines)
                        {
                            string currentLine = line.Trim(Common.Common.Separators);
                            if (line.Contains("-->"))
                            {
                                newSentenceDetected = true;
                                engLishSentence = string.Empty;
                                danishSentence = string.Empty;

                                startTime = TimeSpan.Parse(currentLine.Substring(0, 12).Replace(",", "."));
                                endTime = TimeSpan.Parse(currentLine.Substring(16, 12).Replace(",", "."));

                            }
                            else if (newSentenceDetected)
                            {
                                if (string.IsNullOrEmpty(currentLine))
                                {
                                    newSentenceDetected = false;

                                    Word newWord = new Word();

                                    newWord.TargetWord = danishSentence.Trim(Common.Common.Separators);
                                    newWord.Meaning = engLishSentence.Trim(Common.Common.Separators);

                                    foreach (char ch in Common.Common.Separators)
                                    {
                                        newWord.TargetWord = newWord.TargetWord.Replace(ch, ' ');
                                        newWord.Meaning = newWord.Meaning.Replace(ch, ' ');
                                    }

                                    newWord.IsMovieSubtitle = true;
                                    newWord.StartTime = startTime;
                                    newWord.EndTime = endTime;
                                    newWord.MovieName = movieName;
                                    newWord.NextReviewDate = DateTime.Now.AddDays(1);
                                    try
                                    {
                                        CreateDefaultWord(newWord);
                                    }
                                    catch (Exception e)
                                    {
                                        var words = GetWords(0, newWord.TargetWord);
                                        newWord.Id = words[0].Id;
                                        UpdateWord(newWord.Id, newWord);
                                    }

                                    startTime = TimeSpan.MinValue;
                                    endTime = TimeSpan.MinValue;
                                    danishSentence = string.Empty;
                                    engLishSentence = string.Empty;

                                    Logger.Log("Word " + newWord.TargetWord + " from movie " + newWord.MovieName + " inserted successfully");
                                }
                                else
                                {
                                    engLishSentence += " " + englishLines[index].Trim(Common.Common.Separators);
                                    danishSentence += " " + danishLines[index].Trim(Common.Common.Separators);
                                }
                            }
                            index++;
                        }

                    }
                }

            }

        }

        public async Task<string> SendCrossDomainCallForHtml(string url)
        {
            string result = string.Empty;
            Stream stream = await GetStream(url);
            StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
            var text = readStream.ReadToEnd();
            string inputString = text;
            string asAscii = Encoding.ASCII.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.GetEncoding(
                        Encoding.ASCII.EncodingName,
                        new EncoderReplacementFallback(string.Empty),
                        new DecoderExceptionFallback()
                        ),
                    Encoding.UTF8.GetBytes(inputString)
                )
            );
            return text;
        }

        public void RemoveSpecialCharsFromWords()
        {

            foreach (Word dbWord in DalInstance.GetWords(0, string.Empty))
            {
                foreach (char ch in Common.Common.Separators)
                {
                    dbWord.TargetWord = dbWord.TargetWord.Replace(ch, ' ');
                    dbWord.Meaning = dbWord.Meaning.Replace(ch, ' ');
                }
                UpdateWord(dbWord.Id, dbWord);
            }

        }

        public void InsertSubtitlesByTimeConsideration(string movieName, TimeSpan timeSpan)
        {



            foreach (string file in Directory.GetFiles(Common.Common.VideoDirectory))
            {
                if (Path.GetExtension(file).ToLower().EndsWith("srt"))
                {
                    if (Path.GetFileNameWithoutExtension(file).EndsWith("da"))
                    {
                        string englishSubtitle = file;
                        string danishSubtitle = englishSubtitle.Replace("_en.srt", "_da.srt");
                        var danishWords = ExtractWords(danishSubtitle);
                        var englishWords = ExtractWords(englishSubtitle);
                        foreach (Word word in danishWords)
                        {
                            TimeSpan zeroTimeSpan = new TimeSpan(0, 0, 0, 0);
                            List<Word> neighbours = englishWords.Where(x =>
                           (
                               (x.StartTime - word.StartTime <= timeSpan && x.StartTime - word.StartTime >= zeroTimeSpan)
                               ||
                               (word.StartTime - x.StartTime <= timeSpan && word.StartTime - x.StartTime >= zeroTimeSpan)
                           )
                            ).ToList();
                            if (neighbours.Count > 0)
                            {
                                word.Meaning = string.Empty;
                                foreach (Word meaningWord in neighbours)
                                {
                                    word.Meaning += " " + meaningWord.TargetWord;
                                }

                                foreach (char ch in Common.Common.Separators)
                                {
                                    word.TargetWord = word.TargetWord.Replace(ch, ' ');
                                    word.Meaning = word.Meaning.Replace(ch, ' ');
                                }

                                word.IsMovieSubtitle = true;
                                word.MovieName = movieName;
                                word.NextReviewDate = DateTime.Now.AddDays(1);

                                try
                                {
                                    CreateDefaultWord(word);
                                }
                                catch (Exception ee)
                                {
                                    var words = GetWords(0, word.TargetWord);
                                    var foundWord = words[0];
                                    if (foundWord.Meaning == "undefined")
                                    {
                                        word.Id = foundWord.Id;
                                        UpdateWord(word.Id, word);
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }


        public string GetWordDirectory(string simpleWord)
        {
            string dir = string.Empty;
            var wordChars = simpleWord.ToCharArray();
            foreach (char wordChar in wordChars)
                dir += wordChar + "\\";

            return dir;
        }

        public static async void SaveGoogleImagesForWord(string trimedWord, string targetDirectory)
        {

            var googleImageUrl = "https://www.google.dk/search?site=&tbm=isch&source=hp&biw=1920&bih=919&q=ABCDEFGH&oq=ABCDEFGH&gs_l=img.3..0l10.2852.3597.0.4226.4.4.0.0.0.0.142.414.2j2.4.0....0...1.1.64.img..0.4.411.lJv85Iott1M&gws_rd=cr&ei=fZeSVr3PJsOuswGtnqawAg";
            var googleImageCustomizedUrl = googleImageUrl.Replace("ABCDEFGH", trimedWord);
            var googleImageFile = targetDirectory + trimedWord + "Image.html";
            await SaveGoogleImageFile(googleImageCustomizedUrl, googleImageFile);
            await ExtractGoogleImageFiles(trimedWord, targetDirectory, googleImageFile);

        }

        public static async Task<string> SendCrossDomainCallForBinaryFile(string url, string saveFilePath)
        {
            try
            {

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, saveFilePath);
                }

                //string result = string.Empty;
                //Stream stream = await GetStream(url);
                //var byteArr = ReadFully(stream);
                //using (FileStream fileStream = new FileStream(saveFilePath, FileMode.OpenOrCreate))
                //{
                //    fileStream.Write(byteArr, 0, byteArr.Length);
                //}

                return "it is ok";

            }
            catch (Exception e1)
            {
                return e1.Message;
            }

        }

        private void BatchImport()
        {
            using (StreamReader sr = new StreamReader(@"d:\words.txt"))
            {
                string contrnet = sr.ReadToEnd();
                string[] lines = contrnet.Split('\n');
                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                        continue;

                    try
                    {

                        string[] parts = line.Split('\t');
                        var word = new Word();
                        word.TargetWord = parts[0].Trim(Common.Common.Separators);
                        word.Meaning = parts[1].Trim(Common.Common.Separators);
                        word.NextReviewDate = DateTime.Now.AddDays(1);
                        word.TargetLanguageId = (long)Languages.Danish;
                        word.MeaningLanguageId = (long)Languages.English;
                        if (GetWordByTargetWord(word.TargetWord) == null)
                            CreateWord(word, false);
                    }
                    catch (Exception eee)
                    {

                    }
                }
            }
        }

        private List<Word> ExtractWords(string subtitleFile)
        {
            int idIndex = 1;
            List<Word> result = new List<Word>();
            string englishContent = Common.Common.ReadFile(subtitleFile);

            List<string> englishLines = new List<string>();
            englishLines.AddRange(englishContent.Split('\n'));

            List<string> danishLines = new List<string>();

            int index = 0;
            TimeSpan startTime = TimeSpan.MinValue;
            TimeSpan endTime = TimeSpan.MinValue;
            string engLishSentence = string.Empty;

            bool newSentenceDetected = false;
            foreach (string line in englishLines)
            {
                string currentLine = line.Trim(Common.Common.Separators);
                if (line.Contains("-->"))
                {
                    newSentenceDetected = true;
                    engLishSentence = string.Empty;

                    int indexOfArrow = currentLine.IndexOf("-->");
                    startTime = TimeSpan.Parse(currentLine.Substring(0, indexOfArrow).Replace(",", "."));
                    endTime = TimeSpan.Parse(currentLine.Substring(indexOfArrow + 3).Replace(",", "."));

                }
                else if (newSentenceDetected)
                {
                    if (string.IsNullOrEmpty(currentLine))
                    {
                        newSentenceDetected = false;

                        Word newWord = new Word();

                        newWord.TargetWord = engLishSentence.Trim(Common.Common.Separators);

                        foreach (char ch in Common.Common.Separators)
                        {
                            newWord.TargetWord = newWord.TargetWord.Replace(ch, ' ');
                        }

                        newWord.IsMovieSubtitle = true;
                        newWord.StartTime = startTime;
                        newWord.EndTime = endTime;
                        newWord.NextReviewDate = DateTime.Now.AddDays(1);
                        newWord.Id = idIndex;
                        result.Add(newWord);
                        idIndex++;

                        startTime = TimeSpan.MinValue;
                        endTime = TimeSpan.MinValue;
                        engLishSentence = string.Empty;

                    }
                    else
                    {
                        engLishSentence += " " + englishLines[index].Trim(Common.Common.Separators);
                    }
                }
                index++;
            }


            return result;
        }

        public bool CreateDefaultWord(Word word)
        {
            var searchedWord = GetWords(0, word.TargetWord);
            if (searchedWord == null || searchedWord.Count == 0)
            {

                word.Id = 0;
                word.TargetWord = Common.Common.HarrassWord(word.TargetWord);
                word.NextReviewDate = DateTime.Now.AddDays(1);
                word.TargetLanguageId = (long)Languages.Danish;
                word.MeaningLanguageId = (long)Languages.English;
                CreateWord(word, true);

                return true;
            }
            return false;
        }

        private void RunAgentProcess()
        {
            try
            {
                Process.Start(Common.Common.AgentPath);
            }
            catch (Exception)
            {
            }
        }

        private bool AgentIsRunning()
        {
            var processes = Process.GetProcesses();
            foreach (Process process in processes)
                if (process.ProcessName.ToLower().Contains("mehrsan.agent"))
                    return true;

            return false;
        }

        public Word GetWordByTargetWord(string word)
        {
            return DalInstance.GetWordByTargetWord(word);
        }

        public List<Word> GetWords(long id, string targetWord)
        {
            var result = DalInstance.GetWords(id, targetWord);
            result = result.Select(x => GetSerializableWord(x)).ToList();
            return result;
        }

        private async Task<Stream> GetStream(string url)
        {
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var stream = await response.Content.ReadAsStreamAsync();
            return stream;
        }

        private bool AddHistory(History history)
        {
            var dalGenericInstance = new DALGeneric<History>(DalInstance.DbContext);
            return dalGenericInstance.Create(history);
        }

        public bool CreateWord(Word word, bool createHistory)
        {
            word.TargetWord = Common.Common.HarrassWord(word.TargetWord);
            word.Meaning = Common.Common.HarrassWord(word.Meaning);

            word.TargetWord = word.TargetWord.Trim(Common.Common.Separators);
            word.Meaning = word.Meaning.Trim(Common.Common.Separators);
            var dalGenericInstance = new DALGeneric<Word>(DalInstance.DbContext);
            dalGenericInstance.Create(word);
            if (createHistory)
            {

                History history = new History()
                {
                    WordId = word.Id,
                    Result = true,
                    ReviewTime = DateTime.Now,
                    ReviewPeriod = 1,
                    ReviewTimeSpan = Common.Common.DefaultReviewTimeSpan,
                    UpdatedWord = word.TargetWord,
                    UpdatedMeaning = word.Meaning
                };
                AddHistory(history);
            }
            return true;
        }

        public bool UpdateWord(long id, Word inpWord)
        {
            if (string.IsNullOrEmpty(inpWord.TargetWord))
                return false;


            Word word = null;
            if (id > 0)
            {
                word = GetWords(id, string.Empty)[0];
            }
            if (!string.IsNullOrEmpty(inpWord.TargetWord))
                word.TargetWord = inpWord.TargetWord;

            if (!string.IsNullOrEmpty(inpWord.Meaning))
                word.Meaning = inpWord.Meaning;

            if (inpWord.WrittenByMe != null)
                word.WrittenByMe = inpWord.WrittenByMe;

            if (inpWord.TargetLanguageId != 0)
                word.TargetLanguageId = inpWord.TargetLanguageId;

            if (inpWord.MeaningLanguageId != 0)
                word.MeaningLanguageId = inpWord.MeaningLanguageId;

            if (inpWord.StartTime != null && inpWord.StartTime.Value != TimeSpan.MinValue)
                word.StartTime = inpWord.StartTime;
            if (inpWord.EndTime != null && inpWord.EndTime.Value != TimeSpan.MinValue)
                word.EndTime = inpWord.EndTime;

            int count = word.TargetWord.Split(' ').Length - 1;
            word.TargetWord = Common.Common.HarrassWord(word.TargetWord);
            word.Meaning = Common.Common.HarrassWord(word.Meaning);

            bool updateResult = DalInstance.UpdateWord(id,
                word.TargetWord.Trim(Common.Common.Separators),
                word.Meaning.Trim(Common.Common.Separators),
                word.StartTime,
                word.EndTime,
                0,
                (short?)count,
                word.WrittenByMe,
                null,
                word.TargetLanguageId,
                word.MeaningLanguageId) > 0;

            return updateResult;
        }

        private static async Task ExtractGoogleImageFiles(string trimedWord, string targetDirectory, string googleImageFile)
        {
            try
            {
                if (File.Exists(googleImageFile))
                {
                    using (StreamReader sr1 = new StreamReader(googleImageFile))
                    {
                        string imageTableText = ExtractImageTableFromGoogleImageFile(sr1);

                        var imageIndex = -1;
                        var imageCounter = 1;
                        bool downloadedNow = false;
                        do
                        {
                            var imgSrcStartAttr = "src=\"";
                            imageIndex = imageTableText.IndexOf(imgSrcStartAttr);
                            if (imageIndex > -1)
                            {
                                imageTableText = imageTableText.Substring(imageIndex + imgSrcStartAttr.Length);//Trim from first of text to the place of first src tag
                                var imgSrcEndAttr = "\"";
                                var endImageIndex = imageTableText.IndexOf(imgSrcEndAttr);
                                var imageUrl = imageTableText.Substring(0, endImageIndex);
                                var imageFile = targetDirectory + imageCounter + ".jpg";
                                if (!File.Exists(imageFile))
                                {
                                    downloadedNow = true;
                                    await SendCrossDomainCallForBinaryFile(imageUrl, imageFile);
                                }
                                imageTableText = imageTableText.Substring(endImageIndex);
                                imageCounter++;
                            }



                        } while (imageIndex != -1 && imageCounter <= Mehrsan.Common.Common.MaxImagePerWord);
                        string message = imageCounter + " Image loaded for word " + trimedWord;
                        
                    }
                }
                else
                {
                    string message = "Failed to load google image file for word " + trimedWord;
                }
            }
            catch (Exception e)
            {

            }
        }

        private static string ExtractImageTableFromGoogleImageFile(StreamReader sr1)
        {
            var fileText = sr1.ReadToEnd();
            var startTableTag = "<table class=\"images_table\"";
            var imageTableIndex = fileText.IndexOf(startTableTag);
            var imageTableText = fileText.Substring(imageTableIndex + startTableTag.Length);
            var endTableTag = "</table>";
            var endTableIndex = imageTableText.IndexOf(endTableTag);
            imageTableText = imageTableText.Substring(endTableTag.Length, endTableIndex);
            return imageTableText;
        }

        public static async Task SaveGoogleImageFile(string googleImageCustomizedUrl, string googleImageFile)
        {

            if (!File.Exists(googleImageFile))
            {
                Thread.Sleep(Mehrsan.Common.Common.DownloadGoogleImageWaitTime * 1000);

                await SendCrossDomainCallForBinaryFile(googleImageCustomizedUrl, googleImageFile);
            }
        }

        private string ExtractMp3Url(string text)
        {
            int audioIndex = text.IndexOf(".mp3\"");
            if (audioIndex > -1)
            {
                string temp = text.Substring(audioIndex - 100, 104);
                int firstOfUrlindex = temp.LastIndexOf("\"");
                string mp3Url = temp.Substring(firstOfUrlindex + 1);
                return mp3Url;
            }
            return null;
        }

        public bool DeleteWord(long id)
        {
            return DalInstance.DeleteWord(id);
        }

        public History GetLastHistory(long wordId)
        {
            return DalInstance.GetLastHistory(wordId);
        }

        #endregion
    }
}
