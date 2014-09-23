using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LewCMS.Core.Content;
using System.Reflection;
using LewCMS.Core;
using System.Collections.Generic;
using System.Linq;

namespace LewCMS.UnitTesting
{
    [TestClass]
    public class InitializeServiceTests
    {
        private static string FILE_PERSIST_PATH = @"C:\tolu00\Playground\LewCMS\LewCMS.UnitTesting\App_Data\LewCMS";
        //private static string FILE_PERSIST_PATH = @"C:\Users\Tobias\Documents\Visual Studio 2013\Projects\MyWebApplication\LewCMS.UnitTesting\App_Data";

        [ClassInitialize]
        public static void InitializeTestClass(TestContext testContext)
        {
            Application.Current.SetApplicationAssembly(Assembly.GetExecutingAssembly());
            ServicesTestHelper.Instance.SetInitializeService(new InitializeService());
        }

        [TestMethod]
        public void LoadPageTypes()
        {
            var service = ServicesTestHelper.Instance.InitializeService;
            List<IPageType> pageTypes = service.GetPageTypes(Application.Current.ApplicationAssembly).ToList();

            Assert.AreEqual<int>(2, pageTypes.Count);
            Assert.AreEqual<int>(3, pageTypes[0].Properties.Count);
            Assert.AreEqual<int>(2, pageTypes[1].Properties.Count);
            Assert.AreEqual<string>("66f37878-25bb-471c-9363-d15e400b6cbf", pageTypes[0].Id);
            Assert.AreEqual<string>("dd9f76ef-3e63-4a73-8170-9e84ec703b07", pageTypes[1].Id);

        }
    }
}
