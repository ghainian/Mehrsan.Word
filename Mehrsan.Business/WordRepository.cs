using Mehrsan.Business.Interface;
using Mehrsan.Common;
using Mehrsan.Dal.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mehrsan.Dal.DB.Interface;

namespace Mehrsan.Business
{
    public sealed class WordRepository : IWordRepository
    {
        #region Fields

        private IWordApis _wordApisInstance = null;
        private IDAL _dalInstance;

        #endregion

        #region Properties

        public IDAL Dal { get { return _dalInstance; } } 
        public IWordApis WordApisInstance { get { return _wordApisInstance; } }

        #endregion

        #region Methods

        /// <summary>
        /// This is the repository for handling word processing requirements
        /// </summary>
        public WordRepository()
        {
            _dalInstance = new DAL();
            _wordApisInstance = new WordApis(Dal);
        }

        public WordRepository(IDAL dal)
        {
            _wordApisInstance = new WordApis(dal);
            _dalInstance = dal;
        }

        public bool WordExists(long id)
        {
            using (_dalInstance.DbContext)
            {
                return Dal.WordExists(id);
            }
        }

        

        public List<History> GetHistories(long wordId, DateTime reviewTime)
        {
            using (_dalInstance.DbContext)
            {
                return Dal.GetHistories(wordId, reviewTime);
            }
        }

        public List<ChartData> GetChartData()
        {
            using (_dalInstance.DbContext)
            {
                List<ChartData> result = Dal.GetChartData();

                return result;
            }
        }

        public bool CreateDefaultWord(Word word)
        {
            using (_dalInstance.DbContext)
            {
                word.TargetWord = word.TargetWord.Trim();
                word.NofSpace = (short)(word.TargetWord.Length - word.TargetWord.Replace(" ", "").Length);
                return WordApisInstance.CreateDefaultWord(word);
            }
        }

        public void MergeRepetitiveWords()
        {
            using (_dalInstance.DbContext)
            {
                Dal.MergeRepetitiveWords();
            }
        }

        public void UpdateNofSpaces()
        {
            using (_dalInstance.DbContext)
            {
                Dal.UpdateNofSpaces();
            }
        }

        public List<Word> GetAllWords(string userId, string containText)
        {
            using (_dalInstance.DbContext)
            {
                if (containText.Length < 1)
                    return new List<Word>();
                List<Word> words = Dal.GetAllWords(userId, containText);

                var newWords = words.Select(s => WordApisInstance.GetSerializableWord(s)).ToList();

                return newWords;
            }
        }

        /// <summary>
        /// Returns the first word in search result condidering the search key
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns>Returns the first word object in search result condidering the search key</returns>
        public Word GetWordByTargetWord(string searchKey)
        {
            using (_dalInstance.DbContext)
            {
                var result =  WordApisInstance.GetWordByTargetWord(searchKey);
                return result;
            }
        }

        public bool DeleteWord(long id)
        {
            using (_dalInstance.DbContext)
            {
                return WordApisInstance.DeleteWord(id);
            }
        }

        public List<Word> GetWords(long id, string targetWord)
        {
            using (_dalInstance.DbContext)
            {
                return WordApisInstance.GetWords(id, targetWord);
            }
        }

        public History GetLastHistory(long wordId)
        {
            using (_dalInstance.DbContext)
            {
                return WordApisInstance.GetLastHistory(wordId);
            }
        }

        public bool UpdateWordStatus(bool knowsWord, long wordId, long reviewTime)
        {
            if (reviewTime < 0)
            {
                throw new Exception($"Review time can't be negative {reviewTime}");
            }
            using (_dalInstance.DbContext)
            {
                if (wordId == 0)
                    throw new Exception("Wrong id is passed");
                Word targetWord = WordApisInstance.GetWords(wordId, string.Empty)[0];
                string[] wordParts = targetWord.TargetWord.Split(' ');
                List<Word> containingWords = new List<Word>();
                containingWords.Add(targetWord);

                foreach (string word in wordParts)
                {
                    if (string.IsNullOrEmpty(word))
                        continue;
                    List<Word> searchdWords = WordApisInstance.GetWords(0, word);
                    if (searchdWords != null && searchdWords.Count > 0)
                    {
                        if (containingWords.Where(x => x.Id == searchdWords[0].Id).ToList().Count == 0)
                            containingWords.Add(searchdWords[0]);
                    }
                }

                if (containingWords.Where(x => x.Id == wordId).ToList().Count == 0)
                    containingWords.Add(new Word() { Id = wordId });

                foreach (Word word in containingWords)
                {
                    var lastHistory = WordApisInstance.GetLastHistory(word.Id);
                    if (lastHistory == null)
                    {
                        lastHistory = new History();
                        lastHistory.ReviewPeriod = 1;
                    }
                    if (lastHistory.ReviewPeriod < -1)
                        lastHistory.ReviewPeriod = 1;
                    var reviewPeriod = knowsWord ? lastHistory.ReviewPeriod * 2 : 1;

                    word.TargetWord = Common.Common.HarrassWord(word.TargetWord);

                    if (string.IsNullOrEmpty(word.TargetWord) || string.IsNullOrEmpty(word.Meaning))
                        continue;

                    if (knowsWord || word.Id == wordId)
                    {
                        if (reviewPeriod > Common.Common.MaxReviewDate)
                            reviewPeriod = Common.Common.MaxReviewDate;

                        History history = new History()
                        {
                            WordId = word.Id,
                            Result = knowsWord,
                            ReviewTime = DateTime.Now,
                            ReviewPeriod = reviewPeriod,
                            ReviewTimeSpan = reviewTime,
                            UpdatedWord = word.TargetWord,
                            UpdatedMeaning = word.Meaning
                        };
                        new DALGeneric<History>(Dal.DbContext).Create(history);

                        int res = Dal.UpdateWord(word.Id, string.Empty, string.Empty, null, null, reviewPeriod, null, null, null);

                    }
                }
            }
            return true;
        }

        public bool AddHistory(History history)
        {
            using (_dalInstance.DbContext)
            {
                return AddHistory(history);
            }
        }

        public bool SetWordAmbiguous(long wordId)
        {
            using (_dalInstance.DbContext)
            {
                return Dal.UpdateWord(wordId, string.Empty, string.Empty, null, null, 0, null, null, true) > 0;
            }
        }

        public bool CreateWord(Word word, bool createHistory)
        {
            using (_dalInstance.DbContext)
            {
                return WordApisInstance.CreateWord(word, createHistory);
            }
        }

        public bool UpdateWord(long id, Word inpWord)
        {
            using (_dalInstance.DbContext)
            {
                var result = WordApisInstance.UpdateWord(id, inpWord);
                return result;
            }
        }
        
        public async Task GetWordsRelatedInfo()
        {
            using (_dalInstance.DbContext)
            {
                var baseUrl = "http://ordnet.dk/ddo/ordbog?query=";
                long index = 0;
                string messages = string.Empty;

                foreach (Word dbWord in Dal.GetWords(0, string.Empty))
                {
                    string targetWord = dbWord.TargetWord;
                    foreach (char ch in Common.Common.Separators)
                        targetWord = targetWord.Replace(ch, ' ').Trim(Common.Common.Separators);
                    List<string> simpleWords = new List<string>(targetWord.Split(' '));
                    //var googleImageUrl = System.Configuration.ConfigurationSettings.AppSettings["GoogleImageUrl"].ToString();

                    foreach (string simpleWord in simpleWords)
                    {
                        var trimedWord = simpleWord.Trim(Common.Common.Separators);
                        if (string.IsNullOrEmpty(trimedWord))
                            continue;


                        try
                        {
                            index++;


                            var url = baseUrl + trimedWord;
                            string targetDirectory = @"D:\Code\mehran\Mehrsan_School\Mehrsan.Word\Mehrsan.Word\Words\";
                            string wordDirectory = WordApisInstance.GetWordDirectory(trimedWord);
                            targetDirectory = targetDirectory + wordDirectory;
                            if (!Directory.Exists(targetDirectory))
                                Directory.CreateDirectory(targetDirectory);

                            string mp3File = targetDirectory + trimedWord + ".mp3";

                            WordApisInstance.SaveGoogleImagesForWord(trimedWord, targetDirectory);

                            string filePath = targetDirectory + trimedWord + ".html";
                            if (!File.Exists(filePath))
                            {
                                //string html = await SendCrossDomainCallForHtml(url);
                                //using (StreamWriter writer = new StreamWriter(filePath))
                                //{
                                //    writer.Write(html);
                                //    Thread.Sleep(100);
                                //    messages += (index + ")" + trimedWord + " created successfully") + "\n";
                                //}
                            }
                            else
                            {
                                //using (StreamReader reader = new StreamReader(filePath))
                                //{
                                //    string fileContent = reader.ReadToEnd();
                                //    string mp3Url = ExtractMp3Url(fileContent);

                                //    if (!string.IsNullOrEmpty(mp3Url))
                                //    {
                                //        if (File.Exists(mp3File))
                                //        {
                                //            // messages += (index + ")" + trimedWord + " mp3 file already exists") + "\n";
                                //        }
                                //        else
                                //        {
                                //            await SendCrossDomainCallForBinaryFile(mp3Url, mp3File);
                                //            //messages += (index + ")" + trimedWord + " mp3 file downloaded successfully") + "\n";
                                //            // Thread.Sleep(150);
                                //        }

                                //    }
                                //    else
                                //    {
                                //        // messages += (index + ")" + trimedWord + " has no mp3 file ") + "\n";
                                //    }
                                //}
                            }
                        }
                        catch (Exception e)
                        {

                            messages += (index + ")" + trimedWord + " encountered erro") + "\n";
                        }

                    }
                    if (messages.Length > 60000)
                    {
                        //System.Diagnostics.Debug.WriteLine(messages);
                        messages = string.Empty;
                    }

                }

                System.Diagnostics.Debug.WriteLine(messages);
                messages = string.Empty;

            }
        }

        public bool CreateGraph()
        {
            using (_dalInstance.DbContext)
            {
                List<Word> allWords = null;

                allWords = (from d in Dal.GetWords(0, string.Empty) orderby d.Id select d).ToList();
                long maxSrcGraphId = 0;
                var graphs = Dal.GetGraphs();
                //if (graphs.Count > 0)
                //    maxSrcGraphId = (from g in graphs orderby g.Id select g.SrcWordId).Max(s => s);


                var srcWords = (from d in allWords orderby d.Id where d.Id > maxSrcGraphId select d).ToList();
                foreach (Word srcWord in srcWords)
                {
                    string srcWordNew = srcWord.TargetWord.ToLower().Trim();
                    var splittedNewWord = srcWordNew.Split(Common.Common.Separators);
                    List<Word> wordsUseNewWord = new List<Word>();
                    List<Word> wordsUsedByNewWord = new List<Word>();

                    foreach (string word in splittedNewWord)
                    {
                        wordsUsedByNewWord = Dal.GetWordsLike(word);
                        foreach (Word wordUsedByNewWord in wordsUsedByNewWord)
                        {
                            if (WordApisInstance.WholeWordIsUsed(srcWord, wordUsedByNewWord))
                            {
                                Dal.AddToGraph(srcWord, wordUsedByNewWord);
                            }
                        }
                    }

                    wordsUseNewWord.AddRange(Dal.GetWordsLike(srcWordNew));

                    foreach (Word wordUseNewWord in wordsUseNewWord)
                    {
                        if (WordApisInstance.WholeWordIsUsed(wordUseNewWord, srcWord))
                        {
                            Dal.AddToGraph(wordUseNewWord, srcWord);
                        }
                    }
                }

                return true;
            }
        }

        public List<Word> LoadRelatedSentences(long wordId)
        {
            using (_dalInstance.DbContext)
            {
                return Dal.LoadRelatedSentences(wordId);
            }
        }

        public List<Word> GetWordsForReview(string userId)
        {
            using (_dalInstance.DbContext)
            {
                List<Word> words = Dal.GetWordsForReview(userId, DateTime.Now, Common.Common.NofWordsForPreview);
                var newWords = words.Select(s => WordApisInstance.GetSerializableWord(s)).ToList();
                return newWords;
            }
        }

        #endregion
    }
}
