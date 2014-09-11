using LewCMS.Core.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Serialization;

namespace LewCMS.Core.Service
{
    public interface IContentRepository
    {
        IEnumerable<IPageType> GetPageTypes();
        IPage GetPage(string pageId);
        IEnumerable<IPage> GetAllPages();
        IEnumerable<IPage> GetPages(Func<PageMetaData, bool> predicate);
        IPage AddPage(IPage page);
        void UpdatePage(IPage page);
        void DeletePage(string pageId);
    }

    public class ContentRepository : IContentRepository
    {
        #region Constants

        private const string PAGES_FOLDER_NAME = "Pages";

        #endregion

        #region Private Properties

        private IInitializeService _initializeService;
        private IContentCacheService _contentCacheService;
        private ISerializer _serializer;
        private string _filePersistDirectoryPath = string.Empty;

        #endregion

        #region Constructors

        public ContentRepository(IInitializeService initializeService, IContentCacheService contentCacheService, ISerializer serializer) : this(initializeService, contentCacheService, serializer, Configuration.PERSITS_VIRTUAL_FILE_PATH)
        {}

        public ContentRepository(IInitializeService initializeService, IContentCacheService contentCacheService, ISerializer serializer, string filePersistDirectoryPath)
        {
            this._initializeService = initializeService;
            this._contentCacheService = contentCacheService;
            this._serializer = serializer;
            this._filePersistDirectoryPath = filePersistDirectoryPath;

            this._contentCacheService.InitialCaching(this.LoadAllPages());
        }

        #endregion

        #region Public Methods

        public IEnumerable<IPageType> GetPageTypes()
        {
            IEnumerable<IPageType> pageTypes = this._contentCacheService.GetPageTypes();
            if (pageTypes == null)
            {
                pageTypes = this._initializeService.GetPageTypes(Application.Current.ApplicationAssembly);
                this._contentCacheService.CachePageTypes(pageTypes);
            }

            return pageTypes;
        }

        public IPage GetPage(string pageId)
        {
            IPage page = this._contentCacheService.GetPage(pageId);

            if(page == null)
            {
                page = this.LoadPage(pageId);
                this._contentCacheService.CachePage(page);
            }

            return page;
        }

        public IEnumerable<IPage> GetAllPages()
        {
            // TODO: If cache is in sync. Use cached pages, else get all cached pages and compare with stored metadata file.
            IEnumerable<PageMetaData> pagesMetaData = this._contentCacheService.GetPagesMetaData() ?? this.LoadPagesMetaData();

            foreach (PageMetaData metaData in pagesMetaData)
            {
                yield return this.GetPage(metaData.PageId);
            }

        }

        public IEnumerable<IPage> GetPages(Func<PageMetaData, bool> predicate)
        {
            IEnumerable<PageMetaData> pagesMetaData = this._contentCacheService.GetPagesMetaData() ?? this.LoadPagesMetaData();

            foreach (PageMetaData metaData in pagesMetaData.Where(predicate))
            {
                yield return this.GetPage(metaData.PageId);
            }
        }

        public IPage AddPage(IPage page)
        {
            DirectoryInfo pagesDirectory = this.GetDirectory(ContentRepository.PAGES_FOLDER_NAME);
            
            if (this.PageIsCreated(page, pagesDirectory))
            {
                throw new Exception("Page with that id is already persisted. Use update to change an existing page");
            }

            FileInfo shallowPage = this.CreateShallowPageFile(page, pagesDirectory);

            try { this.SavePage(page, pagesDirectory); }
            catch (Exception ex) { throw new Exception(string.Format("Error trying to save page with id: {0}", page.Id), ex); }

            try{ this.SavePageMetaData(page, pagesDirectory); }
            catch (Exception ex) 
            {   // TODO: Delete page
                throw new Exception(string.Format("Error trying to save MetaData. Page id: {0}", page.Id), ex);
            }

            try { this._contentCacheService.CachePage(page); }
            catch (Exception ex) { throw new Exception(string.Format("Error trying to cache page with id: {0}", page.Id), ex); }

            return page;
        }

        public void UpdatePage(IPage page)
        {
            throw new NotImplementedException();
        }

        public void DeletePage(string pageId)
        {
            try
            {
                this.DeletePageFromFile(pageId);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error trying to delete page with id: {0}", pageId), ex);
            }

            this._contentCacheService.RemovePage(pageId);
            
        }

        #endregion

        #region Private Methods

        private List<PageMetaData> LoadPagesMetaData()
        {
            DirectoryInfo pagesFolder = this.GetDirectory(ContentRepository.PAGES_FOLDER_NAME);
            IEnumerable<FileInfo> fileInfoPagesMetaData = pagesFolder.GetFiles("MetaData.json");

            if(fileInfoPagesMetaData.Count() == 0)
            {
                return new List<PageMetaData>();
            }

            return this._serializer.DeserializeFromFile<List<PageMetaData>>(fileInfoPagesMetaData.First().FullName);
        }

        private PageMetaData GetPageMetaData(string pageId)
        {
            return this.LoadPagesMetaData().First(m => m.PageId == pageId);
        }

        private IPage LoadPage(string pageId)
        {
            PageMetaData pageMetaData = this.GetPageMetaData(pageId);
            DirectoryInfo pagesFolder = this.GetDirectory(ContentRepository.PAGES_FOLDER_NAME);
            IEnumerable<FileInfo> allVersions = pagesFolder.GetFiles(string.Format("{0}[*].json", pageId)).OrderBy(f => f.Name);
            FileInfo latestVersion = allVersions.Last();

            return this._serializer.DeserializeFromFile<IPage>(latestVersion.FullName, pageMetaData.GetPageInstanceType());
        }

        private void SavePage(IPage page, DirectoryInfo pagesDirectory)
        {
            FileInfo fileInfoPage = this.CreateShallowPageFile(page, pagesDirectory);
            this._serializer.SerializeToFile<IPage>(page, fileInfoPage.FullName);
        }

        private void SavePageMetaData(IPage page, DirectoryInfo pagesDirectory)
        {
            List<PageMetaData> pagesMetaData = this.LoadPagesMetaData();

            PageMetaData pageMetaData = pagesMetaData.FirstOrDefault(m => m.PageId == page.Id);

            if(pageMetaData == null)
            {
                pageMetaData = new PageMetaData(page);
                pagesMetaData.Add(pageMetaData);
                this.SavePagesMetaData(pagesMetaData);
                return;
            }

            pageMetaData = new PageMetaData(page);
            this.SavePagesMetaData(pagesMetaData);
        }

        private void SavePagesMetaData(List<PageMetaData> pagesMetaData)
        {
            FileInfo fileInfoPageMetaData = this.CreateShallowPageMetaDataFile(this.GetDirectory(ContentRepository.PAGES_FOLDER_NAME));
            this._serializer.SerializeToFile<List<PageMetaData>>(pagesMetaData, fileInfoPageMetaData.FullName);
        }

        private FileInfo CreateShallowPageFile(IPage page, DirectoryInfo pagesDirectory)
        {
            string filePath = string.Format(@"{0}\{1}[1].json", pagesDirectory.FullName, page.Id);
            return new FileInfo(filePath);
        }

        private FileInfo CreateShallowPageMetaDataFile(DirectoryInfo pagesDirectory)
        {
            string filePath = string.Format(@"{0}\MetaData.json", pagesDirectory.FullName);
            return new FileInfo(filePath);
        }

        private IEnumerable<IPage> LoadAllPages()
        {
            IEnumerable<PageMetaData> metaData = this.LoadPagesMetaData();
            return metaData.Select(m => this.LoadPage(m.PageId));
        }

        /// <summary>
        /// Checks if any page with the page's id already exists in the given folder.
        /// </summary>
        /// <param name="page">The page instance</param>
        /// <param name="pagesDirectory">The directory to search</param>
        /// <returns>True: if a page with the page's id already exists. False if no file with the page's id exists in the given folder.</returns>
        private bool PageIsCreated(IPage page, DirectoryInfo pagesDirectory)
        {
            string searchPattern = string.Concat(page.Id, "[*].xml");
            var earlierVersionsOfPage = pagesDirectory.EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly);

            return earlierVersionsOfPage == null ? true : false;
        }

        private DirectoryInfo GetDirectory(string folderName)
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine(this._filePersistDirectoryPath, folderName));

            if(!di.Exists)
            {
                di.Create();
            }

            return di;
        }

        private void DeletePageFromFile(string pageId)
        {
            List<PageMetaData> pagesMetaData = this.LoadPagesMetaData();
            PageMetaData pageMetaData = pagesMetaData.FirstOrDefault(m => m.PageId == pageId);
            pagesMetaData.Remove(pageMetaData);

            DirectoryInfo pagesDirectoryInfo = this.GetDirectory(ContentRepository.PAGES_FOLDER_NAME);

            pagesDirectoryInfo.GetFiles(string.Format("{0}[*].json", pageId)).First().Delete();

            this.SavePagesMetaData(pagesMetaData);
        }

        #endregion
    }
}
