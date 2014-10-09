using LewCMS.V2.Services;
using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Users
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IRepository repository) : base(repository) { }

        public Task CreateAsync(ApplicationUser user)
        {
            Repository.Save(user);
            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            Repository.Delete(user);
            return Task.FromResult<object>(null);
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            ApplicationUser user = Repository.GetFor<ApplicationUser, ApplicationUser>(u => u.Id == userId);
            return Task.FromResult<ApplicationUser>(user);
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            ApplicationUser user = Repository.GetFor<ApplicationUser, ApplicationUser>(u => u.UserName == userName);
            return Task.FromResult<ApplicationUser>(user);
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            Repository.Save(user);
            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            IUserPassword userPassword = Repository.GetFor<IUserPassword, UserPassword>(up => up.UserId == user.Id);
            return Task.FromResult<string>(userPassword.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            IUserPassword userPassword = Repository.GetFor<IUserPassword, UserPassword>(up => up.UserId == user.Id);
            return Task.FromResult<bool>(userPassword != null && !string.IsNullOrWhiteSpace(userPassword.PasswordHash));
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                IUserPassword userPassword = Repository.GetFor<IUserPassword, UserPassword>(up => up.UserId == user.Id);
                Repository.Delete(userPassword);
            }
            else
            {
                IUserPassword userPassword = new UserPassword
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    PasswordHash = passwordHash
                };
                Repository.Save(userPassword);
            }


            return Task.FromResult<object>(null);
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult<bool>(false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult<bool>(false);
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            throw new NotImplementedException();
        }
    }
}
