using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using LewCMS.Core.Utilities;

namespace LewCMS.Core.Service
{
    public interface IContentService
    {
        IEnumerable<IPageType> GetPageTypes();
        IPage GetPage(string pageId);
        IEnumerable<IPage> GetPages(Func<PageMetaData, bool> predicate);
        IEnumerable<IPage> GetAllPages();
        IPage AddPage(string pageTypeId, string pageName);
        IPage AddPage(string pageTypeId, string pageName, string parentId);
        IPage UpdatePage(IPage page);
        void DeletePage(string pageId);
    }

    public class ContentService : IContentService
    {
        private IContentRepository _contentRepository;

        public ContentService(IContentRepository contentRepository)
        {
            this._contentRepository = contentRepository;
        }

        public IEnumerable<IPageType> GetPageTypes()
        {
            return this._contentRepository.GetPageTypes();
        }

        public IPage GetPage(string pageId)
        {
            return this._contentRepository.GetPage(pageId, -1);
        }

        public IPage AddPage(string pageTypeId, string pageName)
        {
            return this.AddPage(pageTypeId, pageName, null);
        }

        public IPage AddPage(string pageTypeId, string pageName, string parentId)
        {
            var pageType = this.GetPageTypes().FirstOrDefault(p => p.Id == pageTypeId);
            IPage page = Activator.CreateInstance(Application.Current.ApplicationAssembly.GetType(pageType.TypeName)) as IPage;
            page.Id = Guid.NewGuid().ToString();
            page.Route = RouteHelper.CreatePageRoute(this._contentRepository, page.Id, pageName, parentId);
            page.Name = pageName;
            page.Version = 1;
            page.PageType = pageType as PageType;
            page.ParentId = parentId;
            page.CreatedAt = DateTime.Now;
            page.UpdatedAt = page.CreatedAt;

            page.OnInit();
            this._contentRepository.AddPage(page);

            return page;
        }

        public IPage UpdatePage(IPage page)
        {
            page.Route = this.CreatePageRoute(page.Id, page.Name, page.ParentId);
            return this._contentRepository.UpdatePage(page);
        }

        public void DeletePage(string pageId)
        {
            this._contentRepository.DeletePage(pageId);
        }

        public IEnumerable<IPage> GetPages(Func<PageMetaData, bool> predicate)
        {
            return this._contentRepository.GetPages(predicate);
        }

        public IEnumerable<IPage> GetAllPages()
        {
            return this._contentRepository.GetAllPages();
        }

        # region Private Methods

        //private string CreatePageRoute(string pageId, string pageName, string parentId)
        //{
        //    string parentRoute = string.IsNullOrWhiteSpace(parentId) ? string.Empty : this._contentRepository.GetPageMetaData(m => m.PageId == parentId).PageRoute;
        //    string route = string.Concat(parentRoute, "/", HttpUtility.UrlEncode(pageName.ToLower()).Replace("+", "-"));
        //    return this.GetPageRouteWithSuffix(pageId, route, firstIteration: true);
        //}

        //private string GetPageRouteWithSuffix(string pageId, string route, bool firstIteration = false)
        //{
        //    IEnumerable<PageMetaData> pagesMetaData = this._contentRepository.GetPagesMetaData(m => m.PageRoute.ToLower() == route.ToLower() && m.PageId != pageId);

        //    if (!pagesMetaData.Any())
        //    {
        //        return route;
        //    }

        //    string newRoute = route;

        //    if (firstIteration)
        //    {
        //        newRoute = string.Concat(newRoute, "-1");
        //        return this.GetPageRouteWithSuffix(pageId, newRoute);
        //    }

        //    string[] routeFragments = route.Split('-');
        //    int enumeration = int.Parse(routeFragments.Last()) + 1;
        //    routeFragments[routeFragments.Length - 1] = enumeration.ToString();

        //    return this.GetPageRouteWithSuffix(pageId, string.Join("-", routeFragments));
        //}

        # endregion
    }
}
