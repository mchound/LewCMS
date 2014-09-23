using LewCMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.Core.Content
{
    public interface IContentRepository
    {
        IEnumerable<IPageType> GetPageTypes();
        IEnumerable<IPageType> GetPageTypes(Func<IPageType, bool> predicate);

        IPageInfo GetPageInfoFor(Func<IPageInfo, bool> predicate);
        IEnumerable<IPageInfo> GetPageInfo();
        IEnumerable<IPageInfo> GetPageInfo(Func<IPageInfo, bool> predicate);

        IPage GetPage(string pageId);
        IPage GetPage(string pageId, int version);
        IPage GetPage(IPageInfo pageInfo);

        IEnumerable<IPage> GetPages();
        IEnumerable<IPage> GetPages(ContentVersionSelect contentVersionSelect);
        IEnumerable<IPage> GetPages(Func<IPageInfo, bool> predicate);
        IEnumerable<IPage> GetPages(Func<IPageInfo, bool> predicate, ContentVersionSelect contentVersionSelect);

        int GetPageVersions(string pageId);
        int GetPageVersions(IPageInfo pageInfo);
        int GetPageVersions(IPage page);

        void Persist(IPage page);

        void Delete(string pageId, int version);
    }
}
