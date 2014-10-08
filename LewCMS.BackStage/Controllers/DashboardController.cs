using LewCMS.V2.Users;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LewCMS.BackStage.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            UserManager<ApplicationUser, string> userManager = new UserManager<ApplicationUser, string>(new UserStoreService());

            userManager.Create<ApplicationUser, string>(new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "MyUserName" }, "password");

            //IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;

            

            return View();
        }
    }
}