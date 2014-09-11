using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PageTypeAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string Id { get; set; }
        public string ControllerName { get; set; }
    }
}
