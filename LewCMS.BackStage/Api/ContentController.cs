using LewCMS.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LewCMS.BackStage.Api
{
    public class PageModel
    {
        public string PageTypeId { get; set; }
        public string PageName { get; set; }
    }

    public class ContentController : BaseApiController
    {

        //public ContentController(IContentService contentService) : base(contentService)
        //{}

        [HttpGet]
        [Route("LewCMS-api/pageTypes")]
        public HttpResponseMessage GetPageTypes()
        {
            //return Request.CreateResponse<object>(HttpStatusCode.OK, new { data = this._contentService.GetPageTypes() });
            return Request.CreateResponse();
        }

        [HttpGet]
        [Route("LewCMS-api/page-tree")]
        public HttpResponseMessage GetPageTree()
        {
            List<object> pageTree = new List<object>()
            {
                new {title = "One", children = new[] { new {title = "One_1"}, new {title = "One_2"}}},
                new {title = "Two", children = new[] { new {title = "Two_1"}, new {title = "Two_2"}}},
                new {title = "Three"}
            };

            return Request.CreateResponse<object>(HttpStatusCode.OK, pageTree);
        }

        [HttpPost]
        [Route("LewCMS-api/create")]
        public HttpResponseMessage Create(PageModel page)
        {
            return Request.CreateResponse<string>(HttpStatusCode.OK, string.Empty);
        }
    }
}
