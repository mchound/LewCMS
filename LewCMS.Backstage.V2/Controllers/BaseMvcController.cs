using LewCMS.V2.Contents;
using LewCMS.V2.Users;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LewCMS.BackStage.V2.Controllers
{
    public abstract class BaseMvcController : Controller
    {
        private IAuthenticationManager _authenticationManager;
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return _authenticationManager ?? (_authenticationManager = HttpContext.GetOwinContext().Authentication);
            }
        }

        private IUserManager _userManager;
        public IUserManager UserManager
        {
            get { return _userManager; }
        }

        private IContentService _contentService;

        public IContentService ContentService
        {
            get { return _contentService; }
            set { _contentService = value; }
        }


        private SignInManager<ApplicationUser, string> _signInManager;

        public SignInManager<ApplicationUser, string> SignInManager
        {
            get
            {
                if (_signInManager == null)
                {
                    _signInManager = new SignInManager<ApplicationUser, string>(this.UserManager as DefaultUserManager, AuthenticationManager);
                }

                return _signInManager;
            }

        }

        public BaseMvcController(IUserManager userManager, IContentService contentService)
        {
            this._userManager = userManager;
            this._contentService = contentService;
        }

    }
}