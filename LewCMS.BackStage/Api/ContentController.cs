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

    public class ContentController : ApiController
    {
        [HttpPost]
        [Route("LewCMS-api/create")]
        public HttpResponseMessage Create(PageModel page)
        {
            return Request.CreateResponse<string>(HttpStatusCode.OK, string.Empty);
        }
    }
}
