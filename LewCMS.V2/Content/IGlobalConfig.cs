using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2
{
    public interface IGlobalConfig : IContent
    {
    }

    public abstract class GlobalConfig : Content, IGlobalConfig
    {
        public override IContentInfo ContentInfo()
        {
            return new GlobalConfigInfo(this);
        }
    }
}
