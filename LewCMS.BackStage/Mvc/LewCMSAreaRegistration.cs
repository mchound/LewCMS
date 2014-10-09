using System.Web.Mvc;

namespace LewCMS.BackStage.Mvc
{
    public class LewCMSAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LewCMS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LewCMS_login",
                "LewCMS/login",
                new { controller = "Dashboard", action = "Login", id = UrlParameter.Optional },
                new[] { "LewCMS.BackStage.Controllers" }
            );

            context.MapRoute(
                "LewCMS_logout",
                "LewCMS/logout",
                new { controller = "Dashboard", action = "Logout", id = UrlParameter.Optional },
                new[] { "LewCMS.BackStage.Controllers" }
            );

            context.MapRoute(
                "LewCMS_default",
                "LewCMS/{controller}/{action}/{id}",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
                new[] { "LewCMS.BackStage.Controllers"}
            );

            
        }
    }
}