using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Services
{
    public abstract class BaseService
    {
        private IRepository _repository;
        public IRepository Repository
        {
            get { return _repository; }
            set { _repository = value; }
        }

        public BaseService(IRepository repository)
        {
            this._repository = repository;
        }
        
    }
}
