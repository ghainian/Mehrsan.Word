using System;
using Mehrsan.Business;
using Mehrsan.Business.Interface;
using Mehrsan.Dal.DB;
using Mehrsan.Test.Controllers;
using Mehrsan.Test.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mehrsan.Test.Business
{
    [TestClass]
    public class WordRepositoryTest : Setup
    {

        #region Fields
        private static IWordRepository _wordRepository = null;
        private TestContext testContextInstance;
        private static long _wordId = 0;
        #endregion

        #region Properties

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
        #endregion


        [TestMethod]
        public void WordExists()
        {
            var result = _wordRepository.WordExists(_wordId);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetHistories()
        {
            var histories = _wordRepository.GetHistories(_wordId, DateTime.MinValue);
            Assert.AreEqual(histories.Count, 1);

            _wordRepository = new WordRepository();
            histories = _wordRepository.GetHistories(_wordId, histories[0].ReviewTime);
            Assert.AreEqual(histories.Count, 1, $"Checking histories of today for the word with id {_wordId}");

        }
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            
        }


        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
         public void MyTestInitialize()
        {
            TestModel.Instance.Initialise();
            _wordRepository = new WordRepository();
            _wordRepository.CreateWord(TestModel.Instance.SampleWord, true);
            _wordId = TestModel.Instance.SampleWord.Id;
            _wordRepository = new WordRepository();
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

    }
}
