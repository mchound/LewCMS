using LewCMS.V2.Contents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Store
{
    public interface IStoreService
    {
        IEnumerable<IStoreInfo> Save(IStorable storable);

        IStorable LoadFor(IStoreInfo storeInfo);
        Tstorable LoadFor<Tstorable, Tinfo>(Func<Tinfo, bool> predicate) where Tstorable : class,IStorable where Tinfo : class,IStoreInfo;

        IEnumerable<IStorable> Load();
        IEnumerable<T> Load<T>() where T : class, IStorable;
        IEnumerable<Tstorable> Load<Tstorable, Tinfo>(Func<Tinfo, bool> predicate) where Tstorable : class,IStorable where Tinfo : class,IStoreInfo;

        IEnumerable<IStoreInfo> LoadStoreInfo();
        IEnumerable<IStoreInfo> LoadStoreInfo(Func<IStoreInfo, bool> predicate);

        IEnumerable<IStoreInfo> Delete(IStoreInfo storeInfo);
        IEnumerable<IStoreInfo> Delete(Func<IStoreInfo, bool> predicate);
    }

    public abstract class BaseStoreService : IStoreService
    {
        protected abstract string STORE_DIRECTORY_KEY_FORMAT { get; }
        protected abstract string CONTENT_TYPES_KEY_FORMAT { get; }

        public IEnumerable<IStoreInfo> Save(IStorable storable)
        {
            if (storable == null)
            {
                return this.LoadStoreInfo();
            }

            string key = this.CreateKey(storable);
            this.Save<IStorable>(key, storable);
            return this.UpdateStoreInfo(storable, StoreInfoAction.AddOrUpdate);
        }


        public IStorable LoadFor(IStoreInfo storeInfo)
        {
            string key = this.CreateKey(storeInfo);
            return this.Load<IStorable>(key);
        }

        public Tstorable LoadFor<Tstorable, Tinfo>(Func<Tinfo, bool> predicate) where Tstorable : class, IStorable where Tinfo : class, IStoreInfo
        {
            return this.LoadStoreInfo(si => si is Tinfo).Select(si => si as Tinfo).Where(predicate).Select(si => this.LoadFor(si) as Tstorable).FirstOrDefault();
        }


        public IEnumerable<IStorable> Load()
        {
            return this.LoadStoreInfo().Select(si => this.LoadFor(si));
        }

        public IEnumerable<T> Load<T>() where T : class, IStorable
        {
            return this.LoadStoreInfo(si => si.RepresentedInterface == typeof(T)).Select(si => this.LoadFor(si) as T);
        }

        public IEnumerable<Tstorable> Load<Tstorable, Tinfo>(Func<Tinfo, bool> predicate) where Tstorable : class, IStorable where Tinfo : class, IStoreInfo
        {
            return this.LoadStoreInfo(si => si is Tinfo).Select(si => si as Tinfo).Where(predicate).Select(ti => this.LoadFor(ti) as Tstorable);
        }


        public IEnumerable<IStoreInfo> Delete(IStoreInfo storeInfo)
        {
            IStorable content = this.LoadFor(storeInfo);
            string key = this.CreateKey(storeInfo);
            this.Delete(key);
            return this.UpdateStoreInfo(content, StoreInfoAction.Delete);
        }

        public IEnumerable<IStoreInfo> Delete(Func<IStoreInfo, bool> predicate)
        {
            IEnumerable<IStorable> storables = this.LoadStoreInfo(predicate).Select(si => this.LoadFor(si));
            IEnumerable<IStoreInfo> afterDeleteStoreInfo = Enumerable.Empty<IStoreInfo>();

            foreach (var storable in storables)
            {
                string key = this.CreateKey(storable);
                this.Delete(key);
                afterDeleteStoreInfo = this.UpdateStoreInfo(storable, StoreInfoAction.Delete);
            }

            return afterDeleteStoreInfo;
        }


        protected virtual IEnumerable<IStoreInfo> UpdateStoreInfo(IStorable storable, StoreInfoAction storeInfoAction)
        {
            IEnumerable<IStoreInfo> _storeInfos = this.LoadStoreInfo();
            List<IStoreInfo> storeInfos = _storeInfos == null ? new List<IStoreInfo>() : _storeInfos.ToList();
            IStoreInfo _storeInfo = storeInfos.FirstOrDefault(si => si.StoreKey == storable.StoreKey);

            switch (storeInfoAction)
            {
                case StoreInfoAction.AddOrUpdate:
                    if (_storeInfo == null)
                    {
                        storeInfos.Add(storable.GetStoreInfo());
                    }
                    else
                    {
                        _storeInfo = storable.GetStoreInfo();
                    }
                    break;
                case StoreInfoAction.Delete:
                    storeInfos.Remove(_storeInfo);
                    break;
                default:
                    break;
            }

            this.Save<IEnumerable<IStoreInfo>>(this.STORE_DIRECTORY_KEY_FORMAT, storeInfos);

            return storeInfos;
        }

        public IEnumerable<IStoreInfo> LoadStoreInfo()
        {
            return this.Load<IEnumerable<IStoreInfo>>(this.STORE_DIRECTORY_KEY_FORMAT) ?? Enumerable.Empty<IStoreInfo>();
        }

        public IEnumerable<IStoreInfo> LoadStoreInfo(Func<IStoreInfo, bool> predicate)
        {
            return this.LoadStoreInfo().Where(predicate);
        }

        protected abstract string CreateKey(IStorable storable);
        protected abstract string CreateKey(IStoreInfo storeInfo);
        protected abstract void Save<T>(string key, T content) where T : class;
        protected abstract T Load<T>(string key) where T : class;
        protected abstract void Delete(string key);
    }
}
