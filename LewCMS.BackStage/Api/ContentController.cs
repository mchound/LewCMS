using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LewCMS.BackStage.Api
{
    public class ContentController : ApiController
    {
        [HttpGet]
        [Route("LewCMS-api/test")]
        public HttpResponseMessage Create()
        {
            return Request.CreateResponse<string>(HttpStatusCode.OK, string.Empty);
        }
    }
}
