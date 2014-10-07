using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2
{
    public interface IPage : IContent
    {
        string ParentId { get; set; }
        string Route { get; set; }
    }

    public abstract class Page : Content, IPage
    {
        public string Route { get; set; }
        public string ParentId { get; set; }
        
        public override IContent Clone()
        {
            IPage clone = base.Clone() as IPage;
            clone.Route = this.Route;
            clone.ParentId = this.ParentId;
            return clone;
        }

        public override IContentInfo ContentInfo()
        {
            return new PageInfo(this);
        }
    }
}
