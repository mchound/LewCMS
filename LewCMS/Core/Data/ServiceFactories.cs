using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LewCMS.Core.Data
{
    public class LewCMSConfig
    {
        private static LewCMSConfig _instance = new LewCMSConfig();
        private Func<ILewCMSCacheService> _cacheServiceFactory = () => null;
        private Func<ILewCMSInitializeService> _initializeServiceFactory = () => null;
        private Func<ILewCMSRepository> _repositoryServiceFactory = () => null;
        private Func<ILewContentService> _contentServiceFactory = () => null;

        public Assembly ApplicationAssembly { get; private set; }

        public static LewCMSConfig Current
        {
            get { return _instance; }
        }

        public void SetApplicationAssembly(Assembly applicationAssembly)
        {
            this.ApplicationAssembly = applicationAssembly;
        }

        public void SetCacheService(ILewCMSCacheService cacheService)
        {
            if (cacheService == null)
            {
                throw new ArgumentNullException("cacheService");
            }

            _cacheServiceFactory = () => cacheService;
        }

        public void SetInitializeService(ILewCMSInitializeService initializeService)
        {
            if (initializeService == null)
            {
                throw new ArgumentNullException("initializeService");
            }

            _initializeServiceFactory = () => initializeService;
        }

        public void SetRepository(ILewCMSRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            _repositoryServiceFactory = () => repository;
        }

        public void SetContentService(ILewContentService contentService)
        {
            if (contentService == null)
            {
                throw new ArgumentNullException("contentService");
            }

            _contentServiceFactory = () => contentService;
        }

        public ILewContentService GetContentService()
        {
            return this._contentServiceFactory();
        }

        public ILewCMSCacheService GetCacheService()
        {
            return this._cacheServiceFactory();
        }

        public ILewCMSRepository GetRepository()
        {
            return this._repositoryServiceFactory();
        }

        public ILewCMSInitializeService GetInitializeService()
        {
            return this._initializeServiceFactory();
        }

    }
}
