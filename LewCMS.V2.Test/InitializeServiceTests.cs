using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using LewCMS.V2.Startup;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LewCMS.V2.Contents;

namespace LewCMS.V2.Test
{
    [TestClass]
    public class InitializeServiceTests
    {
        [ClassInitialize]
        public static void InitializeTestClass(TestContext testContext)
        {
            Application.Current.SetApplicationAssembly(Assembly.GetExecutingAssembly());
        }

        [TestMethod]
        public void Intialize()
        {
            var service = this.GetService();
            IEnumerable<IPageType> pageTypes = service.GetPageTypes(Application.Current.ApplicationAssembly);
            IEnumerable<ISectionType> sectionTypes = service.GetSectionTypes(Application.Current.ApplicationAssembly);
            IEnumerable<IGlobalConfigType> globalConfigTypes = service.GetGlobalConfigTypes(Application.Current.ApplicationAssembly);

            Assert.AreEqual<int>(2, pageTypes.Count());
            Assert.AreEqual<int>(2, sectionTypes.Count());
            Assert.AreEqual<int>(2, globalConfigTypes.Count());

        }

        private IInitializeService GetService()
        {
            return new DefaultInitializeService();
        }
    }
}
