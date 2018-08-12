using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mehrsan.Dal.DB;
using Mehrsan.Dal.DB.Interface;

namespace Mehrsan.Business.Interface
{
    public interface IWordRepository
    {

        #region Properties
        IDAL Dal { get; }
        IWordApis WordApisInstance { get; }
        #endregion

        #region Methods
        bool AddHistory(History history);
        bool CreateDefaultWord(Word word);
        bool CreateGraph();
        bool CreateWord(Word word, bool createHistory);
        bool DeleteWord(long id);
        List<Word> GetAllWords(string userId, string containText);
        List<ChartData> GetChartData();
        List<History> GetHistories(long wordId, DateTime reviewTime);
        History GetLastHistory(long wordId);
        Word GetWordByTargetWord(string word);
        string GetWordOnly(long id);
        List<Word> GetWords(long id, string targetWord);
        List<Word> GetWordsForReview(string userId);
        Task GetWordsRelatedInfo();
        List<Word> LoadRelatedSentences(long wordId);
        void MergeRepetitiveWords();
        bool SetWordAmbiguous(long wordId);
        void UpdateNofSpaces();
        bool UpdateWord(long id, Word inpWord);
        bool UpdateWordStatus(bool knowsWord, long wordId, long reviewTime);
        bool WordExists(long id); 
        #endregion
    }
}