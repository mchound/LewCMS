using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2
{
    public interface IContentType
    {
        string Id { get; set; }
        string DisplayName { get; set; }
        string TypeName { get; set; }
        List<Property> Properties { get; set; }
    }

    public abstract class ContentType : IContentType
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public List<Property> Properties { get; set; }
        public string TypeName { get; set; }

        public ContentType()
        {
            this.Properties = new List<Property>();
        }
    }
}
