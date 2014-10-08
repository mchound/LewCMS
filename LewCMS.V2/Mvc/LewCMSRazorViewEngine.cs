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
            AreaViewLocationFormats = new[]
            {
                "~/BackStage/{2}/Views/{1}/{0}.cshtml",
                "~/BackStage/{2}/Views/{1}/{0}.vbhtml",
                "~/BackStage/{2}/Views/Shared/{0}.cshtml",
                "~/BackStage/{2}/Views/Shared/{0}.vbhtml",
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
            AreaMasterLocationFormats = new[]
            {
                "~/BackStage/{2}/Views/{1}/{0}.cshtml",
                "~/BackStage/{2}/Views/{1}/{0}.vbhtml",
                "~/BackStage/{2}/Views/Shared/{0}.cshtml",
                "~/BackStage/{2}/Views/Shared/{0}.vbhtml",
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
            AreaPartialViewLocationFormats = new[]
            {
                "~/BackStage/{2}/Views/{1}/{0}.cshtml",
                "~/BackStage/{2}/Views/{1}/{0}.vbhtml",
                "~/BackStage/{2}/Views/Shared/{0}.cshtml",
                "~/BackStage/{2}/Views/Shared/{0}.vbhtml",
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
        }
    }
}
