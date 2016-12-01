using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurenTech.BusinessAction
{
    public abstract class BaseAction<IRepository>
    {
        private IRepository _repository;

        public BaseAction()
        {
            _repository = (IRepository)RepositoryMapper.GetRepositoryInstance<IRepository>();
        }

        protected IRepository Repository
        {
            get
            {
                return _repository;
            }
        }
    }
}
