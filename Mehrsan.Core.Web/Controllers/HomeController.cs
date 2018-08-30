using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mehrsan.Core.Web.Models;
using Mehrsan.Business;
using System.Net;
using System.IO;
using Mehrsan.Dal.DB;
using Microsoft.Extensions.Logging;
using Mehrsan.Business.Interface;
using Microsoft.AspNetCore.Authorization;
using Mehrsan.Common;

namespace Mehrsan.Core.Web.Controllers
{

    [Authorize(Policy = "AdminUser")]
    public class HomeController : BaseController
    {
        #region Fields


        private readonly IWordRepository _wordRepository = null;

        #endregion

        #region Methods

        public HomeController(IWordEntities context, Common.Interface.ILogger logger,IWordRepository wordRepository)
            :base(logger)
        {
            try
            {

                _wordRepository = wordRepository;
            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
        }


        public IActionResult Index()
        {
            try
            {
                var words = GetWordsForReview();
                return View(words);
            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
            return null;
        }

        public IActionResult About()
        {
            try
            {
                ViewData["Message"] = "Your application description page.";

                return View();
            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
            return null;
        }

        public IActionResult Contact()
        {
            try
            {
                ViewData["Message"] = "Your contact page.";

                return View();
            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
            
            return null;

        }

        public IActionResult Privacy()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {

                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
            return null;
        }





        // GET: api/Word/GetWords
        public List<Mehrsan.Dal.DB.Word> GetWordsForReview()
        {
            try
            {
                //AccountController ac = new AccountController();
                UserInfoViewModel userInfo = new UserInfoViewModel();// ac.GetUserInfo();

                return _wordRepository.GetWordsForReview(userInfo.UserId);

            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }

            return null;
        }

        [AcceptVerbs("GET", "POST")]
        public List<ChartData> GetReviewHistory()
        {
            try
            {

                //.Where(x => x.X < 100 && x.X > 0)
                List<ChartData> result = _wordRepository.GetChartData().ToList();


                return result;
            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }

            return null;

        }

        [AcceptVerbs("GET", "POST")]
        public List<Dal.DB.Word> GetAllWords(string containText)
        {
            try
            {

                UserInfoViewModel userInfo = new UserInfoViewModel();
                return _wordRepository.GetAllWords(userInfo.UserId, containText);

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
            return null;
        }

        [AcceptVerbs("GET", "POST")]
        public Mehrsan.Dal.DB.Word GetWordByTargetWord(Mehrsan.Dal.DB.Word word)
        {
            try
            {
                if (word == null)
                    return null;

                var foundWord = _wordRepository.GetWordByTargetWord(word.TargetWord);
                if (foundWord != null)
                    foundWord = _wordRepository.WordApisInstance.GetSerializableWord(foundWord);

                return foundWord;
            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }

            return null;
        }

        public List<Mehrsan.Dal.DB.Word> LoadRelatedSentences(long wordId)
        {
            try
            {

                return _wordRepository.LoadRelatedSentences(wordId);

            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }

            return null;
        }

        [AcceptVerbs("GET", "POST")]
        public bool CreateGraph()
        {
            try
            {
                return _wordRepository.CreateGraph();
            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
            return false;
        }



        // GET: api/Word/5
        //[ResponseType(typeof(Mehrsan.Dal.DB.Word))]
        [AcceptVerbs("GET", "POST")]
        public Mehrsan.Dal.DB.Word GetWord(long id, string targetWord)
        {
            try
            {

                var result = _wordRepository.GetWords(id, targetWord).FirstOrDefault();
                return result;

            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
            return null;
        }

        public string GetMethod(string url)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream resStream = response.GetResponseStream();
                return "";

            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
            return null;
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
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
            return false;
        }

        // POST: api/Word
        //[ResponseType(typeof(Mehrsan.Dal.DB.Word))]
        public bool UpdateWord(Mehrsan.Dal.DB.Word word)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return false;
                }

                if (_wordRepository.UpdateWord(word.Id, word))
                    return true;

            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }

            return false;
        }

        [AcceptVerbs("GET", "POST")]
        public bool SetWordAmbiguous(long wordId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return false;
                }

                if (_wordRepository.SetWordAmbiguous(wordId))
                    return true;

            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }

            return false;
        }

        [AcceptVerbs("GET", "POST")]
        public async Task GetWordsInfoFromOrdNet()
        {
            try
            {

                _wordRepository.GetWordsRelatedInfo();

            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }

        }


        [AcceptVerbs("GET", "POST")]
        public bool UpdateWordStatus(bool knowsWord, long wordId, long reviewTime)
        {

            try
            {

                return _wordRepository.UpdateWordStatus(knowsWord, wordId, reviewTime);

            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
            return false;
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
            catch (Exception ex)
            {

                Logger.Log(ex);
            }

            return NotFound();

        }

        protected override void Dispose(bool disposing)
        {
            try
            {

                base.Dispose(disposing);
            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
        }

        #endregion

    }
}
