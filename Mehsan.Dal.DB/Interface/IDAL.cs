using System;
using System.Collections.Generic;

namespace Mehrsan.Dal.DB.Interface
{
    public interface IDAL
    {
        #region Properties
        WordEntities DbContext { get; }
        
        #endregion

        #region Methods

        bool AddToGraph(Word srcWord, Word dstWord);
        bool DeleteWord(long id);
        List<Word> GetAllWords(string userId, string containText);
        List<ChartData> GetChartData();
        List<Graph> GetGraphs();
        List<History> GetHistories(long wordId, DateTime reviewTime);
        History GetLastHistory(long wordId);
        Word GetWordByTargetWord(string searchKey);
        List<Word> GetWords(long id, string targetWord);
        List<Word> GetWordsForReview(string userId, DateTime reviewDate, int resultCount);
        List<Word> GetWordsLike(string word);
        List<AspNetUserClaim> GetUserClaims(string searchText);
        List<Word> LoadRelatedSentences(long wordId);
        void MergeRepetitiveWords();
        void UpdateNofSpaces();
        int UpdateWord(long wordId, string word, string meaning, TimeSpan? startTime, TimeSpan? endTime, int reviewPeriod, short? nofSpace, bool? writtenByMe, bool? isAmbiguous, long targetLanguageId = 0, long meaningLanguageId = 0);
        bool WordExists(long id);
        List<AspNetUser> GetUsers(string searchText);
        WordEntities NewWordEntitiesInstance();

        #endregion


    }
}