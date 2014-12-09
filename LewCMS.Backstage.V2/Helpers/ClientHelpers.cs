using LewCMS.BackStage.V2.Models.ClientModels;
using LewCMS.V2.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LewCMS.BackStage.V2.Helpers
{
    public static class ContentTypeHelpers
    {
        private static object ForClient(this IContentType contentType)
        {
            return new
            {
                id = contentType.Id,
                name = contentType.DisplayName,
                category = contentType.Category
            };
        }

        public static IEnumerable<object> ForClient(this IEnumerable<IContentType> contentTypes)
        {
            return contentTypes.ToLookup(ct => ct.Category).OrderBy(l => l.Key).Select(l => new
            {
                category = l.Key,
                contents = l.Select(ct => ct.ForClient())
            });
        }

        public static object ForClient(this PageTreeItem pageTreeItem)
        {
            return new
            {
                id = pageTreeItem.Id,
                name = pageTreeItem.Name,
                parentId = pageTreeItem.ParentId,
                isStartPage = pageTreeItem.IsStartPage,
                hasChildren = pageTreeItem.HasChildren,
                children = pageTreeItem.Children.Select(pti => pti.ForClient())
            };
        }
    }
}