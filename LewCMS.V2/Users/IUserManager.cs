using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Users
{
    public interface IUserManager
    {
    }

    public class DefaultUserManager : UserManager<ApplicationUser, string>, IUserManager
    {
        public DefaultUserManager(IUserService userService) : base(userService)
        {
            this.UserValidator = new UserValidator<ApplicationUser, string>(this){AllowOnlyAlphanumericUserNames = false};
        }
    }
}
