using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2
{
    public interface ISectionInfo : IContentInfo
    {
        
    }

    public class SectionInfo : ContentInfo, ISectionInfo
    {
        public SectionInfo()
        {

        }

        public SectionInfo(ISection section) : base(section){}

        public override Type RepresentedInterface
        {
            get { return typeof(ISection); }
        }

        public override string StoreDirectory
        {
            get { return "Sections"; }
        }
    }
}
