using LewCMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LewCMS.BackStage.Controllers
{
    public class ContentController : Controller
    {
        public ContentController()
        {
            
        }

        [Route(Name = "LewCMS-editUI/Page/{id:int}")]
        public ActionResult Index(string id)
        {
            return View(id);
        }
    }
}