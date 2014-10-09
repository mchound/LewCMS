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

    public class UserPassword : IUserPassword, IStoreInfo
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PasswordHash { get; set; }

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
            get { return typeof(IUserPassword); }
        }
    }
}
