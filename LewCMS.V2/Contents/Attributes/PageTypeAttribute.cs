using LewCMS.V2.Contents.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Contents
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PageTypeAttribute : ContentTypeAttribute
    {
        public string ControllerName { get; set; }
    }
}
