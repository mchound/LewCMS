using LewCMS.BackStage.Models.ClientViewModels;
using LewCMS.V2.Contents;
using System.Collections.Generic;
using System.Linq;

namespace LewCMS.BackStage.Helpers
{
    public static class ClientHelpers
    {
        // Content type

        private static object CreateContentType(IContentType contentType)
        {
            return new
            {
                id = contentType.Id,
                name = contentType.DisplayName,
                category = contentType.Category
            };
        }

        private static object CreateContentTypes(IEnumerable<IContentType> contentTypes)
        {
            return contentTypes.Select(ct => ClientHelpers.CreateContentType(ct));
        }

        public static object AsClientModel(this IContentType contentType)
        {
            return ClientHelpers.CreateContentType(contentType);
        }

        public static object AsClientModels(this IEnumerable<IContentType> contentTypes)
        {
            return ClientHelpers.CreateContentTypes(contentTypes);
        }

        public static IEnumerable<object> AsClientModelsGroupedByCategory(this IEnumerable<IContentType> contentTypes)
        {
            return contentTypes.ToLookup(ct => ct.Category).OrderBy(l => l.Key).Select(l => new
            {
                category = l.Key,
                contents = l.Select(ct => ClientHelpers.CreateContentType(ct))
            });
        }

        // Page tree pages

        private static object TransformForClient(PageTreeItem pageTreeItem)
        {
            return new
            {
                id = pageTreeItem.Id,
                name = pageTreeItem.Name,
                parentId = pageTreeItem.ParentId,
                isStartPage = pageTreeItem.IsStartPage,
                hasChildren = pageTreeItem.HasChildren,
                children = ClientHelpers.TransformForClient(pageTreeItem.Children)
            };
        }

        private static IEnumerable<object> TransformForClient(IEnumerable<PageTreeItem> pageTreeItems)
        {
            return pageTreeItems.Select(p => ClientHelpers.TransformForClient(p));
        }

        public static object AsClientModel(this PageTreeItem pageTreeItem)
        {
            return ClientHelpers.TransformForClient(pageTreeItem);
        }

    }
}