using System;
using System.Collections.Generic;

namespace Mehrsan.Dal.DB
{
    public interface IDAL
    {
        int AddHistory(History history);
        bool AddToGraph(Word srcWord, Word dstWord);
        int AddWord(Word word);
        bool DeleteWord(long id);
        List<Word> GetAllWords(string userId, string containText);
        List<ChartData> GetChartData();
        List<Graph> GetGraphs();
        List<History> GetHistories(long wordId, DateTime reviewTime);
        History GetLastHistory(long wordId);
        Word GetWordByTargetWord(string word);
        List<Word> GetWords(long id, string targetWord);
        List<Word> GetWordsForReview(string userId, DateTime reviewDate, int resultCount);
        List<Word> GetWordsLike(string word);
        List<Word> LoadRelatedSentences(long wordId);
        void MergeRepetitiveWords();
        void UpdateNofSpaces();
        int UpdateWord(long wordId, string word, string meaning, TimeSpan? startTime, TimeSpan? endTime, int reviewPeriod, short? nofSpace, bool? writtenByMe, bool? isAmbiguous);
        bool WordExists(long id);
    }
}