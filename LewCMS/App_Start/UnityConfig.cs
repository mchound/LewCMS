using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using LewCMS.Core.Content;

namespace LewCMS
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            container.RegisterType<IContentService, ContentService>();
            container.RegisterType<IContentRepository, ContentRepository>();
            container.RegisterType<IContentCacheService, ContentCacheService>();
            container.RegisterType<IInitializeService, InitializeService>();
            container.RegisterType<ISerializer, LewCMSJsonSerializer>();
            container.RegisterType<IRouteManager, RouteManager>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}