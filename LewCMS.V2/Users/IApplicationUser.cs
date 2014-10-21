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

    public class ApplicationUser : BaseInfo, IApplicationUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public override string StoreKey
        {
            get { return this.Id; }
        }

        public override string StoreDirectory
        {
            get { return "Users"; }
        }

        public IStoreInfo GetStoreInfo()
        {
            return this;
        }

        public override IEnumerable<Type> RepresentedInterfaces
        {
            get { return base.GetRepresentedInterfaces(typeof(IApplicationUser)); }
        }
    }
}
