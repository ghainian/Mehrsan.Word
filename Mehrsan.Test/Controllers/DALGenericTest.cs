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
    public class DALgenericTest:Setup
    {
        public DALgenericTest()
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
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Delete()
        {
            try
            {
                var newWord = TestModel.Instance.SampleWord;
                DALGeneric<Word>.Instance.Create(newWord);

                Assert.IsTrue(newWord.Id > 0);
                DALGeneric<Word>.Instance.Delete(newWord.Id);

                var wordExists = DALGeneric<Word>.Instance.Exists(newWord.Id);
                
                Assert.IsFalse(wordExists);
                
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
                var newWord = TestModel.Instance.SampleWord;
                DALGeneric<Word>.Instance.Create(newWord);

                Assert.IsTrue(newWord.Id > 0);
                DALGeneric<Word>.Instance.Delete(newWord.Id);

               
                var wordExists = DALGeneric<Word>.Instance.Exists(newWord.Id);

                Assert.IsFalse(wordExists);

            }
            catch (Exception ee)
            {

            }
        }
        [TestMethod]
        public void LoadList()
        {
            try
            {
                var testClaim = TestModel.Instance.SampleClaim;
                var result = DALGeneric<AspNetUserClaim>.Instance.Create(testClaim);
                Assert.IsTrue(result);
                var parameters = new List<string>() { nameof(testClaim.UserId), nameof(testClaim.ClaimType), nameof(testClaim.ClaimValue) };
                var values = new List<object>() { testClaim.UserId , testClaim.ClaimType, testClaim.ClaimValue };
                var loadedClaim = DALGeneric<AspNetUserClaim>.Instance.Load(parameters, values);
                Assert.IsTrue(loadedClaim[0].UserId == testClaim.UserId);
                Assert.IsTrue(loadedClaim[0].ClaimType == testClaim.ClaimType);
                Assert.IsTrue(loadedClaim[0].ClaimValue == testClaim.ClaimValue);
                DALGeneric<AspNetUserClaim>.Instance.Delete(testClaim.Id);

                var exists =DALGeneric<AspNetUserClaim>.Instance.Exists(testClaim.Id);
                Assert.IsFalse(exists);
            }
            catch (Exception ee)
            {

            }
        }
    

    }
}
