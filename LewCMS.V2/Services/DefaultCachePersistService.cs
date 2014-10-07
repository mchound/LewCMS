using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LewCMS.V2.Services
{
    public interface ICachePersistService : IPersistService
    {
        void SavePersistedContentInfo(IEnumerable<IContentInfo> contentInfo);
        IEnumerable<IContentInfo> LoadPersistedContentInfo();
        void SaveContentTypes(IEnumerable<IContentType> contentTypes);
        void ClearCache();
    }

    public class DefaultCachePersistService : BasePersistService, ICachePersistService
    {
        private const string CONTENT_PERSISTED_DIRECTORY_KEY_FORMAT = "LewCMS.Cache.Persisted.ContentDirectory";

        protected override string CONTENT_KEY_FORMAT
        {
            get { return "LewCMS.Cache.Content-{0}[version-{1}][lang-{2}]"; }
        }

        protected override string CONTENT_DIRECTORY_KEY_FORMAT
        {
            get { return "LewCMS.Cache.ContentDirectory"; }
        }

        protected override string CONTENT_TYPES_KEY_FORMAT
        {
            get { return "LewCMS.Cache.ContentTypes"; }
        }

        public DefaultCachePersistService()
        {
            
        }

        protected override string CreateKey(IContent content)
        {
            return this.CreateKey(content.Id, content.Version, content.Culture.TwoLetterISOLanguageName);
        }

        protected override string CreateKey(IContentInfo contentInfo)
        {
            return this.CreateKey(contentInfo.Id, contentInfo.Version, contentInfo.Culture.TwoLetterISOLanguageName);
        }

        protected override string CreateKey(string id, int version, string language)
        {
            return string.Format(this.CONTENT_KEY_FORMAT, id, version.ToString(), language);
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

        public void SavePersistedContentInfo(IEnumerable<IContentInfo> contentInfo)
        {
            this.Save<IEnumerable<IContentInfo>>(CONTENT_PERSISTED_DIRECTORY_KEY_FORMAT, contentInfo);
        }

        public IEnumerable<IContentInfo> LoadPersistedContentInfo()
        {
            return this.Load<IEnumerable<IContentInfo>>(CONTENT_PERSISTED_DIRECTORY_KEY_FORMAT);
        }

        public void SaveContentTypes(IEnumerable<IContentType> contentTypes)
        {
            this.Save<IEnumerable<IContentType>>(this.CONTENT_TYPES_KEY_FORMAT, contentTypes);
        }

        public void ClearCache()
        {
            IEnumerable<IContentInfo> contentInfo = this.LoadContentInfo() ?? Enumerable.Empty<IContentInfo>();
            string key = string.Empty;

            foreach (var info in contentInfo)
	        {
		        key = this.CreateKey(info);
                this.Delete(key);
	        }

            HttpRuntime.Cache.Remove(CONTENT_PERSISTED_DIRECTORY_KEY_FORMAT);
            HttpRuntime.Cache.Remove(this.CONTENT_DIRECTORY_KEY_FORMAT);
            HttpRuntime.Cache.Remove(this.CONTENT_KEY_FORMAT);
            HttpRuntime.Cache.Remove(this.CONTENT_TYPES_KEY_FORMAT);
        }
    }
}
