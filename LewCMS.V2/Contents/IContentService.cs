using LewCMS.V2.Contents;
using LewCMS.V2.Services;
using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Startup
{
    public interface IContentService
    {
        void Initialize(IInitializeService initializeService, Assembly applicationAssembly);
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
    }

}
