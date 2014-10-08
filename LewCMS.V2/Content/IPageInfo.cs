using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2
{
    public interface IPageInfo : IContentInfo
    {
        string Route { get; set; }
        string ParentId { get; set; }
    }

    public class PageInfo : ContentInfo, IPageInfo
    {
        public string Route { get; set; }
        public string ParentId { get; set; }

        public PageInfo()
        {

        }

        public PageInfo(IPage page) : base(page)
        {
            this.Route = page.Route;
            this.ParentId = page.ParentId;
        }

        public override Type RepresentedInterface
        {
            get { return typeof(IPage); }
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
            get { return "Pages"; }
        }
    }
}
