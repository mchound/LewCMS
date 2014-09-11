using MyWebApplication.PageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebApplication.Controllers
{
    public class MyFirstPageTypeController : Controller
    {
        MyFirstPageType _currentPage;

        public MyFirstPageTypeController(MyFirstPageType currentPage)
        {
            this._currentPage = currentPage;
        }

        public ActionResult Index()
        {
            return View(this._currentPage);
        }
    }
}