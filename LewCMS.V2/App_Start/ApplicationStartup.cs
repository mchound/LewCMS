using System.Web.Routing;
using System.Web.Mvc;
using System.Reflection;
using System.Web.Hosting;
using LewCMS.V2.Mvc;
using LewCMS.V2.VirtualFileSystem;
using LewCMS.V2;
using System.Web.Http;
using LewCMS.V2.Users;
using LewCMS.V2.Store;
using System.Linq;
using Microsoft.AspNet.Identity;
using System;
using LewCMS.V2.Contents;
using LewCMS.V2.Startup;
using LewCMS.V2.Validation;
using System.ComponentModel.DataAnnotations;

namespace LewCMS.Core.Startup
{
    public class LewCMSStartup
    {
        public static void OnStartup()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //ControllerBuilder.Current.SetControllerFactory(typeof(LewCMSControllerFactory));
            HostingEnvironment.RegisterVirtualPathProvider(new LewCMSVirtualPathProvider(Configuration.BACKSTAGE_FILE_PATH));
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Insert(0, new LewCMSRazorViewEngine());

            Application.Current.SetApplicationAssembly(Assembly.GetCallingAssembly());
            UnityConfig.RegisterComponents();

            ClientValidation.Factories.Add(typeof(RequiredAttribute), new RequiredClientValidationFactory());
            ClientValidation.Factories.Add(typeof(MinLengthAttribute), new MinLengthClientValidationFactory());

            CreateAdminUser();
            InitializeContentService();
        }

        private static void InitializeContentService()
        {
            IContentService contentService = DependencyResolver.Current.GetService<IContentService>();
            contentService.Initialize(DependencyResolver.Current.GetService<IInitializeService>(), Application.Current.ApplicationAssembly);
        }

        private static void CreateAdminUser()
        {
            IRepository repository = DependencyResolver.Current.GetService<IRepository>();
            IUserService userService = DependencyResolver.Current.GetService<IUserService>();
            
            if(repository.Get<IApplicationUser>().Count() <= 0)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin@lewcms.com"
                };

                UserManager<ApplicationUser, string> userManager = new UserManager<ApplicationUser, string>(userService);
                IdentityResult result = userManager.Create<ApplicationUser, string>(user, "admin1234");
            }
        }
    }

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                "LewCMS_login",
                "LewCMS/login",
                new { controller = "Dashboard", action = "Login", id = UrlParameter.Optional },
                new[] { "LewCMS.BackStage.Controllers" }
            );

            routes.MapRoute(
                "LewCMS_logout",
                "LewCMS/logout",
                new { controller = "Dashboard", action = "Logout", id = UrlParameter.Optional },
                new[] { "LewCMS.BackStage.Controllers" }
            );

            routes.MapRoute(
                "LewCMS_default",
                "LewCMS/{controller}/{action}/{id}",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
                new[] { "LewCMS.BackStage.Controllers" }
            );

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
