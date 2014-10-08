using LewCMS.V2.Store;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Users
{
    public class UserStoreService : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>
    {

        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return Task.FromResult<ApplicationUser>(null);
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            return Task.FromResult<object>(null);
        }
    }

    public class ApplicationUser : IUser, IStorable
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string StoreKey
        {
            get { return this.Id; }
        }

        public string StoreDirectory
        {
            get { return "Users"; }
        }

        public IStoreInfo GetStoreInfo()
        {
            return null;
        }
    }

}
