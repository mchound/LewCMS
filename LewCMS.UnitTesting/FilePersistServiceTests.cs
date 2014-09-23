using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LewCMS.Core.Content;
using System.Reflection;
using LewCMS.Core;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MyWebApplication.PageTypes;

namespace LewCMS.UnitTesting
{
    [TestClass]
    public class FilePersistServiceTests
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
            ServicesTestHelper.Instance.SetInitializeService(new InitializeService());
            ServicesTestHelper.Instance.SetFilePersistsService(new FilePersistService(new LewCMSJsonSerializer(), ServicesTestHelper.FILE_PERSIST_PATH));
        }

        [TestInitialize]
        public void InitTests()
        {
            var service = this.GetFilePersistService();
            service.Initialize(this.LoadPageTypes());
        }

        [TestCleanup]
        public void CleanUpTests()
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
        }

        [TestMethod]
        public void Initialize_Persist_Service()
        {
            var service = this.GetFilePersistService();
            service.Initialize(this.LoadPageTypes());
            List<IPageType> pageTypes = service.LoadPageTypes().ToList();

            Assert.AreEqual<int>(2, pageTypes.Count);
            Assert.AreEqual<int>(3, pageTypes[0].Properties.Count);
            Assert.AreEqual<int>(2, pageTypes[1].Properties.Count);
            Assert.AreEqual<string>("66f37878-25bb-471c-9363-d15e400b6cbf", pageTypes[0].Id);
            Assert.AreEqual<string>("dd9f76ef-3e63-4a73-8170-9e84ec703b07", pageTypes[1].Id);
        }

        [TestMethod]
        public void Save_And_Load_Pages()
        {
            var service = this.GetFilePersistService();

            // Init
            service.Initialize(this.LoadPageTypes());

            // Load page types
            List<IPageType> pageTypes = service.LoadPageTypes().ToList();

            // Create pages
            IPage page1 = this.CreatePage(pageTypes[0], "MyPage-1");
            IPage page2 = this.CreatePage(pageTypes[1], "MyPage-2");

            // Save pages
            service.SavePage(page1);
            service.SavePage(page2);

            // Load all pages
            IEnumerable<IPage> pages = service.LoadPages();
            IPage loadedPage1 = pages.First();
            IPage loadedPage2 = pages.Last();

            // Load one page
            IPage singleLoaded1 = service.LoadPage(page1.Id, 1);
            IPage singleLoaded2 = service.LoadPages(pi => pi.PageId == page2.Id).FirstOrDefault();

            IEnumerable<IPageInfo> pageInfos = service.LoadPageInfos();
            IPageInfo singlePageInfo1 = pageInfos.First();
            IPageInfo singlePageInfo2 = service.LoadPageInfo(pi => pi.PageId == page2.Id);

            Assert.AreEqual<int>(2, pages.Count());
            Assert.AreEqual<int>(2, pageInfos.Count());

            Assert.AreEqual<string>(page1.Name, loadedPage1.Name);
            Assert.AreEqual<string>(page1.Route, loadedPage1.Route);
            Assert.AreEqual<string>(page1.Id, loadedPage1.Id);

            Assert.AreEqual<string>(page2.Name, loadedPage2.Name);
            Assert.AreEqual<string>(page2.Route, loadedPage2.Route);
            Assert.AreEqual<string>(page2.Id, loadedPage2.Id);

            Assert.AreEqual<string>(page1.Name, singleLoaded1.Name);
            Assert.AreEqual<string>(page1.Route, singleLoaded1.Route);
            Assert.AreEqual<string>(page1.Id, singleLoaded1.Id);

            Assert.AreEqual<string>(page2.Name, singleLoaded2.Name);
            Assert.AreEqual<string>(page2.Route, singleLoaded2.Route);
            Assert.AreEqual<string>(page2.Id, singleLoaded2.Id);

            Assert.AreEqual<string>(page1.Name, singlePageInfo1.PageName);
            Assert.AreEqual<string>(page1.Route, singlePageInfo1.PageRoute);
            Assert.AreEqual<string>(page1.Id, singlePageInfo1.PageId);

            Assert.AreEqual<string>(page2.Name, singlePageInfo2.PageName);
            Assert.AreEqual<string>(page2.Route, singlePageInfo2.PageRoute);
            Assert.AreEqual<string>(page2.Id, singlePageInfo2.PageId);

        }

        [TestMethod]
        public void Delete_Pages()
        {
            var service = this.GetFilePersistService();

            // Init
            service.Initialize(this.LoadPageTypes());

            // Load page types
            List<IPageType> pageTypes = service.LoadPageTypes().ToList();

            // Create pages
            IPage page1 = this.CreatePage(pageTypes[0], "MyPage-1");
            IPage page2 = this.CreatePage(pageTypes[1], "MyPage-2");

            // Save pages
            service.SavePage(page1);
            service.SavePage(page2);

            IEnumerable<IPage> pages = service.LoadPages();

            Assert.AreEqual<int>(2, pages.Count());

            service.Delete(page1.Id, 1);
            service.Delete(page2.Id, 1);

            pages = service.LoadPages();

            Assert.AreEqual<int>(0, pages.Count());
        }

        private IPersistService GetFilePersistService()
        {
            return ServicesTestHelper.Instance.PersistsService;
        }

        private IEnumerable<IPageType> LoadPageTypes()
        {
            var service = ServicesTestHelper.Instance.InitializeService;
            return service.GetPageTypes(Application.Current.ApplicationAssembly);
        }

        private IPage CreatePage(IPageType pageType, string pageName)
        {
            IPage page = Activator.CreateInstance(Application.Current.ApplicationAssembly.GetType(pageType.TypeName)) as IPage;
            page.Id = Guid.NewGuid().ToString();
            page.Route = "/" + pageName;
            page.Name = pageName;
            page.Version = 1;
            page.PageType = pageType as PageType;
            page.ParentId = null;
            page.CreatedAt = DateTime.Now;
            page.UpdatedAt = page.CreatedAt;

            page.OnInit();

            return page;
        }
    }
}

