using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LewCMS.Core.Service;
using System.Reflection;
using System.Linq;
using MyWebApplication.PageTypes;
using LewCMS.Core;
using LewCMS.Core.Serialization;
using System.Collections.Generic;

namespace LewCMS.UnitTesting
{
    [TestClass]
    public class ContentServiceTests
    {
        [ClassInitialize]
        public static void InitializeTestClass(TestContext testContext)
        {
            Application.Current.SetApplicationAssembly(Assembly.GetExecutingAssembly());
            ContentServiceTestHelper.Instance.SetInitializeService(new InitializeService());
            ContentServiceTestHelper.Instance.SetContentCacheService(new ContentCacheService());
            ContentServiceTestHelper.Instance.SetContentRepository(new ContentRepository(ContentServiceTestHelper.Instance.InitializeService, ContentServiceTestHelper.Instance.ContentCacheService, new LewCMSJsonSerializer(), @"C:\Users\Tobias\Documents\Visual Studio 2013\Projects\MyWebApplication\LewCMS.UnitTesting\App_Data"));
            ContentServiceTestHelper.Instance.SetContentService(new ContentService(ContentServiceTestHelper.Instance.ContentRepository));

            var contentService = ContentServiceTestHelper.Instance.ContentService;
            IEnumerable<IPage> allPages = contentService.GetAllPages().ToList();
            foreach (var page in allPages)
            {
                contentService.DeletePage(page.Id);
            }

        }

        [TestCleanup]
        public void CleanUpTests()
        {
            DeleteAllPages();
        }

        [TestMethod]
        public void Get_All_Page_Types()
        {
            var contentService = ContentServiceTestHelper.Instance.ContentService;
            var pageTypes = contentService.GetPageTypes();

            Assert.AreEqual(2, pageTypes.Count());
            Assert.AreEqual(3, pageTypes.First(pt => pt.DisplayName == "MyFirstPageType").Properties.Count());
            Assert.AreEqual(2, pageTypes.First(pt => pt.DisplayName == "MySecondPageType").Properties.Count());
        }

        [TestMethod]
        public void CreatePage()
        {
            var contentService = ContentServiceTestHelper.Instance.ContentService;
            this.CreatePage("66f37878-25bb-471c-9363-d15e400b6cbf", "MyFirstPage");
            this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "MySecondPage");

            IEnumerable<IPage> allPages = contentService.GetAllPages();

            Assert.AreEqual<int>(2, allPages.Count());
        }

        [TestMethod]
        public void InitPagesFromFileOnStartup()
        {
            var contentService = ContentServiceTestHelper.Instance.ContentService;
            this.CreatePage("66f37878-25bb-471c-9363-d15e400b6cbf", "MyFirstPage");
            this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "MySecondPage");

            contentService = this.CreateNewContentService();

            IEnumerable<IPage> allPages = contentService.GetAllPages();

            Assert.AreEqual<int>(2, allPages.Count());
        }

        [TestMethod]
        public void AppendPagesAfterStartup()
        {
            var contentService = ContentServiceTestHelper.Instance.ContentService;
            this.CreatePage("66f37878-25bb-471c-9363-d15e400b6cbf", "MyFirstPage");
            this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "MySecondPage");

            contentService = this.CreateNewContentService();
            contentService.AddPage("66f37878-25bb-471c-9363-d15e400b6cbf", "MyFirstPage");
            contentService.AddPage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "MySecondPage");

            IEnumerable<IPage> allPages = contentService.GetAllPages();

            Assert.AreEqual<int>(4, allPages.Count());
        }

        [TestMethod]
        public void GetPagesFromStore()
        {
            var contentService = ContentServiceTestHelper.Instance.ContentService;
            this.CreatePage("66f37878-25bb-471c-9363-d15e400b6cbf", "MyFirstPage");
            this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "MySecondPage");

            ContentServiceTestHelper.Instance.ContentCacheService.ClearCachedPages();

            IEnumerable<IPage> allPages = contentService.GetAllPages();

            Assert.AreEqual<int>(2, allPages.Count());
        }

        [TestMethod]
        public void ReloadPagesToCache()
        {
            var contentService = ContentServiceTestHelper.Instance.ContentService;
            this.CreatePage("66f37878-25bb-471c-9363-d15e400b6cbf", "MyFirstPage");
            this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "MySecondPage");

            ContentServiceTestHelper.Instance.ContentCacheService.ClearCachedPages();

            Assert.AreEqual<int>(0, ContentServiceTestHelper.Instance.ContentCacheService.CachedPages);

            IEnumerable<IPage> allPages = contentService.GetAllPages().ToList();

            Assert.AreEqual<int>(2, ContentServiceTestHelper.Instance.ContentCacheService.CachedPages);

            Assert.AreEqual<int>(2, allPages.Count());
        }

        private void DeleteAllPages()
        {
            var contentService = ContentServiceTestHelper.Instance.ContentService;
            IEnumerable<IPage> allPages = contentService.GetAllPages().ToList();
            foreach (var page in allPages)
            {
                contentService.DeletePage(page.Id);
            }
        }

        private IContentService CreateNewContentService()
        {
            return new ContentService(new ContentRepository(new InitializeService(), new ContentCacheService(), new LewCMSJsonSerializer(), @"C:\Users\Tobias\Documents\Visual Studio 2013\Projects\MyWebApplication\LewCMS.UnitTesting\App_Data"));
        }

        private void CreatePage(string pageTypeId, string pageName, string parentId = null)
        {
            var contentService = ContentServiceTestHelper.Instance.ContentService;
            contentService.AddPage(pageTypeId, pageName, parentId);
        }
    }
}
