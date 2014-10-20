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
        bool InTrash { get; set; }
    }

    public abstract class Section : Content, ISection
    {
        public bool InTrash { get; set; }

        public override string StoreDirectory
        {
            get { return "Sections"; }
        }

        public override IContent Clone()
        {
            ISection clone = base.Clone() as ISection;
            clone.InTrash = this.InTrash;
            return clone;
        }

        public override IStoreInfo GetStoreInfo()
        {
            return new SectionInfo(this);
        }
    }
}
