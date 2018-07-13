using Mehrsan.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrsan.Dal.DB
{
    public class DAL
    {
        public static WordEntities Instance { get; set; }


        //public static WordEntities Entity { get; set; } = new WordEntities();
        

        public static bool DeleteWord(long id)
        {
            using (var entity = Instance)
            {
                Word word = entity.Words.Find(id);
                if (word == null)
                {
                    return false;
                }

                var relatedHistories = (from h in entity.Histories where h.WordId == id select h).ToList();
                foreach (History history in relatedHistories)
                {
                    entity.Histories.Remove(history);
                }

                //var relatedGraphRecords = (from g in entity.Graphs where g.SrcWordId == id || g.DstWordId == id select g).ToList();
                //foreach (Graph graph in relatedGraphRecords)
                //{
                //    entity.Graphs.Remove(graph);
                //}



                entity.Words.Remove(word);

                entity.SaveChanges();

                return true;
            }
        }
        public static bool WordExists(long id)
        {
            using (var entity = Instance)
                return entity.Words.Count(e => e.Id == id) > 0;
        }

        public static List<Word> LoadRelatedSentences(long wordId)
        {
            return new List<Word>();
//            using (var entity = Instance)
//            {
//                List<Word> words = (from w in entity.Words
//                                    join
//g in entity.Graphs on w.Id equals g.SrcWordId
//                                    join
//dstWord in entity.Words on g.DstWordId equals dstWord.Id
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
//            }

        }

        public static List<History> GetHistories(long wordId, DateTime reviewTime)
        {
            using (var entity = Instance)
            {

                IQueryable<History> q = (from h in entity.Histories select h);

                if (wordId != 0)
                    q = (from item in q where item.WordId == wordId select item);

                if (reviewTime != DateTime.MinValue)
                    q = (from item in q where item.ReviewTime == reviewTime select item);

                return q.ToList();

            }
        }
        public static void MergeRepetitiveWords()
        {


            using (var entity = Instance)
            {

                IQueryable<object> q =
                     (from w in entity.Words
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
                        UpdateWord(wordWithMinId.Id, wordWithMinId.TargetWord, wordWithMinId.Meaning,null,null,
                            0, null, null, null);
                    }


                }
            }

        }

        public static void UpdateNofSpaces()
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

        public static List<Word> GetAllWords(string userId, string containText)
        {

            using (var entity = Instance)
            {
                IQueryable<Word> query = (from w in entity.Words
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
        }

        public static Word GetWordByTargetWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                return null;

            word = word.Trim(Common.Common.Separators);
            using (var db = Instance)
            {
                var wordInDb = (from w in db.Words where w.TargetWord.Trim().ToLower() == word.ToLower() select w).FirstOrDefault();

                return wordInDb;
            }
        }

        public static bool AddToGraph(Word srcWord, Word dstWord)
        {
            using (var db = Instance)
            {
                //var graph = (from g in db.Graphs where g.SrcWordId == srcWord.Id && g.DstWordId == dstWord.Id select g).FirstOrDefault();

                //if (graph == null && srcWord.Id != dstWord.Id)
                //{
                //    Graph newGraph = new Graph() { SrcWordId = srcWord.Id, DstWordId = dstWord.Id };
                //    db.Graphs.Add(newGraph);
                //    db.SaveChanges();
                //    return true;
                //}
            }
            return false;
        }

        public static int AddWord(Word word)
        {
            using (var entity = Instance)
            {
                entity.Words.Add(word);
                return entity.SaveChanges();
            }
        }

        //public static int SetWordAmbiguous(long wordId)
        //{
        //    using (var entity = Instance)
        //    {
        //        Word updatedWord = entity.Words.Find(wordId);
        //        updatedWord.IsAmbiguous = true;
        //        entity.Words.Attach(updatedWord);
        //        entity.Entry(updatedWord).State = EntityState.Modified;
        //        return entity.SaveChanges();
        //    }
        //}

        public static List<Graph> GetGraphs()
        {
            //using (var entity = Instance)
            //    return entity.Graphs.ToList();

            return null;
        }

        public static List<Word> GetWords(long id, string targetWord)
        {
            using (var entity = Instance)
            {

                var q = (from w in entity.Words select w);
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
        }

        //public static long AddWordFile(WordFile wordFile)
        //{
        //    using (var entity = Instance)
        //    {
        //        entity.WordFiles.Add(wordFile);
        //        return entity.SaveChanges();
        //    }
        //}

        public static List<ChartData> GetChartData()
        {
            List<ChartData> result = new List<ChartData>();
            using (var entity = Instance)
            {
                var resultq = (from h in entity.Histories
                               group h by (h.ReviewTime.Year.ToString() + "/" + h.ReviewTime.Month.ToString() + "/" + h.ReviewTime.Day.ToString()) into g
                               select new { Count = g.Count(), Date = g.Key }).ToList();

                result = (from item in resultq select new ChartData() { X = (DateTime.Now - DateTime.Parse(item.Date)).Days, Y = item.Count }).ToList();

                result = (from item in result orderby item.X ascending select item).ToList();

            }


            return result;
        }

        public static int AddHistory(History history)
        {
            long wordId = history.WordId;

            using (var entity = Instance)
            {
                entity.Histories.Add(history);
                return entity.SaveChanges();
            }
        }

        public static History GetLastHistory(long wordId)
        {
            using (var entity = Instance)
            {
                return (from h in entity.Histories where h.WordId == wordId orderby h.Id descending select h).FirstOrDefault();
            }
        }

        public static int UpdateWord(long wordId, string word, string meaning, TimeSpan? startTime, TimeSpan? endTime, int reviewPeriod, short? nofSpace, bool? writtenByMe, bool? isAmbiguous)
        {
            if (reviewPeriod >= Common.Common.MaxReviewDate)
                reviewPeriod = Common.Common.MaxReviewDate;

            using (var entity = Instance)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    var q = (from w in entity.Words where w.Id != wordId && w.TargetWord == word select w);
                    //var list = q.ToList();
                    if (q.Any())
                        return 0;
                }
                Word updatedWord = entity.Words.Find(wordId);
                entity.Words.Attach(updatedWord);

                var entry = entity.Entry(updatedWord);

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
                return entity.SaveChanges();
            }
        }

        public static List<Word> GetWordsForReview(string userId, DateTime reviewDate, int resultCount)
        {
            using (var entity = Instance)
            {

                return (from w in entity.Words
                        where true

                        //&& w.NextReviewDate <= reviewDate
                        //&& w.UserId == userId
                        //&& w.Meaning.ToLower().Contains("undefined")
                        //&& w.IsMovieSubtitle == true
                        //&& w.IsMovieSubtitle == true
                        orderby w.NextReviewDate ascending
                        select w).Take(resultCount).ToList();
            }
        }

        public static List<Word> GetWordsLike(string word)
        {
            using (var entity = Instance)
            {
                var q = (from w in entity.Words
                         where w.TargetWord.ToLower().Contains(word)
                         select w);
                if (q.Any())
                    return q.ToList();
                else
                    return new List<Word>();
            }
        }
    }
}
