using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Store
{
    public interface IStorable
    {
        string Id { get; set; }
        string StoreKey { get; }
        string StoreDirectory { get; }
        IStoreInfo GetStoreInfo();
    }

    public interface IStoreInfo
    {
        string Id { get; set; }
        string StoreKey { get; }
        string StoreDirectory { get; }
        IEnumerable<Type> RepresentedInterfaces { get; }
    }

    public abstract class BaseInfo : IStoreInfo
    {
        public string Id { get; set; }

        public abstract string StoreKey { get; }

        public abstract string StoreDirectory { get; }

        public abstract IEnumerable<Type> RepresentedInterfaces { get; }

        protected virtual IEnumerable<Type> GetRepresentedInterfaces(Type mainInterface)
        {
            return new Type[] { mainInterface }.Concat(mainInterface.GetInterfaces());
        }
    }

}
