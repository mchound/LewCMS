using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2
{
    public interface IGlobalConfigInfo : IContentInfo
    {
        
    }

    public class GlobalConfigInfo : ContentInfo, IGlobalConfigInfo
    {
        public GlobalConfigInfo()
        {

        }

        public GlobalConfigInfo(IGlobalConfig globalConfig) : base(globalConfig) { }

        public override Type ContentTypeInterface
        {
            get { return typeof(IGlobalConfig); }
        }
    }
}
