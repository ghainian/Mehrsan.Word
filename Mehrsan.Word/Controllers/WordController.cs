using System;
using System.Collections.Generic;



using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Mehrsan.Word;
using System.Web.Mvc;
using Mehrsan.Word.Utility.FileUploader;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using Mehrsan.Dal.DB;
using Mehrsan.Business;
using Mehrsan.Word.Models;

namespace Mehrsan.Word.Controllers
{
    //[System.Web.Http.Authorize]
    public class WordController : ApiController, IWordController
    {
        
        

        private string StorageRoot
        {
            get { return Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Files")); }
        }

        
        

        // GET: api/Word/GetWords
        public List<Mehrsan.Dal.DB.Word> GetWordsForReview()
        {
            AccountController ac = new AccountController();
            UserInfoViewModel userInfo = ac.GetUserInfo();

            return WordManager.GetWordsForReview(userInfo.UserId);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<ChartData> GetReviewHistory()
        {
            //.Where(x => x.X < 100 && x.X > 0)
            List<ChartData> result = WordManager.GetChartData().ToList();

            
            return result;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<Dal.DB.Word> GetAllWords(string containText)
        {
            AccountController ac = new AccountController();
            UserInfoViewModel userInfo = ac.GetUserInfo();

            return WordManager.GetAllWords(userInfo.UserId , containText);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
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

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public bool CreateGraph()
        {
            return WordManager.CreateGraph();
        }

        

        // GET: api/Word/5
        [ResponseType(typeof(Mehrsan.Dal.DB.Word))]
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
        [ResponseType(typeof(Mehrsan.Dal.DB.Word))]
        public bool PostWord(Mehrsan.Dal.DB.Word word)
        {

            if (!ModelState.IsValid)
            {
                return false;
            }
            AccountController ac = new AccountController();
            UserInfoViewModel userInfo = ac.GetUserInfo();
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
        [ResponseType(typeof(Mehrsan.Dal.DB.Word))]
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

        [System.Web.Http.AcceptVerbs("GET", "POST")]
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

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public  async Task GetWordsInfoFromOrdNet()
        {
            WordManager.GetWordsRelatedInfo();
        }

      
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public bool UpdateWordStatus(bool knowsWord,long wordId,long reviewTime )
        {

            
            return WordManager.UpdateWordStatus(knowsWord, wordId , reviewTime);
        }


        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [System.Web.Mvc.HttpPost]
        public async Task ImportWords()
        {


            var requestStream = await Request.Content.ReadAsMultipartAsync();


            HttpContent fileContent = requestStream.Contents.SingleOrDefault(c => c.Headers.ContentType != null);

            byte[] fileBytes = await fileContent.ReadAsByteArrayAsync();
            Guid g = Guid.NewGuid();
            DateTime time = DateTime.Now;
            string fileName = g.ToString() + time.Hour + "_" + time.Minute + "_" + time.Second;
            string outputPath = @"D:\" + fileName + ".txt";
            using (FileStream fs = File.OpenWrite(outputPath))
            {
                fs.Write(fileBytes, 0, (int)fileContent.Headers.ContentLength.Value);
            }

            using (StreamReader reader = new StreamReader(outputPath))
            {
                string content = reader.ReadToEnd();
                List<string> words = new List<string>();
                var lastIndex = -1;
                string wordSeparator = "secondSeparator";
                string partSeparator = "firstSeparator";
                do
                {
                    lastIndex = content.IndexOf(wordSeparator);
                    if (lastIndex != -1)
                    {
                        string word = content.Substring(0, lastIndex);
                        words.Add(word.Trim());
                        content = content.Substring(word.Length + wordSeparator.Length);
                    }
                } while (lastIndex != -1);

                foreach (string word in words)
                {
                    int targetWordIndex = word.IndexOf(partSeparator);
                    string targetWord = word.Substring(0, targetWordIndex);
                    int meaningStartIndex = targetWordIndex + partSeparator.Length;
                    string meaning = word.Substring(meaningStartIndex);
                    Mehrsan.Dal.DB.Word newWord = new Mehrsan.Dal.DB.Word() { TargetWord = targetWord, Meaning = meaning };
                    PostWord(newWord);
                }
            }

            return;
        }

        

        // DELETE: api/Word/5
        [ResponseType(typeof(Mehrsan.Dal.DB.Word))]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IHttpActionResult DeleteWord(long id)
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