using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Contents
{
    public interface ISection : IContent
    {
    }

    public abstract class Section : Content, ISection
    {
        public override string StoreDirectory
        {
            get { return "Sections"; }
        }

        public override IStoreInfo GetStoreInfo()
        {
            return new SectionInfo(this);
        }
    }
}
