using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurenTech.BusinessAction
{
    public abstract class BaseAction<IRepository, Request, Response>
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

        public abstract Response Execute(Request request);

        public Task<Response> ExecuteAsync(Request request)
        {
            var task = new Task<Response>(() => Execute(request));
            task.Start();
            return task;
        }
    }
}
