using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LewCMS.Core
{
    public interface IPageType
    {
        string Id { get; set; }
        string DisplayName { get; set; }
        string TypeName { get; set; }
        string ControllerName { get; set; }
        List<Property> Properties { get; set; }
        void LoadProperties(IPage page);
    }

    public class PageType : IPageType
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public List<Property> Properties { get; set; }
        public string TypeName { get; set; }
        public string ControllerName { get; set; }

        public PageType ()
	    {
            this.Properties = new List<Property>();
	    }

        public void LoadProperties(IPage page)
        {
            foreach (IProperty property in this.Properties)
            {
                property.Set(page[property.Name]);
            }
        }
    }
}
