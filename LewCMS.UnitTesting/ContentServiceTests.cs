using LewCMS.Core;
using LewCMS.Core.Content;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.UnitTesting
{
    [TestClass]
    public class ContentServiceTests
    {
        [ClassInitialize]
        public static void InitializeTestClass(TestContext testContext)
        {
            string pagesFolderPath = Path.Combine(ServicesTestHelper.FILE_PERSIST_PATH, "Pages");
            string pageTypesFolderPath = Path.Combine(ServicesTestHelper.FILE_PERSIST_PATH, "PageTypes");

            if (Directory.Exists(pagesFolderPath))
            {
                Directory.Delete(pagesFolderPath, true);
            }

            if (Directory.Exists(pageTypesFolderPath))
            {
                Directory.Delete(pageTypesFolderPath, true);
            }

            Application.Current.SetApplicationAssembly(Assembly.GetExecutingAssembly());
            var initializeService = new InitializeService();
            var serializer = new LewCMSJsonSerializer();
            var filePersistService = new FilePersistService(serializer, ServicesTestHelper.FILE_PERSIST_PATH);
            var contentCacheService = new ContentCacheService();
            var contentRepository = new ContentRepository(contentCacheService, filePersistService, initializeService);
            var routeManager = new RouteManager(contentRepository);
            var contentService = new ContentService(contentRepository, routeManager);

            ServicesTestHelper.Instance.SetContentService(contentService);
            ServicesTestHelper.Instance.SetContentCacheService(contentCacheService);
        }

        [TestCleanup]
        public void Cleanup()
        {
            string pagesFolderPath = Path.Combine(ServicesTestHelper.FILE_PERSIST_PATH, "Pages");
            if (Directory.Exists(pagesFolderPath))
            {
                Directory.Delete(pagesFolderPath, true);
            }

            ServicesTestHelper.Instance.ContentCacheService.ClearCache();
        }

        [TestMethod]
        public void Create_And_Save_Pages()
        {
            var service = this.GetService();
            IEnumerable<IPageType> pageTypes = service.GetPageTypes();

            Assert.AreEqual<int>(2, pageTypes.Count());

            IPage page1 = service.Create(pageTypes.First());
            service.Save(page1);
            IPage page2 = service.Create(pageTypes.First().Id);
            service.Save(page2);
            IPage page1_1 = service.Create(pageTypes.First(), page1.Id);
            service.Save(page1_1);
            IPage page1_2 = service.Create(pageTypes.First().Id, page1.Id);
            service.Save(page1_2);

            Assert.AreEqual<string>(page1.Name, page2.Name);
            Assert.AreEqual<string>(page1.Name, page1_1.Name);
            Assert.AreEqual<string>(page1.Name, page1_2.Name);
            Assert.AreEqual<string>("/myfirstpagetypepage", page1.Route);
            Assert.AreEqual<string>("/myfirstpagetypepage-1", page2.Route);
            Assert.AreEqual<string>("/myfirstpagetypepage/myfirstpagetypepage", page1_1.Route);
            Assert.AreEqual<string>("/myfirstpagetypepage/myfirstpagetypepage-1", page1_2.Route);
            Assert.AreEqual<string>(page1.Id, page1_1.ParentId);
            Assert.AreEqual<string>(page1.Id, page1_2.ParentId);
        }

        private IContentService GetService()
        {
            return ServicesTestHelper.Instance.ContentService;
        }
    }
}
