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

        IContent LoadContentFor(Func<IContentInfo, bool> predicate);
        IContent LoadContentFor(IContentInfo contentInfo);

        IEnumerable<IContent> LoadContent();
        IEnumerable<IContent> LoadContent(Func<IContentInfo, bool> predicate);
        
        IContentInfo LoadContentInfoFor(Func<IContentInfo, bool> predicate);

        IEnumerable<IContentInfo> LoadContentInfo();
        IEnumerable<IContentInfo> LoadContentInfo(Func<IContentInfo, bool> predicate);

        IEnumerable<IContentType> LoadContentTypes();

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


        public virtual IEnumerable<IContent> LoadContent()
        {
            return this.LoadContentInfo().Select(ci => this.LoadContentFor(ci));
        }

        public virtual IEnumerable<IContent> LoadContent(Func<IContentInfo, bool> predicate)
        {
            return this.LoadContentInfo(predicate).Select(ci => this.LoadContentFor(ci));
        }


        public virtual IContentInfo LoadContentInfoFor(Func<IContentInfo, bool> predicate)
        {
            return this.LoadContentInfo().FirstOrDefault(predicate);
        }


        public virtual IEnumerable<IContentInfo> LoadContentInfo()
        {
            return this.Load<IEnumerable<IContentInfo>>(this.CONTENT_DIRECTORY_KEY_FORMAT);
        }

        public virtual IEnumerable<IContentInfo> LoadContentInfo(Func<IContentInfo, bool> predicate)
        {
            return this.LoadContentInfo().Where(predicate);
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

        protected virtual IEnumerable<IContentInfo> UpdateContentInfo(IContent content, ContentInfoAction contentInfoAction)
        {
            IEnumerable<IContentInfo> _contentInfos = this.LoadContentInfo();
            List<IContentInfo> contentInfos = _contentInfos == null ? new List<IContentInfo>() : _contentInfos.ToList();
            IContentInfo _contentInfo = _contentInfos.FirstOrDefault(ci => ci.Id == content.Id && ci.Version == content.Version && ci.Culture == content.Culture);

            switch (contentInfoAction)
            {
                case ContentInfoAction.AddOrUpdate:
                    if (_contentInfo == null)
                    {
                        contentInfos.Add(content.ContentInfo);
                    }
                    else
                    {
                        _contentInfo = content.ContentInfo;
                    }
                    break;
                case ContentInfoAction.Delete:
                    contentInfos.Remove(_contentInfo);
                    break;
                default:
                    break;
            }

            this.SavePageInfos(contentInfos);

            return contentInfos;
        }

        protected abstract string CreateKey(IContent content);
        protected abstract string CreateKey(IContentInfo contentInfo);
        protected abstract string CreateKey(string id, string version, string language);
        protected abstract void SavePageInfos(IEnumerable<IContentInfo> contentInfo);
        protected abstract IEnumerable<IContentInfo> Save<T>(string key, T content);
        protected abstract T Load<T>(string key);
        protected abstract void Delete(string key);

        
    }
}
