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
        private static string FILE_PERSIST_PATH = @"C:\tolu00\Playground\LewCMS\LewCMS.UnitTesting\App_Data";
        //private static string FILE_PERSIST_PATH = @"C:\Users\Tobias\Documents\Visual Studio 2013\Projects\MyWebApplication\LewCMS.UnitTesting\App_Data";

        [ClassInitialize]
        public static void InitializeTestClass(TestContext testContext)
        {
            Application.Current.SetApplicationAssembly(Assembly.GetExecutingAssembly());
            ContentServiceTestHelper.Instance.SetInitializeService(new InitializeService());
            ContentServiceTestHelper.Instance.SetContentCacheService(new ContentCacheService());
            ContentServiceTestHelper.Instance.SetContentRepository(new ContentRepository(ContentServiceTestHelper.Instance.InitializeService, ContentServiceTestHelper.Instance.ContentCacheService, new LewCMSJsonSerializer(), ContentServiceTests.FILE_PERSIST_PATH));
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
        public void Create_Page()
        {
            var contentService = ContentServiceTestHelper.Instance.ContentService;
            this.CreatePage("66f37878-25bb-471c-9363-d15e400b6cbf", "MyFirstPage");
            this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "MySecondPage");

            IEnumerable<IPage> allPages = contentService.GetAllPages();
            IPage firstPage = allPages.First();
            IPage secondPage = allPages.Last();

            Assert.AreEqual<int>(2, allPages.Count());
            Assert.AreEqual<string>("66f37878-25bb-471c-9363-d15e400b6cbf", firstPage.PageType.Id);
            Assert.AreEqual<string>("dd9f76ef-3e63-4a73-8170-9e84ec703b07", secondPage.PageType.Id);
            Assert.AreEqual<string>("MyFirstPage", firstPage.Name);
            Assert.AreEqual<string>("MySecondPage", secondPage.Name);
            Assert.AreEqual<string>("/MyFirstPage".ToLower(), firstPage.Route);
            Assert.AreEqual<string>("/MySecondPage".ToLower(), secondPage.Route);

        }

        [TestMethod]
        public void Create_Child_Pages()
        {
            var contentService = ContentServiceTestHelper.Instance.ContentService;
            IPage firstPage = this.CreatePage("66f37878-25bb-471c-9363-d15e400b6cbf", "MyFirstPage");
            IPage secondPage = this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "MySecondPage");

            IPage firstPageChild = this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "MyFirstPageChild", firstPage.Id);
            IPage secondPageChild = this.CreatePage("66f37878-25bb-471c-9363-d15e400b6cbf", "MySecondPageChild", secondPage.Id);

            IEnumerable<IPage> allPages = contentService.GetAllPages();

            Assert.AreEqual<int>(4, allPages.Count());
            Assert.AreEqual<string>("MyFirstPage", firstPage.Name);
            Assert.AreEqual<string>("MySecondPage", secondPage.Name);
            Assert.AreEqual<string>("MyFirstPageChild", firstPageChild.Name);
            Assert.AreEqual<string>("MySecondPageChild", secondPageChild.Name);

            Assert.AreEqual<string>(firstPage.Id, firstPageChild.ParentId);
            Assert.AreEqual<string>(secondPage.Id, secondPageChild.ParentId);

            Assert.AreEqual<string>("/MyFirstPage".ToLower(), firstPage.Route);
            Assert.AreEqual<string>("/MySecondPage".ToLower(), secondPage.Route);
            Assert.AreEqual<string>("/MyFirstPage/MyFirstPageChild".ToLower(), firstPageChild.Route);
            Assert.AreEqual<string>("/MySecondPage/MySecondPageChild".ToLower(), secondPageChild.Route);

        }

        [TestMethod]
        public void Create_Pages_With_Same_Route()
        {
            var contentService = ContentServiceTestHelper.Instance.ContentService;
            this.CreatePage("66f37878-25bb-471c-9363-d15e400b6cbf", "My-Page-Name-1");
            this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "My-Page-Name-1");
            this.CreatePage("66f37878-25bb-471c-9363-d15e400b6cbf", "My-Page-Name-1");
            this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "My-Page-Name-1");

            List<IPage> allPages = contentService.GetAllPages().ToList();

            IPage firstPage = allPages[0];
            IPage secondPage = allPages[1];
            IPage thirdPage = allPages[2];
            IPage fourthPage = allPages[3];

            IPage childPage1 = this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "My-Child-Page-Name", fourthPage.Id);
            IPage childPage2 = this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "My-Child-Page-Name", fourthPage.Id);
            IPage childPage3 = this.CreatePage("66f37878-25bb-471c-9363-d15e400b6cbf", "My-Child-Page-Name-1", fourthPage.Id);
            IPage childPage4 = this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "My-Child-Page-Name-1", fourthPage.Id);

            allPages = contentService.GetAllPages().ToList();

            Assert.AreEqual<int>(8, allPages.Count());
            Assert.AreEqual<string>("/My-Page-Name-1".ToLower(), firstPage.Route);
            Assert.AreEqual<string>("/My-Page-Name-1-1".ToLower(), secondPage.Route);
            Assert.AreEqual<string>("/My-Page-Name-1-2".ToLower(), thirdPage.Route);
            Assert.AreEqual<string>("/My-Page-Name-1-3".ToLower(), fourthPage.Route);
            Assert.AreEqual<string>("/My-Page-Name-1-3/My-Child-Page-Name".ToLower(), childPage1.Route);
            Assert.AreEqual<string>("/My-Page-Name-1-3/My-Child-Page-Name-1".ToLower(), childPage2.Route);
            Assert.AreEqual<string>("/My-Page-Name-1-3/My-Child-Page-Name-1-1".ToLower(), childPage3.Route);
            Assert.AreEqual<string>("/My-Page-Name-1-3/My-Child-Page-Name-1-2".ToLower(), childPage4.Route);

        }

        [TestMethod]
        public void Init_Pages_From_File_On_Startup()
        {
            var contentService = ContentServiceTestHelper.Instance.ContentService;
            this.CreatePage("66f37878-25bb-471c-9363-d15e400b6cbf", "MyFirstPage");
            this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "MySecondPage");

            contentService = this.CreateNewContentService();

            IEnumerable<IPage> allPages = contentService.GetAllPages();

            Assert.AreEqual<int>(2, allPages.Count());
        }

        [TestMethod]
        public void Append_Pages_After_Startup()
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
        public void Get_Pages_From_Store()
        {
            var contentService = ContentServiceTestHelper.Instance.ContentService;
            this.CreatePage("66f37878-25bb-471c-9363-d15e400b6cbf", "MyFirstPage");
            this.CreatePage("dd9f76ef-3e63-4a73-8170-9e84ec703b07", "MySecondPage");

            ContentServiceTestHelper.Instance.ContentCacheService.ClearCachedPages();

            IEnumerable<IPage> allPages = contentService.GetAllPages();

            Assert.AreEqual<int>(2, allPages.Count());
        }

        [TestMethod]
        public void Reload_Pages_To_Cache()
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
            return new ContentService(new ContentRepository(new InitializeService(), new ContentCacheService(), new LewCMSJsonSerializer(), ContentServiceTests.FILE_PERSIST_PATH));
        }

        private IPage CreatePage(string pageTypeId, string pageName, string parentId = null)
        {
            var contentService = ContentServiceTestHelper.Instance.ContentService;
            return contentService.AddPage(pageTypeId, pageName, parentId);
        }
    }
}
