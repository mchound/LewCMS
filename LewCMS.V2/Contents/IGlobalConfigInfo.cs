using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Contents
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

        public override Type RepresentedInterface
        {
            get { return typeof(IGlobalConfig); }
        }

        public override string StoreKey
        {
            get
            {
                return string.Format("Content-{0}[version-{1}][lang-{2}]", this.Id, this.Version, this.Culture.TwoLetterISOLanguageName);
            }
        }

        public override string StoreDirectory
        {
            get { return "GlobalConfigs"; }
        }
    }
}
