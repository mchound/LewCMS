using LewCMS.V2;
using LewCMS.V2.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LewCMS.BackStage.V2.Api
{
    public abstract class BaseApiController : ApiController
    {
        protected IContentService _contentService;
        protected IRouteManager _routeManager;

        public BaseApiController(IContentService contentService, IRouteManager routeManager)
        {
            this._contentService = contentService;
            this._routeManager = routeManager;
        }

    }
}