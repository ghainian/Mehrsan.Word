using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mehrsan.Dal.DB;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using Mehrsan.Test.Store;

namespace Mehrsan.Test.Controllers
{
    /// <summary>
    /// Summary description for AccountControllerTest
    /// </summary>
    [TestClass]
    public class DALTest : Setup
    {
        public DALTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        #endregion


        [TestMethod]
        public void DeleteWord()
        {
            try
            {
                var newWord = TestModel.Instance.SampleWord;
                DALGeneric<Word>.Instance.Create(newWord);

                var history = TestModel.Instance.SampleHistory;
                history.WordId = newWord.Id;
                DALGeneric<History>.Instance.Create(history);

                DAL.Instance.DeleteWord(newWord.Id);

                var wordExists = DALGeneric<Word>.Instance.Exists(newWord.Id);
                var histExists = DALGeneric<History>.Instance.Exists(history.Id);

                Assert.IsFalse(wordExists);
                Assert.IsFalse(histExists);

            }
            catch (Exception ee)
            {

            }
        }

        [TestMethod]
        public void Create()
        {
            try
            {
                var testClaim = TestModel.Instance.SampleClaim;
                var result = DALGeneric<AspNetUserClaim>.Instance.Create(testClaim);
                Assert.IsTrue(result);
                var loadedClaim = DALGeneric<AspNetUserClaim>.Instance.Load(testClaim.Id);
                Assert.IsTrue(loadedClaim.UserId == testClaim.UserId);
                Assert.IsTrue(loadedClaim.ClaimType == testClaim.ClaimType);
                Assert.IsTrue(loadedClaim.ClaimValue == testClaim.ClaimValue);
            }
            catch (Exception ee)
            {

            }
        }

    }
}
