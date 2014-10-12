using LewCMS.V2.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LewCMS.BackStage.Models.ClientModels
{
    public static class ClientModels
    {
        // Content type

        public static object CreateContentType(IContentType contentType)
        {
            return new
            {
                Id = contentType.Id,
                Name = contentType.DisplayName
            };
        }

        public static object CreateContentTypes(IEnumerable<IContentType> contentTypes)
        {
            return contentTypes.Select(ct => new
            {
                Id = ct.Id,
                Name = ct.DisplayName
            });
        }

        public static object AsClientModel(this IContentType contentType)
        {
            return ClientModels.CreateContentType(contentType);
        }

        public static object AsClientModels(this IEnumerable<IContentType> contentTypes)
        {
            return ClientModels.CreateContentTypes(contentTypes);
        }
    }
}