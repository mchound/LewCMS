using LewCMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LewCMS.Core.Content
{
    public class ContentCacheService : BasePersistService, IContentCacheService
    {
        private const string PAGE_CACHE_KEY_FORMAT = "LewCMS.Cache.Page-{0}[{1}]";
        private const string PAGE_TYPES_CACHE_KEY_FORMAT = "LewCMS.Cache.PageTypes";
        private const string PAGE_INFO_CACHE_KEY_FORMAT = "LewCMS.Cache.PageInfo";
        private const string PERSISTED_PAGE_INFO_CACHE_KEY_FORMAT = "LewCMS.Cache.PersistedPagesList";

        public void SavePersistedPagesList(IEnumerable<IPageInfo> pageInfos)
        {
            this.InsertIntoCache(PERSISTED_PAGE_INFO_CACHE_KEY_FORMAT, pageInfos);
        }

        public IEnumerable<IPageInfo> LoadPersistedPagesList()
        {
            return this.LoadFromCache<IEnumerable<IPageInfo>>(PERSISTED_PAGE_INFO_CACHE_KEY_FORMAT);
        }


        public override void Initialize(IEnumerable<IPageType> pageTypes)
        {
            this.InsertIntoCache(PAGE_TYPES_CACHE_KEY_FORMAT, pageTypes);
        }


        public override IEnumerable<IPageType> LoadPageTypes()
        {
            return this.LoadFromCache<IEnumerable<IPageType>>(PAGE_TYPES_CACHE_KEY_FORMAT);
        }


        public override IEnumerable<IPageInfo> SavePage(IPage page)
        {
            if (page == null)
            {
                return this.LoadPageInfos();
            }

            string cacheKey = string.Format(PAGE_CACHE_KEY_FORMAT, page.Id, page.Version);
            this.InsertIntoCache(cacheKey, page);
            return this.UpdatePageInfo(page, PageInfoAction.AddOrUpdate);
        }


        public override IPage LoadPage(string pageId, int pageVersion)
        {
            string cacheKey = string.Format(PAGE_CACHE_KEY_FORMAT, pageId, pageVersion);
            return this.LoadFromCache<IPage>(cacheKey);
        }

        
        public override IEnumerable<IPageInfo> LoadPageInfos()
        {
            IEnumerable<IPageInfo> pageInfos = this.LoadFromCache<IEnumerable<IPageInfo>>(PAGE_INFO_CACHE_KEY_FORMAT);

            if (pageInfos == null)
            {
                return new List<IPageInfo>();
            }

            return pageInfos.ToList();
        }


        public override IEnumerable<IPageInfo> Delete(string pageId, int version)
        {
            IPage page = this.LoadPage(pageId, version);
            string cacheKey = string.Format(PAGE_CACHE_KEY_FORMAT, pageId, version);
            this.RemoveFromCache(cacheKey);
            return this.UpdatePageInfo(page, PageInfoAction.Delete);
        }


        public void ClearCache()
        {
            IEnumerable<IPageInfo> cachedPages = this.LoadPageInfos();

            foreach (IPageInfo pageInfo in cachedPages)
            {
                this.Delete(pageInfo.PageId, pageInfo.Version);
            }

            this.RemoveFromCache(PAGE_INFO_CACHE_KEY_FORMAT);
            this.RemoveFromCache(PAGE_TYPES_CACHE_KEY_FORMAT);
            this.RemoveFromCache(PERSISTED_PAGE_INFO_CACHE_KEY_FORMAT);
        }

        // PRivate Methods

        private void InsertIntoCache(string cacheKey, object obj)
        {
            HttpRuntime.Cache[cacheKey] = obj;
        }

        private T LoadFromCache<T>(string cacheKey)
        {
            object obj = HttpRuntime.Cache[cacheKey];

            if (obj == null)
            {
                return default(T);
            }

            return (T)obj;
        }

        private void RemoveFromCache(string cacheKey)
        {
            HttpRuntime.Cache.Remove(cacheKey);
        }

        protected override void SavePageInfos(IEnumerable<IPageInfo> pageInfos)
        {
            this.InsertIntoCache(PAGE_INFO_CACHE_KEY_FORMAT, pageInfos);
        }
    }
}
