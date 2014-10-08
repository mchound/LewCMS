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
}
