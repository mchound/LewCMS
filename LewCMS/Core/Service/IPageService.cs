using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.Core.Service
{
    public interface IPageService
    {
        void SetProperty(JObject propertyObject);
    }

    public class PageService : IPageService
    {
        IContentService _contentService;

        public PageService(IContentService contentService)
        {
            this._contentService = contentService;
        }

        public void SetProperty(JObject propertyObject)
        {
            string pageId = propertyObject.GetValue("pageId").Value<string>();
            string propertyName = propertyObject.GetValue("propertyName").Value<string>();
            JToken propertyToken = propertyObject.GetValue("propertyValue");

            IPage page = this._contentService.GetPage(pageId);
            Property property = page.PageType.Properties.First(p => p.Name == propertyName);
            property.Set(propertyToken);

            page[propertyName] = property;

            this._contentService.UpdatePage(page);
        }
    }
}
