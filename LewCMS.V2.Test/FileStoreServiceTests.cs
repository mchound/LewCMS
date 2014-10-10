using LewCMS.V2.Contents;
using LewCMS.V2.Serialization;
using LewCMS.V2.Startup;
using LewCMS.V2.Store;
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
    public class FilePersistsServiceTests
    {

        private static IStoreService service = new DefaultFileStoreService(new DefaultJsonSerializer());

        public static IContentTypeCollection GetContentTypeCollection()
        {
            IInitializeService initializeService = new DefaultInitializeService();
            IContentTypeCollection contentTypeCollection = new ContentTypeCollection();
            contentTypeCollection.PageTypes = initializeService.GetPageTypes(Application.Current.ApplicationAssembly).ToList();
            contentTypeCollection.SectionTypes = initializeService.GetSectionTypes(Application.Current.ApplicationAssembly).ToList();
            contentTypeCollection.GlobalConfigTypes = initializeService.GetGlobalConfigTypes(Application.Current.ApplicationAssembly).ToList();
            return contentTypeCollection;
        }

        [ClassInitialize]
        public static void InitializeTestClass(TestContext testContext)
        {
            if (Directory.Exists(Configuration.PERSITS_VIRTUAL_FILE_PATH))
            {
                Directory.Delete(Configuration.PERSITS_VIRTUAL_FILE_PATH, true);
            }

            Application.Current.SetApplicationAssembly(Assembly.GetExecutingAssembly());
            service.Save(FilePersistsServiceTests.GetContentTypeCollection());
        }

        [TestCleanup]
        public void CleanUpTests()
        {
            if (Directory.Exists(Configuration.PERSITS_VIRTUAL_FILE_PATH))
            {
                Directory.Delete(Configuration.PERSITS_VIRTUAL_FILE_PATH, true);
            }
        }

        [TestInitialize]
        public void TestInit()
        {
            service.Save(FilePersistsServiceTests.GetContentTypeCollection());
        }

        [TestMethod]
        public void Init_Service()
        {
            IContentTypeCollection contentTypes = service.Load<IContentTypeCollection>().First();
            Assert.AreEqual<int>(2, contentTypes.PageTypes.Count());
            Assert.AreEqual<int>(2, contentTypes.SectionTypes.Count());
            Assert.AreEqual<int>(2, contentTypes.GlobalConfigTypes.Count());
        }

        [TestMethod]
        public void Save_Load_And_Delete_Content()
        {
            IEnumerable<IContentType> contentTypes = service.Load<IContentTypeCollection>().First().ContentTypes;

            IEnumerable<IPageType> pageTypes = contentTypes.Where(ct => ct is IPageType).Select(ct => ct as IPageType);
            IEnumerable<ISectionType> sectionTypes = contentTypes.Where(ct => ct is ISectionType).Select(ct => ct as ISectionType);
            IEnumerable<IGlobalConfigType> globalConfigTypes = contentTypes.Where(ct => ct is IGlobalConfigType).Select(ct => ct as IGlobalConfigType);

            IPage page1 = this.CreatePage(pageTypes.First(), "Page1");
            IPage page2 = this.CreatePage(pageTypes.Last(), "Page2");

            ISection section1 = this.CreateContent(sectionTypes.First(), "Section1") as ISection;
            ISection section2 = this.CreateContent(sectionTypes.Last(), "Section2") as ISection;

            IGlobalConfig globalConfig1 = this.CreateContent(globalConfigTypes.First(), "GlobalConfig1") as IGlobalConfig;
            IGlobalConfig globalConfig2 = this.CreateContent(globalConfigTypes.Last(), "GlobalConfig2") as IGlobalConfig;

            service.Save(page1);
            service.Save(page2);

            service.Save(section1);
            service.Save(section2);

            service.Save(globalConfig1);
            service.Save(globalConfig2);

            IEnumerable<IPage> pages = service.Load<IPage>();
            IEnumerable<ISection> sections = service.Load<ISection>();
            IEnumerable<IGlobalConfig> globalConfigs = service.Load<IGlobalConfig>();

            IPage page11 = service.LoadFor<IPage, IPageInfo>(pi => pi.Id == page1.Id);
            IPage page22 = service.LoadFor<IPage, IPageInfo>(pi => pi.Id == page2.Id);

            ISection section11 = service.LoadFor<ISection, ISectionInfo>(si => si.Id == section1.Id);
            ISection section22 = service.LoadFor<ISection, ISectionInfo>(si => si.Id == section2.Id);

            IGlobalConfig globalConfig11 = service.LoadFor<IGlobalConfig, IGlobalConfigInfo>(gi => gi.Id == globalConfig1.Id);
            IGlobalConfig globalConfig22 = service.LoadFor<IGlobalConfig, IGlobalConfigInfo>(gi => gi.Id == globalConfig2.Id);

            Assert.AreEqual<int>(2, pages.Count());
            Assert.AreEqual<int>(2, sections.Count());
            Assert.AreEqual<int>(2, globalConfigs.Count());

            Assert.AreEqual<string>(page1.Name, page11.Name);
            Assert.AreEqual<string>(page2.Name, page22.Name);
            Assert.AreEqual<string>(section1.Name, section11.Name);
            Assert.AreEqual<string>(section2.Name, section22.Name);
            Assert.AreEqual<string>(globalConfig1.Name, globalConfig11.Name);
            Assert.AreEqual<string>(globalConfig2.Name, globalConfig22.Name);

            service.Delete(page11.GetStoreInfo());
            service.Delete(page22.GetStoreInfo());
            service.Delete(section11.GetStoreInfo());
            service.Delete(section22.GetStoreInfo());
            service.Delete(globalConfig11.GetStoreInfo());
            service.Delete(globalConfig22.GetStoreInfo());

            pages = service.Load<IPage>();
            sections = service.Load<ISection>();
            globalConfigs = service.Load<IGlobalConfig>();

            Assert.AreEqual<int>(0, pages.Count());
            Assert.AreEqual<int>(0, sections.Count());
            Assert.AreEqual<int>(0, globalConfigs.Count());
        }

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
