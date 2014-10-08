using LewCMS;
using LewCMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace MyWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        //public HomeController(IContentService contentService)
        //{
        //    IContentService serv = contentService;

        //    var pageTypes = serv.GetPageTypes();
        //}

        public ActionResult Index()
        {
            //ILewContentService service = new LewContentService();
            //IEnumerable<IPageType> pageTypes = service.GetPageTypes();

            //service.CreatePage("66f37878-25bb-471c-9363-d15e400b6cbf", "My first page");
            
            return View();
        }
    }
}