using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LewCMS.V2.Services
{
    public interface IRouteManager
    {
        string CreatePageRoute(string pageId, string pageName, string parentId);
    }

    public class RouteManager : IRouteManager
    {
        IContentRepository _contentRepository;

        public RouteManager(IContentRepository contentRepository)
        {
            this._contentRepository = contentRepository;
        }

        public string CreatePageRoute(string pageId, string pageName, string parentId)
        {
            string parentRoute = string.IsNullOrWhiteSpace(parentId) ? string.Empty : this._contentRepository.GetContentInfoFor<IPageInfo>(pi => pi.Id == parentId).Route;
            string route = string.Concat(parentRoute, "/", HttpUtility.UrlEncode(pageName.ToLower()).Replace("+", "-"));
            return this.AdjustForDuplicateRoutes(pageId, route, firstIteration: true);
        }

        private string AdjustForDuplicateRoutes(string pageId, string route, bool firstIteration = false)
        {
            IEnumerable<IPageInfo> pagesMetaData = this._contentRepository.GetContentInfo().Select(ci => ci as IPageInfo).Where(pi => pi.Route.ToLower() == route.ToLower() && pi.Id != pageId);

            if (!pagesMetaData.Any())
            {
                return route;
            }

            string newRoute = route;

            if (firstIteration)
            {
                newRoute = string.Concat(newRoute, "-1");
                return this.AdjustForDuplicateRoutes(pageId, newRoute);
            }

            string[] routeFragments = route.Split('-');
            int enumeration = int.Parse(routeFragments.Last()) + 1;
            routeFragments[routeFragments.Length - 1] = enumeration.ToString();

            return this.AdjustForDuplicateRoutes(pageId, string.Join("-", routeFragments));
        }
    }
}
