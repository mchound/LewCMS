using LewCMS.BackStage.Models.ClientViewModels;
using LewCMS.V2.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LewCMS.BackStage.Helpers
{
    public static class ContentServiceExtensions
    {
        public static PageTreeItem GetPageTree(this IContentService contentService, int depth = int.MaxValue, IPage root = null)
        {
            IPage _root = root ?? contentService.StartPage;

            if (_root == null)
            {
                return null;
            }

            return new PageTreeItem
            {
                Id = _root.Id,
                IsStartPage = _root.Id == contentService.StartPage.Id,
                Name = _root.Name,
                ParentId = _root.ParentId,
                HasChildren = contentService.GetPageInfo(pi => pi.ParentId == _root.Id).Count() > 0,
                Children = depth > 0 ? _root.GetPageTreeChildrenRecursive(contentService, depth) : Enumerable.Empty<PageTreeItem>()
            };
        }

        public static IEnumerable<IPage> GetChildren(this IContentService contentService, IPage page)
        {
            return contentService.GetPages(pi => pi.ParentId == page.Id) ?? Enumerable.Empty<IPage>();
        }

        private static IEnumerable<PageTreeItem> GetPageTreeChildrenRecursive(this IPage page, IContentService contentService, int depth, int level = 1)
        {
            return contentService.GetChildren(page).Select(p => new PageTreeItem
            { 
                Id = p.Id,
                IsStartPage = p.Id == contentService.StartPage.Id,
                Name = p.Name,
                ParentId = p.ParentId,
                HasChildren = contentService.GetPageInfo(pi => pi.ParentId == p.Id).Count() > 0,
                Children = level < depth ? p.GetPageTreeChildrenRecursive(contentService, depth, level + 1) : Enumerable.Empty<PageTreeItem>()
            
            });
        }
    }
}