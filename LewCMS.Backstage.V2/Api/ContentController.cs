using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LewCMS.BackStage.V2.Http;
using LewCMS.BackStage.V2.Helpers;
using LewCMS.V2.Contents;
using LewCMS.V2;
using LewCMS.BackStage.V2.Models.ClientModels;

namespace LewCMS.BackStage.V2.Api
{
    public class ContentController : BaseApiController
    {
        public ContentController(IContentService contentService, IRouteManager routeManager) : base(contentService, routeManager)
        { }

        [HttpGet]
        [Route("LewCMS-api/pageTypes")]
        public HttpResponseMessage PageTypes()
        {
            return Request.StandardOkResponse(this._contentService.GetPageTypes().ForClient());
        }

        [HttpGet]
        [Route("LewCMS-api/pageTree")]
        public HttpResponseMessage GetPageTree(int? depth, string rootId)
        {
            IPage root = string.IsNullOrWhiteSpace(rootId) ? this._contentService.StartPage : this._contentService.GetFor<IPage, IPageInfo>(pi => pi.Id == rootId);

            if (root == null)
            {
                return Request.StandardOkResponse(null);
            }

            PageTreeItem pageTree = this._contentService.GetPageTree(depth.HasValue ? depth.Value : int.MaxValue, root);
            object _pageTree = pageTree == null ? null : pageTree.ForClient();
            return Request.StandardOkResponse(_pageTree);
        }
    }
}
