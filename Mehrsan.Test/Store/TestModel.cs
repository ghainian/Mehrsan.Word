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
        public static TestModel Instance { get; } = new TestModel();

        public Word SampleWord
        {
            get
            {
                var newWord = new Word();
                newWord.IsAmbiguous = true;
                newWord.Meaning = Guid.NewGuid().ToString();
                newWord.TargetWord = Guid.NewGuid().ToString();
                newWord.WrittenByMe = true;
                newWord.StartTime = new TimeSpan(100);
                newWord.EndTime = new TimeSpan(200);
                newWord.NextReviewDate = DateTime.Now.AddDays(100);
                return newWord;
            }

        }


        public History SampleHistory
        {
            get
            {
                var  history = new History();
                history.Result = true;
                history.ReviewPeriod = 1;
                history.ReviewTime = DateTime.Now;
                history.ReviewTimeSpan = 100000;                
                history.UpdatedMeaning = "this is a test meaning";
                history.UpdatedWord = "this is the updated word";
                return history;
            }

        }

        public AspNetUserClaim SampleClaim
        {
            get
            {
                AspNetUserClaim testClaim = new AspNetUserClaim();
                testClaim.UserId = "2d09dd56-db9f-45e9-9b63-c6c6b79201eb";
                testClaim.ClaimType = "UserType";
                testClaim.ClaimValue = "Admin";
                return testClaim;
            }

        }
        
        private TestModel()
        {

        }
    }
}
