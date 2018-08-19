using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mehrsan.Core.Web.Controllers;

namespace Mehrsan.Core.Web.Test
{
    [TestClass]
    public class HomeControllerTest
    {
        
        [TestMethod]
        public void Index()
        {
            HomeController controller = new HomeController();
            var result = controller.Index();
        }
    }
}
