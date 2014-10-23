using LewCMS.BackStage.Models.ViewModels;
using LewCMS.V2;
using LewCMS.V2.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LewCMS.BackStage.Helpers;
using LewCMS.BackStage.Models.ClientViewModels;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LewCMS.BackStage.Api
{
    public class ContentController : BaseApiController
    {

        public ContentController(IContentService contentService, IRouteManager routeManager) : base(contentService, routeManager)
        { }

        [HttpGet]
        [Route("LewCMS-api/pageTypes")]
        public HttpResponseMessage GetPageTypes()
        {
            return Request.CreateStandardOkResponse(this._contentService.GetPageTypes().AsClientModelsGroupedByCategory());
        }

        [HttpGet]
        [Route("LewCMS-api/page-tree")]
        public HttpResponseMessage GetPageTree(int? depth, string rootId)
        {
            IPage root = string.IsNullOrWhiteSpace(rootId) ? this._contentService.StartPage : this._contentService.GetFor<IPage, IPageInfo>(pi => pi.Id == rootId);

            if (root == null)
            {
                return Request.CreateStandardOkResponse(null);
            }

            PageTreeItem pageTree = this._contentService.GetPageTree(depth.HasValue ? depth.Value : int.MaxValue, root);
            object _pageTree = pageTree == null ? null : pageTree.AsClientModel();
            return Request.CreateStandardOkResponse(_pageTree);
        }

        [HttpGet]
        [Route("LewCMS-api/children")]
        public HttpResponseMessage GetChildren(string parentId)
        {
            if (parentId == "gerrard")
            {
                return Request.CreateResponse<object>(HttpStatusCode.OK, new { success = true, data = "Hej;på;Dif".Split(';') });
            }

            return Request.CreateResponse<object>(HttpStatusCode.OK, new { success = false, errorMessages = "Only one legend"});
        }

        [HttpPost]
        [Route("LewCMS-api/create/page")]
        public HttpResponseMessage CreatePage(CreatePageModel model)
        {
            if(!ModelState.IsValid)
            {
                return Request.CreateStandardErrorResponse(ModelState);
            }

            IPageType pageType = this._contentService.GetPageType(pt => pt.Id == model.ContentTypeId);

            if (pageType == null)
            {
                return Request.CreateStandardErrorResponse(new string[] { string.Format("No Page Type With id: {0} found", model.ContentTypeId) });
            }

            IPage page = pageType.CreateInstance<IPage>(model.Name);
            page.ParentId = model.ParentId;
            page.Route = this._routeManager.CreatePageRoute(page.Id, model.Name, model.ParentId);

            this._contentService.Save(page);

            return Request.CreateStandardOkResponse(new PageTreeItem 
            { 
                Id = page.Id,
                Name = page.Name,
                IsStartPage = page.Route == "/",
                ParentId = page.ParentId,
                HasChildren = false,
                Children = Enumerable.Empty<PageTreeItem>()
            }.AsClientModel());
        }

        [HttpDelete]
        [Route("LewCMS-api/delete/page")]
        public HttpResponseMessage DeletePage(string id)
        {
            IPage page = null;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Request.CreateStandardErrorResponse("Delete failed. No id provided");
            }
            else
            {
                page = this._contentService.GetFor<IPage, PageInfo>(pi => pi.Id == id);
            }

            if (page == null)
            {
                return Request.CreateStandardErrorResponse(string.Format("Delete failed. No page found with id: {0}", id));
            }

            if (this._contentService.GetChildren(page).Count() > 0)
            {
                return Request.CreateStandardErrorResponse("Page has children. Can't delete page with children");
            }
            else
            {
                page.InTrash = true;
                this._contentService.Save(page);
                return Request.CreateStandardOkResponse(new { id = page.Id, parentId = page.ParentId});
            }
        }

        [HttpGet]
        [Route("LewCMS-api/edit/page")]
        public HttpResponseMessage EditPage(string id)
        {
            IPage page = this._contentService.GetPage(pi => pi.Id == id);

            string s = page.ContentType.Properties.ToList()[2].ClientValidationNotation;

            object response = new
            {
                html = MvcHelpers.RenderViewToString("Content", "EditPage", page),
                clientScripts = page.ClientScriptPaths(),
                propertyNames = page.ContentType.Properties.Select(p => p.Name)
            };

            return Request.CreateStandardOkResponse(response);
            
        }
    }
}
