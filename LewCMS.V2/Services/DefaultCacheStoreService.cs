using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LewCMS.V2.Services
{
    public interface ICacheStoreService : IStoreService
    {
        void SavePersistedStoreInfo(IEnumerable<IStoreInfo> storeInfo);
        IEnumerable<IStoreInfo> LoadPersistedStoreInfo();
        void SaveContentTypes(IEnumerable<IContentType> contentTypes);
        void ClearCache();
    }

    public class DefaultCacheStoreService : BaseStoreService, ICacheStoreService
    {
        private const string STORE_KEY_PREFIX = "LewCMS.Cache.";

        private const string PERSISTED_STORE_DIRECTORY_KEY_FORMAT = "LewCMS.Cache.Persisted.StoreDirectory";

        protected override string STORE_DIRECTORY_KEY_FORMAT
        {
            get { return string.Concat(STORE_KEY_PREFIX, "StoreDirectory"); }
        }

        protected override string CONTENT_TYPES_KEY_FORMAT
        {
            get { return "LewCMS.Cache.ContentTypes"; }
        }

        public DefaultCacheStoreService()
        {

        }

        public void SavePersistedStoreInfo(IEnumerable<IStoreInfo> storeInfo)
        {
            this.Save<IEnumerable<IStoreInfo>>(PERSISTED_STORE_DIRECTORY_KEY_FORMAT, storeInfo);
        }

        public IEnumerable<IStoreInfo> LoadPersistedStoreInfo()
        {
            return this.Load<IEnumerable<IStoreInfo>>(PERSISTED_STORE_DIRECTORY_KEY_FORMAT);
        }

        public void SaveContentTypes(IEnumerable<IContentType> contentTypes)
        {
            this.Save<IEnumerable<IContentType>>(this.CONTENT_TYPES_KEY_FORMAT, contentTypes);
        }

        public void ClearCache()
        {
            IEnumerable<IStoreInfo> storeInfo = this.LoadStoreInfo() ?? Enumerable.Empty<IStoreInfo>();
            string key = string.Empty;

            foreach (var info in storeInfo)
            {
                key = this.CreateKey(info);
                this.Delete(key);
            }

            HttpRuntime.Cache.Remove(PERSISTED_STORE_DIRECTORY_KEY_FORMAT);
            HttpRuntime.Cache.Remove(this.STORE_DIRECTORY_KEY_FORMAT);
            HttpRuntime.Cache.Remove(this.CONTENT_TYPES_KEY_FORMAT);
        }

        protected override void Save<T>(string key, T content)
        {
            HttpRuntime.Cache[key] = content;
        }

        protected override T Load<T>(string key)
        {
            object obj = HttpRuntime.Cache[key];

            if (obj == null)
            {
                return default(T);
            }

            return obj as T;
        }

        protected override void Delete(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        protected override string CreateKey(IStorable storable)
        {
            return string.Concat(STORE_KEY_PREFIX, storable.StoreDirectory, storable.StoreKey);
        }

        protected override string CreateKey(IStoreInfo storeInfo)
        {
            return string.Concat(STORE_KEY_PREFIX, storeInfo.StoreDirectory, storeInfo.StoreKey);
        }


    }
}
