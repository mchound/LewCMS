using LewCMS.V2.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LewCMS.BackStage.Api
{
    public abstract class BaseApiController : ApiController
    {
        protected IContentService _contentService;

        public BaseApiController(IContentService contentService)
        {
            this._contentService = contentService;
        }
    }
}
