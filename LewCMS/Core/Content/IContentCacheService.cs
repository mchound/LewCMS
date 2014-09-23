using LewCMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LewCMS.Core.Content
{
    public interface IContentCacheService : IPersistService
    {
        void SavePersistedPagesList(IEnumerable<IPageInfo> pageInfos);
        IEnumerable<IPageInfo> LoadPersistedPagesList();
        void ClearCache();
    }
}
