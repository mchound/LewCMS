using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace LewCMS.V2.VirtualFileSystem
{
    public class LewCMSVirtualFile : VirtualFile
    {
        private string _physicalPath;
        public string PhysicalPath
        {
            get { return this._physicalPath; }
        }

        public LewCMSVirtualFile(string virtualPath, string physicalPath)
            : base(virtualPath)
        {
            this._physicalPath = physicalPath;
        }

        public override Stream Open()
        {
            return File.OpenRead(this._physicalPath);
        }
    }
}
