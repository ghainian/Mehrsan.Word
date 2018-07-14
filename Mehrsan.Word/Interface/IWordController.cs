using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Mehrsan.Dal.DB;

namespace Mehrsan.Word.Controllers
{
    public interface IWordController
    {
        bool CreateGraph();
        IHttpActionResult DeleteWord(long id);
        List<Dal.DB.Word> GetAllWords(string containText);
        string GetMethod(string url);
        List<ChartData> GetReviewHistory();
        Dal.DB.Word GetWord(long id, string targetWord);
        Dal.DB.Word GetWordByTargetWord(Dal.DB.Word word);
        List<Dal.DB.Word> GetWordsForReview();
        Task GetWordsInfoFromOrdNet();
        Task ImportWords();
        List<Dal.DB.Word> LoadRelatedSentences(long wordId);
        bool PostWord(Dal.DB.Word word);
        bool SetWordAmbiguous(long wordId);
        bool UpdateWord(Dal.DB.Word word);
        bool UpdateWordStatus(bool knowsWord, long wordId, long reviewTime);
    }
}