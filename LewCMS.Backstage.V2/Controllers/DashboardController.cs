using LewCMS.BackStage.V2.Models.ViewModels;
using LewCMS.V2.Contents;
using LewCMS.V2.Users;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LewCMS.BackStage.V2.Controllers
{
    public class DashboardController : BaseMvcController
    {
        public DashboardController(IUserManager userManager, IContentService contentService) : base(userManager, contentService) { }
        
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

        [Authorize]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return View("Login");
        }
    }
}