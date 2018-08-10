using System;
using System.Threading.Tasks;
using Mehrsan.Dal.DB;

namespace Mehrsan.Business.Interface
{
    public interface IWordApis
    {
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
    }
}