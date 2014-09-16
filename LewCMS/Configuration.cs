using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace LewCMS
{
    public static class Configuration
    {
        //public const string BACKSTAGE_FILE_PATH = @"C:\Users\Tobias\Documents\Visual Studio 2013\Projects\MyWebApplication\LewCMS.BackStage";
        public const string BACKSTAGE_FILE_PATH = @"C:\tolu00\Playground\LewCMS\LewCMS.UnitTesting\App_Data";
        public const string ROOT_ID = "682B471C-7E7D-41F5-869E-11718A10DF27";
        public const string PERSITS_VIRTUAL_FILE_PATH = @"~\App_Data\LewCMS\FilePersist";
    }

    public class Application
    {
        private static Application _current = new Application();
        private Assembly _applicationAssembly;

        public static Application Current
        {
            get { return _current; }
        }

        public Assembly ApplicationAssembly
        {
            get { return this._applicationAssembly; }
        }

        public void SetApplicationAssembly(Assembly applicationAssembly)
        {
            this._applicationAssembly = applicationAssembly;
        }
    }
}
