using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Services
{
    public interface IPersistService
    {
        void Initialize(IInitializeService initializeService);

        IEnumerable<IContentInfo> Save(IContent content);

        IContent LoadContentFor(IContentInfo contentInfo);
        IContent LoadContentFor(Func<IContentInfo, bool> predicate);
        Tcontent LoadContentFor<Tcontent, Tinfo>(Func<Tinfo, bool> predicate)where Tcontent : class where Tinfo : class,IContentInfo;

        IEnumerable<IContent> LoadContent();
        IEnumerable<T> LoadContent<T>() where T : class;
        IEnumerable<IContent> LoadContent(Func<IContentInfo, bool> predicate);
        IEnumerable<Tcontent> LoadContent<Tcontent, Tinfo>(Func<Tinfo, bool> predicate) where Tcontent : class where Tinfo : class,IContentInfo;
        
        IContentInfo LoadContentInfoFor(Func<IContentInfo, bool> predicate);
        T LoadContentInfoFor<T>(Func<T, bool> predicate) where T : class, IContentInfo;

        IEnumerable<IContentInfo> LoadContentInfo();
        IEnumerable<IContentInfo> LoadContentInfo(Func<IContentInfo, bool> predicate);

        IEnumerable<IContentType> LoadContentTypes();

        IEnumerable<IContentInfo> Delete(IContentInfo contentInfo);
        IEnumerable<IContentInfo> Delete(Func<IContentInfo, bool> predicate);
    }

    public abstract class BasePersistService : IPersistService
    {
        protected abstract string CONTENT_KEY_FORMAT { get; }
        protected abstract string CONTENT_DIRECTORY_KEY_FORMAT { get; }
        protected abstract string CONTENT_TYPES_KEY_FORMAT { get; }

        public virtual void Initialize(IInitializeService initializeService)
        {
            List<IContentType> contentTypes = new List<IContentType>();
            contentTypes.AddRange(initializeService.GetPageTypes(Application.Current.ApplicationAssembly));
            contentTypes.AddRange(initializeService.GetSectionTypes(Application.Current.ApplicationAssembly));
            contentTypes.AddRange(initializeService.GetGlobalConfigTypes(Application.Current.ApplicationAssembly));
            this.Save<IEnumerable<IContentType>>(this.CONTENT_TYPES_KEY_FORMAT, contentTypes);
        }

        public virtual IEnumerable<IContentInfo> Save(IContent content)
        {
            if (content == null)
            {
                return this.LoadContentInfo();
            }

            string key = this.CreateKey(content);
            this.Save<IContent>(key, content);
            return this.UpdateContentInfo(content, ContentInfoAction.AddOrUpdate);
        }


        public virtual IContent LoadContentFor(IContentInfo contentInfo)
        {
            string key = this.CreateKey(contentInfo);
            return this.Load<IContent>(key);
        }

        public virtual IContent LoadContentFor(Func<IContentInfo, bool> predicate)
        {
            IContentInfo contentInfo = this.LoadContentInfo().FirstOrDefault(predicate);
            return this.LoadContentFor(contentInfo);
        }

        public virtual Tcontent LoadContentFor<Tcontent, Tinfo>(Func<Tinfo, bool> predicate)where Tcontent : class where Tinfo : class,IContentInfo
        {
            return this.LoadContentInfo(ci => ci is Tinfo).Select(ci => ci as Tinfo).Where(predicate).Select(ti => this.LoadContentFor(ti) as Tcontent).FirstOrDefault();
        }


        public virtual IEnumerable<IContent> LoadContent()
        {
            return this.LoadContentInfo().Select(ci => this.LoadContentFor(ci));
        }

        public virtual IEnumerable<T> LoadContent<T>() where T : class
        {
            return this.LoadContentInfo(ci => ci.ContentTypeInterface == typeof(T)).Select(ci => this.LoadContentFor(ci) as T);
        }

        public virtual IEnumerable<IContent> LoadContent(Func<IContentInfo, bool> predicate)
        {
            return this.LoadContentInfo(predicate).Select(ci => this.LoadContentFor(ci));
        }

        public virtual IEnumerable<Tcontent> LoadContent<Tcontent, Tinfo>(Func<Tinfo, bool> predicate) where Tcontent : class where Tinfo : class,IContentInfo
        {
            return this.LoadContentInfo(ci => ci is Tinfo).Select(ci => ci as Tinfo).Where(predicate).Select(ti => this.LoadContentFor(ti) as Tcontent);
        }


        public virtual IContentInfo LoadContentInfoFor(Func<IContentInfo, bool> predicate)
        {
            return this.LoadContentInfo().FirstOrDefault(predicate);
        }

        public virtual T LoadContentInfoFor<T>(Func<T, bool> predicate) where T : class, IContentInfo
        {
            return this.LoadContentInfo(ci => ci is T).Select(ci => ci as T).FirstOrDefault(predicate);
        }


        public virtual IEnumerable<IContentInfo> LoadContentInfo()
        {
            return this.Load<IEnumerable<IContentInfo>>(this.CONTENT_DIRECTORY_KEY_FORMAT);
        }

        public virtual IEnumerable<IContentInfo> LoadContentInfo(Func<IContentInfo, bool> predicate)
        {
            return this.LoadContentInfo().Where(predicate);
        }

        
        public virtual void SaveContentInfo(IEnumerable<IContentInfo> contentInfo)
        {
            this.Save<IEnumerable<IContentInfo>>(this.CONTENT_DIRECTORY_KEY_FORMAT, contentInfo);
        }


        public virtual IEnumerable<IContentType> LoadContentTypes()
        {
            return this.Load<IEnumerable<IContentType>>(this.CONTENT_TYPES_KEY_FORMAT);
        }


        public virtual IEnumerable<IContentInfo> Delete(Func<IContentInfo, bool> predicate)
        {
            IEnumerable<IContent> contents = this.LoadContentInfo(predicate).Select(ci => this.LoadContentFor(ci));
            IEnumerable<IContentInfo> afterDeleteContentInfo = Enumerable.Empty<IContentInfo>();
            
            foreach (var content in contents)
            {
                string key = this.CreateKey(content);
                this.Delete(key);
                afterDeleteContentInfo = this.UpdateContentInfo(content, ContentInfoAction.Delete);
            }

            return afterDeleteContentInfo;
        }

        public virtual IEnumerable<IContentInfo> Delete(IContentInfo contentInfo)
        {
            IContent content = this.LoadContentFor(contentInfo);
            string key = this.CreateKey(contentInfo);
            this.Delete(key);
            return this.UpdateContentInfo(content, ContentInfoAction.Delete);
        }


        protected virtual IEnumerable<IContentInfo> UpdateContentInfo(IContent content, ContentInfoAction contentInfoAction)
        {
            IEnumerable<IContentInfo> _contentInfos = this.LoadContentInfo();
            List<IContentInfo> contentInfos = _contentInfos == null ? new List<IContentInfo>() : _contentInfos.ToList();
            IContentInfo _contentInfo = contentInfos.FirstOrDefault(ci => ci.Id == content.Id && ci.Version == content.Version && ci.Culture == content.Culture);

            switch (contentInfoAction)
            {
                case ContentInfoAction.AddOrUpdate:
                    if (_contentInfo == null)
                    {
                        contentInfos.Add(content.ContentInfo());
                    }
                    else
                    {
                        _contentInfo = content.ContentInfo();
                    }
                    break;
                case ContentInfoAction.Delete:
                    contentInfos.Remove(_contentInfo);
                    break;
                default:
                    break;
            }

            this.SaveContentInfo(contentInfos);

            return contentInfos;
        }

        protected abstract string CreateKey(IContent content);
        protected abstract string CreateKey(IContentInfo contentInfo);
        protected abstract string CreateKey(string id, int version, string language);
        protected abstract void Save<T>(string key, T content);
        protected abstract T Load<T>(string key) where T : class;
        protected abstract void Delete(string key);

        
    }
}
