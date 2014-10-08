﻿using System.Web.Routing;
using System.Web.Mvc;
using System.Reflection;
using System.Web.Hosting;
using LewCMS.V2.Mvc;
using LewCMS.V2.VirtualFileSystem;
using LewCMS.V2;
using System.Web.Http;

namespace LewCMS.Core.Startup
{
    public class LewCMSStartup
    {
        public static void OnStartup()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), new LewCMSHttpControllerSelector(GlobalConfiguration.Configuration));
            ControllerBuilder.Current.SetControllerFactory(typeof(LewCMSControllerFactory));
            //GlobalConfiguration.Configuration.Services.Replace(typeof(IAssembliesResolver), new LewCMSAssemblyResolver());
            HostingEnvironment.RegisterVirtualPathProvider(new LewCMSVirtualPathProvider(Configuration.BACKSTAGE_FILE_PATH));
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Insert(0, new LewCMSRazorViewEngine());

            Application.Current.SetApplicationAssembly(Assembly.GetCallingAssembly());
            //UnityConfig.RegisterComponents();
        }
    }

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CatchAll",
                url: "{*url}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.IgnoreRoute("{*staticfile}", new { staticfile = @".*\.(css|js|gif|jpg)(/.*)?" });
        }
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}