using LewCMS.V2.Startup;
using LewCMS.V2.Store;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Users
{
    public interface IUserService : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserLockoutStore<ApplicationUser, string>, IUserTwoFactorStore<ApplicationUser, string>
    {

    }
}
