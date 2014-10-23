using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LewCMS.BackStage.Helpers
{
    public static class MvcHelpers
    {
        public static string RenderViewToString(string controllerName, string viewName, object model)
        {
            ViewDataDictionary viewData = new ViewDataDictionary();
            viewData.Model = model;
            var context = HttpContext.Current;
            var contextBase = new HttpContextWrapper(context);
            var routeData = new RouteData();
            routeData.Values.Add("controller", controllerName);

            var controllerContext = new ControllerContext(contextBase, routeData, new EmptyController());

            var razorViewEngine = new RazorViewEngine();
            var razorViewResult = ViewEngines.Engines.FindView(controllerContext, viewName, string.Empty);

            var writer = new StringWriter();
            var viewContext = new ViewContext(controllerContext, razorViewResult.View, new ViewDataDictionary(viewData), new TempDataDictionary(), writer);
            razorViewResult.View.Render(viewContext, writer);
            razorViewResult.ViewEngine.ReleaseView(controllerContext, razorViewResult.View);

            return writer.ToString();
        }
    }


    public class EmptyController : ControllerBase
    {
        protected override void ExecuteCore()
        {

        }
    }
}