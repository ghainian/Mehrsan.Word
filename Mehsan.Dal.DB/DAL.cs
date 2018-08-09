using Mehrsan.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mehrsan.Dal.DB
{
    public sealed class DAL : IDAL
    {
        #region Fields

        private static readonly IDAL _instance;
        private static WordEntities _dbContext;

        #endregion

        #region Properties



        public static IDAL Instance { get { return _instance; } }

        public WordEntities DbContext { get => _dbContext; set => _dbContext = value; }

        #endregion

        #region Methods

        public WordEntities NewWordEntitiesInstance()
        {
            DbContext = new WordEntities(WordEntities.Options);
            return DbContext;

        }

        static DAL()
        {
            _instance = new DAL();
        }

        private DAL()
        {

        }

        public bool DeleteWord(long id)
        {

            Word word = DbContext.Words.Find(id);
            if (word == null)
            {
                return false;
            }

            var relatedHistories = (from h in DbContext.Histories where h.WordId == id select h).ToList();
            foreach (History history in relatedHistories)
            {
                DbContext.Histories.Remove(history);
            }


            DbContext.Words.Remove(word);

            DbContext.SaveChanges();

            return true;

        }
        public bool WordExists(long id)
        {
            return DbContext.Words.Count(e => e.Id == id) > 0;
        }

        public List<Word> LoadRelatedSentences(long wordId)
        {
            return new List<Word>();

            //                List<Word> words = (from w in DbContext.Words
            //                                    join
            //g in DbContext.Graphs on w.Id equals g.SrcWordId
            //                                    join
            //dstWord in DbContext.Words on g.DstWordId equals dstWord.Id
            //                                    where w.Id == wordId
            //                                    orderby dstWord.NextReviewDate, dstWord.Id ascending
            //                                    select dstWord).Take(Common.Common.NofRelatedSentences).ToList();

            //                List<Word> newWords = new List<Word>();
            //                foreach (Word w in words)
            //                {
            //                    Word newWord = new Word()
            //                    {
            //                        Id = w.Id,
            //                        TargetWord = w.TargetWord,
            //                        Meaning = w.Meaning,
            //                        Graphs = null,
            //                        Graphs1 = null,

            //                    };
            //                    newWords.Add(newWord);
            //                }
            //                return newWords;


        }

        public List<History> GetHistories(long wordId, DateTime reviewTime)
        {


            IQueryable<History> q = (from h in DbContext.Histories select h);

            if (wordId != 0)
                q = (from item in q where item.WordId == wordId select item);

            if (reviewTime != DateTime.MinValue)
                q = (from item in q where item.ReviewTime == reviewTime select item);

            return q.ToList();

        }
        public void MergeRepetitiveWords()
        {



            IQueryable<object> q =
                 (from w in DbContext.Words
                  group w.Id by w.TargetWord into g
                  where g.ToList().Count > 1
                  select new Item() { TargetWord = g.Key, RepetitiveWords = g.ToList() });

            if (q.Any())
            {
                var result = q.ToList();
                foreach (Item item in result)
                {
                    long minId = item.RepetitiveWords.Min();
                    Word wordWithMinId = null;
                    List<Word> repetitiveWords = new List<Word>();
                    string newMeaning = string.Empty;
                    foreach (long id in item.RepetitiveWords)
                    {
                        Word word = GetWords(id, string.Empty)[0];

                        newMeaning += word.Meaning.Trim(Common.Common.Separators) + " ,";

                        if (minId == id)
                            wordWithMinId = word;
                        else
                            DeleteWord(word.Id);
                    }

                    wordWithMinId.Meaning = newMeaning;
                    UpdateWord(wordWithMinId.Id, wordWithMinId.TargetWord, wordWithMinId.Meaning, null, null,
                        0, null, null, null);
                }



            }

        }

        public void UpdateNofSpaces()
        {
            var words = GetAllWords(string.Empty, string.Empty).OrderBy(x => x.NofSpace).ToList();
            //var words = GetAllWords(string.Empty).Where(x => x.TargetWord.Contains(" i i ") ).ToList();
            using (StreamWriter sw = new StreamWriter(@"D:\1.txt"))
            {
                foreach (Word word in words)
                {
                    word.TargetWord = Common.Common.HarrassWord(word.TargetWord);
                    word.Meaning = Common.Common.HarrassWord(word.Meaning);

                    //if (word.TargetWord.Contains(" i i ") &&
                    //    word.Meaning.Contains(" i i ")
                    //    )
                    //{
                    //    //word.TargetWord = word.TargetWord.Substring(1);
                    //    //word.TargetWord = word.TargetWord.Substring(0, word.TargetWord.Length -1);

                    //    //word.Meaning = word.Meaning.Substring(1);
                    //    //word.Meaning = word.Meaning.Substring(0, word.Meaning.Length - 1);

                    //    //if (word.TargetWord.Contains("i   i"))
                    //    //{
                    //        word.TargetWord = word.TargetWord.Replace(" i i ", " ");
                    //        word.Meaning = word.Meaning.Replace("i   i", " ");
                    //    //}
                    //}

                    string tt = word.TargetWord;
                    string tt1 = word.Meaning;

                    if (word.Meaning.Length % 2 != 0)
                    {
                        word.Meaning = word.Meaning.Trim(Common.Common.Separators);
                    }
                    if (word.Meaning.Length % 2 == 0)
                    {
                        char[] chars = word.Meaning.ToLower().ToCharArray();
                        bool isSymetric = true;
                        for (int i = 0; i < word.Meaning.Length / 2; i++)
                        {
                            if (chars[i] != chars[chars.Length / 2 + i])
                            {
                                isSymetric = false;
                                break;
                            }
                        }

                        if (isSymetric)
                        {
                            word.Meaning = word.Meaning.Substring(0, word.Meaning.Length / 2).Trim();
                            UpdateWord(word.Id, word.TargetWord, word.Meaning, null, null, 0, null, null, null);
                            sw.WriteLine(word.Id + "\t" + word.TargetWord + "\t" + word.Meaning);
                        }
                    }
                    word.TargetWord = word.TargetWord.Trim(Common.Common.Separators);
                    int count = word.TargetWord.Split(' ').Length - 1;
                    word.NofSpace = (byte)count;
                    if (word.NofSpace < 0)
                        word.NofSpace = null;

                    UpdateWord(word.Id, word.TargetWord, word.Meaning, null, null, 0, word.NofSpace, word.WrittenByMe, null);
                }
            }
        }

        public List<Word> GetAllWords(string userId, string containText)
        {

            IQueryable<Word> query = (from w in DbContext.Words
                                      orderby w.TargetWord
                                      select w);
            if (!string.IsNullOrEmpty(containText))
            {
                query = (from word in query where word.TargetWord.Contains(containText) select word);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                query = (from word in query where word.UserId == userId select word);
            }
            return query.ToList();

        }

        public Word GetWordByTargetWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                return null;

            word = word.Trim(Common.Common.Separators);

            var wordInDb = (from w in DbContext.Words where w.TargetWord.Trim().ToLower() == word.ToLower() select w).FirstOrDefault();

            return wordInDb;

        }

        public bool AddToGraph(Word srcWord, Word dstWord)
        {

            //var graph = (from g in db.Graphs where g.SrcWordId == srcWord.Id && g.DstWordId == dstWord.Id select g).FirstOrDefault();

            //if (graph == null && srcWord.Id != dstWord.Id)
            //{
            //    Graph newGraph = new Graph() { SrcWordId = srcWord.Id, DstWordId = dstWord.Id };
            //    db.Graphs.Add(newGraph);
            //    db.SaveChanges();
            //    return true;
            //}

            return false;
        }



        //public  int SetWordAmbiguous(long wordId)
        //{
        //    using (DbContext)
        //    {
        //        Word updatedWord = DbContext.Words.Find(wordId);
        //        updatedWord.IsAmbiguous = true;
        //        DbContext.Words.Attach(updatedWord);
        //        DbContext.Entry(updatedWord).State = EntityState.Modified;
        //        return DbContext.SaveChanges();
        //    }
        //}

        public List<Graph> GetGraphs()
        {
            //using (DbContext)
            //    return DbContext.Graphs.ToList();

            return null;
        }

        public List<Word> GetWords(long id, string targetWord)
        {


            var q = (from w in DbContext.Words select w);
            Word word = null;

            if (id != 0)
                q = (from w in q where w.Id == id select w);

            if (!string.IsNullOrEmpty(targetWord))
            {
                targetWord = targetWord.ToLower().Trim();
                q = (from w in q where w.TargetWord.ToLower().Trim() == targetWord select w);
            }

            return q.ToList();

        }

        //public  long AddWordFile(WordFile wordFile)
        //{
        //    using (DbContext)
        //    {
        //        DbContext.WordFiles.Add(wordFile);
        //        return DbContext.SaveChanges();
        //    }
        //}

        public List<ChartData> GetChartData()
        {
            List<ChartData> result = new List<ChartData>();

            var resultq = (from h in DbContext.Histories
                           group h by (h.ReviewTime.Year.ToString() + "/" + h.ReviewTime.Month.ToString() + "/" + h.ReviewTime.Day.ToString()) into g
                           select new { Count = g.Count(), Date = g.Key }).ToList();

            result = (from item in resultq select new ChartData() { X = (DateTime.Now - DateTime.Parse(item.Date)).Days, Y = item.Count }).ToList();

            result = (from item in result orderby item.X ascending select item).ToList();

            return result;
        }



        public History GetLastHistory(long wordId)
        {

            return (from h in DbContext.Histories where h.WordId == wordId orderby h.Id descending select h).FirstOrDefault();

        }

        public int UpdateWord(long wordId, string word, string meaning, TimeSpan? startTime, TimeSpan? endTime, int reviewPeriod, short? nofSpace, bool? writtenByMe, bool? isAmbiguous)
        {
            if (reviewPeriod >= Common.Common.MaxReviewDate)
                reviewPeriod = Common.Common.MaxReviewDate;


            if (!string.IsNullOrEmpty(word))
            {
                var q = (from w in DbContext.Words where w.Id != wordId && w.TargetWord == word select w);
                //var list = q.ToList();
                if (q.Any())
                    return 0;
            }
            Word updatedWord = DbContext.Words.Find(wordId);
            DbContext.Words.Attach(updatedWord);

            var entry = DbContext.Entry(updatedWord);

            if (startTime != null && startTime.Value != TimeSpan.MinValue)
            {
                updatedWord.StartTime = startTime;
                entry.Property(e => e.StartTime).IsModified = true;
            }

            if (endTime != null && endTime.Value != TimeSpan.MinValue)
            {
                updatedWord.EndTime = endTime;
                entry.Property(e => e.EndTime).IsModified = true;

            }

            if (nofSpace != null)
            {
                updatedWord.NofSpace = nofSpace;
                entry.Property(e => e.NofSpace).IsModified = true;
            }

            if (reviewPeriod != 0)
            {
                if (reviewPeriod > Common.Common.MaxReviewDate)
                    reviewPeriod = Common.Common.MaxReviewDate;
                updatedWord.NextReviewDate = DateTime.Now.AddDays(reviewPeriod);
                entry.Property(e => e.NextReviewDate).IsModified = true;
            }

            if (!string.IsNullOrEmpty(word))
            {
                updatedWord.TargetWord = word;
                entry.Property(e => e.TargetWord).IsModified = true;
            }

            if (!string.IsNullOrEmpty(meaning))
            {
                updatedWord.Meaning = meaning;
                entry.Property(e => e.Meaning).IsModified = true;
            }

            if (writtenByMe != null)
            {
                updatedWord.WrittenByMe = writtenByMe;
                entry.Property(e => e.WrittenByMe).IsModified = true;
            }

            if (isAmbiguous != null)
            {
                updatedWord.IsAmbiguous = isAmbiguous;
                entry.Property(e => e.IsAmbiguous).IsModified = true;
            }
            return DbContext.SaveChanges();

        }

        public List<Word> GetWordsForReview(string userId, DateTime reviewDate, int resultCount)
        {


            return (from w in DbContext.Words
                    where true

                    //&& w.NextReviewDate <= reviewDate
                    //&& w.UserId == userId
                    //&& w.Meaning.ToLower().Contains("undefined")
                    //&& w.IsMovieSubtitle == true
                    //&& w.IsMovieSubtitle == true
                    orderby w.NextReviewDate ascending
                    select w).Take(resultCount).ToList();

        }

        public List<Word> GetWordsLike(string word)
        {
            using (DbContext)
            {
                var q = (from w in DbContext.Words
                         where w.TargetWord.ToLower().Contains(word)
                         select w);
                if (q.Any())
                    return q.ToList();
                else
                    return new List<Word>();
            }
        }


        public List<AspNetUser> GetUsers(string searchText)
        {

            using (DbContext)
            {
                var query = DbContext.AspNetUsers;

                if (!string.IsNullOrEmpty(searchText))
                {
                    var lastQuery = (from item in query where item.UserName.Contains(searchText) select item);
                    return lastQuery.ToList();
                }


                return query.ToList();
            }
        }

        public List<AspNetUserClaim> GetUserClaims(string searchText)
        {
            var query = DbContext.AspNetUserClaims;
            
            if (!string.IsNullOrEmpty(searchText))
            {
                var lastQuery = (from item in query where item.ClaimType.Contains(searchText) select item);
                return lastQuery.ToList();
            }


            return query.ToList();

        }

        #endregion
    }
}
