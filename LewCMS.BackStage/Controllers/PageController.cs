using LewCMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LewCMS.BackStage.Controllers
{
    public class PageController : Controller
    {
        public PageController()
        {
            
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            return View();
        }
    }
}