using LewCMS.V2.Contents;
using LewCMS.V2.Services;
using LewCMS.V2.Startup;
using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Contents
{
    public interface IContentService
    {
        void Initialize(IInitializeService initializeService, Assembly applicationAssembly);

        IPage StartPage { get; }

        IEnumerable<IPageType> GetPageTypes();
        IPageType GetPageType(Func<IPageType, bool> predicate);

        IEnumerable<T> GetContentInfo<T>(Func<T, bool> predicate) where T : class, IContentInfo;
        IEnumerable<IPageInfo> GetPageInfo(Func<IPageInfo, bool> predicate);

        Tcontent GetFor<Tcontent, Tinfo>(Func<Tinfo, bool> predicate) where Tcontent : class, IContent where Tinfo : class, IContentInfo;
        IEnumerable<Tcontent> Get<Tcontent, Tinfo>(Func<Tinfo, bool> predicate) where Tcontent : class, IContent where Tinfo : class, IContentInfo;

        IPage GetPage(Func<IPageInfo, bool> predicate);
        IEnumerable<IPage> GetPages(Func<IPageInfo, bool> predicate);

        void Save(IStorable storable);

        void Delete(IContentInfo contentInfo);
        void Delete(Func<IContentInfo, bool> predicate);
    }

    public class DefaultContentService : BaseService, IContentService
    {
        public DefaultContentService(IRepository repository) : base(repository) { }

        public void Initialize(IInitializeService initializeService, Assembly applicationAssembly)
        {
            IContentTypeCollection contentTypeCollection = new ContentTypeCollection();
            contentTypeCollection.PageTypes = initializeService.GetPageTypes(applicationAssembly).ToList();
            contentTypeCollection.SectionTypes = initializeService.GetSectionTypes(applicationAssembly).ToList();
            contentTypeCollection.GlobalConfigTypes = initializeService.GetGlobalConfigTypes(applicationAssembly).ToList();
            this.Repository.Save(contentTypeCollection);
        }

        public IPage StartPage
        {
            get 
            {
                return this.Repository.GetFor<IPage, IPageInfo>(pi => pi.Route == "/");
            }
        }

        public IEnumerable<IPageType> GetPageTypes()
        {
            return this.GetContentTypeCollection().PageTypes;
        }

        public IPageType GetPageType(Func<IPageType, bool> predicate)
        {
            return this.GetContentTypeCollection().PageTypes.FirstOrDefault(predicate);
        }


        public Tcontent GetFor<Tcontent, Tinfo>(Func<Tinfo, bool> predicate) where Tcontent : class, IContent where Tinfo : class, IContentInfo
        {
            return this.Repository.GetFor<Tcontent, Tinfo>(predicate);
        }

        public IEnumerable<Tcontent> Get<Tcontent, Tinfo>(Func<Tinfo, bool> predicate) where Tcontent : class, IContent where Tinfo : class, IContentInfo
        {
            return this.Repository.Get<Tcontent, Tinfo>(predicate) ?? Enumerable.Empty<Tcontent>();
        }


        public IPage GetPage(Func<IPageInfo, bool> predicate)
        {
            return this.Repository.GetFor<IPage, IPageInfo>(pi => predicate(pi) && !pi.InTrash);
        }

        public IEnumerable<IPage> GetPages(Func<IPageInfo, bool> predicate)
        {
            return this.Repository.Get<IPage, IPageInfo>(pi => predicate(pi) && !pi.InTrash);
        }


        public IEnumerable<T> GetContentInfo<T>(Func<T, bool> predicate) where T : class, IContentInfo
        {
            return this.Repository.GetStoreInfo<T>(predicate);
        }

        public IEnumerable<IPageInfo> GetPageInfo(Func<IPageInfo, bool> predicate)
        {
            return this.Repository.GetStoreInfo<IPageInfo>(predicate);
        }

        public void Save(IStorable storable)
        {
            this.Repository.Save(storable);
        }


        public void Delete(IContentInfo contentInfo)
        {
            this.Repository.Delete(contentInfo);
        }

        public void Delete(Func<IContentInfo, bool> predicate)
        {
            IContentInfo contentInfo = GetContentInfo<IContentInfo>(predicate).FirstOrDefault();

            if (contentInfo == null)
            {
                return;
            }

            this.Delete(contentInfo);
        }


        private IContentTypeCollection GetContentTypeCollection()
        {
            return this.Repository.Get<IContentTypeCollection>().First();
        }
    }

}
