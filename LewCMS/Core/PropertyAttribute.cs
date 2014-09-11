using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PropertyAttribute : Attribute
    {
        public string View { get; set; }
    }
}
