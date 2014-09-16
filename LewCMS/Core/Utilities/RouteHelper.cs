using LewCMS.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LewCMS.Core.Utilities
{
    public static class RouteHelper
    {
        public static string CreatePageRoute(IContentRepository contentRepository, string pageId, string pageName, string parentId)
        {
            string parentRoute = string.IsNullOrWhiteSpace(parentId) ? string.Empty : contentRepository.GetPageMetaData(m => m.PageId == parentId).PageRoute;
            string route = string.Concat(parentRoute, "/", HttpUtility.UrlEncode(pageName.ToLower()).Replace("+", "-"));
            return RouteHelper.AdjustWithSuffixIfDuplicates(contentRepository, pageId, route, firstIteration: true);
        }

        private static string AdjustWithSuffixIfDuplicates(IContentRepository contentRepository, string pageId, string route, bool firstIteration = false)
        {
            IEnumerable<PageMetaData> pagesMetaData = contentRepository.GetPagesMetaData(m => m.PageRoute.ToLower() == route.ToLower() && m.PageId != pageId);

            if (!pagesMetaData.Any())
            {
                return route;
            }

            string newRoute = route;

            if (firstIteration)
            {
                newRoute = string.Concat(newRoute, "-1");
                return RouteHelper.AdjustWithSuffixIfDuplicates(contentRepository, pageId, newRoute);
            }

            string[] routeFragments = route.Split('-');
            int enumeration = int.Parse(routeFragments.Last()) + 1;
            routeFragments[routeFragments.Length - 1] = enumeration.ToString();

            return RouteHelper.AdjustWithSuffixIfDuplicates(contentRepository, pageId, string.Join("-", routeFragments));
        }
    }
}
