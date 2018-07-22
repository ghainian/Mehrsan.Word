using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mehrsan.Dal.DB;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;

namespace Mehrsan.Test.Controllers
{
    /// <summary>
    /// Summary description for AccountControllerTest
    /// </summary>
    [TestClass]
    public class DALTest:Setup
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
        public void Create()
        {
            try
            {   
                AspNetUserClaim aspNetUserClaim = new AspNetUserClaim();
                aspNetUserClaim.UserId = "2d09dd56-db9f-45e9-9b63-c6c6b79201eb";
                aspNetUserClaim.ClaimType = "UserType";
                aspNetUserClaim.ClaimValue = "Admin";
                var result = DAL.Instance.CreateClaim(aspNetUserClaim);

                Assert.IsTrue(result);
            }
            catch (Exception ee)
            {
                
            }
        }
    }
}
