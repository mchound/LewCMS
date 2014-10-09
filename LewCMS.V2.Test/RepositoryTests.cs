using LewCMS.V2.Contents;
using LewCMS.V2.Serialization;
using LewCMS.V2.Startup;
using LewCMS.V2.Store;
using LewCMS.V2.Store.Cache;
using LewCMS.V2.Store.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Test
{
    [TestClass]
    public class RepositoryTests
    {

        private static IFileStoreService fileStoreService = new DefaultFileStoreService(new DefaultJsonSerializer());
        private static ICacheStoreService cacheStoreService = new DefaultCacheStoreService();
        private static IRepository repository;

        [ClassInitialize]
        public static void InitializeTestClass(TestContext testContext)
        {
            if (Directory.Exists(Configuration.PERSITS_VIRTUAL_FILE_PATH))
            {
                Directory.Delete(Configuration.PERSITS_VIRTUAL_FILE_PATH, true);
            }

            Application.Current.SetApplicationAssembly(Assembly.GetExecutingAssembly());
            repository = new DefaultRepository(new DefaultInitializeService(), fileStoreService, cacheStoreService);
        }

        [TestCleanup]
        public void CleanUpTests()
        {
            if (Directory.Exists(Configuration.PERSITS_VIRTUAL_FILE_PATH))
            {
                Directory.Delete(Configuration.PERSITS_VIRTUAL_FILE_PATH, true);
            }

            cacheStoreService.ClearCache();
        }

        [TestInitialize]
        public void TestInit()
        {
            repository = new DefaultRepository(new DefaultInitializeService(), fileStoreService, cacheStoreService);
        }

        [TestMethod]
        public void Init_Service()
        {
            IEnumerable<IContentType> contentTypes = repository.GetContentTypes();
            Assert.AreEqual<int>(6, contentTypes.Count());
        }

        [TestMethod]
        public void Save_Load_And_Delete_Content()
        {
            IEnumerable<IContentType> contentTypes = repository.GetContentTypes();

            IEnumerable<IPageType> pageTypes = contentTypes.Where(ct => ct is IPageType).Select(ct => ct as IPageType);
            IEnumerable<ISectionType> sectionTypes = contentTypes.Where(ct => ct is ISectionType).Select(ct => ct as ISectionType);
            IEnumerable<IGlobalConfigType> globalConfigTypes = contentTypes.Where(ct => ct is IGlobalConfigType).Select(ct => ct as IGlobalConfigType);

            IPage page1 = this.CreatePage(pageTypes.First(), "Page1");
            IPage page2 = this.CreatePage(pageTypes.Last(), "Page2");

            ISection section1 = this.CreateContent(sectionTypes.First(), "Section1") as ISection;
            ISection section2 = this.CreateContent(sectionTypes.Last(), "Section2") as ISection;

            IGlobalConfig globalConfig1 = this.CreateContent(globalConfigTypes.First(), "GlobalConfig1") as IGlobalConfig;
            IGlobalConfig globalConfig2 = this.CreateContent(globalConfigTypes.Last(), "GlobalConfig2") as IGlobalConfig;

            repository.Save(page1);
            repository.Save(page2);

            repository.Save(section1);
            repository.Save(section2);

            repository.Save(globalConfig1);
            repository.Save(globalConfig2);

            IEnumerable<IPage> pages = repository.Get<IPage>();
            IEnumerable<ISection> sections = repository.Get<ISection>();
            IEnumerable<IGlobalConfig> globalConfigs = repository.Get<IGlobalConfig>();

            IPage page11 = repository.GetFor<IPage, IPageInfo>(pi => pi.Id == page1.Id);
            IPage page22 = repository.GetFor<IPage, IPageInfo>(pi => pi.Id == page2.Id);

            ISection section11 = repository.GetFor<ISection, ISectionInfo>(si => si.Id == section1.Id);
            ISection section22 = repository.GetFor<ISection, ISectionInfo>(si => si.Id == section2.Id);

            IGlobalConfig globalConfig11 = repository.GetFor<IGlobalConfig, IGlobalConfigInfo>(gi => gi.Id == globalConfig1.Id);
            IGlobalConfig globalConfig22 = repository.GetFor<IGlobalConfig, IGlobalConfigInfo>(gi => gi.Id == globalConfig2.Id);

            Assert.AreEqual<int>(2, pages.Count());
            Assert.AreEqual<int>(2, sections.Count());
            Assert.AreEqual<int>(2, globalConfigs.Count());

            Assert.AreEqual<string>(page1.Name, page11.Name);
            Assert.AreEqual<string>(page2.Name, page22.Name);
            Assert.AreEqual<string>(section1.Name, section11.Name);
            Assert.AreEqual<string>(section2.Name, section22.Name);
            Assert.AreEqual<string>(globalConfig1.Name, globalConfig11.Name);
            Assert.AreEqual<string>(globalConfig2.Name, globalConfig22.Name);

            repository.Delete(page11.GetStoreInfo());
            repository.Delete(page22.GetStoreInfo());
            repository.Delete(section11.GetStoreInfo());
            repository.Delete(section22.GetStoreInfo());
            repository.Delete(globalConfig11.GetStoreInfo());
            repository.Delete(globalConfig22.GetStoreInfo());

            pages = repository.Get<IPage>();
            sections = repository.Get<ISection>();
            globalConfigs = repository.Get<IGlobalConfig>();

            Assert.AreEqual<int>(0, pages.Count());
            Assert.AreEqual<int>(0, sections.Count());
            Assert.AreEqual<int>(0, globalConfigs.Count());
        }

        //[TestMethod]
        //public void Large_Scale()
        //{
        //    IEnumerable<IContentType> contentTypes = repository.GetContentTypes();
        //    var v = 1;
        //    IEnumerable<IPageType> pageTypes = contentTypes.Where(ct => ct is IPageType).Select(ct => ct as IPageType);
        //    IEnumerable<ISectionType> sectionTypes = contentTypes.Where(ct => ct is ISectionType).Select(ct => ct as ISectionType);
        //    IEnumerable<IGlobalConfigType> globalConfigTypes = contentTypes.Where(ct => ct is IGlobalConfigType).Select(ct => ct as IGlobalConfigType);

        //    IPage page;
        //    ISection section;
        //    IGlobalConfig globalConfig;

        //    for (int i = 0; i < 250; i++)
        //    {
        //        page = this.CreatePage(pageTypes.First(), "Page" + i);
        //        repository.Save(page);
        //    }

        //    for (int i = 0; i < 250; i++)
        //    {
        //        section = this.CreateContent(sectionTypes.First(), "Section" + i) as ISection;
        //        repository.Save(section);
        //    }

        //    for (int i = 0; i < 250; i++)
        //    {
        //        globalConfig = this.CreateContent(globalConfigTypes.First(), "GlobalConfig" + i) as IGlobalConfig;
        //        repository.Save(globalConfig);
        //    }

        //    IEnumerable<IPage> pages = repository.Get<IPage>();
        //    IEnumerable<ISection> sections = repository.Get<ISection>();
        //    IEnumerable<IGlobalConfig> globalConfigs = repository.Get<IGlobalConfig>();

        //}

        private IContent CreateContent(IContentType contentType, string name)
        {
            return contentType.CreateInstance(name);
        }

        private IPage CreatePage(IPageType pageType, string pageName)
        {
            IPage page = this.CreateContent(pageType, pageName) as IPage;
            page.Route = "/" + pageName;
            page.ParentId = null;

            return page;
        }
    }
}
