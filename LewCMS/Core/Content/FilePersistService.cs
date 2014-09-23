using LewCMS.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LewCMS.Core.Content
{
    public class FilePersistService : BasePersistService
    {
        private const string PAGE_FILE_NAME_FORMAT = "Page-{0}[{1}].json";
        private const string PAGE_TYPES_FILE_NAME_FORMAT = "PageTypes.json";
        private const string PAGE_INFO_FILE_NAME_FORMAT = "PageInfo.json";
        private const string PAGES_FOLDER_NAME = "Pages";
        private const string PAGE_TYPES_FOLDER_NAME = "PageTypes";

        private string _pageFolderPath;
        private string _pageTypesFolderPath;
        private ISerializer _serializer;

        public FilePersistService(ISerializer serializer)
        {
            this._serializer = serializer;
            this._pageFolderPath = string.Concat(Configuration.PERSITS_VIRTUAL_FILE_PATH, @"\", PAGES_FOLDER_NAME);
            this._pageTypesFolderPath = string.Concat(Configuration.PERSITS_VIRTUAL_FILE_PATH, @"\", PAGE_TYPES_FOLDER_NAME);
            this.CreateFolderStructure();
        }


        public override void Initialize(IEnumerable<IPageType> pageTypes)
        {
            string fileName = string.Concat(this._pageTypesFolderPath, @"\", PAGE_TYPES_FILE_NAME_FORMAT);
            this.Save<IEnumerable<IPageType>>(pageTypes, fileName);
        }


        public override IEnumerable<IPageType> LoadPageTypes()
        {
            string fileName = string.Concat(this._pageTypesFolderPath, @"\", PAGE_TYPES_FILE_NAME_FORMAT);
            return this.Load<IEnumerable<IPageType>>(fileName, typeof(IEnumerable<IPageType>));
        }


        public override IEnumerable<IPageInfo> SavePage(IPage page)
        {
            if (page == null)
            {
                return this.LoadPageInfos();
            }

            string fileName = this.CreatePageFileName(page);
            this.Save<IPage>(page, fileName);
            return this.UpdatePageInfo(page, PageInfoAction.AddOrUpdate);
        }


        public override IPage LoadPage(string pageId, int pageVersion)
        {
            string fileName = this.CreatePageFileName(pageId, pageVersion);
            return this.Load<IPage>(fileName, typeof(IPage));
        }


        public override IEnumerable<IPageInfo> LoadPageInfos()
        {
            string fileName = Path.Combine(this._pageFolderPath, PAGE_INFO_FILE_NAME_FORMAT);
            IEnumerable<IPageInfo> pageInfos = this.Load<IEnumerable<IPageInfo>>(fileName, typeof(IEnumerable<IPageInfo>));

            if (pageInfos == null)
            {
                return new List<IPageInfo>();
            }

            return pageInfos.ToList();
        }


        public override IEnumerable<IPageInfo> Delete(string pageId, int version)
        {
            IPage page = this.LoadPage(pageId, version);
            string fileName = this.CreatePageFileName(pageId, version);
            this.Delete(fileName);
            return this.UpdatePageInfo(page, PageInfoAction.Delete);
        }

        // Protected

        protected override void SavePageInfos(IEnumerable<IPageInfo> pageInfos)
        {
            string fileName = Path.Combine(this._pageFolderPath, PAGE_INFO_FILE_NAME_FORMAT);
            this.Save<IEnumerable<IPageInfo>>(pageInfos, fileName);
        }

        // Private Methods

        private void CreateFolderStructure()
        {
            if (!Directory.Exists(this._pageFolderPath))
            {
                Directory.CreateDirectory(this._pageFolderPath);
            }
            if (!Directory.Exists(this._pageTypesFolderPath))
            {
                Directory.CreateDirectory(this._pageTypesFolderPath);    
            }
        }

        private string CreatePageFileName(IPage page)
        {
            return this.CreatePageFileName(page.Id, page.Version);
        }

        private string CreatePageFileName(string pageId, int version)
        {
            return Path.Combine(this._pageFolderPath, string.Format(PAGE_FILE_NAME_FORMAT, pageId, version));
        }

        private void Save<T>(T objToSave, string fileName)
        {
            this.CreateFolderStructure();
            string serializedObject = this._serializer.Serialize<T>(objToSave);
            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(serializedObject);
            sw.Close();
            sw.Dispose();
        }

        private T Load<T>(string fileName, Type objectType) where T : class
        {
            if (!File.Exists(fileName))
            {
                return default(T);
            }
            StreamReader sr = new StreamReader(fileName);
            string serializedObject = sr.ReadToEnd();
            sr.Close();
            return this._serializer.Deserialize<T>(serializedObject, objectType);
        }

        private void Delete(string fileName)
        {
            File.Delete(fileName);
        }
    }
}
