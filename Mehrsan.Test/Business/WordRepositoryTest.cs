using System;
using Mehrsan.Business;
using Mehrsan.Business.Interface;
using Mehrsan.Common;
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
        private static string _userId = "7d8d23dd-2983-4ae0-8507-87a17e12bb9a";
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

        [TestMethod]
        public void GetChartData()
        {
            var chartDataList = _wordRepository.GetChartData();
            Assert.AreEqual(chartDataList[0].X, 0);
            Assert.IsTrue(chartDataList[0].Y > 1);
            Assert.IsTrue(chartDataList[0].Total > 1);
        }

        [TestMethod]
        public void CreateDefaultWord()
        {
            TestModel.Instance.Initialise();
            _wordRepository.CreateDefaultWord(TestModel.Instance.SampleWord);
            var word = TestModel.Instance.SampleWord;
            Assert.IsTrue( word.Id > 0);
            Assert.IsTrue(( word.NextReviewDate - DateTime.Now).TotalDays <= 1  );
            Assert.AreEqual( word.TargetLanguageId , (long) Languages.Danish);
            Assert.AreEqual( word.MeaningLanguageId, (long) Languages.English);

            _wordRepository = new WordRepository();//Checking that the history is created
            var histories = _wordRepository.GetHistories( word.Id, DateTime.MinValue);
            Assert.AreEqual(histories.Count, 1);

        }

        [TestMethod]
        public void UpdateNofSpaces()
        {
            _wordRepository.UpdateNofSpaces();
            _wordRepository = new WordRepository();//Checking that the history is created
            var words = _wordRepository.GetWords(23, string.Empty);
            Assert.AreEqual(words[0].NofSpace, (short)5);

        }

        [TestMethod]
        public void GetAllWords()
        {
            var words = _wordRepository.GetAllWords(_userId,string .Empty);
            Assert.AreEqual(words.Count , 0);
            _wordRepository = new WordRepository();//Checking that the history is created
            var searchKey = "læg";
            words = _wordRepository.GetAllWords(_userId, searchKey);
            foreach(var word in words)
            {
                Assert.IsTrue(word.TargetWord.Contains(searchKey));
            }
        }

        [TestMethod]
        public void GetWordByTargetWord()
        {
            var searchKey = "læg";
            var word = _wordRepository.GetWordByTargetWord(searchKey);            
            Assert.IsTrue(word.TargetWord.Contains(searchKey));

        }

        [TestMethod]
        public void DeleteWord()
        {
            var word = _wordRepository.CreateDefaultWord(TestModel.Instance.SampleWord);
            _wordRepository = new WordRepository();
            var wordId = TestModel.Instance.SampleWord.Id;
            bool result = _wordRepository.WordExists(wordId);
            Assert.IsTrue(result);
            _wordRepository = new WordRepository();
            result = _wordRepository.DeleteWord(wordId);
            _wordRepository = new WordRepository();
            result = _wordRepository.WordExists(wordId);
            Assert.IsFalse(result);

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
