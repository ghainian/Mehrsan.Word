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

namespace Mehrsan.Core.Web.Controllers
{
    //[System.Web.Http.Authorize]
    public class WordController : Controller
    {
        

        private string StorageRoot
        {
            get {
                return string.Empty;// Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Files"));
            }
        }


        public List<Mehrsan.Dal.DB.Word> Index()
        {
            UserInfoViewModel userInfo = new UserInfoViewModel();// ac.GetUserInfo();

            return WordManager.GetWordsForReview(userInfo.UserId);
        }



        // GET: api/Word/GetWords
        public List<Mehrsan.Dal.DB.Word> GetWordsForReview()
        {
            //AccountController ac = new AccountController();
            UserInfoViewModel userInfo = new UserInfoViewModel();// ac.GetUserInfo();

            return WordManager.GetWordsForReview(userInfo.UserId);
        }

        [AcceptVerbs("GET", "POST")]
        public List<ChartData> GetReviewHistory()
        {
            //.Where(x => x.X < 100 && x.X > 0)
            List<ChartData> result = WordManager.GetChartData().ToList();

            
            return result;
        }

        [AcceptVerbs("GET", "POST")]
        public List<Dal.DB.Word> GetAllWords(string containText)
        {
            //AccountController ac = new AccountController();
            //UserInfoViewModel userInfo = ac.GetUserInfo();
            UserInfoViewModel userInfo = new UserInfoViewModel();
            return WordManager.GetAllWords(userInfo.UserId , containText);
        }

        [AcceptVerbs("GET", "POST")]
        public Mehrsan.Dal.DB.Word GetWordByTargetWord(Mehrsan.Dal.DB.Word  word)
        {
            if (word == null)
                return null;

            var foundWord= WordManager.GetWordByTargetWord(word.TargetWord);
            if(foundWord != null)
                foundWord = WordManager.GetSerializableWord(foundWord);

            return foundWord;
        }

        public List<Mehrsan.Dal.DB.Word> LoadRelatedSentences(long wordId)
        {
            return WordManager.LoadRelatedSentences(wordId);
        }

        [AcceptVerbs("GET", "POST")]
        public bool CreateGraph()
        {
            return WordManager.CreateGraph();
        }

        

        // GET: api/Word/5
        //[ResponseType(typeof(Mehrsan.Dal.DB.Word))]
        public Mehrsan.Dal.DB.Word GetWord(long id,string targetWord)
        {
            var result = WordManager.GetWords(id, targetWord).FirstOrDefault();
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

            if (!ModelState.IsValid)
            {
                return false;
            }
            //AccountController ac = new AccountController();
            //UserInfoViewModel userInfo = ac.GetUserInfo();
            UserInfoViewModel userInfo = new UserInfoViewModel();
            word.UserId = userInfo.UserId;
            if (WordManager.PostWord(word))
            {
                return true;
            }
            else
            {
                return false;
            }

            return false;
        }

        // POST: api/Word
        //[ResponseType(typeof(Mehrsan.Dal.DB.Word))]
        public bool UpdateWord(Mehrsan.Dal.DB.Word word)
        {

            if (!ModelState.IsValid)
            {
                return false;
            }

            if (WordManager.UpdateWord(word.Id, word))
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

            if (WordManager.SetWordAmbiguous(wordId))
                return true;

            return false;
        }

        [AcceptVerbs("GET", "POST")]
        public  async Task GetWordsInfoFromOrdNet()
        {
            WordManager.GetWordsRelatedInfo();
        }

      
        [AcceptVerbs("GET", "POST")]
        public bool UpdateWordStatus(bool knowsWord,long wordId,long reviewTime )
        {

            
            return WordManager.UpdateWordStatus(knowsWord, wordId , reviewTime);
        }


        // DELETE: api/Word/5
        //[ResponseType(typeof(Mehrsan.Dal.DB.Word))]
        [AcceptVerbs("GET", "POST")]
        public IActionResult DeleteWord(long id)
        {
            if (WordManager.DeleteWord(id))
            {
                return Ok();

            }

            return NotFound();

        }

        protected override void Dispose(bool disposing)
        {
            
            base.Dispose(disposing);
        }

        
    }
}