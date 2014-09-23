using LewCMS.Core.Utilities;
using LewCMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace LewCMS.Core.Content
{
    public interface IContentService
    {
        IPage Create(string pageTypeId);
        IPage Create(IPageType pageType);
        IPage Create(string pageTypeId, string parentId);
        IPage Create(IPageType pageType, string parentId);
        IPage Create(string pageTypeId, string parentId, string pageName);
        IPage Create(IPageType pageType, string parentId, string pageName);

        void Save(IPage page);

        IPage Upgrade(IPage page);
        IPage Upgrade(IPageInfo pageInfo);
        IPage Upgrade(string pageId);

        void Delete(IPage page);
        void Delete(IPageInfo pageInfo);
        void Delete(string pageId);
        void Delete(string pageId, int version);

        IEnumerable<IPage> Get();
        IEnumerable<IPage> Get(ContentVersionSelect contentVersionSelect);
        IEnumerable<IPage> Get(Func<IPageInfo, bool> predicate);
        IEnumerable<IPage> Get(Func<IPageInfo, bool> predicate, ContentVersionSelect contentVersionSelect);
    }
}
