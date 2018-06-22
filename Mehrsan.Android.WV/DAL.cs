using Mehrsan.Android.WV.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mehrsan.Android.WV
{
    public class DAL
    {

        #region Fields
        private static SQLite.SQLiteConnection database = MainActivity.database;
        private static int reviewIndex;
        private static List<Word> reviewWords;
        #endregion endregion

        #region Methods

        public static int AddHistory(History history,bool isLocalHitory)
        {
            long wordId = history.WordId;
            history.CreatedInMobile = isLocalHitory;
            database.Insert(history);
            return 1;
        }

        public static bool UpdateWordStatus(bool knowsWord, int wordId, long reviewTime)
        {

            if (wordId == 0)
                throw new Exception("Wrong id is passed");
            Word targetWord = GetWords(wordId, string.Empty)[0];
            string[] wordParts = targetWord.TargetWord.Split(' ');
            List<Word> containingWords = new List<Word>();
            containingWords.Add(targetWord);

            foreach (string word in wordParts)
            {
                if (string.IsNullOrEmpty(word))
                    continue;
                List<Word> searchdWords = GetWords(0, word);
                if (searchdWords != null && searchdWords.Count > 0)
                {
                    if (containingWords.Where(x => x.WordId == searchdWords[0].WordId).ToList().Count == 0)
                        containingWords.Add(searchdWords[0]);
                }
            }

            if (containingWords.Where(x => x.WordId == wordId).ToList().Count == 0)
                containingWords.Add(new Word() { WordId = wordId });

            foreach (Word word in containingWords)
            {
                var lastHistory = DAL.GetLastHistory(word.WordId);
                if (lastHistory == null)
                {
                    lastHistory = new History() { WordId = word.WordId, UpdatedMeaning = word.Meaning, UpdatedWord = word.TargetWord, CreatedInMobile = true, ReviewPeriod = 1, ReviewTime = DateTime.Now, Result = true, ReviewTimeSpan = 3000 };
                }

                var reviewPeriod = knowsWord ? lastHistory.ReviewPeriod * 2 : 1;

                word.TargetWord = Common.HarrassWord(word.TargetWord);

                if (string.IsNullOrEmpty(word.TargetWord) || string.IsNullOrEmpty(word.Meaning))
                    continue;

                if (knowsWord || word.WordId == wordId)
                {
                    if (reviewPeriod > Common.MaxReviewDate)
                        reviewPeriod = Common.MaxReviewDate;

                    History history = new History()
                    {
                        WordId = word.WordId,
                        Result = knowsWord,
                        ReviewTime = DateTime.Now,
                        ReviewPeriod = reviewPeriod,
                        ReviewTimeSpan = reviewTime,
                        UpdatedWord = word.TargetWord,
                        UpdatedMeaning = word.Meaning
                    };
                    DAL.AddHistory(history,true);

                    DAL.UpdateWord(word.WordId, string.Empty, string.Empty, reviewPeriod, null, null,null);
                }
            }
            return true;
        }

        public static int UpdateWord(long wordId, string word, string meaning, int reviewPeriod, short? nofSpace, bool? writtenByMe, bool? isAmbiguous)
        {

            Word updatedWord = GetWords(wordId, string.Empty).FirstOrDefault();
            updatedWord.UpdatedInMobile = true;
            if (nofSpace != null)
            {
                updatedWord.NofSpace = nofSpace;
            }

            if (reviewPeriod != 0)
            {
                if (reviewPeriod > Common.MaxReviewDate)
                    reviewPeriod = Common.MaxReviewDate;
                updatedWord.NextReviewDate = DateTime.Now.AddDays(reviewPeriod);
            }

            if (!string.IsNullOrEmpty(word))
            {
                updatedWord.TargetWord = word;
            }

            if (!string.IsNullOrEmpty(meaning))
            {
                updatedWord.Meaning = meaning;
            }

            if (writtenByMe != null)
            {
                updatedWord.WrittenByMe = writtenByMe;
            }

            if (isAmbiguous != null)
            {
                updatedWord.IsAmbiguous = isAmbiguous;
            }

            database.Update(updatedWord);

            return 1;
        }

        public static List<History> GetMobileHistories()
        {
            var query = string.Empty;
            query = "select * from History where CreatedInMobile=1";
            List<History> histories = database.Query<History>(query);
            return histories;

        }
        private static History GetLastHistory(long wordId)
        {
            var query = string.Empty;
            query = "select * from History where WordId=" + wordId.ToString() + " ORDER BY WordId DESC LIMIT 1";
            List<History> histories = database.Query<History>(query);
            return histories.FirstOrDefault();

        }

        private static List<Word> GetWords(long id, string word)
        {
            var query = string.Empty;
            query = "select * from Word where WordId > 0 ";
            if (id != 0)
                query += " and  WordId=" + id.ToString();

            if (!string.IsNullOrEmpty(word))
                query += " and  TargetWord='" + word + "'";

            List<Word> words = database.Query<Word>(query);

            return words;
        }

        public static Word GetWordForReview()
        {
            reviewWords = DAL.GetWordsForReview();
            if (reviewWords != null && reviewWords.Count > 0)
            {
                reviewIndex++;
                if (reviewIndex >= reviewWords.Count)
                {
                    reviewWords = DAL.GetWordsForReview();
                    reviewIndex = 0;
                }
            }

            var model = reviewWords[reviewIndex];
            return model;
        }

        public static List<Word> GetWordsForReview()
        {
            List<Word> words = database.Query<Word>("select * from Word  ORDER BY NextReviewDate LIMIT 10");
            return words;
        }

        internal static int GetNofTodayHistories()
        {
            var query = string.Empty;
            var t = DateTime.Now.AddDays(-1);
            string time = t.Year.ToString()+"-"+t.Month.ToString("00")+"-"+t.Day.ToString("00");
            
            query = "select* from History ORDER BY ReviewTime DESC LIMIT 2000";

            List<History> histories = database.Query<History>(query);
            histories = histories.Where(x => x.ReviewTime > t).ToList();
            return histories.Count;
        }

        #endregion

    }
}
