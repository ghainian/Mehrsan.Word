using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mehrsan.Core.Web.Models;
using System.Net;
using System.Net.Http;
using System.IO;
using Mehrsan.Dal.DB;
using Mehrsan.Business;
using Microsoft.Extensions.Logging;
using Mehrsan.Business.Interface;
using Microsoft.AspNetCore.Authorization;

namespace Mehrsan.Core.Web.Controllers
{
    //[System.Web.Http.Authorize]
    
    public class WordController : Controller
    {
        #region Fields

        private readonly ILogger<HomeController> _logger = null;
        private readonly IWordRepository _wordRepository = null;

        #endregion

        #region Methods

        public WordController(IWordEntities context, ILogger<HomeController> logger, IWordRepository wordRepository)
        {
            _logger = logger;
            _wordRepository = wordRepository;
        }



        public IActionResult Index()
        {
            UserInfoViewModel userInfo = new UserInfoViewModel();// ac.GetUserInfo();

            return View();
        }



        // GET: api/Word/GetWords
        public List<Mehrsan.Dal.DB.Word> GetWordsForReview()
        {
            //AccountController ac = new AccountController();
            UserInfoViewModel userInfo = new UserInfoViewModel();// ac.GetUserInfo();

            return _wordRepository.GetWordsForReview(userInfo.UserId);
        }

        [AcceptVerbs("GET", "POST")]
        public List<ChartData> GetReviewHistory()
        {
            //.Where(x => x.X < 100 && x.X > 0)
            List<ChartData> result = _wordRepository.GetChartData().ToList();


            return result;
        }

        [AcceptVerbs("GET", "POST")]
        public List<Dal.DB.Word> GetAllWords(string containText)
        {
            //AccountController ac = new AccountController();
            //UserInfoViewModel userInfo = ac.GetUserInfo();
            UserInfoViewModel userInfo = new UserInfoViewModel();
            return _wordRepository.GetAllWords(userInfo.UserId, containText);
        }

        [AcceptVerbs("GET", "POST")]
        public Mehrsan.Dal.DB.Word GetWordByTargetWord(Mehrsan.Dal.DB.Word word)
        {
            if (word == null)
                return null;

            var foundWord = _wordRepository.GetWordByTargetWord(word.TargetWord);
            if (foundWord != null)
                foundWord = _wordRepository.WordApisInstance.GetSerializableWord(foundWord);

            return foundWord;
        }

        public List<Mehrsan.Dal.DB.Word> LoadRelatedSentences(long wordId)
        {
            return _wordRepository.LoadRelatedSentences(wordId);
        }

        [AcceptVerbs("GET", "POST")]
        public bool CreateGraph()
        {
            return _wordRepository.CreateGraph();
        }



        // GET: api/Word/5
        //[ResponseType(typeof(Mehrsan.Dal.DB.Word))]
        public Mehrsan.Dal.DB.Word GetWord(long id, string targetWord)
        {
            var result = _wordRepository.GetWords(id, targetWord).FirstOrDefault();
            return result;
        }

        public string GetMethod(string url)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream resStream = response.GetResponseStream();
            return "";
        }


        // POST: api/Word
        //[ResponseType(typeof(Mehrsan.Dal.DB.Word))]
        public bool PostWord(Mehrsan.Dal.DB.Word word)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return false;
                }
                //AccountController ac = new AccountController();
                //UserInfoViewModel userInfo = ac.GetUserInfo();
                UserInfoViewModel userInfo = new UserInfoViewModel();
                word.UserId = userInfo.UserId;
                if (_wordRepository.CreateDefaultWord(word))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {

                _logger.LogCritical(e.Message + e.StackTrace, e);
            }
            return false;
        }

        // POST: api/Word
        [Authorize(Policy = "AdminUser")]
        public bool UpdateWord(Mehrsan.Dal.DB.Word word)
        {

            if (!ModelState.IsValid)
            {
                return false;
            }

            if (_wordRepository.UpdateWord(word.Id, word))
                return true;

            return false;
        }

        [AcceptVerbs("GET", "POST")]
        public bool SetWordAmbiguous(long wordId)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            if (_wordRepository.SetWordAmbiguous(wordId))
                return true;

            return false;
        }

        [AcceptVerbs("GET", "POST")]
        public async Task GetWordsInfoFromOrdNet()
        {
            _wordRepository.GetWordsRelatedInfo();
        }


        [AcceptVerbs("GET", "POST")]
        public bool UpdateWordStatus(bool knowsWord, long wordId, long reviewTime)
        {


            return _wordRepository.UpdateWordStatus(knowsWord, wordId, reviewTime);
        }


        // DELETE: api/Word/5
        //[ResponseType(typeof(Mehrsan.Dal.DB.Word))]
        [AcceptVerbs("GET", "POST")]
        public IActionResult DeleteWord(long id)
        {
            try
            {

                if (_wordRepository.DeleteWord(id))
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {

                _logger.LogCritical(e.Message + e.StackTrace, e);
            }

            return NotFound();

        }

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }

        #endregion

    }
}