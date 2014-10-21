using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Contents
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PropertyAttribute : Attribute
    {
        public string View { get; set; }
        public string ViewPath { get; set; }
        public string ClientScript { get; set; }
        public string ClientScriptPath { get; set; }
    }
}
