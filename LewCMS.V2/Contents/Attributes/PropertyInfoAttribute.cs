using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Contents
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyInfoAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
