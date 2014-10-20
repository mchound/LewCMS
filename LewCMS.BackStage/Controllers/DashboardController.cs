using LewCMS.BackStage.Models.ViewModels;
using LewCMS.V2.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LewCMS.BackStage.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController(IUserManager userManager) : base(userManager) { }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            SignInStatus status = await this.SignInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, shouldLockout: false);

            switch (status)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Dashboard");
                case SignInStatus.Failure:
                case SignInStatus.LockedOut:
                case SignInStatus.RequiresVerification:
                default:
                    ModelState.AddModelError("", "Invalid login attempt");
                    return View(model);
            }
        }

        
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return View("Login");
        }
    }
}