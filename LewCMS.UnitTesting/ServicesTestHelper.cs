using LewCMS.Core.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.UnitTesting
{
    public class ServicesTestHelper
    {
        private static ServicesTestHelper _instance = new ServicesTestHelper();
        private IContentService _contentService;
        private IInitializeService _initializeService;
        private IContentCacheService _contentCacheService;
        private IContentRepository _contentRepository;
        private IPersistService _filePersistsService;

        public static string FILE_PERSIST_PATH = @"C:\tolu00\Playground\LewCMS\LewCMS.UnitTesting\App_Data\LewCMS";
        //public static string FILE_PERSIST_PATH = @"C:\Users\Tobias\Documents\Visual Studio 2013\Projects\MyWebApplication\LewCMS.UnitTesting\App_Data\LewCMS";

        public ServicesTestHelper()
        {

        }

        public static ServicesTestHelper Instance 
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

        public IPersistService PersistsService
        {
            get { return this._filePersistsService; }
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

        public void SetFilePersistsService(IPersistService filePersistsService)
        {
            this._filePersistsService = filePersistsService;
        }
    }
}
