using LewCMS.V2.Contents;
using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LewCMS.V2
{
    public interface IRouteManager
    {
        string CreatePageRoute(string pageId, string pageName, string parentId);
    }

    public class RouteManager : IRouteManager
    {
        IRepository _repository;

        public RouteManager(IRepository repository)
        {
            this._repository = repository;
        }

        public string CreatePageRoute(string pageId, string pageName, string parentId)
        {

            if(this._repository.GetStoreInfo<IPageInfo>().Count() == 0 && string.IsNullOrWhiteSpace(parentId))
            {
                return "/";
            }

            string parentRoute = string.IsNullOrWhiteSpace(parentId) ? string.Empty : this._repository.GetStoreInfo<IPageInfo>(pi => pi.Id == parentId).FirstOrDefault().Route;
            string route = string.Concat(parentRoute, "/", HttpUtility.UrlEncode(pageName.ToLower()).Replace("+", "-"));
            return this.AdjustForDuplicateRoutes(pageId, route, firstIteration: true);
        }

        private string AdjustForDuplicateRoutes(string pageId, string route, bool firstIteration = false)
        {
            IEnumerable<IPageInfo> storeInfos = this._repository.GetStoreInfo<IPageInfo>().Where(pi => pi.Route.ToLower() == route.ToLower() && pi.Id != pageId);

            if (!storeInfos.Any())
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
