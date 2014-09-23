using LewCMS.Core.Content;
using LewCMS.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.Core.Service
{
    public class FilePersistService : BasePersistService
    {
        private const string PAGES_FOLDER_NAME = "Pages";
        private const string PAGE_TYPES_FOLDER_NAME = "PageTypes";
        private const string PAGE_FILE_NAME_FORMAT = "Page-{0}[1].json";
        private const string PAGE_TYPE_FILE_NAME_FORMAT = "PageTypes.json";
        private const string PAGE_INFO_FILE_NAME_FORMAT = "PageInfo.json";

        private string _persistPath;
        private string _pagesPersistPath;
        private string _pageTypesPersistPath;
        private ISerializer _serializer;


        // Constructors

        public FilePersistService(string filePersistPath, ISerializer serializer)
        {
            this._persistPath = filePersistPath;
            this._pagesPersistPath = string.Concat(filePersistPath, @"\", PAGES_FOLDER_NAME);
            this._pageTypesPersistPath = string.Concat(filePersistPath, @"\", PAGE_TYPES_FOLDER_NAME);
            this._serializer = serializer;
        }


        // Public Methods

        public override void Initialize(IEnumerable<IPageType> pageTypes)
        {
            string serializedPageTypes = this._serializer.Serialize<List<IPageType>>(pageTypes.ToList());
            string fileName = string.Concat(this._pageTypesPersistPath, @"\", PAGE_TYPE_FILE_NAME_FORMAT);
            this.Save(fileName, serializedPageTypes);
        }


        public override IEnumerable<IPageType> LoadPageTypes()
        {
            string fileName = string.Concat(this._pageTypesPersistPath, @"\", PAGE_TYPE_FILE_NAME_FORMAT);
            string serializedPageTypes = this.Load(fileName);
            return this._serializer.Deserialize<IEnumerable<IPageType>>(serializedPageTypes);
        }


        public override IEnumerable<IPageInfo> SavePage(IPage page)
        {
            if (page == null)
            {
                return this.LoadPageInfos();
            }

            string serializedPage = this._serializer.Serialize<IPage>(page);
            string fileName = string.Concat(this._pagesPersistPath, @"\", string.Format(PAGE_FILE_NAME_FORMAT, page.Id, page.Version));
            this.Save(fileName, serializedPage);

            return this.UpdatePageInfo(page, PageInfoAction.AddOrUpdate);
        }


        public override IPage LoadPage(string pageId, int version)
        {
            string pageFileName = string.Format(PAGE_FILE_NAME_FORMAT, pageId, version);
            string pageFilePath = string.Concat(this._pagesPersistPath, @"\", pageFileName);
            string pageAsString = this.Load(pageFilePath);
            IPageInfo pageInfo = this.LoadPageInfo(pi => pi.PageId == pageId && pi.Version == version);
            Type pageInstanceType = pageInfo.GetPageInstanceType();
            return this._serializer.Deserialize<IPage>(pageAsString, pageInstanceType);
        }

        public override IEnumerable<IPage> LoadPages(string pageId)
        {
            return this.LoadPages(pi => pi.PageId == pageId);
        }

        public override IEnumerable<IPage> LoadPages(Func<IPageInfo, bool> predicate)
        {
            IEnumerable<IPageInfo> pageInfos = this.LoadPageInfos(predicate);

            foreach (IPageInfo pageInfo in pageInfos)
            {
                yield return this.LoadPage(pageInfo.PageId, pageInfo.Version);
            }
        }

        public override IEnumerable<IPage> LoadPages()
        {
            return this.LoadPages(pi => true);
        }


        public override IPageInfo LoadPageInfo(Func<IPageInfo, bool> predicate)
        {
            return this.LoadPageInfos().FirstOrDefault(predicate);
        }

        public override IEnumerable<IPageInfo> LoadPageInfos()
        {
            string fileInfosFilePath = string.Concat(this._pagesPersistPath, @"\", PAGE_INFO_FILE_NAME_FORMAT);
            string serializedFileInfos = this.Load(fileInfosFilePath);
            return this._serializer.Deserialize<IEnumerable<IPageInfo>>(serializedFileInfos);
        }

        public override IEnumerable<IPageInfo> LoadPageInfos(Func<IPageInfo, bool> predicate)
        {
            return this.LoadPageInfos().Where(predicate);
        }


        public override IEnumerable<IPageInfo> Delete(string pageId, int version)
        {
            string fileName = string.Concat(this._pagesPersistPath, @"\", string.Format(PAGE_FILE_NAME_FORMAT, pageId, version));
            this.Delete(fileName);
            return this.UpdatePageInfo(pageId, version, PageInfoAction.Delete);
        }

        // Private Methods

        private void Save(string filePath, string fileString)
        {
            StreamWriter sw = new StreamWriter(filePath);
            sw.Write(fileString);
            sw.Close();
            sw.Dispose();
        }

        private string Load(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            string fileStream = sr.ReadToEnd();
            sr.Close();
            return fileStream;
        }

        private void Delete(string filePath)
        {
            File.Delete(filePath);
        }

        protected override void SavePageInfos(IEnumerable<IPageInfo> pageInfos)
        {
            string serializedPageInfos = this._serializer.Serialize<List<IPageInfo>>(pageInfos.ToList());
            string fileName = string.Concat(this._pagesPersistPath, @"\", PAGE_INFO_FILE_NAME_FORMAT);
            this.Save(fileName, serializedPageInfos);
        }
    }
}
