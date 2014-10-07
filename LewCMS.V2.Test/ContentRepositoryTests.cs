using LewCMS.V2.Services;
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
    public class ContentRepositoryTests
    {

        private static IPersistService filePersistsService = new DefaultFilePersistService(new DefaultJsonSerializer());
        private static ICachePersistService cachePersistsService = new DefaultCachePersistService();
        private static IContentRepository contentRepository = new DefaultContentRepository(new DefaultInitializeService(), filePersistsService, cachePersistsService);

        [ClassInitialize]
        public static void InitializeTestClass(TestContext testContext)
        {
            string contentFolder = Path.Combine(Configuration.PERSITS_VIRTUAL_FILE_PATH, "Content");

            if (Directory.Exists(contentFolder))
            {
                Directory.Delete(contentFolder, true);
            }

            Application.Current.SetApplicationAssembly(Assembly.GetExecutingAssembly());
        }

        [TestCleanup]
        public void CleanUpTests()
        {
            string contentFolder = Path.Combine(Configuration.PERSITS_VIRTUAL_FILE_PATH, "Content");

            if (Directory.Exists(contentFolder))
            {
                Directory.Delete(contentFolder, true);
            }

            cachePersistsService.ClearCache();
        }

        [TestInitialize]
        public void TestInit()
        {
            filePersistsService.Initialize(new DefaultInitializeService());
            cachePersistsService.Initialize(new DefaultInitializeService());
        }

        [TestMethod]
        public void Init_Service()
        {
            IEnumerable<IContentType> contentTypes = contentRepository.GetContentTypes();
            Assert.AreEqual<int>(6, contentTypes.Count());
        }

        [TestMethod]
        public void Save_Load_And_Delete_Content()
        {
            IEnumerable<IContentType> contentTypes = contentRepository.GetContentTypes();

            IEnumerable<IPageType> pageTypes = contentTypes.Where(ct => ct is IPageType).Select(ct => ct as IPageType);
            IEnumerable<ISectionType> sectionTypes = contentTypes.Where(ct => ct is ISectionType).Select(ct => ct as ISectionType);
            IEnumerable<IGlobalConfigType> globalConfigTypes = contentTypes.Where(ct => ct is IGlobalConfigType).Select(ct => ct as IGlobalConfigType);

            IPage page1 = this.CreatePage(pageTypes.First(), "Page1");
            IPage page2 = this.CreatePage(pageTypes.Last(), "Page2");

            ISection section1 = this.CreateContent(sectionTypes.First(), "Section1") as ISection;
            ISection section2 = this.CreateContent(sectionTypes.Last(), "Section2") as ISection;

            IGlobalConfig globalConfig1 = this.CreateContent(globalConfigTypes.First(), "GlobalConfig1") as IGlobalConfig;
            IGlobalConfig globalConfig2 = this.CreateContent(globalConfigTypes.Last(), "GlobalConfig2") as IGlobalConfig;

            contentRepository.Save(page1);
            contentRepository.Save(page2);

            contentRepository.Save(section1);
            contentRepository.Save(section2);

            contentRepository.Save(globalConfig1);
            contentRepository.Save(globalConfig2);

            IEnumerable<IPage> pages = contentRepository.GetContent<IPage>();
            IEnumerable<ISection> sections = contentRepository.GetContent<ISection>();
            IEnumerable<IGlobalConfig> globalConfigs = contentRepository.GetContent<IGlobalConfig>();

            IPage page11 = contentRepository.GetContentFor<IPage, IPageInfo>(pi => pi.Id == page1.Id);
            IPage page22 = contentRepository.GetContentFor<IPage, IPageInfo>(pi => pi.Id == page2.Id);

            ISection section11 = contentRepository.GetContentFor<ISection, ISectionInfo>(si => si.Id == section1.Id);
            ISection section22 = contentRepository.GetContentFor<ISection, ISectionInfo>(si => si.Id == section2.Id);

            IGlobalConfig globalConfig11 = contentRepository.GetContentFor<IGlobalConfig, IGlobalConfigInfo>(gi => gi.Id == globalConfig1.Id);
            IGlobalConfig globalConfig22 = contentRepository.GetContentFor<IGlobalConfig, IGlobalConfigInfo>(gi => gi.Id == globalConfig2.Id);

            Assert.AreEqual<int>(2, pages.Count());
            Assert.AreEqual<int>(2, sections.Count());
            Assert.AreEqual<int>(2, globalConfigs.Count());

            Assert.AreEqual<string>(page1.Name, page11.Name);
            Assert.AreEqual<string>(page2.Name, page22.Name);
            Assert.AreEqual<string>(section1.Name, section11.Name);
            Assert.AreEqual<string>(section2.Name, section22.Name);
            Assert.AreEqual<string>(globalConfig1.Name, globalConfig11.Name);
            Assert.AreEqual<string>(globalConfig2.Name, globalConfig22.Name);

            contentRepository.Delete(page11.ContentInfo());
            contentRepository.Delete(page22.ContentInfo());
            contentRepository.Delete(section11.ContentInfo());
            contentRepository.Delete(section22.ContentInfo());
            contentRepository.Delete(globalConfig11.ContentInfo());
            contentRepository.Delete(globalConfig22.ContentInfo());

            pages = contentRepository.GetContent<IPage>();
            sections = contentRepository.GetContent<ISection>();
            globalConfigs = contentRepository.GetContent<IGlobalConfig>();

            Assert.AreEqual<int>(0, pages.Count());
            Assert.AreEqual<int>(0, sections.Count());
            Assert.AreEqual<int>(0, globalConfigs.Count());
        }

        [TestMethod]
        public void Get_Content_Versions()
        {
            IEnumerable<IContentType> contentTypes = contentRepository.GetContentTypes();
            IEnumerable<IPageType> pageTypes = contentTypes.Where(ct => ct is IPageType).Select(ct => ct as IPageType);

            IPage page1 = this.CreatePage(pageTypes.First(), "Page1");
            IPage page1_1 = page1.Clone() as IPage;
            IPage page1_2 = page1.Clone() as IPage;
            IPage page1_3 = page1.Clone() as IPage;
            
            page1_1.Version = 2;
            page1_2.Version = 3;
            page1_3.Version = 4;

            contentRepository.Save(page1);
            contentRepository.Save(page1_1);
            contentRepository.Save(page1_2);
            contentRepository.Save(page1_3);

            IEnumerable<IPage> pages = contentRepository.GetContent<IPage>();

            Assert.AreEqual<int>(4, pages.Count());
            Assert.AreEqual<int>(4, contentRepository.GetContentVersions(page1));
            Assert.AreEqual<int>(4, contentRepository.GetContentVersions(page1.ContentInfo()));
            Assert.AreEqual<int>(4, contentRepository.GetContentVersions(page1_3.Id));
        }

        private IContent CreateContent(IContentType contentType, string name)
        {
            IContent content = Activator.CreateInstance(Application.Current.ApplicationAssembly.GetType(contentType.TypeName)) as IContent;
            content.Id = Guid.NewGuid().ToString();
            content.ContentType = contentType;
            content.Name = name;
            content.Version = 1;
            content.CreatedAt = DateTime.Now;
            content.UpdatedAt = content.CreatedAt;

            content.OnInit();

            return content;
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
