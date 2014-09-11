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

        private CMS _cms;

        public PageController()
        {
            this._cms = CMS.Instance();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            return View(_cms.PageTypes);
        }

        //public ActionResult Create(string pageTypeId)
        //{
        //    PageType pageType = this._cms.PageTypes.FirstOrDefault(p => p.Id == pageTypeId);
        //    IPage page = pageType.CreateNewPage("Page-Name" + DateTime.Now.Ticks.ToString(), null);
        //    this._cms.Pages.Add(page as Page);
        //    this._cms.Persist();

        //    return RedirectToAction("Edit", new { id = page.Id });
        //}

        public ActionResult Edit(string id)
        {
            IPage page = this._cms.Pages.First(p => p.Id == id);
            page.PageType.LoadProperties(page);

            return View();
        }
    }
}