using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mehrsan.Dal.DB;

namespace Mehrsan.Business.Interface
{
    public interface IWordApis
    {

        #region Properties
        IDAL DalInstance { get; }
        #endregion

        #region Methods
        Word GetSerializableWord(Word word);
        string GetWordDirectory(string simpleWord);
        void InsertSubtitles(string movieName);
        void InsertSubtitlesByTimeConsideration(string movieName, TimeSpan timeSpan);
        void InsertSubtitlesWidoutMeaning(string movieName);
        void RemoveSpecialCharsFromWords();
        void SaveGoogleImagesForWord(string trimedWord, string targetDirectory);
        Task<string> SendCrossDomainCallForBinaryFile(string url, string saveFilePath);
        Task<string> SendCrossDomainCallForHtml(string url);
        void UpdateSubtitlesInfo(string movieName);
        bool WholeWordIsUsed(Word wordUsedByMainWord, Word mainWord);
        Word GetWordByTargetWord(string word);
        bool DeleteWord(long id);
        List<Word> GetWords(long id, string targetWord);
        bool UpdateWord(long id, Word inpWord);
        bool CreateWord(Word word, bool createHistory); 
        #endregion
    }
}