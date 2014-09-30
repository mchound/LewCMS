using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2
{
    public interface ISection : IContent
    {
    }

    public abstract class Section : Content, ISection
    {
        public override IContentInfo ContentInfo
        {
            get { throw new NotImplementedException(); }
        }
    }
}
