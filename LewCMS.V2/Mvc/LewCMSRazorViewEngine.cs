using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LewCMS.V2.Mvc
{
    public class LewCMSRazorViewEngine : RazorViewEngine
    {
        public LewCMSRazorViewEngine()
        {
            ViewLocationFormats = new[]
            {
                "~/BackStage/LewCMS/Views/{1}/{0}.cshtml",
                "~/BackStage/LewCMS/Views/{1}/{0}.vbhtml",
                "~/BackStage/LewCMS/Views/Shared/{0}.cshtml",
                "~/BackStage/LewCMS/Views/Shared/{0}.vbhtml",
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml"
            };
            MasterLocationFormats = new[]
            {
                "~/BackStage/LewCMS/Views/{1}/{0}.cshtml",
                "~/BackStage/LewCMS/Views/{1}/{0}.vbhtml",
                "~/BackStage/LewCMS/Views/Shared/{0}.cshtml",
                "~/BackStage/LewCMS/Views/Shared/{0}.vbhtml",
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml"
            };
            PartialViewLocationFormats = new[]
            {
                "~/BackStage/LewCMS/Views/{1}/{0}.cshtml",
                "~/BackStage/LewCMS/Views/{1}/{0}.vbhtml",
                "~/BackStage/LewCMS/Views/Shared/{0}.cshtml",
                "~/BackStage/LewCMS/Views/Shared/{0}.vbhtml",
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml"
            };
        }
    }
}
