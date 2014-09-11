using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Hosting;

namespace LewCMS.Core.VirtualFileSystem
{
    public class LewCMSVirtualPathProvider : VirtualPathProvider
    {
        private string _backStagePhysicalPath;
        private const string BACKSTAGE_VIRTUAL_PATH = "/BackStage/LewCMS";

        public LewCMSVirtualPathProvider(string backStagePhysicalPath)
        {
            this._backStagePhysicalPath = backStagePhysicalPath;
        }

        public override bool FileExists(string virtualPath)
        {
            if (this.IsVirtual(virtualPath))
                return true;

            return base.FileExists(virtualPath);
        }

        public override bool DirectoryExists(string virtualDir)
        {
            if (this.IsVirtual(virtualDir))
                return true;

            return base.DirectoryExists(virtualDir);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (this.IsVirtual(virtualPath, false))
                return new LewCMSVirtualFile(virtualPath, this.MapVirtualPathToPhyscialPath(virtualPath));

            return base.GetFile(virtualPath);
        }

        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            return base.GetDirectory(virtualDir);
        }

        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (this.IsVirtual(virtualPath, false))
            {
                return new CacheDependency(new string[] { this.MapVirtualPathToPhyscialPath(virtualPath) }, new string[] { virtualPath }, utcStart);
            }
            else
            {
                return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
            }
        }

        public override string GetFileHash(string virtualPath, IEnumerable virtualPathDependencies)
        {
            string hash = base.GetFileHash(virtualPath, virtualPathDependencies);
            return hash;
        }

        public override string GetCacheKey(string virtualPath)
        {
            string cacheKey = base.GetCacheKey(virtualPath);
            return cacheKey;
        }

        private bool IsVirtual(string virtualPath, bool exists = true)
        {
            string virtPath = virtualPath.Replace("~", string.Empty);

            bool result = (virtPath.StartsWith(LewCMSVirtualPathProvider.BACKSTAGE_VIRTUAL_PATH) &&
                    exists &&
                    File.Exists(virtPath.Replace(LewCMSVirtualPathProvider.BACKSTAGE_VIRTUAL_PATH, this._backStagePhysicalPath)))
                    ||
                    (virtPath.StartsWith(LewCMSVirtualPathProvider.BACKSTAGE_VIRTUAL_PATH) && !exists);

            return result;

        }

        private string MapVirtualPathToPhyscialPath(string virtualPath)
        {
            return virtualPath.Replace(LewCMSVirtualPathProvider.BACKSTAGE_VIRTUAL_PATH, this._backStagePhysicalPath);
        }
    }

}
