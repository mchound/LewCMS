using LewCMS.V2.Serialization;
using LewCMS.V2.Startup;
using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Store.FileSystem
{
    public class DefaultFileStoreService : BaseStoreService, IFileStoreService
    {
        private ISerializeService _serializeService;

        public DefaultFileStoreService(ISerializeService serializeService)
        {
            this._serializeService = serializeService;
        }

        protected override string STORE_DIRECTORY_KEY_FORMAT
        {
            get { return Path.Combine(Configuration.PERSITS_VIRTUAL_FILE_PATH, "StoreDirectory.json"); }
        }

        protected override string CONTENT_TYPES_KEY_FORMAT
        {
            get { return Path.Combine(Configuration.PERSITS_VIRTUAL_FILE_PATH, "ContentTypes.json"); }
        }

        protected override string CreateKey(IStorable storable)
        {
            return string.Concat(Path.Combine(Configuration.PERSITS_VIRTUAL_FILE_PATH, storable.StoreDirectory, storable.StoreKey), ".json");
        }

        protected override string CreateKey(IStoreInfo storeInfo)
        {
            return string.Concat(Path.Combine(Configuration.PERSITS_VIRTUAL_FILE_PATH, storeInfo.StoreDirectory, storeInfo.StoreKey), ".json");
        }

        protected override void Save<T>(string key, T content)
        {
            this.CreateFolderStructure(key);
            string serializedObject = this._serializeService.Serialize<T>(content);
            StreamWriter sw = new StreamWriter(key);
            sw.Write(serializedObject);
            sw.Close();
            sw.Dispose();
        }

        protected override T Load<T>(string key)
        {
            if (!File.Exists(key))
            {
                return default(T);
            }
            StreamReader sr = new StreamReader(key);
            string serializedObject = sr.ReadToEnd();
            sr.Close();
            return this._serializeService.Deserialize<T>(serializedObject);
        }

        protected override void Delete(string key)
        {
            if (File.Exists(key))
            {
                File.Delete(key);
            }
        }

        private void CreateFolderStructure(string filePath)
        {
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
