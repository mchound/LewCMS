using LewCMS.V2.Contents;
using LewCMS.V2.Startup;
using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Store.Cache
{
    public interface ICacheStoreService : IStoreService
    {
        void SavePersistedStoreInfo(IEnumerable<IStoreInfo> storeInfo);
        IEnumerable<IStoreInfo> LoadPersistedStoreInfo();
        void ClearCache();
    }
}
