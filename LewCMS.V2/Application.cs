using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2
{
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
