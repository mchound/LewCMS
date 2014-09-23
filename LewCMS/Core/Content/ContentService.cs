using LewCMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.Core.Content
{
    public class ContentService : IContentService
    {
        private IContentRepository _contentRepository;
        private IRouteManager _routeManager;

        public ContentService(IContentRepository contentRepository, IRouteManager routeManager)
        {
            this._contentRepository = contentRepository;
            this._routeManager = routeManager;
        }

        public IPage Create(string pageTypeId)
        {
            return this.Create(pageTypeId, null, null);
        }

        public IPage Create(IPageType pageType)
        {
            return this.Create(pageType, null);
        }

        public IPage Create(string pageTypeId, string parentId)
        {
            return this.Create(pageTypeId, parentId, null);
        }

        public IPage Create(IPageType pageType, string parentId)
        {
            return this.Create(pageType, parentId, null);
        }

        public IPage Create(string pageTypeId, string parentId, string pageName)
        {
            IPageType pageType = this._contentRepository.GetPageTypes(pt => pt.Id == pageTypeId).FirstOrDefault();

            if (pageType == null)
            {
                throw new Exception(string.Format("No page type with id: {0} exists", pageTypeId));
            }

            return this.Create(pageType, parentId, pageName);
        }

        public IPage Create(IPageType pageType, string parentId, string pageName)
        {
            string _pageName = pageName ?? string.Concat(pageType.DisplayName, "Page");

            IPage page = Activator.CreateInstance(Application.Current.ApplicationAssembly.GetType(pageType.TypeName)) as IPage;
            page.Id = Guid.NewGuid().ToString();
            page.Route = this._routeManager.CreatePageRoute(page.Id, _pageName, parentId);
            page.Name = pageName;
            page.Version = 1;
            page.PageType = pageType as PageType;
            page.ParentId = parentId;
            page.CreatedAt = DateTime.Now;
            page.UpdatedAt = page.CreatedAt;

            page.OnInit();

            return page;
        }


        public void Save(IPage page)
        {
            this._contentRepository.Persist(page);
        }


        public IPage Upgrade(IPage page)
        {
            IPage clone = page.Clone();
            clone.Status = ContentStatus.Working;
            clone.Version++;
            return clone;
        }

        public IPage Upgrade(IPageInfo pageInfo)
        {
            IPage page = this._contentRepository.GetPage(pageInfo);
            return this.Upgrade(page);
        }

        public IPage Upgrade(string pageId)
        {
            IPage page = this._contentRepository.GetPage(pageId);
            return this.Upgrade(page);
        }


        public IEnumerable<IPage> Get()
        {
            return this._contentRepository.GetPages();
        }

        public IEnumerable<IPage> Get(ContentVersionSelect contentVersionSelect)
        {
            return this._contentRepository.GetPages(contentVersionSelect);
        }

        public IEnumerable<IPage> Get(Func<IPageInfo, bool> predicate)
        {
            return this._contentRepository.GetPages(predicate);
        }

        public IEnumerable<IPage> Get(Func<IPageInfo, bool> predicate, ContentVersionSelect contentVersionSelect)
        {
            return this._contentRepository.GetPages(predicate, contentVersionSelect);
        }


        public void Delete(IPage page)
        {
            this.Delete(page.Id, page.Version);
        }

        public void Delete(IPageInfo pageInfo)
        {
            this.Delete(pageInfo.PageId, pageInfo.Version);
        }

        public void Delete(string pageId)
        {
            IEnumerable<IPageInfo> pageInfos = this._contentRepository.GetPageInfo(pi => pi.PageId == pageId);
            foreach (IPageInfo pageInfo in pageInfos)
            {
                this.Delete(pageInfo.PageId, pageInfo.Version);
            }
        }

        public void Delete(string pageId, int version)
        {
            this._contentRepository.Delete(pageId, version);
        }


        public IEnumerable<IPageType> GetPageTypes()
        {
            return this._contentRepository.GetPageTypes();
        }
    }
}
