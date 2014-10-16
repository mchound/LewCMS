using LewCMS.V2.Contents;
using LewCMS.V2.Serialization;
using LewCMS.V2.Startup;
using LewCMS.V2.Store;
using LewCMS.V2.Store.Cache;
using LewCMS.V2.Store.FileSystem;
using LewCMS.V2.Users;
using Microsoft.Practices.Unity;
using System.Web.Http;
using System.Web.Mvc;

namespace LewCMS.V2
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IRepository, DefaultRepository>();
            container.RegisterType<IInitializeService, DefaultInitializeService>();
            container.RegisterType<IFileStoreService, DefaultFileStoreService>();
            container.RegisterType<ICacheStoreService, DefaultCacheStoreService>();
            container.RegisterType<ISerializeService, DefaultJsonSerializer>();
            container.RegisterType<IUserManager, DefaultUserManager>();
            container.RegisterType<IContentService, DefaultContentService>();
            container.RegisterType<IRouteManager, RouteManager>();

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
        }
    }
}
