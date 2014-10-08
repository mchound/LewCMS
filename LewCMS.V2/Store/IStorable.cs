using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Store
{
    public interface IStorable
    {
        string StoreKey { get; }
        string StoreDirectory { get; }
        IStoreInfo GetStoreInfo();
    }

    public interface IStoreInfo
    {
        string StoreKey { get; }
        string StoreDirectory { get; }
        Type RepresentedInterface { get; }
    }

    public abstract class BaseInfo : IStoreInfo
    {
        Jag borde lägga till id här så det vi ska spara åtminstonde har Id att lämna en nyckel på
        public abstract string StoreKey { get; }

        public abstract string StoreDirectory { get; }

        public abstract Type RepresentedInterface { get; }
    }

}
