using LewCMS.Core.Content;
using Microsoft.Practices.Unity;
//using System.Web.Http;
using System.Web.Mvc;

namespace LewCMS
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IInitializeService, InitializeService>();
            container.RegisterType<IPersistService, FilePersistService>();
            container.RegisterType<IRouteManager, RouteManager>();
            container.RegisterType<IContentRepository, ContentRepository>();
            container.RegisterType<IContentCacheService, ContentCacheService>();
            container.RegisterType<IContentService, ContentService>();
            container.RegisterType<ISerializer, LewCMSJsonSerializer>();
            
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
        }
    }
}