using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Services
{
    public class DefaultContentRepository : IContentRepository
    {
        private IInitializeService _initializeService;
        private IPersistService _filePersistsService;
        private ICachePersistService _cachePersistsService;

        public DefaultContentRepository(IInitializeService initializeService, IPersistService filePersistsService, ICachePersistService cachePersistsService)
        {
            this._initializeService = initializeService;
            this._filePersistsService = filePersistsService;
            this._cachePersistsService = cachePersistsService;
        }

        public IContent GetContentFor(IContentInfo contentInfo)
        {
            IContent content = this._cachePersistsService.LoadContentFor(contentInfo);

            if (content == null)
            {
                content = this._filePersistsService.LoadContentFor(contentInfo);
                this._cachePersistsService.Save(content);
            }

            return content;
        }

        public IContent GetContentFor(Func<IContentInfo, bool> predicate)
        {
            IContentInfo contentInfo = this.GetContentInfoFor(predicate);

            if (contentInfo == null)
            {
                return null;
            }

            return this.GetContentFor(contentInfo);
        }

        public Tcontent GetContentFor<Tcontent, Tinfo>(Func<Tinfo, bool> predicate) where Tcontent : class, IContent where Tinfo : class, IContentInfo
        {
            Tcontent content = this._cachePersistsService.LoadContentFor<Tcontent, Tinfo>(predicate);

            if (content == null)
            {
                content = this._filePersistsService.LoadContentFor<Tcontent, Tinfo>(predicate);
                this._cachePersistsService.Save(content);
            }

            return content;
        }


        public IEnumerable<IContent> GetContent()
        {
            IEnumerable<IContentInfo> contentInfo = this.GetContentInfo();

            foreach (var info in contentInfo)
            {
                yield return this.GetContentFor(info);
            }
        }

        public IEnumerable<T> GetContent<T>() where T : class
        {
            return this.GetContent(ci => ci.ContentTypeInterface == typeof(T)).Select(c => c as T);
        }

        public IEnumerable<IContent> GetContent(Func<IContentInfo, bool> predicate)
        {
            IEnumerable<IContentInfo> contentInfo = this.GetContentInfo().Where(predicate);

            foreach (var info in contentInfo)
            {
                yield return this.GetContentFor(info);
            }
        }

        public IEnumerable<Tcontent> GetContent<Tcontent, Tinfo>(Func<Tinfo, bool> predicate) where Tcontent : class where Tinfo : class, IContentInfo
        {
            IEnumerable<Tinfo> contentInfo = this.GetContentInfo().Where(ci => ci is Tinfo).Select(ci => ci as Tinfo).Where(predicate);

            foreach (var info in contentInfo)
            {
                yield return this.GetContentFor(info) as Tcontent;
            }
        }

        public IContentInfo GetContentInfoFor(Func<IContentInfo, bool> predicate)
        {
            IContentInfo contentInfo = this._cachePersistsService.LoadContentInfoFor(predicate);

            if (contentInfo == null)
            {
                contentInfo = this._filePersistsService.LoadContentInfoFor(predicate);
            }

            return contentInfo;
        }

        public T GetContentInfoFor<T>(Func<T, bool> predicate) where T : class, IContentInfo
        {
            T contentInfo = this._cachePersistsService.LoadContentInfoFor<T>(predicate);

            if (contentInfo == null)
            {
                contentInfo = this._filePersistsService.LoadContentInfoFor<T>(predicate);
            }

            return contentInfo;
        }

        public IEnumerable<IContentInfo> GetContentInfo()
        {
            IEnumerable<IContentInfo> contentInfo = this._cachePersistsService.LoadPersistedContentInfo();

            if (contentInfo == null)
            {
                contentInfo = this._filePersistsService.LoadContentInfo();
                this._cachePersistsService.SavePersistedContentInfo(contentInfo);
            }

            return contentInfo;
        }

        public IEnumerable<IContentInfo> GetContentInfo(Func<IContentInfo, bool> predicate)
        {
            IEnumerable<IContentInfo> contentInfo = this._cachePersistsService.LoadPersistedContentInfo();

            if (contentInfo == null)
            {
                contentInfo = this._filePersistsService.LoadContentInfo();
                this._cachePersistsService.SavePersistedContentInfo(contentInfo);
            }

            return contentInfo.Where(predicate);
        }

        public IEnumerable<IContentType> GetContentTypes()
        {
            IEnumerable<IContentType> contentTypes = this._cachePersistsService.LoadContentTypes();

            if (contentTypes == null)
            {
                contentTypes = this._filePersistsService.LoadContentTypes();
                this._cachePersistsService.SaveContentTypes(contentTypes);
            }

            return contentTypes;
        }

        public IEnumerable<IContentInfo> Delete(IContentInfo contentInfo)
        {
            IEnumerable<IContentInfo> _contentInfo = this._filePersistsService.Delete(contentInfo);
            this._cachePersistsService.Delete(contentInfo);
            this._cachePersistsService.SavePersistedContentInfo(_contentInfo);
            return _contentInfo;
        }

        public IEnumerable<IContentInfo> Delete(Func<IContentInfo, bool> predicate)
        {
            IEnumerable<IContentInfo> _contentInfo = this._filePersistsService.Delete(predicate);
            this._cachePersistsService.Delete(predicate);
            this._cachePersistsService.SavePersistedContentInfo(_contentInfo);
            return _contentInfo;
        }

        public int GetContentVersions(string id)
        {
            return this.GetContentInfo(ci => ci.Id == id).Max(ci => ci.Version);
        }

        public int GetContentVersions(IContentInfo contentInfo)
        {
            return this.GetContentInfo(ci => ci.Id == contentInfo.Id && ci.Culture == contentInfo.Culture).Max(ci => ci.Version);
        }

        public int GetContentVersions(IContent content)
        {
            return this.GetContentInfo(ci => ci.Id == content.Id && ci.Culture == content.Culture).Max(ci => ci.Version);
        }

        public void Save(IContent content)
        {
            this._cachePersistsService.Save(content);
            IEnumerable<IContentInfo> contentInfo = this._filePersistsService.Save(content);
            this._cachePersistsService.SavePersistedContentInfo(contentInfo);
        }
    }
}
