using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Services
{
    public class DefaultFilePersistService : BasePersistService
    {
        private ISerializeService _serializeService;
        private string _folderPath;
        private const string FOLDER_NAME = "Content";

        protected override string CONTENT_KEY_FORMAT
        {
            get { return "Content-{0}[version-{1}][lang-{2}].json"; }
        }

        protected override string CONTENT_DIRECTORY_KEY_FORMAT
        {
            get { return Path.Combine(this._folderPath, "ContentDirectory.json"); }
        }

        protected override string CONTENT_TYPES_KEY_FORMAT
        {
            get { return Path.Combine(this._folderPath, "ContentTypes.json"); }
        }

        public DefaultFilePersistService(ISerializeService serializeService)
        {
            this._serializeService = serializeService;
            this._folderPath = Path.Combine(Configuration.PERSITS_VIRTUAL_FILE_PATH, FOLDER_NAME);
            this.CreateFolderStructure();
        }

        protected override string CreateKey(IContent content)
        {
            return this.CreateKey(content.Id, content.Version, content.Culture.TwoLetterISOLanguageName);
        }

        protected override string CreateKey(IContentInfo contentInfo)
        {
            return this.CreateKey(contentInfo.Id, contentInfo.Version, contentInfo.Culture.TwoLetterISOLanguageName);
        }

        protected override string CreateKey(string id, int version, string language)
        {
            return Path.Combine(this._folderPath, string.Format(this.CONTENT_KEY_FORMAT, id, version.ToString(), language));
        }

        protected override void Save<T>(string key, T content)
        {
            this.CreateFolderStructure();
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
            if(File.Exists(key))
            {
                File.Delete(key);
            }
        }

        private void CreateFolderStructure()
        {
            if (!Directory.Exists(this._folderPath))
            {
                Directory.CreateDirectory(this._folderPath);
            }
        }
    }
}
