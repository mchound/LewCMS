using LewCMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.Core.Content
{
    public class ContentRepository : IContentRepository
    {
        private IContentCacheService _contentCacheService;
        private IPersistService _filePersistManager;
        private IInitializeService _initializeService;

        public ContentRepository(IContentCacheService contentCacheService, IPersistService filePersistManager, IInitializeService initializeService)
        {
            this._contentCacheService = contentCacheService;
            this._filePersistManager = filePersistManager;
            this._initializeService = initializeService;
            var pageTypes = this._initializeService.GetPageTypes(Application.Current.ApplicationAssembly);
            this._filePersistManager.Initialize(pageTypes);
            this._contentCacheService.Initialize(pageTypes);
        }


        public IEnumerable<IPageType> GetPageTypes()
        {
            IEnumerable<IPageType> pageTypes = this._contentCacheService.LoadPageTypes();

            if (pageTypes == null)
            {
                pageTypes = this._filePersistManager.LoadPageTypes();
                this._contentCacheService.Initialize(pageTypes);
            }

            return pageTypes;
        }

        public IEnumerable<IPageType> GetPageTypes(Func<IPageType, bool> predicate)
        {
            return this.GetPageTypes().Where(predicate);
        }


        public IPageInfo GetPageInfoFor(Func<IPageInfo, bool> predicate)
        {
            IPageInfo pageInfo = this._contentCacheService.LoadPageInfo(predicate);

            if (pageInfo == null)
            {
                pageInfo = this._filePersistManager.LoadPageInfo(predicate);
            }

            return pageInfo;
        }

        public IEnumerable<IPageInfo> GetPageInfo()
        {
            IEnumerable<IPageInfo> pageInfos = this._contentCacheService.LoadPersistedPagesList();

            if (pageInfos == null)
            {
                pageInfos = this._filePersistManager.LoadPageInfos();
                this._contentCacheService.SavePersistedPagesList(pageInfos);
            }

            return pageInfos;
        }

        public IEnumerable<IPageInfo> GetPageInfo(Func<IPageInfo, bool> predicate)
        {
            return this.GetPageInfo().Where(predicate);
        }


        public IPage GetPage(string pageId)
        {
            int latestVersion = this.GetPageVersions(pageId);
            return this.GetPage(pageId, latestVersion);
        }

        public IPage GetPage(IPageInfo pageInfo)
        {
            return this.GetPage(pageInfo.PageId, pageInfo.Version);
        }

        public IPage GetPage(string pageId, int version)
        {
            IPage page = this._contentCacheService.LoadPage(pageId, version);

            if (page == null)
            {
                page = this._filePersistManager.LoadPage(pageId, version);
                this._contentCacheService.SavePage(page);
            }

            return page;
        }


        public IEnumerable<IPage> GetPages()
        {
            IEnumerable<IPageInfo> persistedPages = this.GetPageInfo();

            foreach (IPageInfo pageInfo in persistedPages)
            {
                yield return this.GetPage(pageInfo);
            }
        }

        public IEnumerable<IPage> GetPages(ContentVersionSelect contentVersionSelect)
        {
            switch (contentVersionSelect)
            {
                case ContentVersionSelect.All:
                    return this.GetPages();

                case ContentVersionSelect.Latest:
                    IEnumerable<IPageInfo> persistedPages = this.GetPageInfo();
                    return persistedPages.GroupBy(pi => pi.PageId).Select(g => this.GetPage(g.OrderBy(pi => pi.Version).First()));

                default:
                    return Enumerable.Empty<IPage>();
            }
        }

        public IEnumerable<IPage> GetPages(Func<IPageInfo, bool> predicate)
        {
            IEnumerable<IPageInfo> persistedPages = this.GetPageInfo(predicate);
            return persistedPages.Select(pi => this.GetPage(pi.PageId));
        }

        public IEnumerable<IPage> GetPages(Func<IPageInfo, bool> predicate, ContentVersionSelect contentVersionSelect)
        {
            switch (contentVersionSelect)
            {
                case ContentVersionSelect.All:
                    return this.GetPages(predicate);

                case ContentVersionSelect.Latest:
                    IEnumerable<IPageInfo> persistedPages = this.GetPageInfo(predicate);
                    return persistedPages.GroupBy(pi => pi.PageId).Select(g => this.GetPage(g.OrderBy(pi => pi.Version).First()));

                default:
                    return Enumerable.Empty<IPage>();
            }
        }


        public int GetPageVersions(string pageId)
        {
            return this.GetPageInfo(pi => pi.PageId == pageId).Max(pi => pi.Version);
        }

        public int GetPageVersions(IPageInfo pageInfo)
        {
            return this.GetPageVersions(pageInfo.PageId);
        }

        public int GetPageVersions(IPage page)
        {
            return this.GetPageVersions(page.Id);
        }

        public void Persist(IPage page)
        {
            this._contentCacheService.SavePage(page);
            IEnumerable<IPageInfo> persistedPageInfos = this._filePersistManager.SavePage(page);
            this._contentCacheService.SavePersistedPagesList(persistedPageInfos);
        }


        public void Delete(string pageId, int version)
        {
            IEnumerable<IPageInfo> pageInfos = this._filePersistManager.Delete(pageId, version);
            this._contentCacheService.Delete(pageId, version);
            this._contentCacheService.SavePersistedPagesList(pageInfos);
        }
    }
}
