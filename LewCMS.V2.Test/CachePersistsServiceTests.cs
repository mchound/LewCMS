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
    public class CachePersistsServiceTests
    {

        private static IPersistService service = new DefaultCachePersistService();

        [ClassInitialize]
        public static void InitializeTestClass(TestContext testContext)
        {
            Application.Current.SetApplicationAssembly(Assembly.GetExecutingAssembly());
            service.Initialize(new DefaultInitializeService());
        }

        [TestCleanup]
        public void CleanUpTests()
        {
            
        }

        [TestMethod]
        public void Init_Service()
        {
            IEnumerable<IContentType> contentTypes = service.LoadContentTypes();
            Assert.AreEqual<int>(6, contentTypes.Count());
        }

        [TestMethod]
        public void Save_Load_And_Delete_Content()
        {
            IEnumerable<IContentType> contentTypes = service.LoadContentTypes();

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

            IEnumerable<IPage> pages = service.LoadContent<IPage>();
            IEnumerable<ISection> sections = service.LoadContent<ISection>();
            IEnumerable<IGlobalConfig> globalConfigs = service.LoadContent<IGlobalConfig>();

            IPage page11 = service.LoadContentFor<IPage, IPageInfo>(pi => pi.Id == page1.Id);
            IPage page22 = service.LoadContentFor<IPage, IPageInfo>(pi => pi.Id == page2.Id);

            ISection section11 = service.LoadContentFor<ISection, ISectionInfo>(si => si.Id == section1.Id);
            ISection section22 = service.LoadContentFor<ISection, ISectionInfo>(si => si.Id == section2.Id);

            IGlobalConfig globalConfig11 = service.LoadContentFor<IGlobalConfig, IGlobalConfigInfo>(gi => gi.Id == globalConfig1.Id);
            IGlobalConfig globalConfig22 = service.LoadContentFor<IGlobalConfig, IGlobalConfigInfo>(gi => gi.Id == globalConfig2.Id);

            Assert.AreEqual<int>(2, pages.Count());
            Assert.AreEqual<int>(2, sections.Count());
            Assert.AreEqual<int>(2, globalConfigs.Count());

            Assert.AreEqual<string>(page1.Name, page11.Name);
            Assert.AreEqual<string>(page2.Name, page22.Name);
            Assert.AreEqual<string>(section1.Name, section11.Name);
            Assert.AreEqual<string>(section2.Name, section22.Name);
            Assert.AreEqual<string>(globalConfig1.Name, globalConfig11.Name);
            Assert.AreEqual<string>(globalConfig2.Name, globalConfig22.Name);

            service.Delete(page11.ContentInfo());
            service.Delete(page22.ContentInfo());
            service.Delete(section11.ContentInfo());
            service.Delete(section22.ContentInfo());
            service.Delete(globalConfig11.ContentInfo());
            service.Delete(globalConfig22.ContentInfo());

            pages = service.LoadContent<IPage>();
            sections = service.LoadContent<ISection>();
            globalConfigs = service.LoadContent<IGlobalConfig>();

            Assert.AreEqual<int>(0, pages.Count());
            Assert.AreEqual<int>(0, sections.Count());
            Assert.AreEqual<int>(0, globalConfigs.Count());
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
