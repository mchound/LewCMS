using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LewCMS.Core.Service
{
    public interface IContentCacheService
    {
        IEnumerable<IPageType> GetPageTypes();
        IPage GetPage(string pageId, int version);
        List<PageMetaData> GetPagesMetaData();
        void InitialCaching(IEnumerable<IPage> pages);
        void CachePage(IPage page);
        void CachePageTypes(IEnumerable<IPageType> pageTypes);
        void RemovePage(string pageId);
        void RemovePage(string pageId, int version);
        void ClearCachedPages();
        bool IsSynced { get; }
        int CachedPages { get;  }
    }

    public class ContentCacheService : IContentCacheService
    {
        # region Cache keys

        private string cacheKeyPageTypes = "LewCMS-pageTypes";
        private string cacheKeyPage = "LewCMS-page-{0}[{1}]";
        private string cacheKeyPageMetaData = "LewCMS-PageMetaData";

        # endregion

        #region Public Properties

        public bool IsSynced 
        { 
            get
            {
                return this.CacheIsSynced();
            }
        }

        public int CachedPages
        {
            get
            {
                return this.CachePages();
            }
        }

        #endregion

        public ContentCacheService()
        {
            HttpRuntime.Cache.Remove(this.cacheKeyPageTypes);
            HttpRuntime.Cache.Remove(this.cacheKeyPage);
            HttpRuntime.Cache.Remove(this.cacheKeyPageMetaData);
        }

        public void InitialCaching(IEnumerable<IPage> pages)
        {
            foreach (var page in pages)
            {
                this.CachePage(page);
            }
        }

        public IEnumerable<IPageType> GetPageTypes()
        {
            return this.GetCacheObjectByKey<IEnumerable<IPageType>>("LewCMS-pageTypes");
        }

        public IPage GetPage(string pageId, int version)
        {
            string pageCacheKey = string.Empty;

            if (version == -1)
            {
                int latestVersion = this.GetLatestPageVersionNumber(pageId);
                pageCacheKey = string.Format(this.cacheKeyPage, pageId, latestVersion);
            }
            else
            {
                pageCacheKey = string.Format(this.cacheKeyPage, pageId, version);
            }
            
            return HttpRuntime.Cache[pageCacheKey] as IPage;
        }

        public void CachePage(IPage page)
        {
            this.AddToCacheList<PageMetaData>(new PageMetaData(page), this.cacheKeyPageMetaData);
            this.CacheObject(page, string.Format(this.cacheKeyPage, page.Id, page.Version));
        }

        public void CachePageTypes(IEnumerable<IPageType> pageTypes)
        {
            this.CacheObject(pageTypes, this.cacheKeyPageTypes);
        }

        public void ClearCachedPages()
        {
            IEnumerable<PageMetaData> pagesMetaData = this.GetCacheObjectByKey<IEnumerable<PageMetaData>>(this.cacheKeyPageMetaData);
            string pageCacheKey = string.Empty;

            foreach (PageMetaData metaData in pagesMetaData)
	        {
		        pageCacheKey = string.Format(this.cacheKeyPage, metaData.PageId, metaData.Version);
                HttpRuntime.Cache.Remove(pageCacheKey);
	        }

            HttpRuntime.Cache.Remove(this.cacheKeyPageMetaData);
        }

        public List<PageMetaData> GetPagesMetaData()
        {
            return this.GetCacheObjectByKey<List<PageMetaData>>(this.cacheKeyPageMetaData);
        }

        public void RemovePage(string pageId)
        {
            IEnumerable<PageMetaData> pageMetaData = this.MetaDataForPage(pageId).ToList();

            foreach (var page in pageMetaData)
            {
                HttpRuntime.Cache.Remove(string.Format(this.cacheKeyPage, pageId, page.Version));
                this.RemoveFromCacheList<PageMetaData>(m => m.PageId == pageId && m.Version == page.Version, this.cacheKeyPageMetaData);   
            }
        }

        public void RemovePage(string pageId, int version)
        {
            HttpRuntime.Cache.Remove(string.Format(this.cacheKeyPage, pageId, version));
            this.RemoveFromCacheList<PageMetaData>(m => m.PageId == pageId && m.Version == version, this.cacheKeyPageMetaData);
        }

        #region Private Methods

        private void CacheObject(object obj, string cacheKey)
        {
            HttpRuntime.Cache[cacheKey] = obj;
        }

        private T GetCacheObjectByKey<T>(string cacheKey)
        {
            object obj = HttpRuntime.Cache.Get(cacheKey);

            if (obj == null)
                return default(T);

            return (T)obj;
        }

        private void AddToCacheList<T>(T obj, string cacheKey)
        {
            List<T> list = HttpRuntime.Cache[cacheKey] as List<T>;

            if (list == null)
            {
                list = new List<T>();
            }

            list.Add(obj);

            HttpRuntime.Cache[cacheKey] = list;
        }

        private void RemoveFromCacheList<T>(Func<T, bool> predicate, string cacheKey) where T : class
        {
            List<T> list = HttpRuntime.Cache[cacheKey] as List<T>;
            
            if(list == null)
            {
                return;
            }

            T obj = list.FirstOrDefault(predicate);

            if(obj == null)
            {
                return;
            }

            list.Remove(obj);

            HttpRuntime.Cache[cacheKey] = list;
        }

        private bool CacheIsSynced()
        {
            // TODO: implement check for cache sync
            return true;
        }

        private int CachePages()
        {
            List<PageMetaData> pagesMetaData = this.GetPagesMetaData();
            if(pagesMetaData == null)
            {
                return 0;
            }

            return this.GetPagesMetaData().Count;
        }

        private int GetLatestPageVersionNumber(string pageId)
        {
            IEnumerable<PageMetaData> pagesMetaData = this.GetPagesMetaData();

            if (pagesMetaData == null || pagesMetaData.Count() == 0)
            {
                return 1;
            }

            IEnumerable<PageMetaData> metaDataForPage = pagesMetaData.Where(p => p.PageId == pageId);

            if (metaDataForPage == null || metaDataForPage.Count() == 0)
            {
                return 1;
            }

            return metaDataForPage.Max(p => p.Version);
        }

        private IEnumerable<PageMetaData> MetaDataForPage(string pageId)
        {
            IEnumerable<PageMetaData> pagesMetaData = this.GetPagesMetaData();

            return pagesMetaData == null ? Enumerable.Empty<PageMetaData>() : pagesMetaData.Where(p => p.PageId == pageId);
        }

        #endregion

    }
}
