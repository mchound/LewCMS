using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Services
{
    public class DefaultRepository : IRepository
    {
        private IInitializeService _initializeService;
        private IStoreService _fileStoreService;
        private ICacheStoreService _cacheStoreService;

        public DefaultRepository(IInitializeService initializeService, IStoreService fileStoreService, ICacheStoreService cacheStoreService)
        {
            this._initializeService = initializeService;
            this._fileStoreService = fileStoreService;
            this._cacheStoreService = cacheStoreService;

            IEnumerable<IContentType> contentTypes = this._initializeService.GetContentTypes(Application.Current.ApplicationAssembly);
            
            this._fileStoreService.Initialize(contentTypes);
            this._cacheStoreService.Initialize(contentTypes);
        }

        public void Save(IStorable storable)
        {
            this._cacheStoreService.Save(storable);
            IEnumerable<IStoreInfo> storeInfo = this._fileStoreService.Save(storable);
            this._cacheStoreService.SavePersistedStoreInfo(storeInfo);
        }

        public IStorable GetFor(IStoreInfo storeInfo)
        {
            IStorable storable = this._cacheStoreService.LoadFor(storeInfo);

            if (storable == null)
            {
                storable = this._cacheStoreService.LoadFor(storeInfo);
                this._cacheStoreService.Save(storable);
            }

            return storable;
        }

        public Tstorable GetFor<Tstorable, Tinfo>(Func<Tinfo, bool> predicate) where Tstorable : class, IStorable where Tinfo : class, IStoreInfo
        {
            Tstorable storable = this._cacheStoreService.LoadFor<Tstorable, Tinfo>(predicate);

            if (storable == null)
            {
                storable = this._fileStoreService.LoadFor<Tstorable, Tinfo>(predicate);
                this._cacheStoreService.Save(storable);
            }

            return storable;
        }

        public IEnumerable<IStorable> Get()
        {
            IEnumerable<IStoreInfo> storeInfo = this.GetStoreInfo();

            foreach (var info in storeInfo)
            {
                yield return this.GetFor(info);
            }
        }

        public IEnumerable<T> Get<T>() where T : class, IStorable
        {
            IEnumerable<IStoreInfo> storeInfo = this.GetStoreInfo().Where(si => si.RepresentedInterface == typeof(T));

            foreach (var info in storeInfo)
            {
                yield return this.GetFor(info) as T;
            }
        }

        public IEnumerable<Tstorable> Get<Tstorable, Tinfo>(Func<Tinfo, bool> predicate) where Tstorable : class, IStorable where Tinfo : class, IStoreInfo
        {
            IEnumerable<IStoreInfo> storeInfo = this.GetStoreInfo(si => si is Tinfo).Select(si => si as Tinfo).Where(predicate);

            foreach (var info in storeInfo)
            {
                yield return this.GetFor(info) as Tstorable;
            }
        }

        public IEnumerable<IContentType> GetContentTypes()
        {
            IEnumerable<IContentType> contentTypes = this._cacheStoreService.LoadContentTypes();

            if (contentTypes == null)
            {
                contentTypes = this._fileStoreService.LoadContentTypes();
                this._cacheStoreService.SaveContentTypes(contentTypes);
            }

            return contentTypes;
        }

        public IEnumerable<IStoreInfo> Delete(IStoreInfo storeInfo)
        {
            IEnumerable<IStoreInfo> _storeInfo = this._fileStoreService.Delete(storeInfo);
            this._cacheStoreService.Delete(storeInfo);
            this._cacheStoreService.SavePersistedStoreInfo(_storeInfo);
            return _storeInfo;
        }

        public IEnumerable<IStoreInfo> Delete(Func<IStoreInfo, bool> predicate)
        {
            IEnumerable<IStoreInfo> _storeInfo = this._fileStoreService.Delete(predicate);
            this._cacheStoreService.Delete(predicate);
            this._cacheStoreService.SavePersistedStoreInfo(_storeInfo);
            return _storeInfo;
        }

        protected virtual IEnumerable<IStoreInfo> GetStoreInfo()
        {
            IEnumerable<IStoreInfo> storeInfo = this._cacheStoreService.LoadPersistedStoreInfo();

            if (storeInfo == null)
            {
                storeInfo = this._fileStoreService.LoadStoreInfo();
                this._cacheStoreService.SavePersistedStoreInfo(storeInfo);
            }

            return storeInfo;
        }

        protected virtual IEnumerable<IStoreInfo> GetStoreInfo(Func<IStoreInfo, bool> predicate)
        {
            IEnumerable<IStoreInfo> storeInfo = this._cacheStoreService.LoadPersistedStoreInfo();

            if (storeInfo == null)
            {
                storeInfo = this._fileStoreService.LoadStoreInfo();
                this._cacheStoreService.SavePersistedStoreInfo(storeInfo);
            }

            return storeInfo.Where(predicate);
        }
    }
}
