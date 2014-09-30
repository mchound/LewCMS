using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Services
{
    public class DefaultFilePersistService : BasePersistService
    {
        protected override string CONTENT_KEY_FORMAT
        {sd<get<d
            get { throw new NotImplementedException(); }
        }

        protected override string CONTENT_DIRECTORY_KEY_FORMAT
        {
            get { throw new NotImplementedException(); }
        }

        protected override string CONTENT_TYPES_KEY_FORMAT
        {
            get { throw new NotImplementedException(); }
        }

        protected override string CreateKey(IContent content)
        {
            throw new NotImplementedException();
        }

        protected override string CreateKey(IContentInfo contentInfo)
        {
            throw new NotImplementedException();
        }

        protected override string CreateKey(string id, string version, string language)
        {
            throw new NotImplementedException();
        }

        protected override void SavePageInfos(IEnumerable<IContentInfo> contentInfo)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<IContentInfo> Save<T>(string key, T content)
        {
            throw new NotImplementedException();
        }

        protected override T Load<T>(string key)
        {
            throw new NotImplementedException();
        }

        protected override void Delete(string key)
        {
            throw new NotImplementedException();
        }
    }
}
