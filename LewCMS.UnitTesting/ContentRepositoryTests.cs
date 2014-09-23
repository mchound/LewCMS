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
    public class ContentRepositoryTests
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

            ServicesTestHelper.Instance.SetContentRepository(contentRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            string pagesFolderPath = Path.Combine(ServicesTestHelper.FILE_PERSIST_PATH, "Pages");
            if (Directory.Exists(pagesFolderPath))
            {
                Directory.Delete(pagesFolderPath, true);
            }
        }

        [TestMethod]
        public void Get_Page_Types()
        {
            var service = this.GetService();
            IEnumerable<IPageType> pageTypes = service.GetPageTypes();
            IEnumerable<IPageType> predicatePageTypes = service.GetPageTypes(pt => pt.Id == "66f37878-25bb-471c-9363-d15e400b6cbf");
            IPageType pageType = predicatePageTypes.First();

            Assert.AreEqual<int>(2, pageTypes.Count());
            Assert.AreEqual<int>(1, predicatePageTypes.Count());
            Assert.AreEqual<string>("MyFirstPageType", pageType.DisplayName);
        }


        [TestMethod]
        public void Persist_Pages()
        {
            var service = this.GetService();

            IEnumerable<IPageType> pageTypes = service.GetPageTypes();
            IPage page1 = this.CreatePage(pageTypes.First(), "Page-1");
            IPage page2 = this.CreatePage(pageTypes.First(), "Page-2");

            service.Persist(page1);
            service.Persist(page2);

            IEnumerable<IPageInfo> pageInfos = service.GetPageInfo();
            IPageInfo singlePageInfo = service.GetPageInfoFor(pi => pi.PageId == page1.Id);
            IPageInfo singlePageInfoPage1 = pageInfos.First();
            IPageInfo singlePageInfoPage2 = pageInfos.Last();

            Assert.AreEqual<int>(2, pageInfos.Count());
            Assert.AreEqual<string>(page1.Id, singlePageInfo.PageId);
            Assert.AreEqual<string>(page1.Name, singlePageInfo.PageName);
            Assert.AreEqual<string>(page1.Id, singlePageInfoPage1.PageId);
            Assert.AreEqual<string>(page1.Name, singlePageInfoPage1.PageName);
            Assert.AreEqual<string>(page2.Id, singlePageInfoPage2.PageId);
            Assert.AreEqual<string>(page2.Name, singlePageInfoPage2.PageName);

            IEnumerable<IPage> pages = service.GetPages();
            IPage loadedPage1 = service.GetPage(page1.Id, 1);
            IPage loadedPage2 = service.GetPage(page2.Id, 1);

            Assert.AreEqual<int>(2, pages.Count());
            Assert.AreEqual<string>(page1.Id, loadedPage1.Id);
            Assert.AreEqual<string>(page1.Name, loadedPage1.Name);
            Assert.AreEqual<string>(page2.Id, loadedPage2.Id);
            Assert.AreEqual<string>(page2.Name, loadedPage2.Name);
        }

        [TestMethod]
        public void Persist_Pages_With_Versions()
        {
            var service = this.GetService();

            IEnumerable<IPageType> pageTypes = service.GetPageTypes();
            IPage page1 = this.CreatePage(pageTypes.First(), "Page-1");
            IPage page2 = this.CreatePage(pageTypes.Last(), "Page-2");

            service.Persist(page1);
            page1 = page1.Clone();
            page1.Version++;
            service.Persist(page1);
            page1 = page1.Clone();
            page1.Version++;
            service.Persist(page1);

            service.Persist(page2);
            page2 = page2.Clone();
            page2.Version++;
            service.Persist(page2);
            page2 = page2.Clone();
            page2.Version++;
            service.Persist(page2);


            IEnumerable<IPageInfo> pageInfos = service.GetPageInfo();
            IPageInfo singlePageInfo1 = service.GetPageInfoFor(pi => pi.PageId == page1.Id && pi.Version == 2);
            IPageInfo singlePageInfo2 = service.GetPageInfoFor(pi => pi.PageId == page2.Id && pi.Version == 1);

            Assert.AreEqual<int>(6, pageInfos.Count());
            Assert.AreEqual<string>(page1.Id, singlePageInfo1.PageId);
            Assert.AreEqual<string>(page1.Name, singlePageInfo1.PageName);
            Assert.AreEqual<int>(2, singlePageInfo1.Version);
            Assert.AreEqual<string>(page2.Id, singlePageInfo2.PageId);
            Assert.AreEqual<string>(page2.Name, singlePageInfo2.PageName);
            Assert.AreEqual<int>(1, singlePageInfo2.Version);

            IEnumerable<IPage> pages = service.GetPages();
            IEnumerable<IPage> pagesAllVersions = service.GetPages(Enums.ContentVersionSelect.All);
            IEnumerable<IPage> pagesLatestVersions = service.GetPages(Enums.ContentVersionSelect.Latest);
            IEnumerable<IPage> pagesByPredicate1 = service.GetPages(pi => pi.PageId == page1.Id);
            IEnumerable<IPage> pagesByPredicateAndVersion1 = service.GetPages(pi => pi.PageId == page1.Id, Enums.ContentVersionSelect.Latest);
            IEnumerable<IPage> pagesByPredicate2 = service.GetPages(pi => pi.PageId == page2.Id);
            IEnumerable<IPage> pagesByPredicateAndVersion2 = service.GetPages(pi => pi.PageId == page2.Id, Enums.ContentVersionSelect.Latest);
            IPage pageByPageInfo1 = service.GetPage(singlePageInfo1);
            IPage pageById1 = service.GetPage(page1.Id);
            IPage pageByIdAndVersion1 = service.GetPage(page1.Id, 2);
            IPage pageByPageInfo2 = service.GetPage(singlePageInfo2);
            IPage pageById2 = service.GetPage(page2.Id);
            IPage pageByIdAndVersion2 = service.GetPage(page2.Id, 1);

            Assert.AreEqual<int>(6, pages.Count());
            Assert.AreEqual<int>(6, pagesAllVersions.Count());
            Assert.AreEqual<int>(2, pagesLatestVersions.Count());
            Assert.AreEqual<int>(3, pagesByPredicate1.Count());
            Assert.AreEqual<int>(3, pagesByPredicate2.Count());
            Assert.AreEqual<int>(1, pagesByPredicateAndVersion1.Count());
            Assert.AreEqual<int>(1, pagesByPredicateAndVersion2.Count());

            Assert.AreEqual<string>(page1.Id, pageByPageInfo1.Id);
            Assert.AreEqual<string>(page1.Name, pageByPageInfo1.Name);
            Assert.AreEqual<string>(page2.Id, pageByPageInfo2.Id);
            Assert.AreEqual<string>(page2.Name, pageByPageInfo2.Name);

            Assert.AreEqual<string>(page1.Id, pageById1.Id);
            Assert.AreEqual<string>(page1.Name, pageById1.Name);
            Assert.AreEqual<string>(page2.Id, pageById2.Id);
            Assert.AreEqual<string>(page2.Name, pageById2.Name);

            Assert.AreEqual<string>(page1.Id, pageByIdAndVersion1.Id);
            Assert.AreEqual<string>(page1.Name, pageByIdAndVersion1.Name);
            Assert.AreEqual<string>(page2.Id, pageByIdAndVersion2.Id);
            Assert.AreEqual<string>(page2.Name, pageByIdAndVersion2.Name);
        }

        [TestMethod]
        public void Get_Page_Versions()
        {
            var service = this.GetService();

            IEnumerable<IPageType> pageTypes = service.GetPageTypes();
            IPage page1 = this.CreatePage(pageTypes.First(), "Page-1");
            IPage page2 = this.CreatePage(pageTypes.Last(), "Page-2");

            service.Persist(page1);
            page1 = page1.Clone();
            page1.Version++;
            service.Persist(page1);
            page1 = page1.Clone();
            page1.Version++;
            service.Persist(page1);

            service.Persist(page2);
            page2 = page2.Clone();
            page2.Version++;
            service.Persist(page2);
            page2 = page2.Clone();
            page2.Version++;
            service.Persist(page2);

            page2.Version = 2;

            IPageInfo pageInfo1 = service.GetPageInfoFor(pi => pi.PageId == page1.Id && pi.Version == 1);
            IPageInfo pageInfo2 = service.GetPageInfoFor(pi => pi.PageId == page2.Id && pi.Version == 3);

            int pageVersion1 = service.GetPageVersions(page1);
            int pageVersion2 = service.GetPageVersions(page2);

            Assert.AreEqual<int>(3, pageVersion1);
            Assert.AreEqual<int>(3, pageVersion2);

            pageVersion1 = service.GetPageVersions(pageInfo1);
            pageVersion2 = service.GetPageVersions(pageInfo2);

            Assert.AreEqual<int>(3, pageVersion1);
            Assert.AreEqual<int>(3, pageVersion2);

            pageVersion1 = service.GetPageVersions(page1.Id);
            pageVersion2 = service.GetPageVersions(page2.Id);

            Assert.AreEqual<int>(3, pageVersion1);
            Assert.AreEqual<int>(3, pageVersion2);
        }

        [TestMethod]
        public void Delete_Pages()
        {
            var service = this.GetService();

            IEnumerable<IPageType> pageTypes = service.GetPageTypes();
            IPage page1 = this.CreatePage(pageTypes.First(), "Page-1");
            IPage page2 = this.CreatePage(pageTypes.Last(), "Page-2");

            service.Persist(page1);
            page1 = page1.Clone();
            page1.Version++;
            service.Persist(page1);
            page1 = page1.Clone();
            page1.Version++;
            service.Persist(page1);

            service.Persist(page2);
            page2 = page2.Clone();
            page2.Version++;
            service.Persist(page2);
            page2 = page2.Clone();
            page2.Version++;
            service.Persist(page2);

            List<IPage> pages = service.GetPages().ToList();

            Assert.AreEqual<int>(6, pages.Count);

            int pageCounter = pages.Count();

            foreach (var page in pages)
	        {
                service.Delete(page.Id, page.Version);
                Assert.AreEqual<int>(--pageCounter, service.GetPages().Count());
	        }
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

        private IContentRepository GetService()
        {
            return ServicesTestHelper.Instance.ContentRepository;
        }
    }
}
