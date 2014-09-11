using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LewCMS.Core.Service
{
    public interface IContentService
    {
        IEnumerable<IPageType> GetPageTypes();
        IPage GetPage(string pageId);
        IEnumerable<IPage> GetPages(Func<PageMetaData, bool> predicate);
        IEnumerable<IPage> GetAllPages();
        IPage AddPage(string pageTypeId, string pageName);
        IPage AddPage(string pageTypeId, string pageName, string parentId);
        void UpdatePage(IPage page);
        void DeletePage(string pageId);
    }

    public class ContentService : IContentService
    {
        private IContentRepository _contentRepository;

        public ContentService(IContentRepository contentRepository)
        {
            this._contentRepository = contentRepository;
        }

        public IEnumerable<IPageType> GetPageTypes()
        {
            return this._contentRepository.GetPageTypes();
        }

        public IPage GetPage(string pageId)
        {
            return this._contentRepository.GetPage(pageId);
        }

        public IPage AddPage(string pageTypeId, string pageName)
        {
            return this.AddPage(pageTypeId, pageName, null);
        }

        public IPage AddPage(string pageTypeId, string pageName, string parentId)
        {
            var pageType = this.GetPageTypes().FirstOrDefault(p => p.Id == pageTypeId);
            IPage page = Activator.CreateInstance(Application.Current.ApplicationAssembly.GetType(pageType.TypeName)) as IPage;
            page.Route = this.CreatePageRoute(pageName, null);
            page.Name = pageName;
            page.Version = 1;
            page.Id = Guid.NewGuid().ToString();
            page.PageType = pageType as PageType;

            page.OnInit();
            this._contentRepository.AddPage(page);

            return page;
        }

        public void UpdatePage(IPage page)
        {
            throw new NotImplementedException();
        }

        public void DeletePage(string pageId)
        {
            this._contentRepository.DeletePage(pageId);
        }

        public IEnumerable<IPage> GetPages(Func<PageMetaData, bool> predicate)
        {
            return this._contentRepository.GetPages(predicate);
        }

        public IEnumerable<IPage> GetAllPages()
        {
            return this._contentRepository.GetAllPages();
        }

        # region Private Methods

        private string CreatePageRoute(string pageName, string parentId)
        {
            if(string.IsNullOrWhiteSpace(parentId))
            {
                return string.Concat("/", HttpUtility.UrlEncode(pageName).Replace("+", "-"));
            }

            // TODO: Calculate correct page route based on page tree. Think of that the pageName can appear many times
            return string.Concat("/", HttpUtility.UrlEncode(pageName).Replace("+", "-"));
        }

        # endregion
    }
}
