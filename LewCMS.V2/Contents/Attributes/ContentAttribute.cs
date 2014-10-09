using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Contents.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ContentTypeAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string Id { get; set; }
    }
}
