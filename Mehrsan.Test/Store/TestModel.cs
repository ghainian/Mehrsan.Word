using Mehrsan.Dal.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrsan.Test.Store
{
    internal sealed class TestModel
    {
        #region Fields

        Word newWord = new Word();
        History history = new History();
        AspNetUserClaim testClaim = new AspNetUserClaim();
        public static TestModel Instance { get; } = new TestModel();

        #endregion

        #region Properties

        public Word SampleWord { get; set; }

        public History SampleHistory { get; set; }

        public AspNetUserClaim SampleClaim { get; set; }

        #endregion

        #region Methods

        private TestModel()
        {

        }
        
        public void Initialise()
        {

            newWord = new Word();
            history = new History();
            testClaim = new AspNetUserClaim();

            newWord.IsAmbiguous = true;
            newWord.Meaning = Guid.NewGuid().ToString();
            newWord.TargetWord = Guid.NewGuid().ToString();
            newWord.WrittenByMe = true;
            newWord.StartTime = new TimeSpan(100);
            newWord.EndTime = new TimeSpan(200);
            newWord.NextReviewDate = DateTime.Now.AddDays(100);
            SampleWord = newWord;

            history.Result = true;
            history.ReviewPeriod = 1;
            history.ReviewTime = DateTime.Now;
            history.ReviewTimeSpan = 100000;
            history.UpdatedMeaning = "this is a test meaning";
            history.UpdatedWord = "this is the updated word";
            SampleHistory = history;

            testClaim.UserId = "2d09dd56-db9f-45e9-9b63-c6c6b79201eb";
            testClaim.ClaimType = "UserType";
            testClaim.ClaimValue = "Admin";
            SampleClaim = testClaim;
        }
        #endregion
    }
}
