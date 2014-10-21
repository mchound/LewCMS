using LewCMS.Core;
using LewCMS.V2.Contents;
using LewCMS.V2.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LewCMS.BackStage.Controllers
{
    [RouteArea("LewCMS")]
    public class ContentController : BaseController
    {
        public ContentController(IUserManager userManager, IContentService contentService) : base(userManager, contentService)
        {
            
        }

        [Route("content/edit/page")]
        public ActionResult EditPage(string id)
        {
            IPage page = this.ContentService.GetPage(pi => pi.Id == id);
            return View(page);
        }
    }
}