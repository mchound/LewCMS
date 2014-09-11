using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace LewCMS.Core.VirtualFileSystem
{
    public class LewCMSVirtualDirectory : VirtualDirectory
    {
        private string _physicalPath;
        public string PhysicalPath
        {
            get { return this._physicalPath; }
        }
        private string _virtualDirectory;

        public LewCMSVirtualDirectory(string virtualDirectory, string physicalDirectory)
            : base(virtualDirectory)
        {
            this._physicalPath = physicalDirectory;
            this._virtualDirectory = virtualDirectory;
            this.Init();
        }

        private void Init()
        {
            DirectoryInfo dir = new DirectoryInfo(this._physicalPath);

            foreach (var subDir in dir.GetDirectories())
            {
                string virtPath = string.Concat(this._virtualDirectory, subDir.Name);
                LewCMSVirtualDirectory virtDir = new LewCMSVirtualDirectory(virtPath, subDir.FullName);
                this.children.Add(virtDir);
                this.directories.Add(virtDir);
            }

            foreach (var subFile in dir.GetFiles())
            {
                string virtPath = string.Concat(this._virtualDirectory, subFile.Name);
                string physPath = string.Concat(this._physicalPath, subFile.Name);

                LewCMSVirtualFile virtFile = new LewCMSVirtualFile(virtPath, physPath);
                this.children.Add(virtFile);
                this.files.Add(virtFile);
            }
        }

        private ArrayList children = new ArrayList();
        public override System.Collections.IEnumerable Children
        {
            get { return this.children; }
        }

        private ArrayList directories = new ArrayList();
        public override System.Collections.IEnumerable Directories
        {
            get { return this.directories; }
        }

        private ArrayList files = new ArrayList();
        public override System.Collections.IEnumerable Files
        {
            get { return this.files; }
        }
    }
}
