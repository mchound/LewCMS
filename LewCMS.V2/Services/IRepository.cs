﻿using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Services
{
    public interface IRepository
    {
        void Save(IStorable storable);

        IStorable GetFor(IStoreInfo storeInfo);
        Tstorable GetFor<Tstorable, Tinfo>(Func<Tinfo, bool> predicate) where Tstorable : class,IStorable where Tinfo : class,IStoreInfo;

        IEnumerable<IStorable> Get();
        IEnumerable<T> Get<T>() where T : class, IStorable;
        IEnumerable<Tstorable> Get<Tstorable, Tinfo>(Func<Tinfo, bool> predicate) where Tstorable : class,IStorable where Tinfo : class,IStoreInfo;

        IEnumerable<IContentType> GetContentTypes();

        IEnumerable<IStoreInfo> Delete(IStoreInfo storeInfo);
        IEnumerable<IStoreInfo> Delete(Func<IStoreInfo, bool> predicate);
    }
}
