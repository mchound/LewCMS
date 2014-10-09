using LewCMS.V2.Store;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Users
{
    public interface IApplicationUser : IUser, IStorable
    {
        string Email { get; set; }
    }

    public class ApplicationUser : IApplicationUser, IStoreInfo
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
            return this;
        }

        public Type RepresentedInterface
        {
            get { return typeof(IApplicationUser); }
        }
    }
}
