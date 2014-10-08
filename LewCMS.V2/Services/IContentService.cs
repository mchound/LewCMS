using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Services
{
    //public interface IContentService
    //{
    //    IEnumerable<IPageType> GetPageTypes();
    //    IEnumerable<IPageType> GetPageTypes(Func<IPageInfo, bool> predicate);

    //    IEnumerable<ISectionType> GetSectionTypes();
    //    IEnumerable<ISectionType> GetSectionTypes(Func<ISectionInfo, bool> predicate);

    //    IEnumerable<IGlobalConfigType> GetGlobalConfigTypes();
    //    IEnumerable<IGlobalConfigType> GetGlobalConfigTypes(Func<IGlobalConfigInfo, bool> predicate);

    //    IContent GetContentFor(Func<IContentInfo, bool> predicate);
    //    T GetContentFor<T>(Func<IContentInfo, bool> predicate) where T : class, IContent;
    //    Tcontent GetContentFor<Tcontent, Tinfo>(Func<Tinfo, bool> predicate) where Tcontent : class, IContent where Tinfo : class, IContentInfo;

    //    IEnumerable<IContent> GetContent();
    //    IEnumerable<IContent> GetContent(Func<IContentInfo, bool> predicate);
    //    IEnumerable<T> GetContent<T>(Func<IContentInfo, bool> predicate) where T : class, IContent;
    //    IEnumerable<Tcontent> GetContent<Tcontent, Tinfo>(Func<Tinfo, bool> predicate) where Tcontent : class, IContent where Tinfo : class, IContentInfo;

    //    IContent CreateContent(IContentType contentType, string name);
    //    IPage CreatePage(IPageType pageType, string name);
    //    IPage CreatePage(IPageType pageType, string name, string parentId);
    //}

    //public class DefaultContentService
    //{

    //    IContentRepository _contentRepository;
    //    IRouteManager _routeManager;

    //    public DefaultContentService(IContentRepository contentRepository, IRouteManager routeManager)
    //    {
    //        this._contentRepository = contentRepository;
    //        this._routeManager = routeManager;
    //    }
    //}
}
