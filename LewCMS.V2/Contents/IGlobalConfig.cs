using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Contents
{
    public interface IGlobalConfig : IContent
    {
    }

    public abstract class GlobalConfig : Content, IGlobalConfig
    {
        public override string StoreDirectory
        {
            get { return "GlobalConfigs"; }
        }

        public override IStoreInfo GetStoreInfo()
        {
            return new GlobalConfigInfo(this);
        }
    }
}
