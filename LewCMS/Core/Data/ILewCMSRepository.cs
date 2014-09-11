using LewCMS.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace LewCMS.Core.Data
{
    public interface ILewCMSRepository
    {
        IEnumerable<IPageType> GetPageTypes();
        IEnumerable<IPageType> GetPageTypes(Func<IPageType, bool> predicate);
        IPageType GetPageType(Func<IPageType, bool> predicate);
        
        void AddPage(Page page);
        IEnumerable<IPage> GetPages(Func<IPage, bool> predicate);
        IEnumerable<IPage> GetPage(Func<IPage, bool> predicate);
        
    }

    public class LewCMSRepository : ILewCMSRepository
    {
        # region Private properties

        private ILewCMSCacheService _cmsCacheService;
        private string _filePersistFilePath;
        private const string PAGES_DIRECTORY_NAME = @"\Pages\";

        # endregion

        # region Constructors

        public LewCMSRepository(string filePersistFilePath) : this(LewCMSConfig.Current.GetCacheService(), filePersistFilePath)
        {}

        public LewCMSRepository(ILewCMSCacheService cmsCacheService, string filePersistFilePath)
        {
            this._cmsCacheService = cmsCacheService;
            this._filePersistFilePath = filePersistFilePath;
            this.CreateFolderStructure();
            this.LoadPersistedPages();
        }

        # endregion

        # region Public Methods

        public void AddPage(Page page)
        {
            string filePath = string.Concat(this.PagesDirectoryPath(), page.Id, ".xml");
            Type type = page.GetType();
            XmlSerializer serializer = new XmlSerializer(page.GetType(), this.GetPropertyTypes(page.PageType).ToArray());
            StreamWriter sw = new StreamWriter(filePath);
            serializer.Serialize(sw, page);
            sw.Close();
            sw.Dispose();

            this._cmsCacheService.AddPage(page);
        }

        public IList<IPage> GetPages()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPageType> GetPageTypes()
        {
            return this._cmsCacheService.GetPageTypes();
        }

        public IEnumerable<IPageType> GetPageTypes(Func<IPageType, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IPageType GetPageType(Func<IPageType, bool> predicate)
        {
            return this.GetPageTypes().FirstOrDefault(predicate);
        }

        public IEnumerable<IPage> GetPages(Func<IPage, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPage> GetPage(Func<IPage, bool> predicate)
        {
            throw new NotImplementedException();
        }

        # endregion

        # region Private methods

        private void LoadPersistedPages()
        {
            DirectoryInfo di = new DirectoryInfo(this.PagesDirectoryPath());
            List<Page> pages = new List<Page>();
            IEnumerable<IPageType> pageTypes = this.GetPageTypeTypes();

            foreach (var pageFile in di.EnumerateFiles())
            {
                if(pageFile.Extension.ToLower() == ".xml")
                {
                    
                }
            }

        }

        private IEnumerable<IPageType> GetPageTypeTypes()
        {
            IEnumerable<IPageType> pageTypes =  this._cmsCacheService.GetPageTypes();
            List<Type> types = new List<Type>();

            foreach (var pageType in pageTypes)
            {
                
            }

            return pageTypes;
        }

        private void CreateFolderStructure()
        {
            DirectoryInfo di = new DirectoryInfo(string.Concat(this._filePersistFilePath, LewCMSRepository.PAGES_DIRECTORY_NAME));
            
            if(!di.Exists)
            {
                di.Create();
            }
            
        }

        private string PagesDirectoryPath()
        {
            return string.Concat(this._filePersistFilePath, LewCMSRepository.PAGES_DIRECTORY_NAME);
        }

        private IEnumerable<Type> GetPropertyTypes(PageType pageType)
        {
            foreach (IProperty property in pageType.Properties)
            {
                yield return property.GetType();
            }
        }

        # endregion
    }

    public interface ILewCMSCacheService
    {
        IEnumerable<IPageType> GetPageTypes();
        IEnumerable<IPageType> GetPageTypes(Func<IPageType, bool> predicate);
        IPageType GetPageType(Func<IPageType, bool> predicate);
        void AddPage(IPage page);
        void Initialize(ILewCMSInitializeService cmsInitializeService);
    }

    public class LewCMSCacheService : ILewCMSCacheService
    {
        public LewCMSCacheService()
        {
        }

        public void Initialize(ILewCMSInitializeService cmsInitializeService)
        {
            ILewCMSInitializeService initService = cmsInitializeService ?? LewCMSConfig.Current.GetInitializeService();
            HttpRuntime.Cache["LewCMS_PageTypes"] = initService.GetPageTypes();
        }

        public IEnumerable<IPageType> GetPageTypes()
        {
            object cachedPageTypes = HttpRuntime.Cache["LewCMS_PageTypes"];

            if (cachedPageTypes == null)
            {
                throw new Exception("Cache Service is not initialized");
            }

            return (List<IPageType>)HttpRuntime.Cache["LewCMS_PageTypes"];
        }

        public IEnumerable<IPageType> GetPageTypes(Func<IPageType, bool> predicate)
        {
            return this.GetPageTypes().Where(predicate);
        }

        public IPageType GetPageType(Func<IPageType, bool> predicate)
        {
            return this.GetPageTypes().FirstOrDefault(predicate);
        }

        public void AddPage(IPage page)
        {
            List<IPage> pages = HttpRuntime.Cache["LewCMS_Pages"] as List<IPage>;

            if (pages == null)
            {
                pages = new List<IPage>();
            }

            pages.Add(page);

            HttpRuntime.Cache["LewCMS_Pages"] = pages;
        }
    }

    public interface ILewContentService
    {
        IEnumerable<IPageType> GetPageTypes();

        IPage CreatePage(string pageTypeId);
        IPage CreatePage(string pageTypeId, string pageName);
        IPage CreatePage(string pageTypeId, string pageName, string parentId);

        void UpdatePage(IPage page);
        void DeletePage(IPage page);
    }

    public class LewContentService : ILewContentService
    {
        private ILewCMSRepository _cmsRepository;

        public LewContentService() : this(LewCMSConfig.Current.GetRepository()) {}

        public LewContentService(ILewCMSRepository cmsRepository)
        {
            this._cmsRepository = cmsRepository;
        }

        public IEnumerable<IPageType> GetPageTypes()
        {
            return this._cmsRepository.GetPageTypes();
        }

        public IPage CreatePage(string pageTypeId)
        {
            return this.CreatePage(pageTypeId, "New Page");
        }

        public IPage CreatePage(string pageTypeId, string pageName)
        {
            return this.CreatePage(pageTypeId, pageName, null);
        }
        
        public IPage CreatePage(string pageTypeId, string pageName, string parentId)
        {
            if(string.IsNullOrWhiteSpace(pageTypeId))
            {
                throw new ArgumentNullException("pageTypeId");
            }

            IPageType pageType = this._cmsRepository.GetPageType(pt => pt.Id == pageTypeId);
            //Page page = pageType.CreateNewPage(pageName ?? "New Page", parentId) as Page;
            //this._cmsRepository.AddPage(page);

            return null;
        }

        public void UpdatePage(IPage page)
        {
            throw new NotImplementedException();
        }

        public void DeletePage(IPage page)
        {
            throw new NotImplementedException();
        }
    }

    public interface ILewCMSInitializeService
    {
        IList<IPageType> GetPageTypes();
    }

    public class LewCMSInitializeService : ILewCMSInitializeService
    {
        private Assembly _webApplicationAssembly;

        public LewCMSInitializeService(Assembly webApplicationAssembly)
        {
            this._webApplicationAssembly = webApplicationAssembly;
        }

        public IList<IPageType> GetPageTypes()
        {
            IEnumerable<Type> pageTypeTypes = this._webApplicationAssembly.GetTypes().Where(t => t != typeof(Page) && typeof(Page).IsAssignableFrom(t));
            List<IPageType> pageTypes = new List<IPageType>();

            foreach (Type pageType in pageTypeTypes)
            {
                PageTypeAttribute pageTypeAttribute = pageType.GetCustomAttribute<PageTypeAttribute>();
                Guid guid;
                if (pageTypeAttribute == null || string.IsNullOrWhiteSpace(pageTypeAttribute.Id) || !Guid.TryParse(pageTypeAttribute.Id, out guid))
                {
                    throw new Exception("Invalid Page Type Attribute. Id is required");
                }

                PageType _pageType = new PageType();
                _pageType.TypeName = pageType.FullName;
                _pageType.DisplayName = pageTypeAttribute.DisplayName ?? pageType.Name;
                _pageType.Id = pageTypeAttribute.Id;
                _pageType.ControllerName = string.IsNullOrWhiteSpace(pageTypeAttribute.ControllerName) ? pageType.Name : pageTypeAttribute.ControllerName;

                IEnumerable<PropertyInfo> properties = pageType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                IProperty _property;

                foreach (PropertyInfo property in properties)
                {
                    string typeName = property.PropertyType.FullName;

                    switch (typeName)
                    {
                        case "System.String":
                            _property = new PropertyString();
                            break;
                        default:
                            _property = Activator.CreateInstance(this._webApplicationAssembly.GetType(typeName)) as Property;
                            break;
                    }

                    _property.Name = property.Name;
                    _pageType.Properties.Add(_property as Property);
                }

                int pageTypeIndex = pageTypes.FindIndex(p => p.Id == _pageType.Id);

                if (pageTypeIndex > -1)
                {
                    pageTypes[pageTypeIndex] = _pageType;
                }
                else
                {
                    pageTypes.Add(_pageType);
                }

            }

            return pageTypes;
        }
    }

}
