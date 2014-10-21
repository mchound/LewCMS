using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Users
{
    public interface IUserPassword : IStorable, IStoreInfo
    {
        string UserId { get; set; }
        string PasswordHash { get; set; }
    }

    public class UserPassword : BaseInfo, IUserPassword
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PasswordHash { get; set; }

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
            get { return base.GetRepresentedInterfaces(typeof(IUserPassword)); }
        }
    }
}
