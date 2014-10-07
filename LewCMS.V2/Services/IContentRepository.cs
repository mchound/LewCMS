using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Services
{
    public interface IContentRepository
    {
        IContent GetContentFor(IContentInfo contentInfo);
        IContent GetContentFor(Func<IContentInfo, bool> predicate);
        Tcontent GetContentFor<Tcontent, Tinfo>(Func<Tinfo, bool> predicate) where Tcontent : class, IContent where Tinfo : class,IContentInfo;

        IEnumerable<IContent> GetContent();
        IEnumerable<T> GetContent<T>() where T : class;
        IEnumerable<IContent> GetContent(Func<IContentInfo, bool> predicate);
        IEnumerable<Tcontent> GetContent<Tcontent, Tinfo>(Func<Tinfo, bool> predicate) where Tcontent : class where Tinfo : class,IContentInfo;

        IContentInfo GetContentInfoFor(Func<IContentInfo, bool> predicate);
        T GetContentInfoFor<T>(Func<T, bool> predicate) where T : class, IContentInfo;

        IEnumerable<IContentInfo> GetContentInfo();
        IEnumerable<IContentInfo> GetContentInfo(Func<IContentInfo, bool> predicate);

        IEnumerable<IContentType> GetContentTypes();

        IEnumerable<IContentInfo> Delete(IContentInfo contentInfo);
        IEnumerable<IContentInfo> Delete(Func<IContentInfo, bool> predicate);

        int GetContentVersions(string id);
        int GetContentVersions(IContentInfo contentInfo);
        int GetContentVersions(IContent content);

        void Save(IContent content);
    }
}
