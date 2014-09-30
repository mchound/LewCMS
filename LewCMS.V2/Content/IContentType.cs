using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Content
{
    public interface IContentType
    {
        string Id { get; set; }
        string DisplayName { get; set; }
        string TypeName { get; set; }
        List<Property> Properties { get; set; }
    }
}
