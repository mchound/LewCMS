using LewCMS.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.Core.Content
{
    public interface IPersistService
    {
        void Initialize(IEnumerable<IPageType> pageTypes);

        IEnumerable<IPageInfo> SavePage(IPage page);

        IPage LoadPage(string pageId, int pageVersion);
        IEnumerable<IPage> LoadPages(string pageId);
        IEnumerable<IPage> LoadPages(Func<IPageInfo, bool> predicate);
        IEnumerable<IPage> LoadPages();

        IPageInfo LoadPageInfo(Func<IPageInfo, bool> predicate);
        IEnumerable<IPageInfo> LoadPageInfos();
        IEnumerable<IPageInfo> LoadPageInfos(Func<IPageInfo, bool> predicate);

        IEnumerable<IPageType> LoadPageTypes();

        IEnumerable<IPageInfo> Delete(string pageId, int version);
    }

    public abstract class BasePersistService : IPersistService
    {
        public abstract void Initialize(IEnumerable<IPageType> pageTypes);

        public abstract IEnumerable<IPageInfo> SavePage(IPage page);

        public abstract IPage LoadPage(string pageId, int pageVersion);

        public virtual IEnumerable<IPage> LoadPages(string pageId)
        {
            return this.LoadPages(pi => pi.PageId == pageId);
        }

        public virtual IEnumerable<IPage> LoadPages(Func<IPageInfo, bool> predicate)
        {
            IEnumerable<IPageInfo> pageInfos = this.LoadPageInfos(predicate);

            foreach (IPageInfo pageInfo in pageInfos)
            {
                yield return this.LoadPage(pageInfo.PageId, pageInfo.Version);
            }
        }

        public virtual IEnumerable<IPage> LoadPages()
        {
            return this.LoadPages(pi => true);
        }

        public virtual IPageInfo LoadPageInfo(Func<IPageInfo, bool> predicate)
        {
            return this.LoadPageInfos().FirstOrDefault(predicate);
        }

        public abstract IEnumerable<IPageInfo> LoadPageInfos();

        public virtual IEnumerable<IPageInfo> LoadPageInfos(Func<IPageInfo, bool> predicate)
        {
            return this.LoadPageInfos().Where(predicate);
        }

        public abstract IEnumerable<IPageType> LoadPageTypes();

        public abstract IEnumerable<IPageInfo> Delete(string pageId, int version);

        protected IEnumerable<IPageInfo> UpdatePageInfo(IPage page, PageInfoAction pageInfoAction)
        {
            IEnumerable<IPageInfo> _pageInfos = this.LoadPageInfos();
            List<IPageInfo> pageInfos = _pageInfos == null ? new List<IPageInfo>() : _pageInfos.ToList();
            IPageInfo pageInfo = pageInfos.FirstOrDefault(pi => pi.PageId == page.Id && pi.Version == page.Version);

            switch (pageInfoAction)
            {
                case PageInfoAction.AddOrUpdate:
                    if (pageInfo == null)
                    {
                        pageInfos.Add(new PageInfo(page));
                    }
                    else
                    {
                        pageInfo = new PageInfo(page);
                    }
                    break;
                case PageInfoAction.Delete:
                    pageInfos.Remove(pageInfo);
                    break;
                default:
                    break;
            }

            this.SavePageInfos(pageInfos);

            return pageInfos;
        }

        protected IEnumerable<IPageInfo> UpdatePageInfo(string pageId, int version, PageInfoAction pageInfoAction)
        {
            IPage page = this.LoadPage(pageId, version);
            return this.UpdatePageInfo(page, pageInfoAction);
        }

        protected abstract void SavePageInfos(IEnumerable<IPageInfo> pageInfos);
    }
}
