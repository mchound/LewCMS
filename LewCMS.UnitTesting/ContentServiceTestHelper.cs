using LewCMS.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.UnitTesting
{
    public class ContentServiceTestHelper
    {
        private static ContentServiceTestHelper _instance = new ContentServiceTestHelper();
        private IContentService _contentService;
        private IInitializeService _initializeService;
        private IContentCacheService _contentCacheService;
        private IContentRepository _contentRepository;

        public ContentServiceTestHelper()
        {

        }

        public static ContentServiceTestHelper Instance 
        {
            get { return _instance; }
        }

        public IContentService ContentService
        {
            get { return this._contentService; }
        }

        public IContentCacheService ContentCacheService
        {
            get { return this._contentCacheService; }
        }

        public IInitializeService InitializeService
        {
            get { return this._initializeService; }
        }

        public IContentRepository ContentRepository
        {
            get { return this._contentRepository; }
        }

        public void SetContentService(IContentService contentService)
        {
            this._contentService = contentService;
        }

        public void SetContentCacheService(IContentCacheService contentCacheService)
        {
            this._contentCacheService = contentCacheService;
        }

        public void SetInitializeService(IInitializeService initializeService)
        {
            this._initializeService = initializeService;
        }

        public void SetContentRepository(IContentRepository contentRepository)
        {
            this._contentRepository = contentRepository;
        }
    }
}
