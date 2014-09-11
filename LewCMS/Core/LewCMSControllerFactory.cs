using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace LewCMS.Core
{
    public class LewCMSControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            //CMS cms = CMS.Instance();
            //IPage page = cms.GetPageFromRoute(requestContext.HttpContext.Request.Path);

            IPage page = null;

            if (page == null)
                return base.CreateController(requestContext, controllerName);

            RequestContext customRequestContext = requestContext;
            customRequestContext.RouteData.Values["controller"] = page.PageType.ControllerName;
            Type controllerType = base.GetControllerType(requestContext, page.PageType.ControllerName);

            return (IController)Activator.CreateInstance(controllerType, page);
        }

    }
}
